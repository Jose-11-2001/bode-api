using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BODE.API.Controllers
{
    /// <summary>
    /// Manages all service orders, diagnostics, and repairs
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ServiceOrdersController : ControllerBase
    {
        private readonly IServiceOrderService _service;
        private readonly ILogger<ServiceOrdersController> _logger;

        public ServiceOrdersController(IServiceOrderService service, ILogger<ServiceOrdersController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new service order with initial tasks
        /// </summary>
        /// <param name="request">Service order details including vehicle and tasks</param>
        /// <returns>The created service order</returns>
        /// <response code="201">Service order created successfully</response>
        /// <response code="400">Invalid request data</response>
        /// <response code="404">Vehicle not found</response>
        [HttpPost]
        [ProducesResponseType(typeof(ServiceOrderResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Create([FromBody] CreateServiceOrderRequest request)
        {
            try
            {
                var result = await _service.CreateAsync(request);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Vehicle not found: {VehicleId}", request.VehicleId);
                return NotFound(new ProblemDetails
                {
                    Title = "Vehicle not found",
                    Detail = ex.Message,
                    Status = StatusCodes.Status404NotFound
                });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new ValidationProblemDetails
                {
                    Title = "Validation failed",
                    Detail = ex.Message,
                    Status = StatusCodes.Status400BadRequest
                });
            }
        }

        /// <summary>
        /// Gets a service order by ID
        /// </summary>
        /// <param name="id">The service order ID</param>
        /// <returns>The service order details</returns>
        /// <response code="200">Returns the service order</response>
        /// <response code="404">Service order not found</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ServiceOrderResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _service.GetByIdAsync(id);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Service order not found: {OrderId}", id);
                return NotFound(new ProblemDetails
                {
                    Title = "Service order not found",
                    Detail = ex.Message,
                    Status = StatusCodes.Status404NotFound
                });
            }
        }

        /// <summary>
        /// Gets all service orders for a specific vehicle
        /// </summary>
        /// <param name="vehicleId">The vehicle ID</param>
        /// <returns>List of service orders</returns>
        [HttpGet("vehicle/{vehicleId:int}")]
        [ProducesResponseType(typeof(IEnumerable<ServiceOrderResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByVehicle(int vehicleId)
        {
            var results = await _service.GetByVehicleAsync(vehicleId);
            return Ok(results);
        }

        /// <summary>
        /// Gets all service orders for a specific customer
        /// </summary>
        /// <param name="customerId">The customer ID</param>
        /// <returns>List of service orders</returns>
        [HttpGet("customer/{customerId:int}")]
        [ProducesResponseType(typeof(IEnumerable<ServiceOrderResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByCustomer(int customerId)
        {
            var results = await _service.GetByCustomerAsync(customerId);
            return Ok(results);
        }

        /// <summary>
        /// Gets all service orders by status
        /// </summary>
        /// <param name="status">The service status (Pending, Diagnosing, etc.)</param>
        /// <returns>List of service orders with the specified status</returns>
        [HttpGet("status/{status}")]
        [ProducesResponseType(typeof(IEnumerable<ServiceOrderResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByStatus(ServiceStatus status)
        {
            var results = await _service.GetByStatusAsync(status);
            return Ok(results);
        }

        /// <summary>
        /// Gets all active service orders (not completed or cancelled)
        /// </summary>
        /// <returns>List of active service orders</returns>
        [HttpGet("active")]
        [ProducesResponseType(typeof(IEnumerable<ServiceOrderResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetActive()
        {
            var results = await _service.GetActiveAsync();
            return Ok(results);
        }

        /// <summary>
        /// Gets all service orders with pagination
        /// </summary>
        /// <param name="page">Page number (default: 1)</param>
        /// <param name="pageSize">Items per page (default: 20, max: 100)</param>
        /// <returns>Paginated list of service orders</returns>
        [HttpGet("paged")]
        [ProducesResponseType(typeof(PagedResult<ServiceOrderResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 20;
            if (pageSize > 100) pageSize = 100;

            var results = await _service.GetPagedAsync(page, pageSize);
            return Ok(results);
        }

        /// <summary>
        /// Updates the status of a service order
        /// </summary>
        /// <param name="id">The service order ID</param>
        /// <param name="request">Status update details</param>
        /// <returns>The updated service order</returns>
        /// <response code="200">Status updated successfully</response>
        /// <response code="400">Invalid status transition</response>
        /// <response code="404">Service order not found</response>
        [HttpPatch("{id:int}/status")]
        [ProducesResponseType(typeof(ServiceOrderResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateStatusRequest request)
        {
            try
            {
                var result = await _service.UpdateStatusAsync(id, request.Status, request.Notes);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Service order not found: {OrderId}", id);
                return NotFound(new ProblemDetails
                {
                    Title = "Service order not found",
                    Detail = ex.Message,
                    Status = StatusCodes.Status404NotFound
                });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Invalid status transition for order: {OrderId}", id);
                return BadRequest(new ProblemDetails
                {
                    Title = "Invalid status transition",
                    Detail = ex.Message,
                    Status = StatusCodes.Status400BadRequest
                });
            }
        }

        /// <summary>
        /// Adds a diagnostic code to a service order
        /// </summary>
        /// <param name="id">The service order ID</param>
        /// <param name="request">Diagnostic code details</param>
        /// <returns>The updated service order</returns>
        [HttpPost("{id:int}/diagnostic-codes")]
        [ProducesResponseType(typeof(ServiceOrderResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddDiagnosticCode(int id, [FromBody] AddDiagnosticCodeRequest request)
        {
            try
            {
                var result = await _service.AddDiagnosticCodeAsync(id, request);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Service order not found",
                    Detail = ex.Message,
                    Status = StatusCodes.Status404NotFound
                });
            }
        }

        /// <summary>
        /// Clears (resolves) a diagnostic code from a service order
        /// </summary>
        /// <param name="orderId">The service order ID</param>
        /// <param name="codeId">The diagnostic code ID to clear</param>
        /// <returns>The updated service order</returns>
        [HttpDelete("{orderId:int}/diagnostic-codes/{codeId:int}")]
        [ProducesResponseType(typeof(ServiceOrderResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ClearDiagnosticCode(int orderId, int codeId)
        {
            try
            {
                var result = await _service.ClearDiagnosticCodeAsync(orderId, codeId);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Diagnostic code not found",
                    Detail = ex.Message,
                    Status = StatusCodes.Status404NotFound
                });
            }
        }

        /// <summary>
        /// Updates the status of a specific task within a service order
        /// </summary>
        /// <param name="taskId">The task ID</param>
        /// <param name="request">Task status update details</param>
        /// <returns>The updated task</returns>
        [HttpPatch("tasks/{taskId:int}")]
        [ProducesResponseType(typeof(ServiceTaskResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateTaskStatus(int taskId, [FromBody] UpdateTaskStatusRequest request)
        {
            try
            {
                var result = await _service.UpdateTaskStatusAsync(taskId, request);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Task not found",
                    Detail = ex.Message,
                    Status = StatusCodes.Status404NotFound
                });
            }
        }

        /// <summary>
        /// Adds a part used in the repair to a service order
        /// </summary>
        /// <param name="id">The service order ID</param>
        /// <param name="request">Part details</param>
        /// <returns>The updated service order</returns>
        [HttpPost("{id:int}/parts")]
        [ProducesResponseType(typeof(ServiceOrderResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddPart(int id, [FromBody] AddPartRequest request)
        {
            try
            {
                var result = await _service.AddPartAsync(id, request);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Service order not found",
                    Detail = ex.Message,
                    Status = StatusCodes.Status404NotFound
                });
            }
        }

        /// <summary>
        /// Removes a part from a service order
        /// </summary>
        /// <param name="orderId">The service order ID</param>
        /// <param name="partId">The part ID to remove</param>
        /// <returns>The updated service order</returns>
        [HttpDelete("{orderId:int}/parts/{partId:int}")]
        [ProducesResponseType(typeof(ServiceOrderResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemovePart(int orderId, int partId)
        {
            try
            {
                var result = await _service.RemovePartAsync(orderId, partId);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Part not found",
                    Detail = ex.Message,
                    Status = StatusCodes.Status404NotFound
                });
            }
        }

        /// <summary>
        /// Recalculates all totals for a service order (labor, parts, tax, grand total)
        /// </summary>
        /// <param name="id">The service order ID</param>
        /// <returns>The updated service order with recalculated totals</returns>
        [HttpPost("{id:int}/calculate")]
        [ProducesResponseType(typeof(ServiceOrderResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CalculateTotals(int id)
        {
            try
            {
                var result = await _service.CalculateTotalsAsync(id);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Service order not found",
                    Detail = ex.Message,
                    Status = StatusCodes.Status404NotFound
                });
            }
        }

        /// <summary>
        /// Generates a quotation/invoice for a service order
        /// </summary>
        /// <param name="id">The service order ID</param>
        /// <returns>Quotation details</returns>
        [HttpGet("{id:int}/quotation")]
        [ProducesResponseType(typeof(QuotationResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetQuotation(int id)
        {
            try
            {
                var result = await _service.GenerateQuotationAsync(id);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Service order not found",
                    Detail = ex.Message,
                    Status = StatusCodes.Status404NotFound
                });
            }
        }

        /// <summary>
        /// Deletes a service order (soft delete)
        /// </summary>
        /// <param name="id">The service order ID</param>
        /// <returns>No content</returns>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Service order not found",
                    Detail = ex.Message,
                    Status = StatusCodes.Status404NotFound
                });
            }
        }

        /// <summary>
        /// Gets the status history of a service order
        /// </summary>
        /// <param name="id">The service order ID</param>
        /// <returns>List of status history entries</returns>
        [HttpGet("{id:int}/history")]
        [ProducesResponseType(typeof(IEnumerable<StatusHistoryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetStatusHistory(int id)
        {
            try
            {
                var result = await _service.GetStatusHistoryAsync(id);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Service order not found",
                    Detail = ex.Message,
                    Status = StatusCodes.Status404NotFound
                });
            }
        }

        /// <summary>
        /// Assigns a mechanic to a service order
        /// </summary>
        /// <param name="id">The service order ID</param>
        /// <param name="mechanicId">The mechanic ID</param>
        /// <returns>The updated service order</returns>
        [HttpPatch("{id:int}/assign/{mechanicId:int}")]
        [ProducesResponseType(typeof(ServiceOrderResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AssignMechanic(int id, int mechanicId)
        {
            try
            {
                var result = await _service.AssignMechanicAsync(id, mechanicId);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Service order or mechanic not found",
                    Detail = ex.Message,
                    Status = StatusCodes.Status404NotFound
                });
            }
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BODE.API.Controllers
{
    /// <summary>
    /// Manages vehicle information
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class VehiclesController : ControllerBase
    {
        private readonly IVehicleService _service;
        private readonly ILogger<VehiclesController> _logger;

        public VehiclesController(IVehicleService service, ILogger<VehiclesController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Registers a new vehicle
        /// </summary>
        /// <param name="request">Vehicle details</param>
        /// <returns>The created vehicle</returns>
        [HttpPost]
        [ProducesResponseType(typeof(VehicleResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Create([FromBody] VehicleRequest request)
        {
            try
            {
                var result = await _service.CreateAsync(request);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Customer not found",
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
        /// Gets a vehicle by ID
        /// </summary>
        /// <param name="id">The vehicle ID</param>
        /// <returns>Vehicle details</returns>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(VehicleResponse), StatusCodes.Status200OK)]
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
                return NotFound(new ProblemDetails
                {
                    Title = "Vehicle not found",
                    Detail = ex.Message,
                    Status = StatusCodes.Status404NotFound
                });
            }
        }

        /// <summary>
        /// Gets all vehicles for a specific customer
        /// </summary>
        /// <param name="customerId">The customer ID</param>
        /// <returns>List of vehicles</returns>
        [HttpGet("customer/{customerId:int}")]
        [ProducesResponseType(typeof(IEnumerable<VehicleResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByCustomer(int customerId)
        {
            var results = await _service.GetByCustomerAsync(customerId);
            return Ok(results);
        }

        /// <summary>
        /// Gets a vehicle by license plate
        /// </summary>
        /// <param name="licensePlate">The license plate number</param>
        /// <returns>Vehicle details</returns>
        [HttpGet("license/{licensePlate}")]
        [ProducesResponseType(typeof(VehicleResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByLicensePlate(string licensePlate)
        {
            try
            {
                var result = await _service.GetByLicensePlateAsync(licensePlate);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Vehicle not found",
                    Detail = ex.Message,
                    Status = StatusCodes.Status404NotFound
                });
            }
        }

        /// <summary>
        /// Gets a vehicle by VIN
        /// </summary>
        /// <param name="vin">The VIN number</param>
        /// <returns>Vehicle details</returns>
        [HttpGet("vin/{vin}")]
        [ProducesResponseType(typeof(VehicleResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByVIN(string vin)
        {
            try
            {
                var result = await _service.GetByVINAsync(vin);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Vehicle not found",
                    Detail = ex.Message,
                    Status = StatusCodes.Status404NotFound
                });
            }
        }

        /// <summary>
        /// Searches for vehicles by make, model, or year
        /// </summary>
        /// <param name="make">Vehicle make (optional)</param>
        /// <param name="model">Vehicle model (optional)</param>
        /// <param name="year">Vehicle year (optional)</param>
        /// <returns>List of matching vehicles</returns>
        [HttpGet("search")]
        [ProducesResponseType(typeof(IEnumerable<VehicleResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Search([FromQuery] string make = null, [FromQuery] string model = null, [FromQuery] int? year = null)
        {
            var results = await _service.SearchAsync(make, model, year);
            return Ok(results);
        }

        /// <summary>
        /// Updates vehicle information
        /// </summary>
        /// <param name="id">The vehicle ID</param>
        /// <param name="request">Updated vehicle details</param>
        /// <returns>The updated vehicle</returns>
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(VehicleResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] VehicleRequest request)
        {
            try
            {
                var result = await _service.UpdateAsync(id, request);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
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
        /// Updates vehicle mileage
        /// </summary>
        /// <param name="id">The vehicle ID</param>
        /// <param name="mileage">The new mileage</param>
        /// <returns>The updated vehicle</returns>
        [HttpPatch("{id:int}/mileage")]
        [ProducesResponseType(typeof(VehicleResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateMileage(int id, [FromBody] int mileage)
        {
            try
            {
                var result = await _service.UpdateMileageAsync(id, mileage);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Vehicle not found",
                    Detail = ex.Message,
                    Status = StatusCodes.Status404NotFound
                });
            }
        }

        /// <summary>
        /// Gets service history for a vehicle
        /// </summary>
        /// <param name="id">The vehicle ID</param>
        /// <returns>Service history</returns>
        [HttpGet("{id:int}/history")]
        [ProducesResponseType(typeof(IEnumerable<ServiceOrderResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetServiceHistory(int id)
        {
            try
            {
                var results = await _service.GetServiceHistoryAsync(id);
                return Ok(results);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Vehicle not found",
                    Detail = ex.Message,
                    Status = StatusCodes.Status404NotFound
                });
            }
        }

        /// <summary>
        /// Gets maintenance schedule for a vehicle
        /// </summary>
        /// <param name="id">The vehicle ID</param>
        /// <returns>Maintenance schedule</returns>
        [HttpGet("{id:int}/maintenance")]
        [ProducesResponseType(typeof(MaintenanceScheduleResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMaintenanceSchedule(int id)
        {
            try
            {
                var result = await _service.GetMaintenanceScheduleAsync(id);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Vehicle not found",
                    Detail = ex.Message,
                    Status = StatusCodes.Status404NotFound
                });
            }
        }

        /// <summary>
        /// Deletes a vehicle
        /// </summary>
        /// <param name="id">The vehicle ID</param>
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
                    Title = "Vehicle not found",
                    Detail = ex.Message,
                    Status = StatusCodes.Status404NotFound
                });
            }
        }
    }
}
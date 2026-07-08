using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BODE.API.Controllers
{
    /// <summary>
    /// Manages customer information
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _service;
        private readonly ILogger<CustomersController> _logger;

        public CustomersController(ICustomerService service, ILogger<CustomersController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Registers a new customer
        /// </summary>
        /// <param name="request">Customer details</param>
        /// <returns>The created customer</returns>
        [HttpPost]
        [ProducesResponseType(typeof(CustomerResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CustomerRequest request)
        {
            try
            {
                var result = await _service.CreateAsync(request);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
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
        /// Gets a customer by ID
        /// </summary>
        /// <param name="id">The customer ID</param>
        /// <returns>Customer details</returns>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(CustomerResponse), StatusCodes.Status200OK)]
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
                    Title = "Customer not found",
                    Detail = ex.Message,
                    Status = StatusCodes.Status404NotFound
                });
            }
        }

        /// <summary>
        /// Gets a customer by email
        /// </summary>
        /// <param name="email">The customer's email address</param>
        /// <returns>Customer details</returns>
        [HttpGet("email/{email}")]
        [ProducesResponseType(typeof(CustomerResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByEmail(string email)
        {
            try
            {
                var result = await _service.GetByEmailAsync(email);
                return Ok(result);
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
        }

        /// <summary>
        /// Gets a customer by phone number
        /// </summary>
        /// <param name="phone">The customer's phone number</param>
        /// <returns>Customer details</returns>
        [HttpGet("phone/{phone}")]
        [ProducesResponseType(typeof(CustomerResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByPhone(string phone)
        {
            try
            {
                var result = await _service.GetByPhoneAsync(phone);
                return Ok(result);
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
        }

        /// <summary>
        /// Searches for customers by name, email, or phone
        /// </summary>
        /// <param name="searchTerm">Search term</param>
        /// <returns>List of matching customers</returns>
        [HttpGet("search")]
        [ProducesResponseType(typeof(IEnumerable<CustomerResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Search([FromQuery] string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || searchTerm.Length < 2)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Invalid search term",
                    Detail = "Search term must be at least 2 characters long",
                    Status = StatusCodes.Status400BadRequest
                });
            }

            var results = await _service.SearchAsync(searchTerm);
            return Ok(results);
        }

        /// <summary>
        /// Gets all customers with pagination
        /// </summary>
        /// <param name="page">Page number</param>
        /// <param name="pageSize">Items per page</param>
        /// <returns>Paginated customer list</returns>
        [HttpGet("paged")]
        [ProducesResponseType(typeof(PagedResult<CustomerResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 20;
            if (pageSize > 100) pageSize = 100;

            var results = await _service.GetPagedAsync(page, pageSize);
            return Ok(results);
        }

        /// <summary>
        /// Updates customer information
        /// </summary>
        /// <param name="id">The customer ID</param>
        /// <param name="request">Updated customer details</param>
        /// <returns>The updated customer</returns>
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(CustomerResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] CustomerRequest request)
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
        /// Gets customer statistics
        /// </summary>
        /// <param name="id">The customer ID</param>
        /// <returns>Customer statistics</returns>
        [HttpGet("{id:int}/stats")]
        [ProducesResponseType(typeof(CustomerStatsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetStats(int id)
        {
            try
            {
                var result = await _service.GetStatsAsync(id);
                return Ok(result);
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
        }

        /// <summary>
        /// Deletes a customer
        /// </summary>
        /// <param name="id">The customer ID</param>
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
                    Title = "Customer not found",
                    Detail = ex.Message,
                    Status = StatusCodes.Status404NotFound
                });
            }
        }
    }
}
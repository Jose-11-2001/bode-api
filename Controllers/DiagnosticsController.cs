using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BODE.API.Controllers
{
    /// <summary>
    /// Handles vehicle diagnostics, OBD scanning, and code interpretation
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class DiagnosticsController : ControllerBase
    {
        private readonly IDiagnosticService _service;
        private readonly ILogger<DiagnosticsController> _logger;

        public DiagnosticsController(IDiagnosticService service, ILogger<DiagnosticsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Performs a complete OBD-II scan on a vehicle
        /// </summary>
        /// <param name="vehicleId">The vehicle ID to scan</param>
        /// <returns>List of diagnostic trouble codes found</returns>
        [HttpGet("scan/{vehicleId:int}")]
        [ProducesResponseType(typeof(OBDScanResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ScanOBD(int vehicleId)
        {
            try
            {
                var result = await _service.ScanOBDAsync(vehicleId);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Vehicle not found for OBD scan: {VehicleId}", vehicleId);
                return NotFound(new ProblemDetails
                {
                    Title = "Vehicle not found",
                    Detail = ex.Message,
                    Status = StatusCodes.Status404NotFound
                });
            }
        }

        /// <summary>
        /// Interprets a diagnostic trouble code (DTC)
        /// </summary>
        /// <param name="code">The diagnostic code (e.g., P0300)</param>
        /// <returns>Detailed interpretation of the code</returns>
        [HttpGet("interpret/{code}")]
        [ProducesResponseType(typeof(CodeInterpretationResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> InterpretCode(string code)
        {
            try
            {
                var result = await _service.InterpretCodeAsync(code);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Code not found",
                    Detail = ex.Message,
                    Status = StatusCodes.Status404NotFound
                });
            }
        }

        /// <summary>
        /// Gets all common diagnostic trouble codes
        /// </summary>
        /// <param name="system">Optional: Filter by system (Engine, Transmission, etc.)</param>
        /// <returns>List of common diagnostic codes</returns>
        [HttpGet("codes/common")]
        [ProducesResponseType(typeof(IEnumerable<DiagnosticCodeReference>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCommonCodes([FromQuery] string system = null)
        {
            var results = await _service.GetCommonCodesAsync(system);
            return Ok(results);
        }

        /// <summary>
        /// Searches for diagnostic codes by keyword
        /// </summary>
        /// <param name="keyword">Search keyword (e.g., "misfire", "oxygen")</param>
        /// <returns>List of matching diagnostic codes</returns>
        [HttpGet("codes/search")]
        [ProducesResponseType(typeof(IEnumerable<DiagnosticCodeReference>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SearchCodes([FromQuery] string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword) || keyword.Length < 3)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Invalid search term",
                    Detail = "Search keyword must be at least 3 characters long",
                    Status = StatusCodes.Status400BadRequest
                });
            }

            var results = await _service.SearchCodesAsync(keyword);
            return Ok(results);
        }

        /// <summary>
        /// Gets diagnostic code statistics for a vehicle
        /// </summary>
        /// <param name="vehicleId">The vehicle ID</param>
        /// <returns>Statistics about diagnostic codes</returns>
        [HttpGet("stats/{vehicleId:int}")]
        [ProducesResponseType(typeof(DiagnosticStatsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDiagnosticStats(int vehicleId)
        {
            try
            {
                var result = await _service.GetDiagnosticStatsAsync(vehicleId);
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
        /// Gets all diagnostic codes for a specific service order
        /// </summary>
        /// <param name="orderId">The service order ID</param>
        /// <returns>List of diagnostic codes</returns>
        [HttpGet("order/{orderId:int}")]
        [ProducesResponseType(typeof(IEnumerable<DiagnosticCodeResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCodesByOrder(int orderId)
        {
            try
            {
                var results = await _service.GetCodesByOrderAsync(orderId);
                return Ok(results);
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
        /// Gets active diagnostic codes for a vehicle
        /// </summary>
        /// <param name="vehicleId">The vehicle ID</param>
        /// <returns>List of active diagnostic codes</returns>
        [HttpGet("vehicle/{vehicleId:int}/active")]
        [ProducesResponseType(typeof(IEnumerable<DiagnosticCodeResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetActiveCodesByVehicle(int vehicleId)
        {
            var results = await _service.GetActiveCodesByVehicleAsync(vehicleId);
            return Ok(results);
        }

        /// <summary>
        /// Clears all diagnostic codes from a vehicle (simulates clearing)
        /// </summary>
        /// <param name="vehicleId">The vehicle ID</param>
        /// <returns>Success status</returns>
        [HttpPost("clear/{vehicleId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ClearAllCodes(int vehicleId)
        {
            try
            {
                await _service.ClearAllCodesAsync(vehicleId);
                return Ok(new { message = "All diagnostic codes cleared successfully" });
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
        /// Gets recommended repair actions for a specific diagnostic code
        /// </summary>
        /// <param name="code">The diagnostic code</param>
        /// <returns>List of recommended actions</returns>
        [HttpGet("{code}/recommendations")]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetRecommendations(string code)
        {
            try
            {
                var results = await _service.GetRecommendedActionsAsync(code);
                return Ok(results);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Code not found",
                    Detail = ex.Message,
                    Status = StatusCodes.Status404NotFound
                });
            }
        }
    }
}
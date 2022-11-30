using box.application.Interfaces;
using box.application.Models.Request;
using box.application.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace box.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StorageController : BaseController
    {
        private readonly ILogger<StorageController> _logger;

        public StorageController(ILogger<StorageController> logger) : base(logger)
        {
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StorageResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFile(
            [FromServices] IStorageUseCase p_Service,
            [FromForm] StorageRequest p_Request)
        {
            try
            {
                return Ok(await p_Service.HandleAsync(p_Request));
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }
    }
}
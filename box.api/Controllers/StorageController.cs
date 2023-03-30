using box.api.Presenters;
using box.api.Request;
using box.application.Interfaces;
using box.application.Models.Request;
using Microsoft.AspNetCore.Mvc;

namespace box.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StorageController : BaseController
    {
        public StorageController() : base()
        {
        }

        /// <summary>
        /// Store a File
        /// </summary>
        /// <param name="p_Service"></param>
        /// <param name="storagePresenter"></param>
        /// <param name="p_Request"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JsonContentResult))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> StoreFile(
            [FromServices] IStorageUseCase p_Service,
            [FromServices] StoragePresenter storagePresenter,
            [FromForm] BodyStorageRequest p_Request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await p_Service.HandleAsync(new StorageRequest(p_Request.File, p_Request.Project), storagePresenter);

            return storagePresenter.ContentResult;
        }

        /// <summary>
        /// Read a file
        /// </summary>
        /// <param name="p_Service"></param>
        /// <param name="p_ProjectCode"></param>
        /// <param name="storageGetPresenter"></param>
        /// <param name="p_FileName"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileContentResult))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFile(
            [FromServices] IStorageUseCase p_Service,
            [FromQuery] string p_ProjectCode,
            [FromServices] StorageGetPresenter storageGetPresenter,
            [FromQuery] string p_FileName
            )
        {
            if (string.IsNullOrEmpty(p_ProjectCode) || string.IsNullOrEmpty(p_FileName))
            {
                return BadRequest(ModelState);
            }

            await p_Service.HandleAsync(new StorageGetRequest(p_ProjectCode, p_FileName), storageGetPresenter);

            return File(storageGetPresenter.ContentResult, "application/octet-stream", p_FileName);
        }
    }
}
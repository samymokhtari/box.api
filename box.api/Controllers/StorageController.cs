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
        private readonly StoragePresenter _storagePresenter;

        public StorageController(StoragePresenter storagePresenter) : base()
        {
            _storagePresenter = storagePresenter;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JsonContentResult))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> StoreFile(
            [FromServices] IStorageUseCase p_Service,
            [FromForm] BodyStorageRequest p_Request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await p_Service.HandleAsync(new StorageRequest(p_Request.File, p_Request.ProjectID), _storagePresenter);

            return _storagePresenter.ContentResult;
        }
    }
}
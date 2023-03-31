using box.api.Presenters;
using box.api.Request;
using box.application.Interfaces;
using box.application.Models.Request;
using Microsoft.AspNetCore.Mvc;

namespace box.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : BaseController
    {
        private readonly ProjectPresenter _projectPresenter;

        public ProjectController(ProjectPresenter storagePresenter)
        {
            _projectPresenter = storagePresenter;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JsonContentResult))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddProject(
            [FromServices] IProjectUseCase useCase,
            [FromServices] EmptyPresenter emptyPresenter,
            [FromBody] BodyProjectRequest body)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await useCase.HandleAsync(new ProjectRequest(body.ProjectName, body.ProjectCode), emptyPresenter);

            return emptyPresenter.ContentResult;
        }

        [HttpGet("{projectCode}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JsonContentResult))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProject(
            [FromServices] IProjectUseCase useCase,

            [FromRoute] string projectCode)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await useCase.HandleAsync(new ProjectRequest(projectCode), _projectPresenter);

            return _projectPresenter.ContentResult;
        }
    }
}
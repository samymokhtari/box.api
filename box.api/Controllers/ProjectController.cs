using box.api.Presenters;
using box.api.Request;
using box.application.Interfaces;
using box.application.Models.Request;
using box.application.Models.Response;
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
            [FromBody] BodyProjectRequest body)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await useCase.HandleAsync(new ProjectRequest(body.ProjectName, body.ProjectCode), _projectPresenter);

            return _projectPresenter.ContentResult;
        }

        [HttpGet("{projectCode}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JsonContentResult))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProject(
            [FromServices] IProjectUseCase useCase,
            [FromServices] IOutputPort<EmptyResponse> outputPort,
            [FromRoute] string projectCode)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await useCase.HandleAsync(new ProjectRequest(projectCode), outputPort);

            return _projectPresenter.ContentResult;
        }
    }
}

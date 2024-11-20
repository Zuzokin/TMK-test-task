using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PipeManager.Core.Abstractions;
using PipeManager.Core.Contracts.Requests;
using PipeManager.Core.Contracts.Responses;
using PipeManager.Core.Models;

namespace PipeManager.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PipesController : ControllerBase
    {
        private readonly IPipesService _pipesService;
        private readonly IPackagesService _packagesService;
        private readonly IMapper _mapper;

        public PipesController(IPipesService pipesService, IPackagesService packagesService, IMapper mapper)
        {
            _pipesService = pipesService;
            _packagesService = packagesService;
            _mapper = mapper;
        }

        // GET: api/Pipes
        [HttpGet]
        public async Task<ActionResult<List<PipeResponse>>> GetAllPipes()
        {
            var pipes = await _pipesService.GetAllPipes();
            if (!pipes.Any())
            {
                return NoContent();
            }

            var response = _mapper.Map<List<PipeResponse>>(pipes);
            return Ok(response);
        }

        // GET: api/Pipes/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<PipeResponse>> GetPipeById(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid pipe ID.");
            }

            var pipe = await _pipesService.GetPipeById(id);
            var response = _mapper.Map<PipeResponse>(pipe);
            return Ok(response);
        }

        // POST: api/Pipes
        [HttpPost]
        public async Task<ActionResult<Guid>> CreatePipe([FromBody] PipeRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // if (request.PackageId is not null)
            // {
            //     var package = await _packagesService.GetPackageById(request.PackageId.Value);
            //
            //     if (package is null)
            //     {
            //         return BadRequest("Invalid package ID.");
            //     }
            // }

            // Маппинг из PipeRequest в Pipe с генерацией нового ID
            var pipe = _mapper.Map<Pipe>(request);
    
            var pipeId = await _pipesService.CreatePipe(pipe);
            return CreatedAtAction(nameof(GetPipeById), new { id = pipeId }, pipeId);
        }


        // PUT: api/Pipes/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdatePipe(Guid id, [FromBody] PipeRequest request)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid pipe ID.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _pipesService.UpdatePipe(
                id,
                request.Label,
                request.IsGood,
                request.Diameter,
                request.Length,
                request.Weight,
                request.SteelGradeId,
                request.PackageId);

            return NoContent();
        }

        // DELETE: api/Pipes/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePipe(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid pipe ID.");
            }

            await _pipesService.DeletePipe(id);
            return NoContent();
        }

        // GET: api/Pipes/statistics
        [Authorize]
        [HttpGet("statistics")]
        public async Task<ActionResult<PipeStatistics>> GetStatistics()
        {
            var statistics = await _pipesService.GetStatistics();
            return statistics == null ? NoContent() : Ok(statistics);
        }

        // GET: api/Pipes/filter
        [Authorize]
        [HttpGet("filter")]
        public async Task<ActionResult<List<PipeResponse>>> FilterPipes(
            [FromQuery] Guid? steelGradeId,
            [FromQuery] bool? isGood,
            [FromQuery] decimal? minWeight,
            [FromQuery] decimal? maxWeight,
            [FromQuery] decimal? minLength,
            [FromQuery] decimal? maxLength,
            [FromQuery] decimal? minDiameter,
            [FromQuery] decimal? maxDiameter,
            [FromQuery] bool? notInPackage)
        {
            var filters = new PipeFilter
            {
                SteelGradeId = steelGradeId,
                IsGood = isGood,
                MinWeight = minWeight,
                MaxWeight = maxWeight,
                MinLength = minLength,
                MaxLength = maxLength,
                MinDiameter = minDiameter,
                MaxDiameter = maxDiameter,
                NotInPackage = notInPackage
            };

            var pipes = await _pipesService.FilterPipes(filters);

            if (!pipes.Any())
            {
                return NoContent();
            }

            var response = _mapper.Map<List<PipeResponse>>(pipes);
            return Ok(response);
        }

    }
}

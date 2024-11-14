using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PipeManager.Core.Abstractions;
using PipeManager.Core.Contracts.Requests;
using PipeManager.Core.Contracts.Responses;
using PipeManager.Core.Models;

namespace PipeManager.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PipesController : ControllerBase
    {
        private readonly IPipesService _pipesService;
        private readonly IMapper _mapper;

        public PipesController(IPipesService pipesService, IMapper mapper)
        {
            _pipesService = pipesService;
            _mapper = mapper;
        }

        // GET: api/Pipes
        [HttpGet]
        public async Task<ActionResult<List<PipeResponse>>> GetAllPipes()
        {
            var pipes = await _pipesService.GetAllPipes();
            var response = _mapper.Map<List<PipeResponse>>(pipes); // Преобразование списка Pipe в список PipeResponse
            return Ok(response);
        }

        // GET: api/Pipes/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<PipeResponse>> GetPipeById(Guid id)
        {
            try
            {
                var pipe = await _pipesService.GetPipeById(id);
                var response = _mapper.Map<PipeResponse>(pipe); // Преобразование Pipe в PipeResponse
                return Ok(response);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Pipe with ID {id} not found.");
            }
        }

        // POST: api/Pipes
        [HttpPost]
        public async Task<ActionResult<Guid>> CreatePipe([FromBody] PipeRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Маппинг PipeRequest на Pipe с добавлением сгенерированного Id
            var pipe = _mapper.Map<Pipe>(request);
            pipe.Id = Guid.NewGuid(); // Генерация нового Id для Pipe

            var pipeId = await _pipesService.CreatePipe(pipe);
            return CreatedAtAction(nameof(GetPipeById), new { id = pipeId }, pipeId);
        }

        // PUT: api/Pipes/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdatePipe(Guid id, [FromBody] PipeRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _pipesService.UpdatePipe(id, request.Label, request.IsGood, request.Diameter, request.Length, request.Weight, request.SteelGradeId, request.PackageId);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Pipe with ID {id} not found.");
            }
        }

        // DELETE: api/Pipes/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePipe(Guid id)
        {
            try
            {
                await _pipesService.DeletePipe(id);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Pipe with ID {id} not found.");
            }
        }

        // GET: api/Pipes/statistics
        [HttpGet("statistics")]
        public async Task<ActionResult<PipeStatistics>> GetStatistics()
        {
            var statistics = await _pipesService.GetStatistics();
            return Ok(statistics);
        }
    }
}
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
    public class SteelGradesController : ControllerBase
    {
        private readonly ISteelGradesService _steelGradesService;
        private readonly IMapper _mapper;

        public SteelGradesController(ISteelGradesService steelGradesService, IMapper mapper)
        {
            _steelGradesService = steelGradesService;
            _mapper = mapper;
        }

        // GET: api/SteelGrades
        [HttpGet]
        public async Task<ActionResult<List<SteelGradeResponse>>> GetAllSteelGrades()
        {
            var steelGrades = await _steelGradesService.GetAllSteelGrades();
            var response = _mapper.Map<List<SteelGradeResponse>>(steelGrades);
            return Ok(response);
        }

        // GET: api/SteelGrades/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<SteelGradeResponse>> GetSteelGradeById(Guid id)
        {
            try
            {
                var steelGrade = await _steelGradesService.GetSteelGradeById(id);
                var response = _mapper.Map<SteelGradeResponse>(steelGrade);
                return Ok(response);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Steel grade with ID {id} not found.");
            }
        }

        // POST: api/SteelGrades
        [HttpPost]
        public async Task<ActionResult<Guid>> CreateSteelGrade([FromBody] SteelGradeRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var steelGradeResult = SteelGrade.Create(Guid.NewGuid(), request.Name);
            if (!steelGradeResult.IsSuccess)
            {
                return BadRequest(steelGradeResult.Error);
            }

            var steelGradeId = await _steelGradesService.CreateSteelGrade(steelGradeResult.Value);
            return CreatedAtAction(nameof(GetSteelGradeById), new { id = steelGradeId }, steelGradeId);
        }

        // PUT: api/SteelGrades/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateSteelGrade(Guid id, [FromBody] SteelGradeRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // todo убрать create
            var steelGradeResult = SteelGrade.Create(id, request.Name);
            if (!steelGradeResult.IsSuccess)
            {
                return BadRequest(steelGradeResult.Error);
            }

            try
            {
                await _steelGradesService.UpdateSteelGrade(id, steelGradeResult.Value.Name);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Steel grade with ID {id} not found.");
            }
        }

        // DELETE: api/SteelGrades/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSteelGrade(Guid id)
        {
            try
            {
                await _steelGradesService.DeleteSteelGrade(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Steel grade with ID {id} not found.");
            }
        }
    }
}

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
            if (!steelGrades.Any())
            {
                return NoContent();
            }

            var response = _mapper.Map<List<SteelGradeResponse>>(steelGrades);
            return Ok(response);
        }

        // GET: api/SteelGrades/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<SteelGradeResponse>> GetSteelGradeById(Guid id)
        {
            var steelGrade = await _steelGradesService.GetSteelGradeById(id);
            var response = _mapper.Map<SteelGradeResponse>(steelGrade);
            return Ok(response);
        }

// POST: api/SteelGrades
        [HttpPost]
        public async Task<ActionResult<Guid>> CreateSteelGrade([FromBody] SteelGradeRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Используем AutoMapper для преобразования из SteelGradeRequest в SteelGrade
            var steelGrade = _mapper.Map<SteelGrade>(request);

            var steelGradeId = await _steelGradesService.CreateSteelGrade(steelGrade);
            return CreatedAtAction(nameof(GetSteelGradeById), new { id = steelGradeId }, steelGradeId);
        }


        // PUT: api/SteelGrades/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateSteelGrade(Guid id, [FromBody] SteelGradeRequest request)
        {
            await _steelGradesService.UpdateSteelGrade(id, request.Name);
            return NoContent();
        }

        // DELETE: api/SteelGrades/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSteelGrade(Guid id)
        {
            await _steelGradesService.DeleteSteelGrade(id);
            return NoContent();
        }
    }
}

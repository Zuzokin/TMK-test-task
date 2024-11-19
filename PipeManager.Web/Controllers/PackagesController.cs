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
    public class PackagesController : ControllerBase
    {
        private readonly IPackagesService _packagesService;
        private readonly IMapper _mapper;

        public PackagesController(IPackagesService packagesService, IMapper mapper)
        {
            _packagesService = packagesService;
            _mapper = mapper;
        }

        // GET: api/Packages
        [HttpGet]
        public async Task<ActionResult<List<PackageResponse>>> GetAllPackages()
        {
            var packages = await _packagesService.GetAllPackages();
            var response = _mapper.Map<List<PackageResponse>>(packages);
            return Ok(response);
        }

        // GET: api/Packages/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<PackageResponse>> GetPackageById(Guid id)
        {
            try
            {
                var package = await _packagesService.GetPackageById(id);
                var response = _mapper.Map<PackageResponse>(package);
                return Ok(response);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Package with ID {id} not found.");
            }
        }

        // POST: api/Packages
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Guid>> CreatePackage([FromBody] PackageRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var packageResult = Package.Create(Guid.NewGuid(), request.Number, request.Date);
            if (!packageResult.IsSuccess)
            {
                return BadRequest(packageResult.Error);
            }

            var package = packageResult.Value;

            var packageId = await _packagesService.CreatePackage(package);
            return CreatedAtAction(nameof(GetPackageById), new { id = packageId }, packageId);
        }

        // PUT: api/Packages/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdatePackage(Guid id, [FromBody] PackageRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var packageResult = Package.Create(id, request.Number, request.Date);
            if (!packageResult.IsSuccess)
            {
                return BadRequest(packageResult.Error);
            }

            var package = packageResult.Value;

            try
            {
                await _packagesService.UpdatePackage(package.Id, package.Number, package.Date);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Package with ID {id} not found.");
            }
        }


        // DELETE: api/Packages/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePackage(Guid id)
        {
            try
            {
                await _packagesService.DeletePackage(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Package with ID {id} not found.");
            }
        }
        
        [HttpPost("{packageId}/add-pipes")]
        public async Task<IActionResult> AddPipesToPackage(Guid packageId, [FromBody] AddPipesRequest request)
        {
            if (!request.PipeIds.Any())
            {
                return BadRequest("Pipe IDs are required.");
            }

            var result = await _packagesService.AddPipesToPackage(packageId, request.PipeIds);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }

            return Ok();
        }
        
        [HttpDelete("{packageId}/remove-pipe/{pipeId}")]
        public async Task<IActionResult> RemovePipeFromPackage(Guid packageId, Guid pipeId)
        {
            var result = await _packagesService.RemovePipeFromPackage(packageId, pipeId);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }

            return Ok();
        }


    }
}

using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PipeManager.Core.Abstractions;
using PipeManager.Core.Contracts.Requests;
using PipeManager.Core.Contracts.Responses;
using PipeManager.Core.Models;

namespace PipeManager.Web.Controllers;

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
    public async Task<IActionResult> GetAllPackages()
    {
        var packages = await _packagesService.GetAllPackages();
        if (!packages.Any())
        {
            return NoContent();
        }

        var response = _mapper.Map<List<PackageResponse>>(packages);
        return Ok(response);
    }

    // GET: api/Packages/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPackageById(Guid id)
    {
        if (id == Guid.Empty)
        {
            return BadRequest("Invalid package ID.");
        }

        var package = await _packagesService.GetPackageById(id);
        var response = _mapper.Map<PackageResponse>(package);

        return Ok(response);
    }

    // POST: api/Packages
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreatePackage([FromBody] PackageRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Используем AutoMapper для преобразования из PackageRequest в Package
        var package = _mapper.Map<Package>(request);

        var packageId = await _packagesService.CreatePackage(package);
        return CreatedAtAction(nameof(GetPackageById), new { id = packageId }, new { id = packageId });
    }


    // PUT: api/Packages/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePackage(Guid id, [FromBody] PackageRequest request)
    {
        if (id == Guid.Empty)
        {
            return BadRequest("Invalid package ID.");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var packageResult = Package.Create(id, request.Number, request.Date);
        if (!packageResult.IsSuccess)
        {
            return BadRequest(packageResult.Error);
        }

        await _packagesService.UpdatePackage(packageResult.Value.Id, packageResult.Value.Number, packageResult.Value.Date);
        return NoContent();
    }

    // DELETE: api/Packages/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletePackage(Guid id)
    {
        if (await _packagesService.HasPipes(id))
        {
            return BadRequest("Cannot delete a package that contains pipes.");
        }

        await _packagesService.DeletePackage(id);
        return NoContent();
    }


    // POST: api/Packages/{packageId}/add-pipes
    [HttpPost("{packageId}/add-pipes")]
    public async Task<IActionResult> AddPipesToPackage(Guid packageId, [FromBody] AddPipesRequest request)
    {
        if (packageId == Guid.Empty)
        {
            return BadRequest("Invalid package ID.");
        }

        if (request.PipeIds == null || !request.PipeIds.Any())
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

    // DELETE: api/Packages/{packageId}/remove-pipe/{pipeId}
    [HttpDelete("{packageId}/remove-pipe/{pipeId}")]
    public async Task<IActionResult> RemovePipeFromPackage(Guid packageId, Guid pipeId)
    {
        if (packageId == Guid.Empty || pipeId == Guid.Empty)
        {
            return BadRequest("Invalid package or pipe ID.");
        }

        var result = await _packagesService.RemovePipeFromPackage(packageId, pipeId);
        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }

        return Ok();
    }
    // GET: api/Packages/{packageId}/pipes
    [HttpGet("{packageId}/pipes")]
    public async Task<ActionResult<List<PipeResponse>>> GetPipesInPackage(Guid packageId)
    {
        if (packageId == Guid.Empty)
        {
            return BadRequest("Invalid package ID.");
        }

        var pipes = await _packagesService.GetPipesInPackage(packageId);

        if (pipes == null || !pipes.Any())
        {
            return NoContent(); // Возвращаем 204 No Content, если в пакете нет труб
        }

        var response = _mapper.Map<List<PipeResponse>>(pipes);
        return Ok(response);
    }

}

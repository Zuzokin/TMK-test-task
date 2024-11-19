using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PipeManager.Application.Services;
using PipeManager.Core.Abstractions;
using PipeManager.Core.Contracts.Requests;

namespace PipeManager.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    
    private readonly IUsersService _usersService;
    private readonly IMapper _mapper;

    public UsersController(IUsersService usersService, IMapper mapper)
    {
        _usersService = usersService;
        _mapper = mapper;
    }
    [HttpPost("api/Users/Register")]
    public async Task<ActionResult> Register(RegisterUserRequest request)
    {
        await _usersService.Register(request.Email, request.Password);

        return Ok();
    }

    [HttpPost("api/Users/Login")]
    public async Task<ActionResult> Login(LoginUserRequest request)
    {
        var token = await _usersService.Login(request.Email, request.Password);

        HttpContext.Response.Cookies.Append("tasty-cookies", token);

        return Ok();
    }
}
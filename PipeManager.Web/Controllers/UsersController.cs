using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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

    // POST: api/Users/register
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
    {
        // Проверка валидации модели
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await _usersService.Register(request.Email, request.Password);
        return Ok(new { Message = "Registration successful." });
    }

    // POST: api/Users/login
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserRequest request)
    {
        // Проверка валидации модели
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var token = await _usersService.Login(request.Email, request.Password);

        // Добавление JWT токена в куки
        // HttpContext.Response.Cookies.Append("auth-token", token, new CookieOptions
        // {
        //     HttpOnly = true,
        //     Secure = true,
        //     SameSite = SameSiteMode.Strict
        // });        
        HttpContext.Response.Cookies.Append("tasty-cookies", token);

        return Ok(new { Message = "Login successful." });
    }
}
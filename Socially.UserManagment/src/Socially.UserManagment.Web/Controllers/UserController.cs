using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Socially.UserManagment.Core.RefreshTokenAggregate;
using Socially.UserManagment.Core.UserAggregate.Specifications;
using Socially.UserManagment.Infrastructure.CookieManagment;
using Socially.UserManagment.Shared.Config.JWT;
using Socially.UserManagment.UseCases.Users.ChangePassword;
using Socially.UserManagment.UseCases.Users.ChangePasswordForget;
using Socially.UserManagment.UseCases.Users.Common.DTOs;
using Socially.UserManagment.UseCases.Users.ForgetPassword;
using Socially.UserManagment.UseCases.Users.Get;
using Socially.UserManagment.UseCases.Users.Login;
using Socially.UserManagment.UseCases.Users.Register;
using Socially.UserManagment.UseCases.Users.Update;
using Socially.UserManagment.UseCases.Users.Verify;

namespace Socially.UserManagment.Web.Users;
[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
  private readonly IMediator _mediator;
  private readonly IOptionsMonitor<JWTSettings> _jwtSettings;
  private readonly ICookieService _cookieService;

  public UserController(IMediator mediator, IOptionsMonitor<JWTSettings> jwtSettings, ICookieService cookieService)
  {
    _mediator = mediator;
    _jwtSettings = jwtSettings;
    _cookieService = cookieService;
  }
  [HttpPost("register")]
  public async Task<ActionResult<Guid>> Register(UserRegistrationDto newUser)
  {
    var command = new RegisterUserCommand(newUser);
    var result = await _mediator.Send(command);
    if(result.IsSuccess)
    {
      return CreatedAtAction(nameof(Register), new {id = result.Value}, result.Value);
    }
    return BadRequest(result.Errors);
  }


  [HttpPost("login")]
  public async Task<ActionResult<string>> Login(UserLoginDto user)
  {
    var command = new LoginCommand(user);
    var result = await _mediator.Send(command);
    if (result.IsSuccess)
    {
      _cookieService.SetCookie("RefreshToken", result.Value.RefreshToken, _jwtSettings.CurrentValue.RefreshTokenExpiryDays);
      return Ok(result.Value.AccessToken);
    }
    return BadRequest(result.Errors);
  }

  [HttpPatch("")]
  [Authorize]
  public async Task<ActionResult<UserUpdateDto>> UpdateUser(UserUpdateDto user)
  {
    var id = Guid.Parse(User.Identity!.Name!);
    var command = new UpdateUserCommand(id, user);
    var result = await _mediator.Send(command);
    if (result.IsSuccess)
    {
      return Ok(result.Value);
    }
    return BadRequest(result.Errors);
  }

  [HttpGet("")]
  [Authorize]
  public async Task<ActionResult<UserDto>> Get()
  {
    var id = Guid.Parse(User.Identity!.Name!);
    var query = new GetUserQuery(id);
    var result = await _mediator.Send(query);
    if (result.IsSuccess)
    {
      return Ok(result.Value);
    }
    return BadRequest(result.Errors);
  }

  [HttpGet("verify/{token}")]
  public async Task<IActionResult> VerifyEmail([FromRoute,Required] string token)
  {
    var command = new VerifyCommand(token);
    var result = await _mediator.Send(command);

    if (result.IsSuccess)
    {
      return Ok();
    }

    return BadRequest("Token is Invalid");
  }

  [HttpGet("changepassword")]
  [Authorize]
  public async Task<IActionResult> ChangePassowrd(ChangePasswordDto passwords)
  {
    var id = Guid.Parse(User.Identity!.Name!);
    var command = new ChangePasswordCommand(id, passwords);
    var result = await _mediator.Send(command);
    if (result.IsSuccess)
    {
      return Ok();
    }
    return BadRequest("Current Password is Wrong!");
  }

  [HttpGet("forgetpassword")]
  public async Task<IActionResult> ForgetPassword([FromBody]string Email)
  {
    var command = new ForgetPasswordCommand(Email);
    var result = await _mediator.Send(command);
    if (result.IsSuccess)
    {
      return Ok();
    }
    return BadRequest("This Email Doesn't Belong to a User");
  }

  [HttpPost("forgetpassword/{token}")]
  public async Task<IActionResult> ChangeForgetPassword([FromRoute]string token,  [FromBody]string password)
  {
    var command = new ChangeForgetPasswordCommand(token, password);
    var result = await _mediator.Send(command);
    if (result.IsSuccess)
    {
      _cookieService.SetCookie("RefreshToken", result.Value.RefreshToken, _jwtSettings.CurrentValue.RefreshTokenExpiryDays);
      return Ok(result.Value.AccessToken);
    }
    return BadRequest(result.Errors);
  }
  }

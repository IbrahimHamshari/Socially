using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Socially.ContentManagment.Infrastructure.CookieManagment;
using Socially.ContentManagment.Shared.Config.JWT;
using Socially.ContentManagment.UseCases.Users.ChangePassword;
using Socially.ContentManagment.UseCases.Users.ChangePasswordForget;
using Socially.ContentManagment.UseCases.Users.Common.DTOs;
using Socially.ContentManagment.UseCases.Users.ForgetPassword;
using Socially.ContentManagment.UseCases.Users.Get;
using Socially.ContentManagment.UseCases.Users.Login;
using Socially.ContentManagment.UseCases.Users.Register;
using Socially.ContentManagment.UseCases.Users.RequestVerification;
using Socially.ContentManagment.UseCases.Users.Update;
using Socially.ContentManagment.UseCases.Users.UploadCoverImage;
using Socially.ContentManagment.UseCases.Users.UploadProfilePicture;
using Socially.ContentManagment.UseCases.Users.Verify;
using Socially.ContentManagment.Web.Extensions;

namespace Socially.ContentManagment.Web.Users;

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
    if (result.IsSuccess)
    {
      return CreatedAtAction(nameof(Register), new { id = result.Value }, result.Value);
    }

    return BadRequest(result.ToProblemDetails());
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
    return BadRequest(result.ToProblemDetails());
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
    return BadRequest(result.ToProblemDetails());
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
    return BadRequest(result.ToProblemDetails());
  }

  [HttpGet("verify/{token}")]
  public async Task<IActionResult> VerifyEmail([FromRoute, Required] string token)
  {
    var command = new VerifyCommand(token);
    var result = await _mediator.Send(command);

    if (result.IsSuccess)
    {
      return Ok();
    }

    return BadRequest(result.ToProblemDetails());
  }

  [HttpPost("changepassword")]
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
    return BadRequest(result.ToProblemDetails());
  }

  [HttpPost("forgetpassword")]
  public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordRequest request)
  {
    var command = new ForgetPasswordCommand(request.Email);
    var result = await _mediator.Send(command);
    if (result.IsSuccess)
    {
      return Ok();
    }
    return BadRequest(result.ToProblemDetails());
  }

  [HttpPost("forgetpassword/{token}")]
  public async Task<IActionResult> ChangeForgetPassword([FromRoute] string token, [FromBody] ChangeForgetPasswordRequest request)
  {
    var command = new ChangeForgetPasswordCommand(token, request.Password);
    var result = await _mediator.Send(command);
    if (result.IsSuccess)
    {
      _cookieService.SetCookie("RefreshToken", result.Value.RefreshToken, _jwtSettings.CurrentValue.RefreshTokenExpiryDays);
      return Ok(result.Value.AccessToken);
    }
    return BadRequest(result.ToProblemDetails());
  }

  [HttpGet("verify")]
  [Authorize]
  public async Task<IActionResult> RequestEmailVerification()
  {
    var id = Guid.Parse(User.Identity!.Name!);
    var command = new RequestVerificationCommand(id);
    var result = await _mediator.Send(command);
    if (result.IsSuccess)
    {
      return Ok();
    }
    return BadRequest(result.ToProblemDetails());
  }

  [HttpPatch("coverimage")]
  [Authorize]
  public async Task<IActionResult> UploadCoverImage(IFormFile file)
  {
    var id = Guid.Parse(User.Identity!.Name!);
    var command = new UploadCoverImageCommand(file, id);
    var result = await _mediator.Send(command);
    if (result.IsSuccess)
    {
      return Ok(result.Value);
    }
    return BadRequest(result.ToProblemDetails());
  }


  [HttpPatch("profilepicture")]
  [Authorize]
  public async Task<IActionResult> UploadProfilePicture(IFormFile file)
  {
    var id = Guid.Parse(User.Identity!.Name!);
    var command = new UploadProfilePictureCommand(file, id);
    var result = await _mediator.Send(command);
    if (result.IsSuccess)
    {
      return Ok(result.Value);
    }
    return BadRequest(result.ToProblemDetails());
  }
}

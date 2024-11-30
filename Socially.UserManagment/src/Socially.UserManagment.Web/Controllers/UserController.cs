using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Socially.UserManagment.UseCases.Users.ChangePassword;
using Socially.UserManagment.UseCases.Users.ChangePasswordForget;
using Socially.UserManagment.UseCases.Users.Common.DTOs;
using Socially.UserManagment.UseCases.Users.ForgetPassword;
using Socially.UserManagment.UseCases.Users.Get;
using Socially.UserManagment.UseCases.Users.Login;
using Socially.UserManagment.UseCases.Users.Register;
using Socially.UserManagment.UseCases.Users.RequestVerification;
using Socially.UserManagment.UseCases.Users.Update;
using Socially.UserManagment.UseCases.Users.UploadCoverImage;
using Socially.UserManagment.UseCases.Users.UploadProfilePicture;
using Socially.UserManagment.UseCases.Users.Verify;
using Socially.UserManagment.Web.Extensions;
using Socially.SharedKernel.Config.JWT;
using Socially.UserManagment.UseCases.Users.FollowUser;
using Socially.UserManagment.UseCases.Users.Refresh;
using SharedKernel.CookieManagment;

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
  [HttpPost("follow")]
  [Authorize]
  public async Task<IActionResult> TriggerFollow(Guid followedId)
  {
    var followerId = Guid.Parse(User.Identity!.Name!);
    var command = new TriggerFollowUserCommand(followerId, followedId);
    var result = await _mediator.Send(command);
    if (result.IsSuccess)
    {
      return Ok();
    }
    return BadRequest(result.ToProblemDetails());
  }

  [HttpGet("refresh")]
  public async Task<IActionResult> RefreshToken(string URL)
  {
    var refreshToken = _cookieService.GetCookie("RefreshToken");
    if(refreshToken == null)
    {
      return Unauthorized();
    }
    var refreshCommand = new RefreshCommand(refreshToken);
    var result = await _mediator.Send(refreshCommand);
    if(result.IsSuccess)
    {
      _cookieService.SetCookie("RefreshToken", result.Value[1], _jwtSettings.CurrentValue.RefreshTokenExpiryDays);
      return Ok(result.Value[0]);
    }
    return Unauthorized();
  }
}

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Socially.Messaging.UseCases.Messages.Common.DTOs;
using Socially.Messaging.UseCases.Messages.MarkMessageAsRead;
using Socially.Messaging.UseCases.Messages.Read;
using Socially.Messaging.UseCases.Messages.Send;

namespace Socially.Messaging.Web.Controllers;
[Route("api/[controller]")]
[ApiController]
public class MessagesController : ControllerBase
{
  private IMediator _mediator;

  public MessagesController(IMediator mediator)
  {
    _mediator = mediator;
  }


  [HttpPost]
  [Authorize]
  public async Task<IActionResult> SendMessage(SendMessageDto request)
  {
    var id = Guid.Parse(User.Identity!.Name!);
    var command = new SendMessageCommand(request, id);
    var messageId = await _mediator.Send(command);
    return Ok(new { MessageId = messageId });
  }


  [HttpGet("conversation")]
  [Authorize]
  public async Task<IActionResult> GetChatHistory(Guid userId2)
  {
    var id = Guid.Parse(User.Identity!.Name!);
    var query = new ReadMessageQuery(id, userId2);
    var chatHistory = await _mediator.Send(query);
    return Ok(chatHistory);
  }


  [HttpPut("{id}/read")]
  public async Task<IActionResult> MarkMessageAsRead(Guid id)
  {
    var command = new MarkMessageAsReadCommand(id);
    await _mediator.Send(command);
    return NoContent();
  }


  //[HttpPut("{id}/delivered")]
  //public async Task<IActionResult> MarkMessageAsDelivered(Guid id)
  //{
  //  var command = new MarkMessageAsDeliveredCommand(id);
  //  await _mediator.Send(command);
  //  return NoContent();
  //}
}


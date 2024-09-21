using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.UserManagment.UseCases.Users.Common.DTOs;
public class UserUpdateDto
{
  public  Guid? Id { get; init; }
  public  string? Email { get; init; }  
  public  string? FirstName { get; init; }  
  public  string? LastName { get; init; }  
  public  string? Bio { get; init; }
  public string? ProfilePictureURL { get; init; }  
  public string? CoverPhotoURL { get; init; }  
  public DateTimeOffset? DateOfBirth { get; init; }  
  public bool? Gender { get; init; } 
}

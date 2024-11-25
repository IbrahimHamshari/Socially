using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Socially.UserManagment.Core.UserAggregate;
using Socially.UserManagment.Core.UserAggregate.Errors;
using Socially.UserManagment.Core.UserAggregate.Specifications;

namespace Socially.UserManagment.UseCases.Users.FollowUser;
public class TriggerFollowUserCommandHandler(IRepository<User> _userRepository) : ICommandHandler<TriggerFollowUserCommand, Result>
{
  public async Task<Result> Handle(TriggerFollowUserCommand request, CancellationToken cancellationToken)
  {
    var followerId = request.FollowerId;
    var followedId = request.FollowedId;

    var followerSpec = new UserByIdSpec(followerId);
    var followedSpec = new UserByIdSpec(followedId);

    var follower = await _userRepository.SingleOrDefaultAsync(followerSpec, cancellationToken);
    if( follower == null)
    {
      return UserErrors.NotFound(followerId);
    }
    var followed = await _userRepository.SingleOrDefaultAsync(followedSpec, cancellationToken);
    if(followed == null)
    {
      return UserErrors.NotFound(followedId);
    }

    follower.TriggerConnection(followed);

    await _userRepository.SaveChangesAsync(cancellationToken);

    return Result.Success();
  }
}

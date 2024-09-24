using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Socially.UserManagment.UseCases.Users.RequestVerification;
public record RequestVerificationCommand(Guid Id) : ICommand<Result>;

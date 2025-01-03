using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleBookingApp.Core.CommandHandler;

public class CommandHandlerResult
{
    public bool Success { get; set; }
    public required string Message { get; set; }
    public Action? PostResultAction { get; set; }
}
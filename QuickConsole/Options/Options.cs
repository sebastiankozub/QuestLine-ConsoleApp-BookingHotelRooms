using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace QuickConsole.Options;


public class UserInterfaceOptions
{
    public const string UserInterfaceSegmentName = "UserInterface";

    [Required]
    [MinLength(1)]
    public required string CommandPrompt { get; set; }

    [Required]
    [ValidateObjectMembers]
    public required UserInterfaceCommandsOptions CommandsAliases { get; set; }
}

public class UserInterfaceCommandsOptions
{
    public const string CommandsSegmentName = UserInterfaceOptions.UserInterfaceSegmentName + ":CommandsAliases";

    [MinLength(3)]
    public required string Help { get; set; }

    [MinLength(3)]
    public required string Exit { get; set; }

    [MinLength(3)]
    public required string Search { get; set; }

    [Required]
    [MinLength(3)]
    public required string Availability { get; set; }
}


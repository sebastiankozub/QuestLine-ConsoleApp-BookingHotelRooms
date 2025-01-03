using System.ComponentModel.DataAnnotations;

namespace ConsoleBookingApp.Configuration;

public class MyFirstClass
{
    public const string MyFirstClassSegmentName = "FirstOptions";

    public required string Option1 { get; set; }
    public int Option2 { get; set; }
}

public class SecondOptions
{
    public const string SecondSegmentName = "SecondOptions";

    public required string SettingOne { get; set; }
    public int SettingTwo { get; set; }
}

public class UserInterfaceOptions
{
    public const string UserInterfaceSegmentName = "UserInterface";

    [Required]
    [MinLength(1)]
    public required string CommandPrompt { get; set; }
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


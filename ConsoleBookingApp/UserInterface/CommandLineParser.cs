using System;
using System.Text.RegularExpressions;

namespace ConsoleBookingApp.UserInterface;

public interface ICommandLineParser
{
    (string? CommandName, string[] Parameters) Parse(string command);
}

public class CommandLineParser : ICommandLineParser
{
    public (string? CommandName, string[] Parameters) Parse(string command)
    {
        string pattern = @"^(\w+)\(([^)]*)\)$";
        Match match = Regex.Match(command.Trim(), pattern);

        if (match.Success)
        {
            string commandName = match.Groups[1].Value;
            string parametersString = match.Groups[2].Value;
            string[] parameters = parametersString.Split(',', StringSplitOptions.RemoveEmptyEntries);
            return (commandName, parameters);
        }
        else
        {
            return (null, []);
        }
    }
}


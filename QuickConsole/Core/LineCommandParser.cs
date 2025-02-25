using System.Text.RegularExpressions;

namespace QuickConsole.Core;

internal interface ILineCommandParser
{
    (string CommandName, string[] Parameters) Parse(string command);
}

internal class LineCommandParser : ILineCommandParser
{
    public (string CommandName, string[] Parameters) Parse(string command)
    {
        //@"^(\w+)\s*\(([^)]*)\)$";
        var pattern = @"^(\w+)\s*\(([^)]*)?\)$";
        Match match = Regex.Match(command.Trim(), pattern);

        if (match.Success)
        {
            string commandName = match.Groups[1].Value.Trim();

            string parametersString = match.Groups[2].Value;
            string[] parameters = parametersString.Split(',').Select(p => p.Trim()).ToArray();

            return (commandName, parameters);
        }
        else
        {
            return ("", []);
        }
    }
}


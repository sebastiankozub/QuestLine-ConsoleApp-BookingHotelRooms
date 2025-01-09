using System.Collections.ObjectModel;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;

namespace QuickConsole.RunCommand;

public interface IQuickRunCommandnArgsParser
{
    Dictionary<string, string> Parse();
    //private void Parse(string command);
    IList<string> GetAllArgs();
    IList<string> GetSwitches(string[] switchList);
    IList<string> GetSwitchParams(string runArgSwitch);
}


public class RunCommandArgsParser : IQuickRunCommandnArgsParser
{
    private readonly string _defaultSwitchStr = "--";
    private readonly string _runSwitch;
    private readonly string[] _switches = ["--", "-", "*"];
    private readonly string[] _runArgs = [];



    public RunCommandArgsParser(QuickRunCommandArgs runCommandArgs, string? switchStr)
    {
        if (switchStr != null)
            _runSwitch = switchStr;
        else
            _runSwitch = _defaultSwitchStr;

        _runArgs = runCommandArgs.args;


        //obc.Select(obc => obc.StartsWith("--")).ToList();
        //int index = obc.IndexOf(qwe.FirstOrDefault(x => x.StartsWith("--")));
    }



    public Dictionary<string, string> Parse()
    {

        var obc = new List<string>() { "--code5", "-OK-234", "fggffg", "--code2OK", "-234", "ffg", "fggf", "--code7OK", "234", "ggfgfgf" };
        var obc2 = new List<string>() { "--code5", "OK-234", "-fggffg", "--code2OK", "-234", "ffg", "fggf", "--code7OK", "234", "fggfgfgf" };


        var swichesList = obc.Where(x => x.StartsWith("--")).ToList();

        var result = obc
            .Select((item, index) => new { item, index })
            .Where(item => swichesList.Any(s => item.item == s))
            .Select(tmp => new { tmp.index, tmp.item }).ToDictionary(x => x.index.ToString(), x => x.item);


        var result2 = obc2
            .Select((item, index) => new { Item = item, Index = index })
            .Where(x => x.Item.StartsWith("--"))
            .Select(x => new
            {
                Switch = x.Item.TrimStart('-'),
                Parameters = obc2
                    .Skip(x.Index + 1)
                    .TakeWhile(item => !item.StartsWith("--"))
                    .ToArray()
            })
            .ToDictionary(x => x.Switch, x => x.Parameters);

        return result;
    }

    public IList<string> GetAllArgs()
    {
        throw new NotImplementedException();
    }

    public IList<string> GetSwitches(string[] switchList)
    {
        throw new NotImplementedException();
    }

    public IList<string> GetSwitchParams(string runArgSwitch)
    {
        throw new NotImplementedException();
    }
}


using QuickConsole;
using System.Collections.ObjectModel;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;

namespace QuickConsole;

public interface IQuickRunCommandnArgsParser
{
    Dictionary<string, string> Parse();
    //private void Parse(string command);
}


public class QuickRunCommandArgsParser : IQuickRunCommandnArgsParser
{
    private readonly string _defaultSwitchStr = "--";
    private readonly string _runSwitch;
    private readonly string[] _switches = ["--", "-", "*"];
    private readonly string[] _runArgs = [];



    public QuickRunCommandArgsParser(QuickRunCommandArgs runCommandArgs, string? switchStr)
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

        var obc = new ObservableCollection<string>() { "--code5", "OK-234", "fggffg", "--code2OK", "-234", "ffg", "fggf", "--code7OK", "-234", "-fggfgfgf" };

        var swichesList = obc.Where(x => x.StartsWith("--")).ToList();

        var result = obc
            .Select((item, index) => new { item, index })
            .Where(item => swichesList.Any(s => item.item == s))
            .Select(tmp => new { tmp.index, tmp.item }).ToDictionary(x => x.index.ToString(), x => x.item);

        return result;
    }
}


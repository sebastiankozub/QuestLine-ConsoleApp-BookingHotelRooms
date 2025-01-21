using System.Text.RegularExpressions;

namespace QuickConsole.Core;

interface IQuickHandlerNameResolver
{
    string Resolve(string command);
}

internal class HandlerNameResolver(QuickHandlerAliasStore quickHandlerAliasStore) : IQuickHandlerNameResolver
{
    private readonly QuickHandlerAliasStore _quickHandlerAliasStore = quickHandlerAliasStore;
    public string Resolve(string command)
    {

        return "CommandName";
    }
}

interface IQuickHandlerAliasStore;

internal class QuickHandlerAliasStore
{
    public readonly Dictionary<string, string> _aliasStore = new();




}

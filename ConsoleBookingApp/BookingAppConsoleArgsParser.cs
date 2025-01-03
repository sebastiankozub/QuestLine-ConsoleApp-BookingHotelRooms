using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleBookingApp;

internal class BookingAppConsoleArgsParser
{
    private readonly string[] _args;

    public BookingAppConsoleArgsParser(BookingAppConsoleArgsParserOptions options)
    {
        if(options.args == null || options.args.Length < 4)
            throw new ArgumentException("Command line arguments not satisfied. Run app with proper --bookings and --hotels parameters' values.");

        _args = options.args;
    }

    public (string HotelsFilename, string BookingsFilname) Parse()
    {
        var hotelRepositoryFilename = GetParameterValue("hotels");
        var bookingRepositoryFilename = GetParameterValue("bookings");

        if (hotelRepositoryFilename is null || bookingRepositoryFilename is null)
            throw new ArgumentException("One or more underlying datafile names not given properly.");

        return (hotelRepositoryFilename, bookingRepositoryFilename);
    }

    private string? GetParameterValue(string key)
    {
        int index = Array.IndexOf(_args, "--" + key);

        if (index >= 0 && _args.Length > index)            
            return _args[index + 1];

        return null;
    }    
}

internal class BookingAppConsoleArgsParserOptions
{
    public string[] args = [];
}

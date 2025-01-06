

namespace ConsoleBookingApp.CommandHandler;

public class AvailabilityCommandValidator
{
    public static (string hotelId, (DateOnly from, DateOnly to) availabitlityPeriod, string roomType) Parse(string[] parameters)
    {
        parameters = parameters.Select(p => p.Trim()).ToArray();

        if (parameters.Any(p => string.IsNullOrEmpty(p)))
            throw new AvailabilityCommandHandlerParseException();

        if (parameters.Length != 3)
            throw new AvailabilityCommandHandlerParseException();

        var hotelId = parameters[0];
        var roomType = parameters[2];

        var date = parameters[1];

        DateOnly from;
        DateOnly to;

        if (date.Length == 8)
        {
            from = to = DateOnly.TryParseExact(date, "yyyyMMdd", out var dateOnly) ? dateOnly : throw new AvailabilityCommandHandlerParseException();
        }
        if (date.Length >= 17)
        {
            var dateRange = date.Split('-').Select(x => x.Trim()).ToArray();

            if (dateRange.Length != 2 || dateRange.Any(date => date.Length != 8))
                throw new AvailabilityCommandHandlerParseException();

            from = DateOnly.TryParseExact(dateRange[0], "yyyyMMdd", out var dateOnlyFrom) ? dateOnlyFrom : throw new AvailabilityCommandHandlerParseException();
            to = DateOnly.TryParseExact(dateRange[1], "yyyyMMdd", out var dateOnlyTo) ? dateOnlyTo : throw new AvailabilityCommandHandlerParseException();
        }
        else
            throw new AvailabilityCommandHandlerParseException();

        if (from > to)
            (from, to) = (to, from);

        return new(hotelId, (from, to), roomType);
    }

    public  static bool Validate(string hotelId, (DateOnly from, DateOnly to) availabilityPerdiod, string roomType)
    {
        if (hotelId.Length > 10 || hotelId.Length < 2)
            throw new AvailabilityCommandHandlerValidateException();

        if (roomType.Length > 10 || roomType.Length < 2)
            throw new AvailabilityCommandHandlerValidateException();

        //if (availabilityPerdiod.from < DateOnly.FromDateTime(DateTime.UtcNow).AddDays(1) || let search for historical data
        if (availabilityPerdiod.to < availabilityPerdiod.from)
            throw new AvailabilityCommandHandlerValidateException();

        return true;
    }
}


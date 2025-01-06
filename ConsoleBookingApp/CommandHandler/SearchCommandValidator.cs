namespace ConsoleBookingApp.CommandHandler;

public class SearchCommandValidator
{
    public static bool Validate(string hotelId, (DateOnly from, DateOnly to) availabilityPerdiod, string roomType)
    {
        if (hotelId.Length > 10 || hotelId.Length < 2)
            throw new SearchCommandHandlerValidateException();

        if (roomType.Length > 10 || roomType.Length < 2)
            throw new SearchCommandHandlerValidateException();

        if (availabilityPerdiod.to < availabilityPerdiod.from)
            throw new SearchCommandHandlerValidateException();

        if (availabilityPerdiod.from < GetTommorowUtcDateOnly())
            throw new SearchCommandHandlerValidateException();

        return true;
    }

    public static (string hotelId, (DateOnly from, DateOnly to) availabitlityPeriod, string roomType) Parse(string[] parameters)
    {
        parameters = parameters.Select(p => p.Trim()).ToArray();

        if (parameters.Any(p => string.IsNullOrEmpty(p)))
            throw new SearchCommandHandlerParseException();

        if (parameters.Length != 3)
            throw new SearchCommandHandlerParseException();

        var hotelId = parameters[0];
        var roomType = parameters[2];

        var days = parameters[1];

        if (string.IsNullOrEmpty(days))
            throw new SearchCommandHandlerParseException();

        if (days.Length >= 5)
            throw new SearchCommandHandlerParseException();

        var numberOfDaysAhead = int.TryParse(days, out var n) ? n : throw new SearchCommandHandlerParseException();

        if (numberOfDaysAhead < 1)
            throw new SearchCommandHandlerParseException("You cannot search for a room availability in the past. Use positive number to number of days ahead or use {Availaility} command.");

        DateOnly from = GetTommorowUtcDateOnly();
        DateOnly to = from.AddDays(numberOfDaysAhead - 1);

        if (from > to)
            (from, to) = (to, from);

        return new(hotelId, (from, to), roomType);
    }

    public static DateOnly GetTommorowUtcDateOnly()
    {
        return DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1));
    }

}
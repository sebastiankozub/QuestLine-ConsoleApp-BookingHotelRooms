//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Microsoft.Extensions.DependencyInjection;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.Linq;
//using System.Text.RegularExpressions;
//using System.Threading.Tasks;


//// Data Transfer Objects (DTOs) and Validation
//public class AvailabilityRequest
//{
//    [Required]
//    public string HotelCode { get; set; }
//    [Required]
//    [RegularExpression(@"^\d{8}(-\d{8})?$", ErrorMessage = "Invalid date format (YYYYMMDD or YYYYMMDD-YYYYMMDD).")]
//    public string DateRange { get; set; }
//    [Required]
//    public string RoomType { get; set; }

//    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
//    {
//        if (DateRange.Contains('-'))
//        {
//            var dates = DateRange.Split('-');
//            if (int.Parse(dates[0]) > int.Parse(dates[1]))
//            {
//                yield return new ValidationResult("Start date cannot be after end date.", new[] { nameof(DateRange) });
//            }
//        }
//    }
//}

//public class AvailabilityResult
//{
//    public bool IsAvailable { get; set; }
//    public decimal Price { get; set; }
//    public string Message { get; set; }
//}

//// CQRS Interfaces
//public interface ICommandLineHandler<TCommand> where TCommand : class
//{
//    Task HandleAsync(TCommand command);
//}

//public interface IQueryHandler<TQuery, TResult> where TQuery : class where TResult : class
//{
//    Task<TResult> HandleAsync(TQuery query);
//}

//// Business Logic (CQRS Handlers)
//public class AvailabilityCommandLineHandler : ICommandLineHandler<AvailabilityRequest>
//{
//    private readonly IAvailabilityService _availabilityService;

//    public AvailabilityCommandLineHandler(IAvailabilityService availabilityService)
//    {
//        _availabilityService = availabilityService;
//    }

//    public async Task HandleAsync(AvailabilityRequest request)
//    {
//        var results = await _availabilityService.GetAvailabilityAsync(request);
//        if (results.IsAvailable)
//        {
//            Console.WriteLine($"Room is available. Price: {results.Price}");
//        }
//        else
//        {
//            Console.WriteLine(results.Message);
//        }
//    }
//}

//public interface IAvailabilityService
//{
//    Task<AvailabilityResult> GetAvailabilityAsync(AvailabilityRequest request);
//}

//public class AvailabilityService : IAvailabilityService
//{
//    public async Task<AvailabilityResult> GetAvailabilityAsync(AvailabilityRequest request)
//    {
//        // Simulate business logic (replace with your actual logic)
//        await Task.Delay(500); // Simulate some work
//        if (request.RoomType == "SGL")
//        {
//            return new AvailabilityResult { IsAvailable = true, Price = 100, Message = null };
//        }
//        else
//        {
//            return new AvailabilityResult { IsAvailable = false, Message = "Room not available." };
//        }
//    }
//}

//// Command Line Handling (Adapters)
//public class AvailabilityCommandLineHandler : ICommandLineHandler
//{
//    private readonly ICommandLineHandler<AvailabilityRequest> _availabilityCommandHandler;
//    private readonly ICommandLineParser _parser;

//    public AvailabilityCommandLineHandler(ICommandLineHandler<AvailabilityRequest> availabilityCommandHandler, ICommandLineParser parser)
//    {
//        _availabilityCommandHandler = availabilityCommandHandler;
//        _parser = parser;
//    }

//    public string CommandName => "Availability";

//    public async Task HandleAsync(string[] parameters)
//    {
//        if (parameters.Length != 3)
//        {
//            Console.WriteLine("Invalid number of parameters for Availability command.");
//            return;
//        }

//        var request = new AvailabilityRequest
//        {
//            HotelCode = parameters[0],
//            DateRange = parameters[1],
//            RoomType = parameters[2]
//        };

//        var validationResults = new List<ValidationResult>();
//        Validator.TryValidateObject(request, new ValidationContext(request), validationResults, true);

//        if (validationResults.Any())
//        {
//            foreach (var error in validationResults)
//            {
//                Console.WriteLine(error.ErrorMessage);
//            }
//            return;
//        }

//        await _availabilityCommandHandler.HandleAsync(request);
//    }
//}

//// ... (Other classes: ICommandLineParser, CommandParser, ExitCommandLineHandler, HelpCommandHandler, CommandLineProcessor, Program – same as before, but adapt the registration of the Availability handler)

//public class Program
//{
//    public static async Task Main(string[] args)
//    {
//        // ... (commandDescriptions and commandHandlers dictionaries - same as before)

//        var serviceCollection = new ServiceCollection()
//            .AddSingleton<ICommandLineParser, CommandParser>()
//            .AddSingleton(commandDescriptions)
//            .AddTransient<IAvailabilityService, AvailabilityService>()
//            .AddTransient<ICommandLineHandler<AvailabilityRequest>, AvailabilityCommandLineHandler>()
//            .AddTransient<ICommandLineHandler, AvailabilityCommandLineHandler>() // Register the command line handler
//            .AddTransient<CommandLineProcessor>();

//        // ... (Dynamic Handler Registration and building the service provider)
//        var serviceProvider = serviceCollection.BuildServiceProvider();
//        // ... (rest of the Main method is the same)
//    }
//}
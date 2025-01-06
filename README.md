Console application for booking a hotel room
--------------------------------------------

it was started as a simple console app to book a hotel room but...
after a day of coding...
I started creating reusable console app framework library
so the project is a booking app only to test
a road to have nice framework for console apps - atm in progress and lot of ideas to fullfil :) 

Architecture
------------

Layered Architecture:
    
1. DataContext - works nice but should be splitted and data persistance logic extracted to IDataStorage interface and JsonFileDataStorage class
                 should be injected only to IBookingAppService but intermediate state force me to inject it also into SaveCommandHandler
2. BookingApp - Business Logic - core logic that uses data layer to fulfill business needs 
3. Command Handler - connecting text ui with business logic - eg. changing data format between ui and core logic - at some point splited into command & query cqrs for cleaner code of data transformation
4. Command Line Processor - parsing command line input and triggering command handler or returning to User Interface
5. User Interface  - at the moment too strongly coupled views geneartion with ui logic and app entry point logic

There is room (or need) for more layers - for example:
1. JsonFileDataStorage - implementuing abstraction above data storage IDataStorage to be used by DataContext: Save() and Inititialize()
2. UserInterace - put something BookingAppConsoleInterface and CommandLineProcessor - resolve mix in User Interface layer -> spliting ui views geneartion and app logic
3. EntryPoint, Configuration, DI - split the responsibilities more & move some to class library projects to avoid dependencies eg. registering extension method AddDataContext()
4. View gereation engine


TODO
----

0. business logic in services and handlers - DONE

1. unit testing of business logic - DONE  
2. handler parsing and validation - DONE  
   improve to return Validation/ParsingResult or by abstract method and get rid quickly implemented static method approach)

3. add ILogger

4!. Generic CommandHandler\<T\> abstract class typed by collection returned from service - much cleaner output formating by FormatOutput<T>() abstract method

4. CommandLineHandlers:
- auto registration when new class added to namespace - DONE
- change to ICommandHandler - in this layer code cannot have anything common with command line console - DONE
- generic ones like: search, availability - collect into collection at the begining of the app - REFACTOR different approach to di configuration
- easier/cleaner code to maintain in few areas then

Needed?
- CommandHandlerData - inherited to create common base imput poco parameter for Handle(CommandLineHandlerData commandData) method

5. CONFIG JSON + Command Aliases class - atm one method and some magic stringd hardcoding
- use dictionary collection in json config for command aliases
- finish command alias mechanism - extracting to new class and unit testing

5.1. Better Help() - list aslo aliases and command description

6. Better Exception messages and servicing exc throwing 
- lot of repeating or messy messages - create mechanism to service that giving doors to multi language


























Code Quality & Design: we value readable and simply designed code. We hate over-engineering and enterprise-style code. Some areas to research:

    YAGNI,
    Kent Beck’s four rules of simple design,
    Design Patterns,
    SOLID,
    Code Smells.

Automated Testing: 
All developers need to ensure that their code works. 
A great way to get confidence in your code is to use techniques such as Test-Driven Development. 
TDD is a topic that’s well worth trying and as a first step you should consider trying a “Code Kata” such as “Bowling game”. 
You may also want to research Behaviour Driven Development (BDD).

Systems design: 
We build a web-based product. Ideally, you should understand how the web works understanding the basics of DNS and HTTP in serving a request from a browser. 
Other areas to investigate include REST vs RPC, Microservice Architecture and message queuing technologies. We make use of Microsoft’s Azure to host our services. 
You will likely not have to apply this to the code test.

Other development practices to research:

    We value DevOps practices and the short iteration cycles they bring.
    Difference between Continuous Integration, Continuous Delivery and Continuous Deployment
    Agile: what does it mean to be Agile? XP, Scrum and Kanban
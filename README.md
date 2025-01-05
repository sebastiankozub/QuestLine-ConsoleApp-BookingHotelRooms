Console application for booking a hotel room
--------------------------------------------

used to create and test nice & reusable console app framework library


Architecture
------------

Layered Architecture:
1. Data - should be splitted and data persistance logic extracted to IDataStorage interface and JsonFileDataStorage class
2. Business Logic - use data layer to fulfill business needs
3. Command Handler - connecting ui with business logic - eg. chaging data format between ui and core logic - at some point splited into command & query cqrs for cleaner code of data transformation
4. Command Line Processor - parsing command line input and triggering command handler or returning to User Interface
5. User Interface - at the moment to strongly mixed views geneartion with ui logic and and app entry point logic - to refactoring

There is room (or need) for more layers - for example:
1. JsonFileDataStorage - implementuing abstraction above data storage IDataStorage to be used by DataContext: Save() and Inititialize()
2. CommandLineReader put between BookingAppConsoleInterface and CommandLineProcessor - resolve mix in User Interface layer -> spliting ui views geneartion and app logic
3. EntryPoint, Configuration, DI - split the responsibilities more & move some to class library projects to avoid dependencies eg. registering extension method AddDataContext()



TODO MUST-TO:
-------------

0! business logic in services and handlers - DONE
0! unit testing of business logic

1. CommandLineHandlers:
- change to ICommandHandler - in this layer code cannot have anything common with command line console - DONE
- generic ones like: search, availability - collect into collection at the begining of the app - REFACTOR different approach to di configuration
- easier/cleaner code to maintain in few areas then
- CommandLineHandlerData - inherited to create common base imput poco parameter for Handle(CommandLineHandlerData commandData) method

2.CONFIG JSON
- use dictionary collection for command aliases

3. Command Aliases class
- finish command alias mechanism - extracting to new class and unit testing

4. Better Exception messages and servicing throwing - lot of repeating messages & code










REVIEWER INFO

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
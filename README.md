ReadMe
------

Architecture
------------

Layered Architecture:
1. Data
2. Business Logic
3. Command Handler - connecting view with business logic - at some point splited into command & query cqrs for cleaner code
4. Command Line Processor
5. User Interface - at the moment mixed view and entry point logic - 

There is room (or need) for more layers - for example:
1. JsonFileDataStorage - implementuing abstraction above data storage IDataStorage to be used by DataContext: Save() and Inititialize()
2. CommandLineReader put between CommandLineProcessor and BookingAppConsoleInterface resolve some mix in User Interface layer -> spliting ui view and logic

TODO MUST-TO:
-------------

1. CommandLineHandlers:
- change to ICommandHandler - in this layer code cannot have anything common with comman line console
- generic ones like: search, availability - collect into collection at the begining of the app
- easier/cleaner code to maintain in few areas then
- CommandLineHandlerData - inherited to create common base imput poco parameter for Handle(CommandLineHandlerData commandData) method

2.CONFIG JSON
- repair config json - use dictionary collection for command aliases

3. Command Aliases
- finish command alias mechanism

4. make some classes cleaner & more compact - add unit testing









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
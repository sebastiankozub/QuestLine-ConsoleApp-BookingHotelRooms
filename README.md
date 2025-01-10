Console application for booking a hotel room
--------------------------------------------








TODO
----

0!. QuickConsole/Core - run as line command interpreter, ui interface I injected and used to trigger update after data back/ or no update if we switch to multithreading handlers and background tasks/ dependency between handlers triggereing 

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

5. CONFIG JSON + Command Aliases class - atm one method and some magic stringd hardcoding
- use dictionary collection in json config for command aliases
- finish command alias mechanism - extracting to new class and unit testing

5.1. Better Help() - list aslo aliases and command description

6. Better Exception messages and servicing exc throwing 
- lot of repeating or messy messages - create mechanism to service that giving doors to multi language


# PowerLog
PowerLog is a lightweight logging library made in C#, that is just built different.

Built with a rich set of features that facilitate abstraction, PowerLog provides a lot of customization options in terms of logging parameters (Take a look at `Log.Write`, you'll see what I'm talking about.), but can also be used for simple and straightforward usage as your average Joe logging libary.

# Features
* Logging with only one function.
```cs
Log Logger = new Log("Readme Logger");
Logger.Information("Hello PowerLog!");
```

* Has an `OnLog` Event, which can be used to display / monitor logs without a sink in your application.
* Supports sinks, and comes with a few default ones (console, debugger, markdown file, simple file, as well as a spectre.console sink) as separate libraries. (You can write a custom one by implementing the `ISink` interface, check the examples section for a detailed tutorial on it.)
* Logger instances.
* Enables the combination of log levels, such as `Information` and `Network`, or `Verbose` and `Error`, for more granular logging control.
* Allows full control over logging level exclusion and inclusion via verbosity masks / allowed severities, built using `Verbosity` presets and methods, and `Severity` extension methods.
* Blazingly fast. (Around ~0.4875 ms per log average worst-case scenario.)
* Completely dependency-free and self-sustained.
* Cross platform support.
* ~~Simple, Documented API.~~ (The documentation is outdated, check the documentation section below for more.)

# Documentation
The documentation in the wiki is currently outdated (last version of the documentation is 1.1.5), and the best place to check out how things work would be the source code, XML documentation or the examples in the README.

However, if you still want to check out the old documentation, [here](https://github.com/Thev2Andy/PowerLog/wiki).

## Not sure what logging is?
[Here](https://en.wikipedia.org/wiki/Logging_(computing)), this should help you understand what logging is. (the first paragraph is what you're looking for)

A logging library provides a simple API for developers to log events happening in their application.

# Examples
## Setting up a logger instance.
1. Logger instances allow developers to set up multiple loggers with different sink sets for different purposes, and is the only way to use PowerLog. (since logger instances are more versatile and are overall superior for general purpose logging and most logging situations.)
2. To set up a logger instance, you need to provide the constructor with the logger's identifier (used for different purposes, such as the formatting of logs and sorting the log files in the file sink by the sink's identifier.), and optionally the logger's verbosity, defaulting to `Severity.Verbose`. (meaning that it will emit all logs.)
```cs
Log Logger = new Log("Readme Logger", Verbosity.Minimum(Severity.Information));
```

___

## Setting up a sink.
1. Setting up a default sink from the utility classes.
    * The official sinks (the ones in the repository) provide extension methods to the `Log` type to instantly push the sink on the logger with just one function, and allows for builder patterns.
```cs
Logger.PushConsoleSink("Sink Identifier");
```
2. Builder pattern example:
```cs
Logger.PushConsoleSink("Console Sink Identifier")
    .PushFileSink("File Sink Identifier");
    .PushDebuggerSink("Debugger Sink Identifier");
```

3. Setting up a sink from an instance.
```cs
ISink Instance = /* Your sink instance. */;
Instance.Logger = Logger; // Make sure the logger is assigned to the sink. (Otherwise it will throw an exception of type `ArgumentException`.)
Logger.Push(Instance);
```

___

## Logging using multiple combined levels.
1. Requires the usage of the `Write` method instead of the overloads.
2. Will arrange the logs when formatting in the order of verbosity, from lowest to highest.
3. Will check against any or all matching flags when calculating verbosity depending on the `StrictFiltering` flag on the logger and sinks.
```cs
Logger.Write("Log Content", (Severity.Information | Severity.Network));
```

___

## Using log templates.
1. Log templates are made using the `Template` struct, and passing it to the log functions.
2. To create a log template, all you need to do is create an instance of the `Template` struct and pass in the required parameters.
3. The `DateTemplate` parameter is effectively the template used in the `DateTime.ToString()` method, so you shouldn't really worry about that. (It also has a default value, "`HH:mm:ss`")
4. The `LogTemplate` parameter is using a custom formatter, here are the wildcards of it:
```
Use `|T|` for the timestamp. (Refers to 'Time / Timestamp'.)
Use `|I|` for the logger identifier. (Refers to 'Identifier'.)
Use `|S|` for the severity. (Refers to 'Severity'.)
Use `|C|` for the content. (Refers to 'Content'.)
Use `|O|` for the sender.  (Refers to 'Object'.)
```
5. Here's the default template's log template pattern: "|[T]| ||I |S: ||C|| (O)|"
```cs
Template Template = new Template("|[T]| ||I |S: ||C|| (O)|", "HH:mm:ss");
```

___

## Writing a custom sink.
1. This section is a bit bigger, beware, but in this section we'll be implementing an extremely basic console sink, without any colors.
2. In order to make a sink, you first have to implement the `ISink` interface.
3. Here's the 3 most important parts of the `ISink` interface:
```cs
public void Emit(Arguments Log);
public void Initialize();
public void Shutdown();
```
4. Let's break them down:
    * `Emit` is the main function of the sink, responsible for processing the log received from the `Log.Write` function, including all the log metadata.
    * `Initialize` is called when the sink is added to a logger's sink stack, allowing for lazy initialization of the sink. It is called after the sink's constructor or initialization method.
    * `Shutdown` is called when the sink is removed from the logger's sink stack and can be used for cleanup operations.
5. Additionally, there are two control functions, `Save` and `Clear`, which can be called from the logger using `Log.Save` and `Log.Clear` for each attached sink.
6. It is crucial to ensure that the sink is associated with the correct logger instance to prevent the logger from throwing an `ArgumentException` when attempting to push the sink.
7. After getting that out of the way, let's implement the console sink:
    * First of all, we want to get the basic stuff going (e.g. the constructor).
    For the constructor, we can just get the sink identifier and the logger instance, and this will cover the properties of the `ISink` interface.
    ```cs
    public SimpleConsoleSink(string Identifier, Log Logger, Severity AllowedSeverities = Verbosity.All)
    {
        this.Identifier = Identifier;
        this.Logger = Logger;
        this.AllowedSeverities = AllowedSeverities;
        this.StrictFiltering = true;
    }
    ```

    * Let's go ahead and pick the next thing, which will be the easiest part of this, the `Clear` method. (Although this whole implementation is really easy, the hardest part is probably the constructor or builder pattern implementation.)
    ```cs
    public void Clear() {
        Console.Clear();
    }
    ```

    * It's time to implement the most important function of a sink, the `Emit` function.
    * There are endless ways to implement this depending on the sink you're making, but for this sink we'll do a very simple implementation and use the `Arguments.FormattedLog` property which will format our log based on it's template, but nothing is stopping you from accessing the fields of the `Arguments` instance.
    * One thing you might want to implement is verbosity, which can be implemented with a simple function call implemented in the library's `Verbosity` class, or as an extension method over `Severity`.
    ```cs
    public void Emit(Arguments Log)
    {
        if (Log.Severity.Passes(AllowedSeverities, StrictFiltering)) {
            Console.WriteLine(Log.FormattedLog);
        }
    }
    ```

8. That's the whole sink, and if you try it out you will see that it's already functional, but let's implement the builder pattern.
    * To implement the builder pattern, we will want to create a static class, let's call it `SimpleConsoleSinkUtilities`.
    ```cs
    public static class SimpleConsoleSinkUtilities { }
    ```

    * Next up, we will want to extend the `Log` class. (that's the reason for the static class)
    * We want to extend the `Log` class with a function that pushes the sink automatically. (Actually you can extend classes with functions only.)
    * Notice the `this` keyword on the first parameter, this is a very crucial step of the extension method.
    * Also, notice the `Log` return type, this is what allows the builder pattern.
    ```cs
    public static Log PushSimpleConsole(this Log Logger, string Identifier, Severity Verbosity = Severity.Verbose) { }
    ```

    * Let's implement this extension function, we essentially want to create an instance of the sink, set the parameters in the constructor and then push it onto the logger's sink stack.
    ```cs
    public SimpleConsoleSink(string Identifier, Log Logger, Severity AllowedSeverities = Verbosity.All)
    {
        this.Identifier = Identifier;
        this.Logger = Logger;
        this.AllowedSeverities = AllowedSeverities;
        this.StrictFiltering = true;
    }
    ```

    * In the end, this is what the builder pattern class will look like:
    ```cs
    public static class SimpleConsoleSinkUtilities
    {
        public static Log PushSimpleConsole(this Log Logger, string Identifier, Severity AllowedSeverities = Verbosity.All) {
            SimpleConsoleSink Sink = new SimpleConsoleSink(Identifier, Logger, AllowedSeverities);
            Logger.Push(Sink);

            return Log;
        }
    }
    ```

    * And this is what the whole sink implementation will look like.
    ```cs
    public class SimpleConsoleSink : ISink
    {
        public string Identifier { get; }
        public Log Logger { get; }
        public Severity AllowedSeverities { get; set; }

        public void Emit(Arguments Log)
        {
            if (Log.Severity.Passes(AllowedSeverities, StrictFiltering)) {
                Console.WriteLine(Log.FormattedLog);
            }
        }

        public void Initialize() { }

        public void Shutdown() { }

        public void Save() { }

        public void Clear() {
            Console.Clear();
        }



        public SimpleConsoleSink(string Identifier, Log Logger, Severity AllowedSeverities = Verbosity.All) {
            this.Identifier = Identifier;
            this.Logger = Logger;
            this.AllowedSeverities = AllowedSeverities;
            this.StrictFiltering = true;
        }
    }



    public static class SimpleConsoleSinkUtilities
    {
        public static Log PushSimpleConsole(this Log Logger, string Identifier, Severity AllowedSeverities = Verbosity.All) {
            SimpleConsoleSink Sink = new SimpleConsoleSink(Identifier, Logger, AllowedSeverities);
            Logger.Push(Sink);

            return Log;
        }
    }
    ```
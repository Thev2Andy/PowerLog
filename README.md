# PowerLog
PowerLog is a lightweight logging library made in C#, that is just built different.

Built with a rich set of features that facilitate both abstraction and regular use, PowerLog provides a lot of customization options in terms of properties in the `Log` and `Template` types, but can also be used for simple and straightforward usage as your average logging library.

# Features
* Logging with only one function.
```cs
Log Logger = new Log("Readme Logger");
Logger.Information("Hello PowerLog!");
```

* Provides hook events such as `OnLog`, which can be used to display / monitor logs without a sink in your application.
* Supports sinks, and comes with a few default ones (console, debugger, markdown file, simple file, as well as a Spectre.Console sink) as separate libraries. (You can write a custom one by implementing the `ISink` interface, check the examples section for a detailed tutorial on it.)
* Logger instances.
* Enables the combination of log levels, such as `Information` and `Network`, or `Verbose` and `Error`, for more granular logging control.
* Allows full control over logging level exclusion and inclusion via verbosity masks / allowed severities, built using `Verbosity` presets and methods, and `Severity` extension methods.
* Blazingly fast, going for around `86.5` ns with no allocations for an empty logger, compared to Serilog's `240` ns with `384` bytes allocated. (The benchmark project is included, so feel free to try it out and / or point out where I did something wrong in the benchmark.)
* Completely dependency-free and self-sustained.
* Supports structured logging.
* Cross platform support.
* _NativeAOT ready._ (Not fully tested, but it doesn't rely on any `System.Reflection` APIs.)
* Simple, documented API via XML documentation. (The old documentation is outdated, check the documentation section below for more.)

# Documentation
The documentation in the wiki is currently outdated (last version of the documentation is 1.1.5), and the best place to check out how things work would be the source code, XML documentation or the examples in the README.

However, if you still want to check out the old documentation, [here](https://github.com/Thev2Andy/PowerLog/wiki).

### What happened?
The old documentation from the wiki was meant to be simple, back 2 years ago when the library was smaller than 300 lines of code, all of which could be documented in a matter of minutes. As the library grew, the API started to get harder and more tedious to document in a single markdown file in the wiki, so I started writing XML documentation (which is in the codebase, and massively helps with IntelliSense) in hopes of using it with DocFX, which didn't really work, and I ultimately failed with DocFX.

The library is no longer released on a SemVer release system, but rather commits, which you can get with [Git Submodules](https://git-scm.com/book/en/v2/Git-Tools-Submodules), but this is also one reason why I'm deciding to cut back on documentation, until I figure something out, possibly using DocFX, or auto-generating markdown files and putting them in another branch or on the `master` branch in a separate `Documentation` folder.

For now I'm keeping the old documentation mainly for historical reference for the older versions of the library.

## Not sure what logging is?
[Here](https://en.wikipedia.org/wiki/Logging_(computing)), this should help you understand what logging is. (the first paragraph is what you're looking for)

A logging library provides a simple API for developers to log events happening in their application.

# Structured Logging.
PowerLog _supports_* structured logging.

Don't know what structured logging is?
* Structured logging represents logging data as more than text, in a way that can hold different parameters / enrichments / contextual properties individually, and in a way that can be parsed and searched by a computer.

Here are some guidelines regarding structured logging in a general context. (And you can apply this to other logging libraries too.)
1. Use logging parameters instead of string concatenation / interpolation.
    * Why:
        * Better performance. (Concatenated / Interpolated logs will need to be reallocated for every log.)
        * Searchability. (You can search for a fixed string, usually the log 'template', and get every log that matches said template.)
        * **Potentially** better formatting. (Certain sinks may do custom formatting and highlight parameters.)
    * Example:
        * Do this:
            ```cs
            Log.Information("Performing cleanup on object with ID ~Object ID~..", new() { { "Object ID", ID } });
            ```

        * Not this:
            ```cs
            Log.Information($"Performing cleanup on object with ID {ID}..");
            ```
2. Packed logs are generally better.
    * Why:
        * You have more information regarding the conditions that caused the log.
        * The structured information in a packed log can be searched, so you could search for logs that occurred in special circumstances.
    * **BUT!**:
        * Be careful to not fall into the trap of packing useless information that clutters up logs.
        * Avoid adding parameters just _for the sake of it_, only provide what's relevant to the log and the task currently being executed. These parameters may make it hard to search for and quickly spot logs, which, last time I checked, was not a good thing. (but maybe it is now, you get the point)

_supports structured logging_* -> PowerLog has most (if not all) of the capabilities required for structured logging, except for serialization, which can be fairly crucial when it comes to formatting and bundling logs up to be analyzed in a third party application. (This will be implemented fairly soon though.)

# Provided Sinks
* Asynchronous Sink: `PowerLog.Sinks.Asynchronous` (Emits logs asynchronously to reduce the performace impact.)
* Console Sink: `PowerLog.Sinks.Terminal` (Emits logs to the standard output.)
* Debugger Sink: `PowerLog.Sinks.Debugger` (Emits logs to trace listeners.)
* File Sink: `PowerLog.Sinks.IO` (Emits logs to a simple text file.)
* Logger Sink: `PowerLog.Sinks.Logger` (Writes emitted logs to another logger.)
* Markdown Sink: `PowerLog.Sinks.Markdown` (Emits logs to a markdown file, in a table format.)
* Spectre Console Sink: `PowerLog.Sinks.SpectreTerminal` (Emits logs to the standard output using `Spectre.Console`.)

**Note**: These sinks are the sinks currently included with the solution and readily available. This does **NOT** include third party sinks.

# Naming Guidelines.
1. Member names.
    * PowerLog exclusively (yes, including parameters and private / local members) uses `PascalCase`. This does **NOT** affect API usage, public API is `PascalCase` either way by the [C# Coding Guidelines and Best Practices](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/identifier-names#naming-conventions), but using `PascalCase` for everything is rather a preference of mine. (_i don't want to hear the readability argument, intellisense is a thing, and it's pretty great actually_)

2. Component names. (For bundled and third party components.)
    * Extension method names.
        * Sink -> `Push{X}` (Example: `PushMarkdown`)
        * Enricher -> `Append{Y}` (Example: `AppendThread`)
        * Filter -> `FilterBy{Z}` (Example: `FilterByPredicate`)

    * Class names.
        * Sink -> `{X}Sink` (Example: `MarkdownSink`)
        * Enricher -> `{Y}Enricher` (Example: `ThreadEnricher`)
        * Filter -> `{Z}Filter` (Example: `PredicateFilter`)

3. `CSPROJ` names.
    * Use `Sentence Case`.
        * If you want to customize the output name (Example: `PowerLog Markdown Sink` -> `PowerLog.Sinks.Markdown`) use the `AssemblyName` property / tag in the `CSPROJ` file.
            ```xml
            <!-- PowerLog Markdown Sink.csproj -->
            <!-- ... -->
                <AssemblyName>PowerLog.Sinks.Markdown</AssemblyName>
            <!-- ... -->
            ```

# Is This Library for Me?
It depends. At the end of the day, it's essential to choose the most suitable tool for the task at hand rather than aligning your entire identity with a single tool. Using Serilog doesn't make you exclusively a "Serilog developer," just as using NLog doesn't solely categorize you as an "NLog developer." You **have** the freedom to select any logging library that best fits the requirements of your projects.

Use this library if:
* You want a tiny library. (~900 LOC)
* You want a dependency-free library.
* You want some nicer features within the library. (Example: Verbosity Masks)
* You want a library with a tiny and fast core.
* You want a more traditional API, without factories or fancy patterns. (Sometimes the old ways are the best.)

Don't use this library if:
* You don't like breaking changes.
* You want community support.
* You want features beyond the core features. (Example: Sinks, Enrichers, Filters)
* You want a known, battle-tested library. (Although PowerLog has impressive benchmarks, it hasn't really been used in any actual projects.)

_Also, if you don't use it, it's your loss, I'll still be using and dogfooding it._

# Examples
## Setting up a logger instance.
1. Logger instances allow developers to set up multiple loggers with different sink sets for different purposes, and is the only way to use PowerLog. (since logger instances are more versatile and are overall superior for general purpose logging and most logging situations.)
2. To set up a logger instance, you need to provide the constructor with the logger's identifier (used for different purposes, such as the formatting of logs and sorting the log files in the file sink by the sink's identifier.), and optionally the logger's verbosity, defaulting to `Verbosity.All`. (meaning that it will emit all logs)
```cs
// Use `Verbosity.All` to allow all log levels.
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
3. The `Date` parameter is effectively the template used in the `DateTime.ToString()` method, so you shouldn't really worry about that. (It also has a default value, "`HH:mm:ss`")
4. The `Format` parameter is using a custom formatter, here are the implemented wildcards:
```
[ Not Conditional ]  |T| -> Timestamp.          (Refers to 'Time / Timestamp'.)
[   Conditional   ]  |I| -> Logger Identifier.  (Refers to 'Identifier'.)
[   Conditional   ]  |S| -> Severity.           (Refers to 'Severity'.)
[ Not Conditional ]  |C| -> Content.            (Refers to 'Content'.)
[   Conditional   ]  |O| -> Sender.             (Refers to 'Object'.)
```
5. Here's the default (Modern) template's log template pattern.
```cs
Template Template = new Template("|T ||[|I |S] ||C|| (O)|", "HH:mm:ss", Options.Compact);
```
6. Conditional wildcards are wildcards where, using the `Options` enumeration, you can configure the parsing behavior for wildcards where data isn't available, or where data may not be available, such as `Logger Identifier` (through "raw" logs), `Severity` (for the `Generic` severity), and `Sender`. (when the sender is `null`)

___

## Using filters.
1. Filters allow users to filter logs based on user-defined conditions.
2. Filters can be set to require one or all of the filters to pass, done through the `Log.StrictFiltering` property.
```cs
Log Logger = new Log("Readme Logger");
Log.StrictFiltering = true; // Defaults to `true`.
Logger.FilterByPredicate((Log) => { return Log.Timestamp.Year >= 2024; }); // Return `true` to allow the log through.

Logger.Information("Hello PowerLog!");
```

___

## Adding and using parameters.
1. Logging parameters allow users to add structured data to logs for a single produced log.
2. In PowerLog, parameters are stored in a parameter dictionary on the produced logs.
```cs
// You can access logging parameters following this syntax: `~{Property Name}~`.
// The placeholder `{Property Name}` is the name of any parameter passed in a log.
Log.Error("An error occurred! ~Error~", new Dictionary<string, Object> { { "Error", "Attempted to divide by 0." } });
```
___

## Adding and using contextual logging properties.
1. Contextual logging properties allow users to add structured properties to logs across multiple logging calls.
2. In PowerLog, contextual logging properties are stored in a context dictionary on the logger.
```cs
Log.Context.Add("Color Override", "84, 0, 255"); // Add a string property.
Log.Context.Add("Highlight Override", true); // Add a boolean property.
Log.Context.Add("Operation ID", 12); // Add an integer property.

// You can access contextual logging properties following this syntax: `~${Property Name}~`.
// The placeholder `{Property Name}` is the name of any property passed in the log context.
Log.Information("These logs will contain all the current log context properties.");
Log.Information("Color: ~$Color Override~");
Log.Information("Highlight: ~$Highlight Override~");
Log.Information("Operation: ~$Operation ID~");

Log.Context.Clear(); // You can also remove or change individual properties.
```

___

## Adding and using enrichers.
1. Enrichment properties allow users to add structured properties to logs for every log produced by a certain logger.
2. In PowerLog, enrichments are appended to an enrichment dictionary on the produced logs.
```cs
Log Logger = new Log("Readme Logger");
Logger.AppendThread("Thread Enricher"); // Assuming we're using the thread enricher.

// You can access enriched logging properties following this syntax: `~@{Property Name}~`.
// The placeholder `{Property Name}` is the name of any enrichment property passed in the log by any appended enrichers.
Logger.Information("Thread ~@Thread ID~ (Name: `~@Thread Name~`, ~@Thread State~) has a priority of `~@Thread Priority~`.");
```

___

## Writing a custom sink.
**Note**: Do **NOT** implement multiple component types or directly implement the `IComponent` interface, your component will be rejected by the logging library and throw exceptions when attempting to use it. This will not be caught at compile time by default. (requires custom roslyn analyzers which I don't have)
1. In this section we'll be implementing an extremely basic console sink, without any colors.
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
    For the constructor, we can just get the sink identifier, the logger instance, and optionally the allowed severities, and this will cover the properties of the `ISink` interface. (If skipping the `AllowedSeverities` property, do make sure that you set something, since the default value is `Verbosity.None`.)
    ```cs
    public SimpleConsoleSink(string Identifier, Log Logger, Severity AllowedSeverities = Verbosity.All)
    {
        this.Identifier = Identifier;
        this.Logger = Logger;
        this.AllowedSeverities = AllowedSeverities; // Replace with `this.AllowedSeverities = Verbosity.All;` or whatever you need for your sink if you decide to not implement the `AllowedSeverities` property.
        this.StrictFiltering = true;
        this.IsEnabled = true;
    }
    ```

    * Let's go ahead and pick the next thing, which will be the easiest part of this, the `Clear` method. (Although this whole implementation is really easy, the hardest part is probably the constructor or builder pattern implementation.)
    ```cs
    public void Clear() {
        Console.Clear();
    }
    ```

    * It's time to implement the most important function of a sink, the `Emit` function.
    * There are endless ways to implement this depending on the sink you're making, but for this sink, we'll do a very simple implementation and use the `Arguments.FormattedLog` property, which will format our log based on its template, but nothing is stopping you from accessing the fields of the `Arguments` instance.
    * Sink verbosity is automatically handled by the logger, so there's no need to check if the current log passes the sink's verbosity.
    ```cs
    public void Emit(Arguments Log)
    {
        Console.WriteLine(Log.FormattedLog);
    }
    ```

8. That's the whole sink, and if you try it out you will see that it's already functional, but let's implement the builder pattern.
    * To implement the builder pattern, we will want to create a static class, let's call it `SimpleConsoleSinkUtilities`.
    ```cs
    public static class SimpleConsoleSinkUtilities { }
    ```

    * Next up, we will want to write an extension method for the `Log` class. (This is why we're writing a static class.)
    * Do notice the `Log` return type, this is what allows the builder pattern / method chaining.
    * For more information regarding extension methods in C#, check out Microsoft's [documentation](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods).
    ```cs
    public static Log PushSimpleConsole(this Log Logger, string Identifier, Severity AllowedSeverities = Verbosity.All) { }
    ```

    * To implement the extension function, we essentially want to create an instance of the sink, set the parameters in the constructor, and then push it onto the logger's sink stack.
    * **Note**: The methods `Log.Push` and `Log.Pop` take in an `IComponent` component type, of which it will determine the type, the appropriate collection, and reject invalid component types. The `Log.Find<Component>` method uses a generic parameter, but follows the same convention.
    ```cs
    public static Log PushSimpleConsole(this Log Logger, string Identifier, Severity AllowedSeverities = Verbosity.All)
    {
            SimpleConsoleSink Sink = new SimpleConsoleSink(Identifier, Logger, AllowedSeverities);
            Logger.Push(Sink);

            return Logger; // Do remember to return the `Logger` instance, this is what allows the method chaining / builder pattern.
    }
    ```

    * In the end, this is what the extension class will look like:
    ```cs
    public static class SimpleConsoleSinkUtilities
    {
        public static Log PushSimpleConsole(this Log Logger, string Identifier, Severity AllowedSeverities = Verbosity.All)
        {
            SimpleConsoleSink Sink = new SimpleConsoleSink(Identifier, Logger, AllowedSeverities);
            Logger.Push(Sink);

            return Logger;
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
        public bool StrictFiltering { get; set; }
        public bool IsEnabled { get; set; }

        public void Emit(Arguments Log)
        {
            Console.WriteLine(Log.FormattedLog);
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
            this.IsEnabled = true;
        }
    }



    public static class SimpleConsoleSinkUtilities
    {
        public static Log PushSimpleConsole(this Log Logger, string Identifier, Severity AllowedSeverities = Verbosity.All) {
            SimpleConsoleSink Sink = new SimpleConsoleSink(Identifier, Logger, AllowedSeverities);
            Logger.Push(Sink);

            return Log; // Do remember to return the `Logger` instance, this is what allows the method chaining / builder pattern.
        }
    }
    ```

___

## Writing a custom enricher.
**Note**: Do **NOT** implement multiple component types or directly implement the `IComponent` interface, your component will be rejected by the logging library and throw exceptions when attempting to use it. This will not be caught at compile time by default. (requires custom roslyn analyzers which I don't have)
1. In this section we'll be implementing an extremely basic property enricher.
2. In order to make an enricher, you first have to implement the `IEnricher` interface.
3. Here's the most important (and only part) of the `IEnricher` interface:
```cs
public void Enrich(in Dictionary<string, Object> Enrichments);
```
4. Let's break it down:
    * `Enrich` is the main function, where enrichers add / modify properties.
    * Enrichment properties are added via the `Add` method on the `Enrichments` dictionary, which will be reflected in the log.
5. It is crucial to ensure that the enricher is associated with the correct logger instance to prevent the logger from throwing an `ArgumentException` when attempting to append the enricher.
6. After getting that out of the way, let's implement the enricher:
    * First of all, we want to get the basic stuff going (e.g. the constructor).
    For the constructor, we can just get the enricher identifier and the logger instance.
    ```cs
    public PropertyEnricher(string Identifier, Log Logger)
    {
        this.Identifier = Identifier;
        this.Logger = Logger;
        this.IsEnabled = true;
    }
    ```

    * Now on to implementing the `Enrich` method.
    * There are endless ways to implement this depending on the enricher you're making, but for this enricher, we'll do a very simple implementation and add a few hardcoded properties. (You basically gather data and add it to the dictionary.)
    ```cs
    public void Enrich(in Dictionary<string, Object> Enrichments)
    {
        Enrichments.Add("Sample String", "Hello, World!");
        Enrichments.Add("Sample Integer", 69);
        Enrichments.Add("Sample Boolean", true);
    }
    ```

7. That's the whole enricher, and if you try it out you will see that it's already functional, but let's implement the builder pattern.
    * To implement the builder pattern, we will want to create a static class, let's call it `PropertyEnricherUtilities`.
    ```cs
    public static class PropertyEnricherUtilities { }
    ```

    * Next up, we will want to write an extension method for the `Log` class. (This is why we're writing a static class.)
    * Do notice the `Log` return type, this is what allows the builder pattern / method chaining.
    * For more information regarding extension methods in C#, check out Microsoft's [documentation](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods).
    ```cs
    public static Log AppendProperty(this Log Logger, string Identifier) { }
    ```

    * To implement the extension function, we essentially want to create an instance of the enricher, set the parameters in the constructor, and then push it onto the logger's enricher stack.
    * **Note**: The methods `Log.Push` and `Log.Pop` take in an `IComponent` component type, of which it will determine the type, the appropriate collection, and reject invalid component types. The `Log.Find<Component>` method uses a generic parameter, but follows the same convention.
    ```cs
    public static Log AppendProperty(this Log Logger, string Identifier)
    {
            PropertyEnricher Enricher = new PropertyEnricher(Identifier, Logger);
            Logger.Push(Enricher);

            return Logger; // Do remember to return the `Logger` instance, this is what allows the method chaining / builder pattern.
    }
    ```

    * In the end, this is what the extension class will look like:
    ```cs
    public static class PropertyEnricherUtilities
    {
        public static Log AppendProperty(this Log Logger, string Identifier)
        {
                PropertyEnricher Enricher = new PropertyEnricher(Identifier, Logger);
                Logger.Push(Enricher);

                return Logger; // Do remember to return the `Logger` instance, this is what allows the method chaining / builder pattern.
        }
    }
    ```

    * And this is what the whole enricher implementation will look like.
    ```cs
    public class PropertyEnricher : IEnricher
    {
        public string Identifier { get; }
        public Log Logger { get; }
        public bool IsEnabled { get; set; }

        public void Enrich(in Dictionary<string, Object> Enrichments)
        {
            Enrichments.Add("Sample String", "Hello, World!");
            Enrichments.Add("Sample Integer", 69);
            Enrichments.Add("Sample Boolean", true);
        }



        public PropertyEnricher(string Identifier, Log Logger)
        {
            this.Identifier = Identifier;
            this.Logger = Logger;
            this.IsEnabled = true;
        }
    }



    public static class PropertyEnricherUtilities
    {
        public static Log AppendProperty(this Log Logger, string Identifier)
        {
                PropertyEnricher Enricher = new PropertyEnricher(Identifier, Logger);
                Logger.Push(Enricher);

                return Logger; // Do remember to return the `Logger` instance, this is what allows the method chaining / builder pattern.
        }
    }
    ```

___

## Writing a custom filter.
**Note**: Do **NOT** implement multiple component types or directly implement the `IComponent` interface, your component will be rejected by the logging library and throw exceptions when attempting to use it. This will not be caught at compile time by default. (requires custom roslyn analyzers which I don't have)
1. In this section we'll be implementing an extremely basic log filter.
2. In order to make a filter, you first have to implement the `IFilter` interface.
3. Here's the most important (and only part) of the `IFilter` interface:
```cs
public bool Filter(Arguments Log);
```
4. Let's break it down:
    * `Filter` is the main function, where filters analyze the log and other external factors, and return either `true` to allow the log through, or `false` to block it.
    * Depending on the logger configuration, only one filter may be required for the specific log to pass. This is controlled by `Log.StrictFiltering` (`true` -> All filter tests.), and determines if a log can pass by matching its severity levels with the verbosity mask (`false` -> Only one matching level required.), and also determines if the log can pass by a single filter.
5. It is crucial to ensure that the filter is associated with the correct logger instance to prevent the logger from throwing an `ArgumentException` when attempting to filter logs using the filter.
6. After getting that out of the way, let's implement the filter:
    * First of all, we want to get the basic stuff going (e.g. the constructor).
    For the constructor, we can just get the filter identifier and the logger instance.
    ```cs
    public RNGFilter(string Identifier, Log Logger)
    {
        this.Identifier = Identifier;
        this.Logger = Logger;
        this.IsEnabled = true;
    }
    ```

    * Now on to implementing the `Filter` method.
    * There are endless ways to implement this depending on the filter you're making, but for this filter, we'll do a very simple implementation and filter by a 50/50 chance.
    ```cs
    public bool Filter(Arguments Log)
    {
        return Random.Shared.NextSingle() < 0.5f;
    }
    ```

7. That's the whole filter, and if you try it out you will see that it's already functional, but let's implement the builder pattern.
    * To implement the builder pattern, we will want to create a static class, let's call it `RNGFilterUtilities`.
    ```cs
    public static class RNGFilterUtilities { }
    ```

    * Next up, we will want to write an extension method for the `Log` class. (This is why we're writing a static class.)
    * Do notice the `Log` return type, this is what allows the builder pattern / method chaining.
    * For more information regarding extension methods in C#, check out Microsoft's [documentation](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods).
    ```cs
    public static Log FilterByRNG(this Log Logger, string Identifier) { }
    ```

    * To implement the extension function, we essentially want to create an instance of the filter, set the parameters in the constructor, and then push it onto the logger's filter stack.
    * **Note**: The methods `Log.Push` and `Log.Pop` take in an `IComponent` component type, of which it will determine the type, the appropriate collection, and reject invalid component types. The `Log.Find<Component>` method uses a generic parameter, but follows the same convention.
    ```cs
    public static Log FilterByRNG(this Log Logger, string Identifier)
    {
            RNGFilter Filter = new RNGFilter(Identifier, Logger);
            Logger.Push(Filter);

            return Logger; // Do remember to return the `Logger` instance, this is what allows the method chaining / builder pattern.
    }
    ```

    * In the end, this is what the extension class will look like:
    ```cs
    public static class RNGFilterUtilities
    {
        public static Log FilterByRNG(this Log Logger, string Identifier)
        {
                RNGFilter Filter = new RNGFilter(Identifier, Logger);
                Logger.Push(Filter);

                return Logger; // Do remember to return the `Logger` instance, this is what allows the method chaining / builder pattern.
        }
    }
    ```

    * And this is what the whole filter implementation will look like.
    ```cs
    public class RNGFilter : IEnricher
    {
        public string Identifier { get; }
        public Log Logger { get; }
        public bool IsEnabled { get; set; }

        public bool Filter(Arguments Log)
        {
            return Random.Shared.NextSingle() < 0.5f;
        }



        public RNGFilter(string Identifier, Log Logger)
        {
            this.Identifier = Identifier;
            this.Logger = Logger;
            this.IsEnabled = true;
        }
    }



    public static class RNGFilterUtilities
    {
        public static Log FilterByRNG(this Log Logger, string Identifier)
        {
                RNGFilter Filter = new RNGFilter(Identifier, Logger);
                Logger.Push(Filter);

                return Logger; // Do remember to return the `Logger` instance, this is what allows the method chaining / builder pattern.
        }
    }
    ```

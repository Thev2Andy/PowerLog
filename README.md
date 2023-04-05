# PowerLog
PowerLog is a lightweight logging library made in C#, that is just built different.

# Features
* Logging with only one function.
```cs
Log Logger = new Log("Readme Logger");
Logger.Information("Hello PowerLog!");
```

* Has an `OnLog` Event, which can be used to display / monitor logs without a sink in your application.
* Supports sinks, and comes with a few default ones (console, debugger, markdown file, simple file) as separate libraries. (You can write a custom one by implementing the `ISink` interface.)
* Logger instances.
* Blazingly fast.
* Cross platform support.
* ~~Simple, Documented API.~~ (the documentation is outdated, the best place to see how things work so far is the source code itself)

# Documentation
The documentation in the wiki is currently outdated (last version of the documentation is 1.1.5), and the best place to check out how things work would be the source code.

However, if you still want to check out the old documentation, [here](https://github.com/Thev2Andy/PowerLog/wiki).

## Not sure what logging is?
[Here](https://en.wikipedia.org/wiki/Logging_(computing)), this should help you understand what logging is. (the first paragraph is what you're looking for)

A logging library provides a simple API for developers to log events happening in their application.
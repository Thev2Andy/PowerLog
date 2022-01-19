# PowerLog

PowerLog is a lightweight logging library made in C#, designed to make logging easy, with only one function call.

# Features
* Logging with only one function. You can read more in the [docs](docs.com).
```cs
PowerLog.Logger.Log("Hello PowerLog!", LogType.Info, true, this);
```

* Has an [`OnLog`](docs.com) Event, which can be used to display logs in your application. (such as a game engine)
* Saves logs next to the calling assembly (application executable assembly mostly, but it can be invoked by other assemblies and save the log there) and clears it on the first log in the assembly process.

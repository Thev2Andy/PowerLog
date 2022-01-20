# PowerLog

PowerLog is a lightweight logging library made in C#, designed to make logging easy, with only one function call.

# Features
* Logging with only one function. You can read more in the [docs](https://github.com/Thev2Andy/PowerLog/wiki).
```cs
PowerLog.Logger.Log("Hello PowerLog!", LogType.Info, true, true, true, this);
```

* Has an [`OnLog`](https://github.com/Thev2Andy/PowerLog/wiki/API-Reference#loggeronlog-eventhandlerlogeventargs) Event, which can be used to display logs in your application. (such as a game engine)
* Saves logs next to the calling assembly, and it can easly be extended using C# events.
* Simple, Documented API.

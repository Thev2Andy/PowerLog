# PowerLog
PowerLog is a lightweight logging library made in C#, designed to make logging easy, with only one function call.

<!-- <img src="https://user-images.githubusercontent.com/85254326/156647228-04a1999e-d998-41b8-aeba-a957856b748e.png" width=450> -->

# Features
* Logging with only one function. You can read more in the [docs](https://github.com/Thev2Andy/PowerLog/wiki).
```cs
PowerLog.Log.LogL("Hello PowerLog!", LogType.Info, LogMode.Default);
```

* Has an [`OnLog`](https://github.com/Thev2Andy/PowerLog/wiki/API-Reference#onlog-eventhandlerlogeventargs) Event, which can be used to display logs in your application. (such as a game engine)
* Saves logs in a `Logs` folder next to the executable, and it can easly be extended using C# events.
* Simple, Documented API.

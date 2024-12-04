Once the package is added/installed in your class you can use methods inherited from `TryCatchExecuteBase` or `TryCatchExecuteStaticBase`.

The difference between these two is that one is an abstract class with virtual methods and the other is a simple class with static methods.
So there are available two methods: `TryToExecute` and `TryToExecuteAsync` with different number of input parameters.

```csharp
public class Foo : TryCatchExecuteBase
{
    public IResult SetFoo(object foo)
    {
        var exec = TryToExecute(
            () => { return Result.Success(); }, 
            () => { return Result.Failure("Error"); });
            
        return exec;
    }
}
```


```csharp
public class Foo : TryCatchExecuteStaticBase
{
    public IResult SetFoo(object foo)
    {
        var exec = TryToExecute(
            () => { return Result.Success(); }, 
            () => { return Result.Failure("Error"); });
            
        return exec;
    }
    
    public bool SetFoo1(object foo)
    {
        var exec = TryToExecute(
            () => { return true; }, 
            false);
            
        return exec;
    }
}
```

Available methods with parameters:

SYNC
- `TResult TryToExecute<TResult>(TResult execRequest, TResult onFailureResult, bool forceCallGarbageCollector = false)`
- `TResult TryToExecute<TResult>(TResult execRequest, TResult onFailureResult, Func<TResult> finallyExecFunc, bool forceCallGarbageCollector = false)`
- `TResult TryToExecute<TResult>(Func<TResult> execFunc, TResult onFailureResult, bool forceCallGarbageCollector = false)`
- `TResult TryToExecute<TResult>(Func<TResult> execFunc, TResult onFailureResult, Func<TResult> finallyExecFunc, bool forceCallGarbageCollector = false)`
- `TResult TryToExecute<TResult>(Func<TResult> execFunc, Func<TResult> onFailureResult, bool forceCallGarbageCollector = false)`
- `TResult TryToExecute<TResult>(Func<TResult> execFunc, Func<TResult> onFailureResult, Func<TResult> finallyExecFunc, bool forceCallGarbageCollector = false)`

**If is NETSTANDARD 2.0 OR GREATER**
- `TResult TryToExecute<TResult, TLogger>(Func<TResult> execFunc, TResult onFailureResult, ILogger<TLogger> exceptionLogger, bool forceCallGarbageCollector = false)`
- `TResult TryToExecute<TResult, TLogger>(Func<TResult> execFunc, TResult onFailureResult, ILogger<TLogger> exceptionLogger, Func<TResult> finallyExecFunc, bool forceCallGarbageCollector = false)`
- `TResult TryToExecute<TResult, TLogger>(Func<TResult> execFunc, Func<TResult> onFailureResult, ILogger<TLogger> exceptionLogger, bool forceCallGarbageCollector = false)`
- `TResult TryToExecute<TResult, TLogger>(Func<TResult> execFunc, Func<TResult> onFailureResult, ILogger<TLogger> exceptionLogger, Func<TResult> finallyExecFunc, bool forceCallGarbageCollector = false)`


ASYNC
- `Task<TResult> TryToExecuteAsync<TResult>(Func<Task<TResult>> execFunc, TResult onFailureResult, bool forceCallGarbageCollector = false)`
- `Task<TResult> TryToExecuteAsync<TResult>(Func<Task<TResult>> execFunc, TResult onFailureResult, Func<TResult> finallyExecFunc, bool forceCallGarbageCollector = false)`
- `Task<TResult> TryToExecuteAsync<TResult, TLogger>(Func<Task<TResult>> execFunc, TResult onFailureResult, ILogger<TLogger> exceptionLogger, bool forceCallGarbageCollector = false)`
- `Task<TResult> TryToExecuteAsync<TResult, TLogger>(Func<Task<TResult>> execFunc, TResult onFailureResult, ILogger<TLogger> exceptionLogger, Func<TResult> finallyExecFunc, bool forceCallGarbageCollector = false)`
- `Task<TResult> TryToExecuteAsync<TResult>(Func<Task<TResult>> execFunc, Func<TResult> onFailureResult, bool forceCallGarbageCollector = false)`
- `Task<TResult> TryToExecuteAsync<TResult>(Func<Task<TResult>> execFunc, Func<TResult> onFailureResult, Func<TResult> finallyExecFunc, bool forceCallGarbageCollector = false)`
 - `Task<TResult> TryToExecuteAsync<TResult>(Func<Task<TResult>> execFunc, Func<Task<TResult>> onFailureResult, bool forceCallGarbageCollector = false)`
 - `Task<TResult> TryToExecuteAsync<TResult>(Func<Task<TResult>> execFunc, Func<Task<TResult>> onFailureResult, Func<TResult> finallyExecFunc, bool forceCallGarbageCollector = false)`

**If is NETSTANDARD 2.0 OR GREATER**
- `Task<TResult> TryToExecuteAsync<TResult, TLogger>(Func<Task<TResult>> execFunc, Func<TResult> onFailureResult, ILogger<TLogger> exceptionLogger, bool forceCallGarbageCollector = false)`
 - `Task<TResult> TryToExecuteAsync<TResult, TLogger>(Func<Task<TResult>> execFunc, Func<TResult> onFailureResult, ILogger<TLogger> exceptionLogger, Func<TResult> finallyExecFunc, bool forceCallGarbageCollector = false)`
 - `Task<TResult> TryToExecuteAsync<TResult, TLogger>(Func<Task<TResult>> execFunc, Func<Task<TResult>> onFailureResult, ILogger<TLogger> exceptionLogger, bool forceCallGarbageCollector = false)`
 - `Task<TResult> TryToExecuteAsync<TResult, TLogger>(Func<Task<TResult>> execFunc, Func<Task<TResult>> onFailureResult, ILogger<TLogger> exceptionLogger, Func<TResult> finallyExecFunc, bool forceCallGarbageCollector = false)`
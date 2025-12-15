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
 - 
 

---

### TryBuilder -> A fluent code invoke API.

TryExecute is a fluent, strongly-typed Try / Catch / Finally execution framework for .NET with first-class support for:

- Async / sync / ValueTask
- Typed catch handlers
- Retry policies with backoff + jitter
- Deterministic retry behavior for testing
- Fallback values
- Cancellation tokens
- Multiple finally blocks (LIFO execution)
- Allocation-aware design

This library is designed for explicit control flow, testability, and developer ergonomics.


---
##### The base methods

-> `Do` - Execute base code.

-> `Catch` - Catches the given handler (Exception catch handler).

-> `Finally` - The final execution function (in the finally block).

-> `Retry` - The retry execute function policy.

-> `Fallback` - The fallback execute function.

-> `Execute`/`ExecuteAsync` - Execute the given flow.

---

##### Basic Usage
```csharp
var result = await TryBuilder
    .Do(async ct =>
    {
        await Task.Delay(100, ct);
        return 42;
    })
    .ExecuteAsync();

if (result.Succeeded)
{
    Console.WriteLine(result.Value);
}
else
{
    Console.WriteLine(result.Exception);
}
```

##### Sync execution
```csharp
var result = TryBuilder
    .Do(() => 10 / 2)
    .Execute();

Console.WriteLine(result.Value);
```

##### Catch Handlers (Typed)

You can attach multiple typed catch handlers.
They are executed in order of registration.
```csharp
var result = await TryBuilder
    .Do<int>(async ct =>
    {
        throw new TimeoutException();
    })
    .Catch<TimeoutException>(ex =>
    {
        Console.WriteLine("Timeout occurred");
    })
    .Catch<Exception>(ex =>
    {
        Console.WriteLine("General error");
    })
    .ExecuteAsync();
```

Notes:

Catch handlers **do not suppress execution**.

If you want to recover, use `Fallback`.

##### Finally Blocks (Multiple, LIFO)

You can register **multiple finally blocks**.
They execute in reverse order (stack-style).

```csharp
var result = await TryBuilder
    .Do(async ct =>
    {
        Console.WriteLine("Doing work");
        return 1;
    })
    .Finally(() => Console.WriteLine("Cleanup 1"))
    .Finally(() => Console.WriteLine("Cleanup 2"))
    .ExecuteAsync();
```
Execution order:

```xml
Doing work
Cleanup 2
Cleanup 1
```

Exceptions thrown inside `Finally` are swallowed by default.


##### Retry Support
**Basic retry**
```csharp
var result = await TryBuilder
    .Do(async ct =>
    {
        throw new IOException("temporary");
    })
    .Retry(new RetryOptions
    {
        MaxAttempts = 3
    })
    .ExecuteAsync();
```



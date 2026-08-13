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

---

### Cancellation (`CancellationToken`)

Every `async` method above has a **token-aware** sibling overload: the execute delegate becomes `Func<CancellationToken, Task<TResult>>` and a trailing `CancellationToken cancellationToken = default` parameter is added. These overloads are purely additive — the existing ones are unchanged.

```csharp
public class Foo : TryCatchExecuteBase
{
    public Task<IResult> SetFooAsync(CancellationToken cancellationToken = default)
        => TryToExecuteAsync(
            async ct =>
            {
                await SomeIoAsync(ct);       // the token flows INTO your delegate
                return Result.Success();
            },
            Result.Failure("Error"),
            cancellationToken: cancellationToken);
}
```

Semantics are consistent across the base classes, `TryRetryPolicy`, and `TryBuilder`:

- The token is checked (`ThrowIfCancellationRequested`) **before** the execute delegate runs, and is passed into the delegate.
- A canceled operation throws `OperationCanceledException` (this also covers `TaskCanceledException`). Cancellation is **never** routed to the `onFailureResult` / fallback value — it propagates to the caller.
- The `finally` block still runs on cancellation.
- Passing `default` / `CancellationToken.None` behaves exactly like the non-token overloads.

For example, the token-aware siblings of the two simplest overloads are:

- `Task<TResult> TryToExecuteAsync<TResult>(Func<CancellationToken, Task<TResult>> execFunc, TResult onFailureResult, bool forceCallGarbageCollector = false, CancellationToken cancellationToken = default)`
- `Task<TResult> TryToExecuteAsync<TResult>(Func<CancellationToken, Task<TResult>> execFunc, Func<TResult> onFailureResult, Func<TResult> finallyExecFunc, bool forceCallGarbageCollector = false, CancellationToken cancellationToken = default)`

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

Matching follows normal C# `catch`-clause rules: a handler matches by **assignability** (a base type catches its subclasses, and `Catch<Exception>` catches everything), and the **first** registered handler that matches wins. Register the most specific types first — a base-type handler registered before a more derived one will match first and the derived handler will never run.
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

Catch handlers are **not** run for cancellation: a canceled token throws `OperationCanceledException` straight out of `Execute`/`ExecuteAsync` (see the **Cancellation** section below), bypassing `Catch` and `Fallback`.

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
    .Retry(new TryRetryOptions
    {
        MaxAttempts = 3
    })
    .ExecuteAsync();
```

`Retry` accepts either a `TryRetryOptions` or a `TryRetryPolicy`. The convenience factories `TryRetryPolicy.Fixed(...)` and `TryRetryPolicy.Exponential(...)` cover the common backoff strategies (fixed / exponential, with optional jitter):

```csharp
var result = await TryBuilder
    .Do(async ct => await CallApiAsync(ct), cancellationToken)
    .Retry(TryRetryPolicy.Exponential(maxAttempts: 4, baseDelay: TimeSpan.FromMilliseconds(200)))
    .ExecuteAsync();
```

The retry backoff delay is cancellable: when the token is canceled, the wait is cut short and `OperationCanceledException` is thrown instead of running the remaining attempts. This holds for both `ExecuteAsync` and the synchronous `Execute`.

##### Cancellation

Pass a `CancellationToken` to `Do(...)`; it is stored on the builder and flows into the try block and into the `Catch`, `Finally`, and `Fallback` handlers that take a token.

```csharp
using var cts = new CancellationTokenSource();

var result = await TryBuilder
    .Do(async ct => await FooAsync(ct), cts.Token)
    .Finally(async ct => await CleanupAsync(ct))    // receives the same token
    .Fallback(async ct => await GetCachedAsync(ct)) // receives the same token
    .ExecuteAsync();
```

On cancellation the builder **throws** `OperationCanceledException` — it does **not** return a `TryResult`, and `Catch` / `Fallback` are skipped — while every `Finally` block still runs. A non-canceled failure behaves as before (handlers run, `Fallback` can recover, and the result is returned).



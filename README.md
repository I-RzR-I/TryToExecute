> **Note** This repository is developed from the beginning using .netstandard1.0.

| Name     | Details |
|----------|----------|
| TryToExecute | [![NuGet Version](https://img.shields.io/nuget/v/TryToExecute.svg?style=flat&logo=nuget)](https://www.nuget.org/packages/TryToExecute/) [![Nuget Downloads](https://img.shields.io/nuget/dt/TryToExecute.svg?style=flat&logo=nuget)](https://www.nuget.org/packages/TryToExecute)|

The repository represents an implementation of the `try/catch/finally` block. It can execute code without worrying about exceptions; it only specifies what to do in this case and delegate it in `sync` or `async` flow.

In the following flow on trying to implement try execution (`TryToExecute`, `TryToExecuteAsync`), you delegate:
- the base execution code `try { }`;
- code for the catch block `catch { }`;
- code for finally block `finally { }`.

To understand more efficiently how you can use available functionalities please consult the [using documentation/file](docs/usage.md).

**In case you wish to use it in your project, u can install the package from <a href="https://www.nuget.org/packages/TryToExecute" target="_blank">nuget.org</a>** or specify what version you want:

> `Install-Package TryToExecute -Version x.x.x.x`

## Content
1. [USING](docs/usage.md)
1. [CHANGELOG](docs/CHANGELOG.md)
1. [BRANCH-GUIDE](docs/branch-guide.md)
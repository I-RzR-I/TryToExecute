#region U S A G E S

using System.Reflection;

#if NETSTANDARD2_0_OR_GREATER
using System.Resources;
#endif

#endregion

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif

[assembly: AssemblyCompany("RzR SOFT & TECH ®")]
[assembly: AssemblyProduct("TryToExecute")]
[assembly: AssemblyCopyright("Copyright © 2022-2024 RzR All rights reserved.")]
[assembly: AssemblyTrademark("RzR SOFT Solution™")]
[assembly: AssemblyDescription("The 'TryToExecute' represents an implementation of the try/catch/finally block. It can execute code without worrying about exceptions; it only specifies what to do in this case and delegate it.")]

[assembly: AssemblyMetadata("TermsOfService", "")]
[assembly: AssemblyMetadata("ContactUrl", "")]
[assembly: AssemblyMetadata("ContactName", "RzR")]
[assembly: AssemblyMetadata("ContactEmail", "ddpRzR@hotmail.com")]
#if NETSTANDARD2_0_OR_GREATER
[assembly: NeutralResourcesLanguage("en-US", UltimateResourceFallbackLocation.MainAssembly)]
#endif
[assembly: AssemblyVersion("1.1.0.0")]
[assembly: AssemblyFileVersion("1.1.0.0")]
[assembly: AssemblyInformationalVersion("1.1.0.0")]

//-------------
// <copyright file="AssemblyInfo.cs" company="Uncas">
//     Copyright (c) Ole Lynge Sørensen. All rights reserved.
// </copyright>
//-------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Uncas.WebTester")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: CLSCompliant(true)]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("d2db8154-0380-40e9-94bf-b49a0ee3f92f")]

[module: SuppressMessage(
    "Microsoft.Design",
    "CA1020:AvoidNamespacesWithFewTypes",
    Scope = "namespace",
    Target = "Uncas.WebTester.Infrastructure")]
[module: SuppressMessage(
    "Microsoft.Design",
    "CA1020:AvoidNamespacesWithFewTypes",
    Scope = "namespace",
    Target = "Uncas.WebTester.Models")]
[module: SuppressMessage(
    "Microsoft.Design",
    "CA1020:AvoidNamespacesWithFewTypes",
    Scope = "namespace",
    Target = "Uncas.WebTester.Repositories")]
[module: SuppressMessage(
    "Microsoft.Design",
    "CA1020:AvoidNamespacesWithFewTypes",
    Scope = "namespace",
    Target = "Uncas.WebTester.Repositories.Sql")]
[module: SuppressMessage(
    "Microsoft.Design",
    "CA1020:AvoidNamespacesWithFewTypes",
    Scope = "namespace",
    Target = "Uncas.WebTester.Utilities")]
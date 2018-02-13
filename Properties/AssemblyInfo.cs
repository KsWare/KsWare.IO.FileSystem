﻿using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("KsWare.IO.FileSystem.NTFS")]
[assembly: AssemblyDescription("A file system library.")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("KsWare")]
[assembly: AssemblyProduct("KsWare.IO.FileSystem.NTFS")]
[assembly: AssemblyCopyright("Copyright © 2018 by KsWare. All rights reserved.")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("9cb5a457-df6f-4109-b272-86586cef4f3b")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]

[assembly: InternalsVisibleTo("KsWare.IO.FileSystem.PrivilegedExecutor")]
[assembly: InternalsVisibleTo("KsWare.IO.FileSystem.Tests")]

// ReSharper disable once CheckNamespace
namespace KsWare.IO.FileSystem {

	public static class AssemblyInfo { }

}

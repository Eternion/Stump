using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Windows.Markup;


[assembly: AssemblyTitle("WPF Application Framework (WAF)")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyProduct("WPF Application Framework (WAF)")]


[assembly: CLSCompliant(true)]
[assembly: ComVisible(false)]
[assembly: Guid("331cc379-bc8e-4129-8b6a-5698bf93b774")]


[assembly: XmlnsDefinition("http://waf.codeplex.com/schemas", "System.Waf.Presentation")]
[assembly: XmlnsDefinition("http://waf.codeplex.com/schemas", "System.Waf.Presentation.Converters")]


[assembly: InternalsVisibleTo("WpfApplicationFramework.Test")]
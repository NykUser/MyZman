using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;

[assembly: CLSCompliant(true)]
#if !NO_COM_ATTRIBUTES
[assembly: ComVisible(false)]
#endif
[assembly: AllowPartiallyTrustedCallers]
[assembly: AssemblyDelaySign(false)]

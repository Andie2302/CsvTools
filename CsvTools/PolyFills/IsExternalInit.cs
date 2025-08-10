// ReSharper disable CheckNamespace
// The namespace must be: namespace System.Runtime.CompilerServices
// This Code enable use of init-only Propertier in .net < 5  
#if !NET5_0_OR_GREATER
namespace System.Runtime.CompilerServices
{
    internal static class IsExternalInit { }
}
#endif

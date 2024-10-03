using System.Reflection;

namespace Core.MongoDB.OpenTelemetry;

internal static class SignalVersionHelper
{
    public static string GetVersion<T>()
    {
        return typeof(T).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()!.InformationalVersion.Split('+')[0];
    }
}
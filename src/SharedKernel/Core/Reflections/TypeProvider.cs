using System.Reflection;

namespace Core.Reflections
{
    public static class TypeProvider
    {
        public static Type? GetTypeFromReferenceAssembly(string typeName)
        {
            var allTypes = Assembly.GetEntryAssembly()?.GetTypes();
            return allTypes?.FirstOrDefault(x => x.Name == typeName || x.FullName == typeName);
        }
    }
}

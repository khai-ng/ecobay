using System.Reflection;

namespace Core.Contract
{
    public static class ContractExtensions
    {
        private static readonly Type[] contractTypes = Assembly.GetExecutingAssembly().GetTypes();
        public static Type? GetContractType(string typeName)
        {
            return contractTypes
                .Where(t => t.Name == typeName)
                .FirstOrDefault();
        }
    }
}

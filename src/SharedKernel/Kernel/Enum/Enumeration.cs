using System;
using System.Reflection;

namespace SharedKernel.Kernel.Enum
{
    public abstract class Enumeration<TEnum> : IEquatable<Enumeration<TEnum>>
        where TEnum : Enumeration<TEnum>
    {

        public int Id { get; protected set; }
        public string Name { get; protected set; } = string.Empty;

        public Enumeration(int id, string name) 
        {
            Id = id;
            Name = name;
        }

        private static readonly Dictionary<int, TEnum> Enumerations = CreateEnumeration();
        private static Dictionary<int, TEnum> CreateEnumeration()
        {
            var enumType = typeof(TEnum);

            var fieldType = enumType.GetFields(
                BindingFlags.Public |
                BindingFlags.Static |
                BindingFlags.FlattenHierarchy)
                .Where(fieldInfo => enumType.IsAssignableFrom(fieldInfo.FieldType))
                .Select(fieldInfo => (TEnum)fieldInfo.GetValue(default)!);
            return fieldType.ToDictionary(x => x.Id);
        }

        public static IReadOnlyCollection<TEnum> GetValues() => Enumerations.Values;
        public static TEnum? FromValue(int value) =>
            Enumerations.TryGetValue(value, out TEnum? enumeration) ? enumeration : default;
        public static TEnum? FromName(string name) =>
            Enumerations.Values.SingleOrDefault(e => e.Name == name);

        public bool Equals(Enumeration<TEnum>? other)
        {
            if(other is null) return false;

            return GetType().Equals(other.GetType()) && Id == other.Id;
        }

        public override bool Equals(object? obj)
        {
            return obj is Enumeration<TEnum> other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return Name.ToString();
        }

        
    }
}

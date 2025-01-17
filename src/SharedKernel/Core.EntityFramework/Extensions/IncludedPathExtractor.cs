using Core.Entities;
using System.Linq.Expressions;

namespace Core.EntityFramework.Extensions
{
    public static class IncludedPathExtractor
    {
        public static IEnumerable<string> Extract<TSource, TDestination>(Expression<Func<TSource, TDestination>> expression)
        {
            var memberAssignments = new HashSet<string>();
            ProcessExpression(expression.Body, memberAssignments);
            return memberAssignments;
        }

        private static void ProcessExpression(Expression expr, HashSet<string> memberAssignments)
        {
            switch (expr)
            {
                case MemberInitExpression memberInit:
                    foreach (var binding in memberInit.Bindings.OfType<MemberAssignment>())
                    {
                        ProcessExpression(binding.Expression, memberAssignments);
                    }
                    break;

                case NewExpression newExpr:
                    foreach (var arg in newExpr.Arguments)
                    {
                        ProcessExpression(arg, memberAssignments);
                    }
                    break;

                case MethodCallExpression methodCall:
                    foreach (var arg in methodCall.Arguments)
                    {
                        ProcessExpression(arg, memberAssignments);
                    }
                    if (methodCall.Object != null)
                    {
                        ProcessExpression(methodCall.Object, memberAssignments);
                    }
                    break;

                case MemberExpression memberExpr:
                    ProcessMemberExpression(memberExpr, "", memberAssignments);
                    break;

                case UnaryExpression unaryExpr:
                    ProcessExpression(unaryExpr.Operand, memberAssignments);
                    break;

                case BinaryExpression binaryExpr:
                    // Handle operations like concatenation, arithmetic, etc.
                    ProcessExpression(binaryExpr.Left, memberAssignments);
                    ProcessExpression(binaryExpr.Right, memberAssignments);
                    break;

                case ConditionalExpression conditionalExpr:
                    // Handle ternary operators
                    ProcessExpression(conditionalExpr.Test, memberAssignments);
                    ProcessExpression(conditionalExpr.IfTrue, memberAssignments);
                    ProcessExpression(conditionalExpr.IfFalse, memberAssignments);
                    break;

                case TypeBinaryExpression typeBinaryExpr:
                    // Handle type checks (is operator)
                    ProcessExpression(typeBinaryExpr.Expression, memberAssignments);
                    break;

                case InvocationExpression invocationExpr:
                    // Handle delegate invocations
                    ProcessExpression(invocationExpr.Expression, memberAssignments);
                    foreach (var arg in invocationExpr.Arguments)
                    {
                        ProcessExpression(arg, memberAssignments);
                    }
                    break;

                case ListInitExpression listInit:
                    // Handle collection initializers
                    ProcessExpression(listInit.NewExpression, memberAssignments);
                    foreach (var init in listInit.Initializers)
                    {
                        foreach (var arg in init.Arguments)
                        {
                            ProcessExpression(arg, memberAssignments);
                        }
                    }
                    break;
            }
        }

        private static void ProcessMemberExpression(MemberExpression memberExpr, string currentPath, HashSet<string> propertyPaths)
        {
            var memberType = memberExpr.Type;
            var isNotSupportedIncludeType = IsPrimitiveOrString(memberType);

            string newPath = !isNotSupportedIncludeType
                ? string.IsNullOrEmpty(currentPath)
                    ? memberExpr.Member.Name
                    : memberExpr.Member.Name + "." + currentPath
                : "";

            if (memberExpr.Expression is MemberExpression parentMember)
            {
                ProcessMemberExpression(parentMember, newPath, propertyPaths);
            }
            else if (memberExpr.Expression is ParameterExpression)
            {
                if (!isNotSupportedIncludeType && !IsExistPath(propertyPaths, newPath))
                {
                    propertyPaths.Add(newPath);
                }
            }
        }

        private static bool IsPrimitiveOrString(Type type)
        {
            return type.IsPrimitive ||
                   type == typeof(string) ||
                   type == typeof(decimal) ||
                   type == typeof(DateTime) ||
                   type == typeof(Guid) ||
                   (type.BaseType != null &&
                       type.BaseType.IsGenericType &&
                       type.BaseType.GetGenericTypeDefinition() == typeof(Enumeration<>)) ||
                   type.IsEnum;
        }

        private static bool IsExistPath(ICollection<string> propertyPaths, string property)
        {
            foreach (var element in propertyPaths)
            {
                if (element.Contains(property)) return true;
            }
            return false;
        }
    }
}

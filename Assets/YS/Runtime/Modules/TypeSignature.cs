using System;
using System.Collections.Generic;
using System.Linq;
namespace YS.Modules {
   
    public static class TypeSignature {

        static Dictionary<Type, (string Namespace, string Name,string FullName)> typeDictionary =
            new (100) {
                {typeof(byte), (null,"byte", "byte")},
                {typeof(sbyte), (null,"sbyte", "sbyte")},
                {typeof(bool), (null,"bool", "bool")},
                {typeof(short), (null,"short", "short")},
                {typeof(ushort), (null,"ushort", "ushort")},
                {typeof(char), (null,"char", "char")},
                {typeof(int), (null,"int", "int")},
                {typeof(uint), (null,"uint", "uint")},
                {typeof(float), (null,"float", "float")},
                {typeof(double), (null,"double", "double")},
                {typeof(long), (null,"long", "long")},
                {typeof(ulong), (null,"ulong", "ulong")},
                {typeof(object), (null,"object", "object")},
                {typeof(string), (null,"string", "string")},
                {typeof(decimal), (null,"decimal", "decimal")}
            };
        static Dictionary<Type,  string > SpecialNameDict =
            new (100) {
                {typeof(byte), "byte"},
                {typeof(sbyte), "sbyte"},
                {typeof(bool), "bool"},
                {typeof(short), "short"},
                {typeof(ushort), "ushort"},
                {typeof(char), "char"},
                {typeof(int), "int"},
                {typeof(uint), "uint"},
                {typeof(float), "float"},
                {typeof(double), "double"},
                {typeof(long), "long"},
                {typeof(ulong), "ulong"},
                {typeof(object), "object"},
                {typeof(string), "string"},
                {typeof(decimal), "decimal"}
            };
        public static bool TryGetSpecialName(Type type, out string name) {
            return (SpecialNameDict.TryGetValue(type, out name));
        }
         static bool IsNullable(this Type type, out Type underlyingType)
        {
            underlyingType = Nullable.GetUnderlyingType(type);
            return underlyingType != null;
        }

         public static (string Namespace, string Name, string FullName) GetNameFull(this Type type) {
              type.Build();
              return typeDictionary[type];
         }

         public static string BuildFullName(this Type type,bool useNestedPlus=false) {
             if (type.IsNullable(out var underlyingType)) {
                 var underlyingName = underlyingType.BuildFullName();
                 return underlyingName + "?";
             }
             if (!type.IsConstructedGenericType&&typeDictionary.TryGetValue(type, out var tuple)) {
                 return tuple.FullName;
             }
             if (type.IsArray) {
                 var elementType = type.GetElementType();
                 elementType.Build();
                 tuple = typeDictionary[elementType];
                 var fullname = tuple.FullName + "[]";
                 typeDictionary[type] = (tuple.Namespace, tuple.Name+"[]",fullname );
                 return fullname;
             }
             var nameSpace = type.Namespace;
            
             var signature = string.IsNullOrWhiteSpace(type.FullName)
                 ? type.Name
                 : type.FullName;
             if (!useNestedPlus&&type.IsNested) {
                 signature=signature.Replace('+', '.');
             }
             if (type.IsConstructedGenericType) {
                 signature = signature.Split('`')[0] + "<" + string.Join(", ", type.GetGenericArguments()
                     .Select((t)=>BuildFullName(t,useNestedPlus))) + ">";
                 return signature;
             }
             if (!string.IsNullOrEmpty(nameSpace)) {
                 var name= signature.Remove(0, nameSpace.Length+1);
                 typeDictionary[type] = (nameSpace, name, signature);
                 return signature;
             }
             var fullName="(global::)." + signature;
             typeDictionary[type] = ("(global::)", signature,fullName);
             return fullName;
         }
        public static string Build(this Type type)
        {
            if (type.IsNullable(out var underlyingType)) {
                var underlyingName = underlyingType.Build();
                return underlyingName + "?";
            }
            if (typeDictionary.TryGetValue(type, out var tuple)) {
                return tuple.FullName;
            }

            if (type.IsArray) {
                var elementType = type.GetElementType();
                elementType.Build();
                tuple = typeDictionary[elementType];
                var fullname = tuple.FullName + "[]";
                typeDictionary[type] = (tuple.Namespace, tuple.Name+"[]",fullname );
                return fullname;
            }
            var nameSpace = type.Namespace;
            
            var signature = string.IsNullOrWhiteSpace(type.FullName)
                ? type.Name
                : type.FullName;
            if (type.IsConstructedGenericType)
                signature= signature.Split('`')[0] + "<" + string.Join(", ", type.GetGenericArguments()
                    .Select(BuildWithOutNameSpace)) + ">";
            if (!string.IsNullOrEmpty(nameSpace)) {
                var name= signature.Remove(0, nameSpace.Length+1);
                typeDictionary[type] = (nameSpace, name, signature);
                return signature;
            }
            var fullName="(global::)." + signature;
            typeDictionary[type] = ("(global::)", signature,fullName);
            return fullName;
        }
       public static string BuildWithOutNameSpace(this Type type)
        {
            if (type.IsNullable(out var underlyingType)) {
                var underlyingName = underlyingType.BuildWithOutNameSpace();
                return underlyingName + "?";
            }
            if (typeDictionary.TryGetValue(type, out var tuple)) {
                return tuple.FullName;
            }

            if (type.IsArray) {
                var elementType = type.GetElementType();
                elementType.Build();
                tuple = typeDictionary[elementType];
                var fullname = tuple.FullName + "[]";
                typeDictionary[type] = (tuple.Namespace, tuple.Name+"[]",fullname );
                return fullname;
            }
            var nameSpace = type.Namespace;
            
            var signature = string.IsNullOrWhiteSpace(type.FullName)
                ? type.Name
                : type.FullName;
            if (type.IsConstructedGenericType)
                signature= signature.Split('`')[0] + "<" + string.Join(", ", type.GetGenericArguments()
                    .Select(BuildWithOutNameSpace)) + ">";
            if (!string.IsNullOrEmpty(nameSpace)) {
                var name= signature.Remove(0, nameSpace.Length+1);
                typeDictionary[type] = (nameSpace, name, signature);
                return name;
            }
            var fullName="(global::)." + signature;
            typeDictionary[type] = ("(global::)", signature,fullName);
            return signature;
        }

    }
}
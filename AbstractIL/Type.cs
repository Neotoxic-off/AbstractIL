using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection.Metadata;

namespace AbstractIL
{
    public static class TypeService
    {
        public static TypeDef Find(ModuleDefMD module, string name)
        {
            return module.Find(name, false);
        }

        public static TypeDef? Find(ModuleDefMD module, string name, string _namespace)
        {
            return module.Types.FirstOrDefault(t => t.Namespace == _namespace && t.Name == name);
        }

        public static IEnumerable<TypeDef> FindByAttribute(ModuleDefMD module, string attributeName)
        {
            return module.Types.Where(t => t.CustomAttributes.Any(a => a.TypeFullName.Contains(attributeName)));
        }

        public static IEnumerable<TypeDef> FindByBaseType(ModuleDefMD module, string baseTypeName)
        {
            return module.Types.Where(t => t.BaseType?.FullName == baseTypeName);
        }

        public static IEnumerable<TypeDef> GetAll(ModuleDefMD module)
        {
            return module.Types;
        }
    }
}
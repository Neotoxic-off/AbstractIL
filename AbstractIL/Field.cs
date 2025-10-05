using dnlib.DotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractIL
{
    public static class Field
    {
        public static FieldDef? Find(TypeDef type, string name)
        {
            return type.Fields.FirstOrDefault(f => f.Name == name);
        }

        public static IEnumerable<FieldDef> FindByType(TypeDef type, string typeName)
        {
            return type.Fields.Where(f => f.FieldType.FullName == typeName);
        }

        public static IEnumerable<FieldDef> GetAll(TypeDef type)
        {
            return type.Fields;
        }

        public static IEnumerable<FieldDef> GetStatic(TypeDef type)
        {
            return type.Fields.Where(f => f.IsStatic);
        }
    }
}

using dnlib.DotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractIL
{
    public static class AttributeService
    {
        public static bool Has(IMemberDef member, string attributeName)
        {
            return member.CustomAttributes.Any(a => a.TypeFullName.Contains(attributeName));
        }

        public static void Remove(IMemberDef member, string attributeName)
        {
            var attrs = member.CustomAttributes.Where(a => a.TypeFullName.Contains(attributeName)).ToList();
            foreach (var attr in attrs)
            {
                member.CustomAttributes.Remove(attr);
            }
        }

        public static void RemoveAll(IMemberDef member)
        {
            member.CustomAttributes.Clear();
        }

        public static IEnumerable<CustomAttribute> Get(IMemberDef member)
        {
            return member.CustomAttributes;
        }
    }
}

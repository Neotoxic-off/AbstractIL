using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection.Metadata;

namespace AbstractIL
{
    public static class AssemblyService
    {
        public static ModuleDefMD Load(string assemblyPath)
        {
            return ModuleDefMD.Load(assemblyPath);
        }

        public static void Save(ModuleDefMD module, string outputPath)
        {
            module.Write(outputPath);
        }

        public static string GetVersion(ModuleDefMD module)
        {
            return module.Assembly.Version.ToString();
        }

        public static void SetVersion(ModuleDefMD module, Version version)
        {
            module.Assembly.Version = version;
        }
    }
}
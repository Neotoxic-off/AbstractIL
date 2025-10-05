using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;

namespace AbstractIL
{
    public static class MethodService
    {
        private static readonly Dictionary<Type, Action<MethodDef, object>> _typeHandlers = new()
        {
            { typeof(bool), (method, value) => InstructionService.Add(method, (bool)value ? OpCodes.Ldc_I4_1 : OpCodes.Ldc_I4_0) },
            { typeof(int), (method, value) => InstructionService.Add(method, OpCodes.Ldc_I4, (int)value) },
            { typeof(string), (method, value) => InstructionService.Add(method, OpCodes.Ldstr, (string)value) }
        };


        public static MethodDef? Find(TypeDef type, string name, bool isStatic, int parameters)
        {
            return type.Methods.FirstOrDefault(m =>
                m.Name == name &&
                m.IsStatic == isStatic &&
                m.Parameters.Count == parameters
            );
        }

        public static MethodDef? Find(TypeDef type, string name)
        {
            return type.Methods.FirstOrDefault(m => m.Name == name);
        }

        public static IEnumerable<MethodDef> FindByAttribute(TypeDef type, string attributeName)
        {
            return type.Methods.Where(m => m.CustomAttributes.Any(a => a.TypeFullName.Contains(attributeName)));
        }

        public static MethodDef? FindConstructor(TypeDef type, int parameters)
        {
            return type.Methods.FirstOrDefault(m => m.IsConstructor && m.Parameters.Count == parameters);
        }

        public static IEnumerable<MethodDef> GetAll(TypeDef type)
        {
            return type.Methods;
        }

        public static IEnumerable<MethodDef> GetPublic(TypeDef type)
        {
            return type.Methods.Where(m => m.IsPublic);
        }

        public static IEnumerable<MethodDef> GetStatic(TypeDef type)
        {
            return type.Methods.Where(m => m.IsStatic);
        }

        public static void Clear(MethodDef method)
        {
            method.Body.Instructions.Clear();
            method.Body.ExceptionHandlers.Clear();
        }

        public static void ReplaceBody(MethodDef method, Action<MethodDef> bodyBuilder)
        {
            Clear(method);
            bodyBuilder(method);
            Finalize(method);
        }

        public static void MakeReturnValue(MethodDef method, object value)
        {
            Clear(method);

            if (value == null)
            {
                InstructionService.Add(method, OpCodes.Ldnull);
            }
            else if (_typeHandlers.TryGetValue(value.GetType(), out var handler))
            {
                handler(method, value);
            }
            else
            {
                throw new ArgumentException($"Type non supporté: {value.GetType()}");
            }

            InstructionService.Add(method, OpCodes.Ret);
            Finalize(method);
        }

        public static void MakeEmpty(MethodDef method)
        {
            Clear(method);
            InstructionService.Add(method, OpCodes.Ret);
            Finalize(method);
        }

        public static void Finalize(MethodDef method)
        {
            method.Body.SimplifyBranches();
            method.Body.OptimizeBranches();
        }

        public static MethodDef Clone(MethodDef source, string newName)
        {
            var cloned = new MethodDefUser(
                newName,
                source.MethodSig,
                source.ImplAttributes,
                source.Attributes
            );

            cloned.Body = new CilBody();
            foreach (var instr in source.Body.Instructions)
            {
                cloned.Body.Instructions.Add(instr);
            }

            foreach (var local in source.Body.Variables)
            {
                cloned.Body.Variables.Add(local);
            }

            return cloned;
        }

        public static bool IsEmpty(MethodDef method)
        {
            return method.Body == null ||
                   method.Body.Instructions.Count == 0 ||
                   (method.Body.Instructions.Count == 1 && method.Body.Instructions[0].OpCode == OpCodes.Ret);
        }

        public static bool Calls(MethodDef method, string targetMethodName)
        {
            return method.Body.Instructions.Any(i =>
                (i.OpCode == OpCodes.Call || i.OpCode == OpCodes.Callvirt) &&
                i.Operand is IMethod m &&
                m.Name == targetMethodName
            );
        }

        public static bool HasExceptionHandlers(MethodDef method)
        {
            return method.Body?.ExceptionHandlers.Count > 0;
        }

        public static int CountMethodCalls(MethodDef method)
        {
            return method.Body.Instructions.Count(i =>
                i.OpCode == OpCodes.Call ||
                i.OpCode == OpCodes.Callvirt
            );
        }
    }
}
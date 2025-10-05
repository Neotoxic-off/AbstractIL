using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractIL
{
    public static class InstructionService
    {
        public static void InsertAt(MethodDef method, int index, OpCode opcode)
        {
            method.Body.Instructions.Insert(index, Instruction.Create(opcode));
        }

        public static void RemoveAt(MethodDef method, int index)
        {
            method.Body.Instructions.RemoveAt(index);
        }

        public static void Replace(MethodDef method, Instruction oldInstr, Instruction newInstr)
        {
            var index = method.Body.Instructions.IndexOf(oldInstr);
            if (index >= 0)
            {
                method.Body.Instructions[index] = newInstr;
            }
        }

        public static int Count(MethodDef method)
        {
            return method.Body?.Instructions.Count ?? 0;
        }

        public static IEnumerable<Instruction> Find(MethodDef method, OpCode opcode)
        {
            return method.Body.Instructions.Where(i => i.OpCode == opcode);
        }

        public static IEnumerable<Instruction> FindString(MethodDef method, string value)
        {
            return method.Body.Instructions.Where(i =>
                i.OpCode == OpCodes.Ldstr &&
                i.Operand is string s &&
                s == value
            );
        }

        public static IEnumerable<Instruction> FindCall(MethodDef method, string methodName)
        {
            return method.Body.Instructions.Where(i =>
                (i.OpCode == OpCodes.Call || i.OpCode == OpCodes.Callvirt) &&
                i.Operand is IMethod m &&
                m.Name == methodName
            );
        }

        public static IEnumerable<string> GetAllStrings(MethodDef method)
        {
            return method.Body.Instructions
                .Where(i => i.OpCode == OpCodes.Ldstr && i.Operand is string)
                .Select(i => (string)i.Operand);
        }

        public static void Add(MethodDef method, OpCode opcode)
        {
            method.Body.Instructions.Add(Instruction.Create(opcode));
        }

        public static void Add(MethodDef method, OpCode opcode, int operand)
        {
            method.Body.Instructions.Add(Instruction.Create(opcode, operand));
        }

        public static void Add(MethodDef method, OpCode opcode, FieldDef operand)
        {
            method.Body.Instructions.Add(Instruction.Create(opcode, operand));
        }

        public static void Add(MethodDef method, OpCode opcode, MethodDef operand)
        {
            method.Body.Instructions.Add(Instruction.Create(opcode, operand));
        }

        public static void Add(MethodDef method, OpCode opcode, string operand)
        {
            method.Body.Instructions.Add(Instruction.Create(opcode, operand));
        }

        public static void Add(MethodDef method, OpCode opcode, TypeDef operand)
        {
            method.Body.Instructions.Add(Instruction.Create(opcode, operand));
        }

        public static void Add(MethodDef method, OpCode opcode, Instruction operand)
        {
            method.Body.Instructions.Add(Instruction.Create(opcode, operand));
        }
    }
}

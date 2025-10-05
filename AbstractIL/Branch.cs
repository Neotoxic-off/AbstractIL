using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractIL
{

    public static class BranchService
    {
        public static Instruction CreateLabel(MethodDef method)
        {
            var nop = Instruction.Create(OpCodes.Nop);
            method.Body.Instructions.Add(nop);
            return nop;
        }

        public static void Add(MethodDef method, OpCode branchOpcode, Instruction target)
        {
            method.Body.Instructions.Add(Instruction.Create(branchOpcode, target));
        }

        public static void AddConditional(MethodDef method, Instruction trueTarget, Instruction falseTarget)
        {
            InstructionService.Add(method, OpCodes.Brtrue, trueTarget);
            InstructionService.Add(method, OpCodes.Br, falseTarget);
        }
    }
}

using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractIL
{
    public static class SignatureService
    {
        public static void Sign(ModuleDefMD module, string signature)
        {
            TypeDef globalType = module.GlobalType;
            MethodDef? cctor = MethodService.Find(globalType, ".cctor");
            FieldDef? signatureField = null;

            if (cctor == null)
            {
                cctor = CreateStaticConstructor(module, globalType);
            }

            signatureField = CreateSignatureField(module, globalType);

            InsertSignatureCode(cctor, signatureField, signature);
            EnsureRetInstruction(cctor);

            MethodService.Finalize(cctor);
        }

        private static MethodDef CreateStaticConstructor(ModuleDefMD module, TypeDef globalType)
        {
            MethodDefUser cctor = new MethodDefUser(
                ".cctor",
                MethodSig.CreateStatic(module.CorLibTypes.Void),
                MethodImplAttributes.IL | MethodImplAttributes.Managed,
                MethodAttributes.Private | MethodAttributes.Static |
                MethodAttributes.SpecialName | MethodAttributes.RTSpecialName
            );

            cctor.Body = new CilBody();
            globalType.Methods.Add(cctor);

            return cctor;
        }

        private static FieldDef CreateSignatureField(ModuleDefMD module, TypeDef globalType)
        {
            FieldDefUser field = new FieldDefUser(
                "Patcher",
                new FieldSig(module.CorLibTypes.String),
                FieldAttributes.Public | FieldAttributes.Static
            );

            globalType.Fields.Add(field);

            return field;
        }

        private static void InsertSignatureCode(MethodDef method, FieldDef field, string signature)
        {
            method.Body.Instructions.Insert(0, Instruction.Create(OpCodes.Ldstr, signature));
            method.Body.Instructions.Insert(1, Instruction.Create(OpCodes.Stsfld, field));
        }

        private static void EnsureRetInstruction(MethodDef method)
        {
            IList<Instruction> instructions = method.Body.Instructions;

            if (instructions.Count == 0 || instructions.Last().OpCode != OpCodes.Ret)
            {
                InstructionService.Add(method, OpCodes.Ret);
            }
        }
    }
}

using System;
using PostSharp.Sdk.AspectInfrastructure;
using PostSharp.Sdk.AspectWeaver;
using PostSharp.Sdk.AspectWeaver.Transformations;
using PostSharp.Sdk.CodeModel;
using PostSharp.Sdk.Collections;

namespace Padre.PostSharp.Weaver
{
    internal class OnThrowTransformation : MethodBodyTransformation
    {
        private Assets assets;

        public OnThrowTransformation(AspectWeaver aspectWeaver)
            : base(aspectWeaver)
        {
            var module = AspectInfrastructureTask.Project.Module;
            assets = module.Cache.GetItem(() => new Assets(module));
        }

        public override string GetDisplayName(MethodSemantics semantic)
        {
            return "FastTrace";
        }

        public AspectWeaverTransformationInstance CreateInstance(AspectWeaverInstance aspectWeaverInstance)
        {
            return new Instance(this, aspectWeaverInstance);
        }

        private sealed class Instance : MethodBodyTransformationInstance
        {
            public Instance(MethodBodyTransformation parent, AspectWeaverInstance aspectWeaverInstance)
                : base(parent, aspectWeaverInstance)
            {
            }

            public override MethodBodyTransformationOptions GetOptions(MetadataDeclaration originalTargetElement, MethodSemantics semantic)
            {
                return MethodBodyTransformationOptions.None;
            }

            public override void Implement(MethodBodyTransformationContext context)
            {
                context.InstructionBlock.MethodBody.Visit(new[] { new Visitor(this, context) });
            }

            private class Visitor : IMethodBodyVisitor
            {
                private readonly Instance instance;
                private readonly MethodBodyTransformationContext context;

                public Visitor(Instance instance, MethodBodyTransformationContext context)
                {
                    this.instance = instance;
                    this.context = context;
                }

                #region Implementation of IMethodBodyVisitor

                public void EnterInstructionBlock(InstructionBlock instructionBlock, InstructionBlockExceptionHandlingKind exceptionHandlingKind)
                {
                }

                public void LeaveInstructionBlock(InstructionBlock instructionBlock, InstructionBlockExceptionHandlingKind exceptionHandlingKind)
                {
                }

                public void EnterInstructionSequence(InstructionSequence instructionSequence)
                {
                }

                public void LeaveInstructionSequence(InstructionSequence instructionSequence)
                {
                }

                public void EnterExceptionHandler(ExceptionHandler exceptionHandler)
                {
                }

                public void LeaveExceptionHandler(ExceptionHandler exceptionHandler)
                {
                }

                #endregion

                public void VisitInstruction(InstructionReader reader)
                {
                    if (reader.CurrentInstruction.OpCodeNumber == OpCodeNumber.Throw)
                    {
                        var assets = ((OnThrowTransformation)instance.Transformation).assets;

                        context.InstructionBlock.MethodBody.InitLocalVariables = true;
                        var variable = reader.CurrentInstructionBlock.DefineLocalVariable(assets.Exception, "");

                        InstructionSequence before, after;
                        reader.CurrentInstructionSequence.SplitAroundReaderPosition(reader, out before, out after);
                        var sequence = reader.CurrentInstructionBlock.AddInstructionSequence(null, NodePosition.After, before);

                        var writer = new InstructionWriter();
                        writer.AttachInstructionSequence(sequence);
                        writer.EmitInstructionLocalVariable(OpCodeNumber.Stloc, variable);
                        instance.AspectWeaverInstance.AspectRuntimeInstanceField.EmitLoadField(writer, context.MethodMapping.CreateWriter());
                        writer.EmitInstructionLocalVariable(OpCodeNumber.Ldloc, variable);
                        writer.EmitInstructionMethod(OpCodeNumber.Callvirt, assets.OnThrow);
                        writer.EmitInstructionLocalVariable(OpCodeNumber.Ldloc, variable);
                        writer.DetachInstructionSequence();
                    }
                }
            }
        }

        private sealed class Assets
        {
            public readonly IMethod OnThrow;
            public readonly ITypeSignature Exception;

            public Assets(ModuleDeclaration module)
            {
                OnThrow = module.FindMethod(typeof(OnThrowAspectAttribute).GetMethod("OnThrow", new[] { typeof(Exception) }), BindingOptions.Default);
                Exception = module.FindType(typeof(Exception));
            }
        }
    }
}
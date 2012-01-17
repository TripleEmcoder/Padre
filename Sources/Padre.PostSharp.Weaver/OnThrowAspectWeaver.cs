using PostSharp.Aspects.Configuration;
using PostSharp.Extensibility;
using PostSharp.Sdk.AspectWeaver;
using PostSharp.Sdk.CodeModel;

namespace Padre.PostSharp.Weaver
{
    internal class OnThrowAspectWeaver : AspectWeaver
    {
        private static readonly AspectConfigurationAttribute defaultConfiguration = new AspectConfigurationAttribute();
        private OnThrowTransformation transformation;

        public OnThrowAspectWeaver()
            : base(
                defaultConfiguration, ReflectionObjectBuilder.Dynamic,
                MulticastTargets.Method | MulticastTargets.InstanceConstructor | MulticastTargets.StaticConstructor | MulticastTargets.Field)
        {
            RequiresRuntimeInstance = true;
            RequiresRuntimeReflectionObject = false;
        }

        protected override void Initialize()
        {
            base.Initialize();

            transformation = new OnThrowTransformation(this);
            ApplyWaivedEffects(this.transformation);
        }

        protected override AspectWeaverInstance CreateAspectWeaverInstance(AspectInstanceInfo aspectInstanceInfo)
        {
            return new Instance(this, aspectInstanceInfo);
        }

        private class Instance : AspectWeaverInstance
        {
            public Instance(AspectWeaver aspectWeaver, AspectInstanceInfo aspectInstanceInfo)
                : base(aspectWeaver, aspectInstanceInfo)
            {
            }

            public override void ProvideAspectTransformations(AspectWeaverTransformationAdder adder)
            {
                adder.Add(this.TargetElement, ((OnThrowAspectWeaver)this.AspectWeaver).transformation.CreateInstance(this));
            }
        }
    }
}
using System;
using PostSharp.Aspects;
using PostSharp.Extensibility;

namespace Padre.PostSharp
{
    [MulticastAttributeUsage(MulticastTargets.Method | MulticastTargets.InstanceConstructor | MulticastTargets.StaticConstructor)]
    [RequirePostSharp("Padre.PostSharp", "Padre.PostSharp")]
    [Serializable]
    public abstract class OnThrowAspectAttribute : MulticastAttribute, IAspect
    {
        public abstract void OnThrow(Exception exception);
    }
}
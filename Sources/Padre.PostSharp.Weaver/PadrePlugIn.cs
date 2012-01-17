using PostSharp.Sdk.AspectWeaver;

namespace Padre.PostSharp.Weaver
{
    public class PadrePlugIn : AspectWeaverPlugIn
    {
        public PadrePlugIn()
            : base(StandardPriorities.User)
        {
        }

        protected override void Initialize()
        {
            BindAspectWeaver<OnThrowAspectAttribute, OnThrowAspectWeaver>();
        }
    }
}
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;

namespace Padre
{
    [Export(typeof(IProcessHandler))]
    public class WebDevHandler : IProcessHandler
    {
        private readonly IProcessAttach attach;

        [ImportingConstructor]
        public WebDevHandler(IProcessAttach attach)
        {
            this.attach = attach;
        }

        public void Handle(Process process)
        {
            if (process.ProcessName.StartsWith("WebDev.WebServer"))
                attach.Attach(process);
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;

namespace Padre
{
    [Export(typeof(IWorker))]
    public class ProcessListener : IWorker
    {
        private readonly IEnumerable<IProcessHandler> handlers;

        [ImportingConstructor]
        public ProcessListener([ImportMany] IEnumerable<IProcessHandler> handlers)
        {
            this.handlers = handlers;
        }

        public void Start()
        {
            foreach (var process in Process.GetProcesses())
                foreach (var handler in handlers)
                    handler.Handle(process);
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}
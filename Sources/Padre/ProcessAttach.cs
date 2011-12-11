using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Samples.Debugging.MdbgEngine;

namespace Padre
{
    [Export(typeof(IProcessAttach))]
    [Export(typeof(IWorker))]
    public class ProcessAttach : IProcessAttach, IWorker
    {
        private readonly ILogSink log;
        private readonly IEnumerable<IDebugAttach> attaches;
        private readonly MDbgEngine engine;

        [ImportingConstructor]
        public ProcessAttach(ILogSink log, [ImportMany] IEnumerable<IDebugAttach> attaches)
        {
            this.log = log;
            this.attaches = attaches;
            engine = new MDbgEngine();
        }

        public void Attach(Process process)
        {
            log.Log("Attaching to {0} ({1}).", process.ProcessName, process.Id);
            var debug = engine.Attach(process.Id);
            foreach (var attach in attaches)
                attach.Attach(debug);

            debug.Go().WaitOne();

            if (!(debug.StopReason is AttachCompleteStopReason))
                throw new Exception("Unexpected");
            
            debug.Go();
        }

        public void Start()
        {
        }

        public void Stop()
        {
            foreach (MDbgProcess process in engine.Processes)
            {
                log.Log("Detaching from {0} ({1}).", process.Name, process.CorProcess.Id);
                process.Detach();
            }
        }
    }
}

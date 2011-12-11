using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Microsoft.Samples.Debugging.CorDebug;
using Microsoft.Samples.Debugging.CorDebug.NativeApi;
using Microsoft.Samples.Debugging.MdbgEngine;

namespace Padre
{
    [Export(typeof(IDebugAttach))]
    public class ExceptionHandler : IDebugAttach
    {
        private readonly ILogSink log;

        [ImportingConstructor]
        public ExceptionHandler(ILogSink log)
        {
            this.log = log;
        }

        public void Attach(MDbgProcess process)
        {
            process.PostDebugEvent +=
                (sender, e) =>
                {
                    log.Log(e.CallbackType.ToString());

                    if (e.CallbackType == ManagedCallbackType.OnException2)
                    {
                        var ce = (CorException2EventArgs)e.CallbackArgs;

                        if (ce.EventType == CorDebugExceptionCallbackType.DEBUG_EXCEPTION_FIRST_CHANCE)
                        {
                            var thread = process.Threads.Lookup(ce.Thread);

                            foreach (var frame in thread.Frames)
                            {
                                if (!frame.IsManaged || frame.Function.FullName.StartsWith("System."))
                                    break;

                                log.Log("{0}({1})", frame.Function.FullName, string.Join(", ", frame.Function.GetArguments(frame).Select(
                                    arg => string.Format("{0} = {1}", arg.Name, arg.GetStringValue(false)))));
                            }
                        }
                    }
                };
        }
    }
}

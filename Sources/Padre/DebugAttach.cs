using System.ComponentModel.Composition;
using Microsoft.Samples.Debugging.MdbgEngine;

namespace Padre
{
    [Export(typeof(IDebugAttach))]
    public class DebugAttach : IDebugAttach
    {
        public void Attach(MDbgProcess process)
        {
            //foreach (MDbgModule module in process.Modules)
            //    module.

            //process.SymbolPath = 
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Samples.Debugging.MdbgEngine;

namespace Padre
{
    public interface IDebugAttach
    {
        void Attach(MDbgProcess process);
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Padre
{
    public interface IProcessAttach
    {
        void Attach(Process process);
    }
}

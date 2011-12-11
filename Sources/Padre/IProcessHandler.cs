using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Padre
{
    public interface IProcessHandler
    {
        void Handle(Process process);
    }
}

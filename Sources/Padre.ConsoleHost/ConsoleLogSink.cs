using System;
using System.ComponentModel.Composition;

namespace Padre.ConsoleHost
{
    [Export(typeof(ILogSink))]
    public class ConsoleLogSink : ILogSink
    {
        public void Log(string format, params object[] args)
        {
            Console.WriteLine(format, args);
        }
    }
}
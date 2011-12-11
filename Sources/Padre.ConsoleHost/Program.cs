using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Padre.ConsoleHost
{
    class Program
    {
        static void Main(string[] args)
        {
            var assembly = Assembly.GetExecutingAssembly();

            var catalog = new AggregateCatalog(
                new AssemblyCatalog(assembly),
                new DirectoryCatalog(Path.GetDirectoryName(assembly.Location))
                );

            var container = new CompositionContainer(catalog);
            var workers = container.GetExportedValues<IWorker>().ToArray();

            if (workers.Length == 0)
                return;

            var flag = new ManualResetEvent(false);
            Console.CancelKeyPress += (sender, e) => flag.Set();

            foreach (var worker in workers)
                worker.Start();

            flag.WaitOne();

            foreach (var worker in workers)
                worker.Stop();
        }
    }
}

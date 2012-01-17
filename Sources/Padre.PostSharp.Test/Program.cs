using System;

namespace Padre.PostSharp.Test
{
    public static class ClrDump
    {
        public static void Dump()
        {
            Console.WriteLine("Thrown");
        }
    }

    [Serializable]
    public class ClrDumpAspectAttribute : OnThrowAspectAttribute
    {
        private bool IsWorthCapturing(Exception exception)
        {
            return true;
        }

        public override void OnThrow(Exception exception)
        {
            if (IsWorthCapturing(exception))
            {
                Console.WriteLine("Dumping because: {0}", exception.Message);
                ClrDump.Dump();
            }
        }
    }

    internal class Program
    {
        [ClrDumpAspect]
        private static void Main(string[] args)
        {
            Console.WriteLine("Before");

            if (DateTime.UtcNow.Year > 2000)
                throw new Exception("Exception");

            Console.WriteLine("After");
        }
    }
}
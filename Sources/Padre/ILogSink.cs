namespace Padre
{
    public interface ILogSink
    {
        void Log(string format, params object[] args);
    }
}
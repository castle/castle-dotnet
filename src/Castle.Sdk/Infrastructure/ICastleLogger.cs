namespace Castle.Infrastructure
{
    public interface ICastleLogger
    {
        void Info(string message);
        void Warn(string message);
        void Error(string message);
    }
}

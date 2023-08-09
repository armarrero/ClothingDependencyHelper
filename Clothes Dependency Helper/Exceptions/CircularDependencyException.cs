namespace ClothersDependencyHelper.Exceptions
{
    public class CircularDependencyException : Exception
    {
        public CircularDependencyException()
        {
        }

        public CircularDependencyException(string? message) : base(message)
        {
        }
    }
}

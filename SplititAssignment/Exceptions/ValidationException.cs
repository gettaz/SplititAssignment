namespace SplititAssignment.Exceptions
{
    public class ValidationException : CustomException
    {
        public ValidationException(string name) : base(name, "Invalid input")
        {
        }
    }
}

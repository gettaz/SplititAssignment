namespace SplititAssignment.Exceptions
{
    public class ValidationException : CustomException
    {
        public ValidationException() : base("InvalidInput", "Invalid input")
        {
        }
    }
}

namespace SplititAssignment.Exceptions
{
    public class CustomException : Exception
    {
        public string Code { get; }

        public string AdditionalInfo { get; }

        public CustomException(string message, string code, string additionalInfo = null)
            : base(message) 
        {
            Code = code;
            AdditionalInfo = additionalInfo;
        }
    }
}

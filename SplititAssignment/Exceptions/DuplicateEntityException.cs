namespace SplititAssignment.Exceptions
{
    public class DuplicateEntityException : CustomException
    {
        public DuplicateEntityException(string entityType, string value, string source)
            : base($"Duplicate {entityType} found for '{value}'. in {source}", "Duplication found")
        {
        }
    }
}

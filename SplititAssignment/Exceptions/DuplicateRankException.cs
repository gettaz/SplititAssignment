namespace SplititAssignment.Exceptions
{
    public class DuplicateRankException : CustomException
    {
        public DuplicateRankException(string name) :base("Duplicate rank found for " + name,
            "Duplicate rank found for " + name)
        {

        }
    }
}

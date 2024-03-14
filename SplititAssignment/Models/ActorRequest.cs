using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SplititAssignment.Models
{
    /// <summary>
    /// Request model for creating or updating an actor.
    /// </summary>
    public class ActorRequest
    {
        /// <summary>
        /// Name of the actor. Only letters are allowed.
        /// </summary>
        [Required(ErrorMessage = "Name is required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Only letters allowed in name")]
        [DefaultValue("John Doe")]
        public string Name { get; set; }

        [DefaultValue("Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.")]
        public string Details { get; set; }

        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Only letters allowed in type")]
        public string Type { get; set; } = "Actor";

        /// <summary>
        /// Rank of the actor. Please enter a positive number.
        /// </summary>
        [DefaultValue(42)]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter a positive Number")]
        public int Rank { get; set; }
        /// <summary>
        /// Source of the actor information.
        /// </summary>
        public string Source { get; set; }
    }
}

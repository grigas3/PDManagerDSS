namespace PDManager.Core.Common.Models
{

    /// <summary>
    /// PDManager DSS Input
    /// This is a helper class used to serialize DSS input from a Html Form to test the DSS functionality
    /// </summary>
    public class DSSInput
    {
        /// <summary>
        /// The DSS Model Id stored in the repository
        /// </summary>
        public string ModelId { get; set; }
        /// <summary>
        /// Json Representation of Dictionary with key and value
        /// </summary>
        public string Input { get; set; }
    }
}

namespace ClothersDependencyHelper.Models
{
    /// <summary>
    /// Represents an item of clothing in the dependency graph.
    /// </summary>
    public class ClothingNode
    {
        /// <summary>
        /// The name of the item of clothing
        /// </summary>
        public string ClothingItem { get; set; } = string.Empty;
        /// <summary>
        /// The clothing items that must be put on before this one can be put on
        /// </summary>
        public HashSet<ClothingNode> Dependencies { get; set; } = new HashSet<ClothingNode>();
        /// <summary>
        /// The clothing that requires the current clothing item to be put on first
        /// </summary>
        public HashSet<ClothingNode> Dependents { get; set; } = new HashSet<ClothingNode>();
    }
}

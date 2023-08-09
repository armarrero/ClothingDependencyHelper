using ClothersDependencyHelper.Models;
using ClothersDependencyHelper.Services;

namespace ClothersDependencyHelper
{
    /// <summary>
    /// Takes a list of clothing dependencies and determines what order the clothing needs to be put on.
    /// </summary>
    public class ClothingDependencyHelper
    {
        private readonly IClothingDependencyHelperService _service;

        public ClothingDependencyHelper(IClothingDependencyHelperService service) 
        { 
            _service = service;
        }
        /// <summary>
        /// Takes a list of clothing dependencies and outputs the order the clothing must be put on.
        /// </summary>
        /// <param name="input"/>The list of clothing dependencies</param>
        /// <returns>A string containing instructions for putting on the clothing items in order.
        /// Each new line represents a list alphabetical, comma-delimited list of clothing items that can be put on for that step</returns>
        public string CreateDressingOrder(string[,] input)
        {
            var output = String.Empty;

            var clothingItems = _service.CreateClothingHierarchy(input);

            var independentNodes = clothingItems.Values.Where(clothingItem => !clothingItem.Dependencies.Any())
            .OrderBy(x => x.ClothingItem, StringComparer.OrdinalIgnoreCase)
            .ToList();

            var queue = new Queue<ClothingNode>();

            independentNodes.ForEach(rootNode =>
            {
                queue.Enqueue(rootNode);
            });

            output = string.Join(Environment.NewLine, _service.Traverse(queue, clothingItems));

            return output;
        }
    }
}

using ClothersDependencyHelper.Exceptions;
using ClothersDependencyHelper.Models;

namespace ClothersDependencyHelper.Services
{
    public class ClothingDependencyHelperService : IClothingDependencyHelperService
    {
        /// <summary>
        /// Breadth First traversal of the clothing dependency graph created in CreateDressingOrder()
        /// Returns an ordered list of comma delimited clothing items grouped by the order they can be put on
        /// Clothing items that can be put on in the same time appear in alphabetical order on the same line
        /// </summary>
        /// <param name="queue">The Queue holding the root nodes of the clothing dependency graph</param>
        /// <param name="clothingItems">A Dictionary holding reference to all the nodes in the dependency graph</param>
        /// <returns>A list of comma-delmited clothing items</returns>
        public List<string> Traverse(Queue<ClothingNode> queue, Dictionary<string, ClothingNode> clothingItems)
        {
            var orderedClothingItemsList = new List<string>();
            var groupedItems = new List<string>();

            foreach (var rootNode in queue)
            {
                groupedItems.Add(rootNode.ClothingItem);
            }

            while (queue.Count() > 0)
            {
                var currentNode = queue.Dequeue();
                if (groupedItems.Any())
                {
                    orderedClothingItemsList.Add(string.Join(", ", groupedItems.OrderBy(item => item, StringComparer.OrdinalIgnoreCase)));
                    groupedItems.Clear();
                }

                foreach (var dependentNode in currentNode.Dependents)
                {
                    var dependentItem = clothingItems[dependentNode.ClothingItem];
                    dependentItem.Dependencies.Remove(currentNode);

                    if (!dependentItem.Dependencies.Any())
                    {
                        queue.Enqueue(dependentItem);
                        groupedItems.Add(dependentItem.ClothingItem);
                    }
                }

                if (queue.Count == 0)
                {
                    orderedClothingItemsList.Add(string.Join(" , ", groupedItems.OrderBy(item => item, StringComparer.OrdinalIgnoreCase)));
                }
            }

            var circularDependenciesList = clothingItems.Values.Where(item => item.Dependencies.Any()).ToList();

            if (circularDependenciesList.Count > 0)
            {
                throw new CircularDependencyException($"Circular dependencies found with clothing item(s): {string.Join("", "", circularDependenciesList)}");
            }

            return orderedClothingItemsList;
        }

        /// <summary>
        /// Creates a bidirectional dependency graph of clothing items based on a list of clothing dependencies
        /// Returns a dictionary of clothing item strings as keys with a reference to their corresponding node
        /// </summary>
        /// <param name="input"/>The list of clothing dependencies</param>
        /// <returns> A dictionary containing all the nodes in the dependency graph </returns>
        public Dictionary<string, ClothingNode> CreateClothingHierarchy(string[,] input)
        {
            var clothingNodeLookup = new Dictionary<string, ClothingNode>();

            for (var i = 0; i < input.GetLength(0); i++)
            {
                //get the clothing item and its dependency
                var dependency = input[i, 0];
                var dependant = input[i, 1];

                CheckForEmptyInputsOrSelfDependency(dependency, dependant);

                //add clothing items and their nodes to the lookup if they don't already exist
                if (!clothingNodeLookup.ContainsKey(dependency))
                    clothingNodeLookup.Add(dependency, new ClothingNode() { ClothingItem = dependency });

                if (!clothingNodeLookup.ContainsKey(dependant))
                    clothingNodeLookup.Add(dependant, new ClothingNode() { ClothingItem = dependant });

                //get the nodes and make sure they were able to be obtained
                ClothingNode dependencyNode, dependantNode;
                if (!clothingNodeLookup.TryGetValue(dependency, out dependencyNode))
                    throw new Exception($"An error occured trying to create clothing item {dependency}.");

                if (!clothingNodeLookup.TryGetValue(dependant, out dependantNode))
                    throw new Exception($"An error occured trying to create clothing item {dependant}.");

                dependencyNode.Dependents.Add(dependantNode);
                dependantNode.Dependencies.Add(dependencyNode);
            }

            return clothingNodeLookup;
        }

        /// <summary>
        /// Ensures that two strings are unique to one another and that they are not empty
        /// This can be refactored to return a boolean and give us the option to throw
        /// or skip the input instead
        /// </summary>
        private void CheckForEmptyInputsOrSelfDependency(string dependency, string dependant)
        {
            if (String.IsNullOrEmpty(dependency) || String.IsNullOrEmpty(dependant))
                throw new ArgumentException($"Empty values found on entry {dependency},{dependant}.");

            if (dependency.Equals(dependant, StringComparison.InvariantCultureIgnoreCase))
                throw new CircularDependencyException($"Clothing Item {dependant} cannot be dependent on itself");
        }
    }
}

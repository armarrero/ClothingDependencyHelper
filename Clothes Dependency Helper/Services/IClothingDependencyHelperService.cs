using ClothersDependencyHelper.Models;

namespace ClothersDependencyHelper.Services
{
    public interface IClothingDependencyHelperService
    {
        List<string> Traverse(Queue<ClothingNode> queue, Dictionary<string, ClothingNode> clothingItems);
        Dictionary<string, ClothingNode> CreateClothingHierarchy(string[,] input);
    }
}

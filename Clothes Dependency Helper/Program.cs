using Microsoft.Extensions.DependencyInjection;
using ClothersDependencyHelper;
using ClothersDependencyHelper.Services;

public class Program
{
    public static void Main()
    {
        var serviceProvider = new ServiceCollection()
            .AddSingleton<IClothingDependencyHelperService, ClothingDependencyHelperService>()
            .AddScoped<ClothingDependencyHelper>()
            .BuildServiceProvider();

        var helper = serviceProvider.GetRequiredService<ClothingDependencyHelper>();
        
        var input = new string[,]
        {
            {"t-shirt", "dress shirt"},
            {"dress shirt", "pants"},
            {"dress shirt", "suit jacket"},
            {"tie", "suit jacket"},
            {"pants", "suit jacket"},
            {"belt", "suit jacket"},
            {"suit jacket", "overcoat"},
            {"dress shirt", "tie"},
            {"suit jacket", "sun glasses"},
            {"sun glasses", "overcoat"},
            {"left sock", "pants"},
            {"pants", "belt"},
            {"suit jacket", "left shoe"},
            {"suit jacket", "right shoe"},
            {"left shoe", "overcoat"},
            {"right sock", "pants"},
            {"right shoe", "overcoat"},
            {"t-shirt", "suit jacket"}
        };

        var output = helper.CreateDressingOrder(input);
        Console.WriteLine(output);
    }
}
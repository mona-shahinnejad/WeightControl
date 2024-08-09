using Ardalis.Result;
using MediatR;
using Spectre.Console;
using WeightControl.Application.Products.Create;
using WeightControl.Application.Products.FilterByWeight;
using WeightControl.Application.Products.List;
using WeightControl.Application.Products.WeightToleranceCheck;
using WeightControl.Application.Products;

namespace WeightControl.Console;

public class WeightController
{
    private readonly IMediator _mediator;

    public WeightController(IMediator mediator)
    {
        _mediator = mediator;
    }

    internal void RunApplication()
    {
        bool exit = false;

        while (!exit)
        {
            // Clear the console screen for a clean menu display
            System.Console.Clear();

            // Display the menu using Spectre.Console
            var selection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold yellow]Please select an option:[/]")
                    .PageSize(10)
                    .AddChoices(new[] {
                        "View Products",
                        "Add Product",
                        "Filter Products By Weight",
                        "Product Weight Tolerance Check",
                        "Exit"
                    }));

            switch (selection)
            {
                case "View Products":
                    DisplayProductList();
                    break;
                case "Add Product":
                    AddProductProcess();
                    break;
                case "Filter Products By Weight":
                    FilterProductsByWeightProcess();
                    break;
                case "Product Weight Tolerance Check":
                    ProductWeightTolerance();
                    break;
                case "Exit":
                    AnsiConsole.MarkupLine("[red]Exiting the application...[/]");
                    exit = true;
                    break;
            }

            if (!exit)
            {
                AnsiConsole.MarkupLine("[blue]Press any key to return to the menu...[/]");
                System.Console.ReadKey();
            }
        }
    }

    private async void ProductWeightTolerance()
    {
        var productId = AnsiConsole.Ask<int>("Enter the [bold yellow]product Id[/]:");
        var minMaxWeight = AskMinMaxWeight();

        var result = await _mediator.Send(new WeightToleranceCheckQuery(productId, minMaxWeight.minWeight, minMaxWeight.maxWeight));

        if (result.IsSuccess && result.Value)
        {
            AnsiConsole.Write(new FigletText("Product weight approved").Centered().Color(Color.Green));
        }
        else if (result.IsNotFound())
        {
            AnsiConsole.Write(new FigletText("Product not found!").Centered().Color(Color.Yellow));
        }
        else
        {
            AnsiConsole.Write(new FigletText("Product weight approved").Centered().Color(Color.Red));
        }
    }

    private async void FilterProductsByWeightProcess()
    {
        var minMaxWeight = AskMinMaxWeight();

        var result = await _mediator.Send(new FilterByWeightQuery(minMaxWeight.minWeight, minMaxWeight.maxWeight));

        ShowProductsTable(result);
    }

    private (double minWeight, double maxWeight) AskMinMaxWeight()
    {
        var minWeight = AnsiConsole.Ask<double>("Enter the [bold yellow]product minimum weight(KG)[/]:");
        var maxWeight = AnsiConsole.Ask<double>("Enter the [bold yellow]product maximum weight(KG)[/]:");

        return (minWeight, maxWeight);
    }

    private async void DisplayProductList()
    {
        var result = await _mediator.Send(new ListProductsQuery(null, null));

        ShowProductsTable(result);
    }

    private void ShowProductsTable(Result<IEnumerable<ProductDTO>> result)
    {
        if (result.IsSuccess & result.Value.Any())
        {
            var table = new Table();
            table.AddColumn("ID");
            table.AddColumn("Name");
            table.AddColumn("Weight(KG)");

            foreach (var product in result.Value)
            {
                table.AddRow(
                    product.Id.ToString(),
                    product.Name,
                    product.Weight.ToString()
                );
            }

            AnsiConsole.Write(table);
        }
        else
        {
            AnsiConsole.Write(new FigletText("There is no products available!").Centered().Color(Color.Red));
        }
    }

    private async void AddProductProcess()
    {
        var productName = AnsiConsole.Ask<string>("Enter the [bold yellow]product name[/]:");
        var productWeight = AnsiConsole.Ask<double>("Enter the [bold yellow]product weight(KG)[/]:");

        var result = await _mediator.Send(new CreateProductCommand(productName, productWeight));

        if (result.IsSuccess)
        {
            AnsiConsole.Write(new FigletText("Product Inserted Successfully").Centered().Color(Color.Green));

        }
        else
        {
            AnsiConsole.Write(new FigletText("There is an Error, Please Try Again!").Centered().Color(Color.Red));
        }
    }
}

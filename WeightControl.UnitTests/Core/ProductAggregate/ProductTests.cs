using AutoFixture.Xunit2;
using FluentAssertions;
using WeightControl.Core.ProductAggregate;

namespace WeightControl.UnitTests.Core.ProductAggregate;

public class ProductTests
{

    [Theory]
    [AutoData]
    public void Constructor_ShouldInitializePropertiesCorrectly(string name, double weight)
    {
        // Act
        var product = new Product(name, weight);

        // Assert
        product.Name.Should().Be(name);
        product.Weight.Should().Be(weight);
    }

    [Theory]
    [AutoData]
    public void UpdateName_ShouldUpdateName_WhenValidNameIsProvided(string initialName, string newName, double weight)
    {
        // Arrange
        var product = new Product(initialName, weight);

        // Act
        product.UpdateName(newName);

        // Assert
        product.Name.Should().Be(newName);
    }

    [Theory]
    [AutoData]
    public void UpdateName_ShouldThrowArgumentException_WhenNullNameIsProvided(string initialName, double weight)
    {
        // Arrange
        var product = new Product(initialName, weight);

        // Act
        Action act = () => product.UpdateName(null);

        // Assert
        act.Should().Throw<ArgumentException>()
           .WithMessage("Value cannot be null. (Parameter 'newName')");
    }

    [Theory]
    [AutoData]
    public void UpdateName_ShouldThrowArgumentException_WhenEmptyNameIsProvided(string initialName, double weight)
    {
        // Arrange
        var product = new Product(initialName, weight);

        // Act
        Action act = () => product.UpdateName(string.Empty);

        // Assert
        act.Should().Throw<ArgumentException>()
           .WithMessage("Required input newName was empty. (Parameter 'newName')");
    }

    [Theory]
    [AutoData]
    public void UpdateName_ShouldThrowArgumentException_WhenWhiteSpaceNameIsProvided(string initialName, double weight)
    {
        // Arrange
        var product = new Product(initialName, weight);

        // Act
        Action act = () => product.UpdateName("   ");

        // Assert
        act.Should().Throw<ArgumentException>()
           .WithMessage("Required input newName was empty. (Parameter 'newName')");
    }

    [Theory]
    [AutoData]
    public void UpdateWeight_ShouldUpdateWeight_WhenValidWeightIsProvided(string name, double weight, double newWeight)
    {
        // Arrange
        var product = new Product(name, weight);

        // Act
        product.UpdateWeight(newWeight);

        // Assert
        product.Weight.Should().Be(newWeight);
    }

    [Theory]
    [AutoData]
    public void UpdateWeight_ShouldThrowArgumentOutOfRangeException_WhenNegativeWeightIsProvided(string name, double weight)
    {
        // Arrange
        var product = new Product(name, weight);

        // Act
        Action act = () => product.UpdateWeight(-1);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>()
           .WithMessage("Input newWeight was out of range (Parameter 'newWeight')");
    }

    [Theory]
    [AutoData]
    public void UpdateWeight_ShouldNotThrowException_WhenMaximumWeightIsProvided(string name, double weight)
    {
        // Arrange
        var product = new Product(name, weight);

        // Act
        Action act = () => product.UpdateWeight(double.MaxValue);

        // Assert
        act.Should().NotThrow();
    }
}

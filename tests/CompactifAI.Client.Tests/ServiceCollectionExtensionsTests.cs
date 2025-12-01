using CompactifAI.Client.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CompactifAI.Client.Tests;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddCompactifAI_WithAction_RegistersServices()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddCompactifAI(options =>
        {
            options.ApiKey = "test-key";
            options.DefaultModel = CompactifAIModels.DeepSeekR1_Slim;
        });

        var provider = services.BuildServiceProvider();

        // Assert
        var client = provider.GetService<ICompactifAIClient>();
        Assert.NotNull(client);
    }

    [Fact]
    public void AddCompactifAI_WithApiKey_RegistersServices()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddCompactifAI("test-api-key");

        var provider = services.BuildServiceProvider();

        // Assert
        var client = provider.GetService<ICompactifAIClient>();
        Assert.NotNull(client);
    }

    [Fact]
    public void AddCompactifAI_ReturnsServiceCollection_ForChaining()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        var result = services.AddCompactifAI("test-key");

        // Assert
        Assert.Same(services, result);
    }

    [Fact]
    public void AddCompactifAI_RegistersAsScoped()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddCompactifAI("test-key");

        var provider = services.BuildServiceProvider();

        // Act - create two scopes and get clients
        using var scope1 = provider.CreateScope();
        using var scope2 = provider.CreateScope();

        var client1 = scope1.ServiceProvider.GetService<ICompactifAIClient>();
        var client2 = scope2.ServiceProvider.GetService<ICompactifAIClient>();

        // Assert - should be different instances from different scopes
        Assert.NotNull(client1);
        Assert.NotNull(client2);
        Assert.NotSame(client1, client2);
    }
}

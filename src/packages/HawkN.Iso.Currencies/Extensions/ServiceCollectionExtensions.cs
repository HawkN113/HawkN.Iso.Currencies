using HawkN.Iso.Currencies.Abstractions;
using HawkN.Iso.Currencies.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
namespace HawkN.Iso.Currencies.Extensions;

/// <summary>
/// Extension methods for registering currency-related services into an <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers <see cref="ICurrencyService"/> with its implementation <see cref="CurrencyService"/>
    /// as a singleton in the dependency injection container.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <returns>The updated <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddCurrencyService(this IServiceCollection services)
    {
        services.TryAddSingleton<ICurrencyService, CurrencyService>();
        return services;
    }
}
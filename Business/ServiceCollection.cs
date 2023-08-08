using Microsoft.Extensions.DependencyInjection;
using Business.Abstracts;
using Business.Implementations;
using Business.Implementations.Handlers.Reservations.Commands;
using Business.Implementations.Handlers.Requests.Commands;
using MediatR;
using Business.Implementations.Handlers.Users.Commands;
using Business.Implementations.Handlers.Books.Commands;
using System.Reflection;
using Business.Implementations.Handlers.Users.Queries;
using Business.Implementations.MapperProfiles;

namespace Business;
public static class ServiceCollectionExtentions
{
    public static IServiceCollection AddBusinessLayer(this IServiceCollection services)
    {
        AddMediatRHandlers(services);
        AddManagers(services);
        AddAutoMapper(services);

        return services;
    }

    private static void AddMediatRHandlers(IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
    }

    private static void AddManagers(IServiceCollection services)
    {
        services.AddScoped<IUserManager, UserManager>();
        services.AddScoped<IBookManager, BookManager>();
        services.AddScoped<IRequestManager, RequestManager>();
        services.AddScoped<IReservationManager, ReservationManager>();
    }

    private static void AddAutoMapper(IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MapperProfile));
    }
}
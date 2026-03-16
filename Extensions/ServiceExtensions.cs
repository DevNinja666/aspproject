using FluentValidation;
using FluentValidation.AspNetCore;
using Invoicer.Data;
using Invoicer.Services;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Invoicer.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

        return services;
    }

    public static IServiceCollection AddProjectServices(this IServiceCollection services)
    {
        services.AddScoped<InvoiceDocumentService>();

        return services;

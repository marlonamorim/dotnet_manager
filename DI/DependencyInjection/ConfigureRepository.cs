﻿using Data.DataConnector;
using Data.Interfaces.DataConnector;
using Data.Repositories;
using Domain.Interfaces.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace DI.DependencyInjection
{
    public class ConfigureRepository
    {
        public static void ConfigureDependenciesRepository(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IUsuarioRepository, UsuarioRepository>();
            serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();

        }
    }
}
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sengoku.API.Repositories
{
    public static class RepositoryExtensions
    {
        public static void RegisterMongoDbRepositories(this IServiceCollection servicesBuilder)
        {
            servicesBuilder.AddSingleton<IMongoClient, MongoClient>(s =>
            {
                var uri = s.GetRequiredService<IConfiguration>().GetSection("MongoSettings:MongoURI").Value;
                return new MongoClient(uri);
            });
            servicesBuilder.AddSingleton<EventsRepository>();
            servicesBuilder.AddSingleton<UserRepository>();
            servicesBuilder.AddSingleton<ProductsRepository>();
        }
    }
}

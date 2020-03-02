using System;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Teams.API.Helpers;
using Teams.API.Tests;

namespace Auto.VehicleCatalog.API.Tests
{
   public class DependencyFixture : IDisposable
    {
        public DependencyFixture()
        {
            var serviceCollection = new ServiceCollection();

            // Add in memory ef database
            // serviceCollection.AddDbContext<DataContext>(options => options.UseInMemoryDatabase(databaseName: "VehicleCatalog"));
            
            //Auto mapper configuration
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfiles());
            });
            var mapper = mockMapper.CreateMapper();
            serviceCollection.AddSingleton<IMapper>(mapper);

            serviceCollection.AddSingleton<FakeMemoryRepositoryHelper>(new FakeMemoryRepositoryHelper());

            // TODO COLOCAR AQUI O HELPER DO REPOSITÓRIO... COLOCAR ELE LÁ NA API PARA FACILITAR 
            // A CARGA PARA O SWAGGER TAMBÉM

            // Build de DI
            ServiceProvider = serviceCollection.BuildServiceProvider();
        }
        public ServiceProvider ServiceProvider { get; private set; }
        public void Dispose()
        {   
            this.ServiceProvider.Dispose();
        }
    }
}

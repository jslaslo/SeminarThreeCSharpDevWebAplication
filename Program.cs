
using Autofac;
using Autofac.Extensions.DependencyInjection;
using LessonThree.Abstractions;
using LessonThree.GraphQL;
using LessonThree.Services;
using StoreMarket.Abstractions;
using StoreMarket.Context;
using StoreMarket.Mappers;
using StoreMarket.Services;

namespace LessonThree
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<StoreContext>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

            builder.Services.AddSingleton<IProductService, ProductService>().AddGraphQLServer().AddQueryType<Query>().AddMutationType<Mutation>();
            builder.Services.AddSingleton<ICategoryService, CategoryService>();

            builder.Services.AddAutoMapper(typeof(MappingProfile));
            /* builder.Host.ConfigureContainer<ContainerBuilder>(c => c
             .RegisterType<ProductService>()
             .As<IProductService>());*/

            builder.Services.AddMemoryCache(m => m.TrackStatistics = true);

            var app = builder.Build();
            app.MapGraphQL();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

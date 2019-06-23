using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using InterviewWeb.Infrastructure;
using InterviewWeb.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace InterviewWeb
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            var builder = new DbContextOptionsBuilder<SODbContext>();
            
            builder.UseInMemoryDatabase("SODbContext")
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .EnableSensitiveDataLogging();
            
            var options = builder.Options;
            var container = new ContainerBuilder();
            
            container.RegisterApiControllers(Assembly.GetExecutingAssembly());

            container.Register(c => new SOContextFactory(options).GetNewDbContext()).SingleInstance();
            container.RegisterType<InMemoryProductRepository>().As<IProductRepository>().SingleInstance();

            Resolver = container.Build();
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(Resolver);
            #region Products
            var products = new List<Product>()
            {

                new Product
                {
                    DateCreated = DateTime.UtcNow, DateDiscontinued = null, Id = 1, InternalCode = "CODE_1",
                    Name = "Apple"
                },
                new Product
                {
                    DateCreated = DateTime.UtcNow, DateDiscontinued = null, Id = 2, InternalCode = "CODE_22",
                    Name = "Pear"
                },
                new Product
                {
                    DateCreated = DateTime.UtcNow, DateDiscontinued = null, Id = 3, InternalCode = "CODE_12",
                    Name = "Grapes"
                },
                new Product
                {
                    DateCreated = DateTime.UtcNow, DateDiscontinued = null, Id = 4, InternalCode = "CODE_4",
                    Name = "Banana"
                },
                new Product
                {
                    DateCreated = DateTime.UtcNow, DateDiscontinued = DateTime.UtcNow, Id = 5, InternalCode = "CODE_5",
                    Name = "Mango"
                },
                new Product
                {
                    DateCreated = DateTime.UtcNow, DateDiscontinued = null, Id = 6, InternalCode = "CODE_11",
                    Name = "Melon"
                },
                new Product
                {
                    DateCreated = DateTime.UtcNow, DateDiscontinued = null, Id = 7, InternalCode = "CODE_6",
                    Name = "Orange"
                },
                new Product
                {
                    DateCreated = DateTime.UtcNow, DateDiscontinued = null, Id = 8, InternalCode = "CODE_7",
                    Name = "Satsuma"
                },
                new Product
                {
                    DateCreated = DateTime.UtcNow, DateDiscontinued = null, Id = 9, InternalCode = "CODE_8",
                    Name = "Kiwi"
                },
                new Product
                {
                    DateCreated = DateTime.UtcNow, DateDiscontinued = null, Id = 10, InternalCode = "CODE_9",
                    Name = "Strawberry"
                }
            };
            #endregion
            var dbContext = Resolver.Resolve<SODbContext>();
            
            dbContext.Database.EnsureCreated();

            dbContext.Products.AddRange(products);
            dbContext.SaveChanges();

            config.Formatters.Remove(config.Formatters.XmlFormatter);
            var jsonFormatter = config.Formatters.JsonFormatter;
            jsonFormatter.UseDataContractJsonSerializer = false; // defaults to false, but no harm done
            jsonFormatter.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
            jsonFormatter.SerializerSettings.Formatting = Formatting.None;
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

        }

        public static IContainer Resolver { get; private set; }

    }
}

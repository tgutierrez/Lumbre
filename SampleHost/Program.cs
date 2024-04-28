// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Lumbre;
using Microsoft.Extensions.DependencyInjection;


var builder = Host.CreateDefaultBuilder()
    .UseLumbreWebFrontend(cfg =>
    {
        cfg.ApplicationBuilder.UseSwagger();
        cfg.ApplicationBuilder.UseSwaggerUI();
        cfg.UseSelfHosting();
    })
    .ConfigureServices(services =>
            services.AddLumbre(l =>
                l.UseMongoDb(c =>
                {
                    c.DatabaseName = "FHIR";
                    c.ConnectionString = "mongodb://localhost:27017";
                }))
                .AddSwaggerGen()
                .AddEndpointsApiExplorer()
            )
            
    .Build();

builder.Run();
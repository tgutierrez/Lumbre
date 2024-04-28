// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Lumbre;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;


var builder = Host.CreateDefaultBuilder()
    .UseLumbreWebFrontend(cfg =>
    {
        cfg.ApplicationBuilder.UseSwagger();
        cfg.ApplicationBuilder.UseSwaggerUI();
        cfg.UseSelfHosting();
    })
    .ConfigureServices((context, services) =>
            services.AddLumbre(l =>
                l.UseMongoDb(c =>
                {
                    var connString = context.Configuration.GetSection("MongoDB").GetValue<string>("ConnString");
                    var dbName     = context.Configuration.GetSection("MongoDB").GetValue<string>("Database");
                    c.DatabaseName = dbName;
                    c.ConnectionString = connString;
                }))
                .AddSwaggerGen()
                .AddEndpointsApiExplorer()
            )
            
    .Build();

builder.Run();
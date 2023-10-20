using Asi.Common;
using Asi.Common.Models;
using Asi.Common.Services;
using Asi.Console;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text;

Console.OutputEncoding = Encoding.Unicode;
Console.InputEncoding = Encoding.Unicode;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<AsiCommonOptions>(builder.Configuration.GetSection("AsiCommonOptions"));
builder.Services.AddHostedService<ContactWorker>();
builder.Services.AddDbContext<AsiContext>();
builder.Services.AddScoped<IContactService, ContactService>();

using IHost host = builder.Build();

using IServiceScope serviceScope = host.Services.CreateScope();
serviceScope.ServiceProvider.GetRequiredService<AsiContext>().Database.EnsureCreated();

await host.RunAsync();
using Asi.Common;
using Asi.Common.Models;
using Asi.Common.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.Configure<AsiCommonOptions>(builder.Configuration.GetSection("AsiCommonOptions"));
builder.Services.AddDbContext<AsiContext>();
builder.Services.AddScoped<IContactService, ContactService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.MapRazorPages();

using var scope = app.Services.CreateScope();
scope.ServiceProvider.GetRequiredService<AsiContext>().Database.EnsureCreated();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();

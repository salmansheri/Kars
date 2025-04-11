using AuctionService.Consumers;
using AuctionService.Data;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(options => {

    options.AddEntityFrameworkOutbox<AuctionDbContext>(opt => {
        opt.QueryDelay = TimeSpan.FromSeconds(10); 
        opt.UsePostgres(); 
        opt.UseBusOutbox(); 
    }); 

    options.AddConsumersFromNamespaceContaining<AuctionCreatedFaultsConsumer>(); 

    options.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("auction", false)); 
    
    options.UsingRabbitMq((context, cfg) => 
    {
        cfg.ConfigureEndpoints(context); 
    }
    ); 
}); 

// Add services to the container.

builder.Services.AddDbContext<AuctionDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))); 

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(); 
    
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

try {
    DbInitializer.InitDb(app); 

} catch(Exception ex)
{
    Console.WriteLine($"Error seeding data: {ex.Message}"); 
    
}

app.Run();

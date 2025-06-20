using AuctionService.Consumers;
using AuctionService.Data;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
        cfg.Host(builder.Configuration["RabbitMq:Host"], "/", host => {
            host.Username(builder.Configuration.GetValue("RabbitMq:Username", "guest")); 
              host.Password(builder.Configuration.GetValue("RabbitMq:Password", "guest")); 
        }); 
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

builder.Services.AddCors(options => {
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
             .WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod(); 
    }); 
}); 

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
    options.Authority = builder.Configuration["IdentityServiceUrl"]; 
    options.RequireHttpsMetadata = false; 
    options.TokenValidationParameters.ValidateAudience = false; 
    options.TokenValidationParameters.NameClaimType = "username"; 


}); 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(); 
    
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseCors("AllowFrontend");

app.MapControllers();





try
{
    DbInitializer.InitDb(app);

}
catch (Exception ex)
{
    Console.WriteLine($"Error seeding data: {ex.Message}");

}

app.Run();

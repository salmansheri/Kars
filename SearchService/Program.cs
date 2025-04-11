using System.Net;
using MassTransit;

using Polly;
using Polly.Extensions.Http;
using SearchService.Consumers;
using SearchService.Data;

using SearchService.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); 
builder.Logging.AddConsole(); 


builder.Services.AddMassTransit(options => {
    options.AddConsumersFromNamespaceContaining<AuctionCreatedConsumer>(); 

    options.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("search", false)); 
    options.UsingRabbitMq((context, cfg) => {
        cfg.Host("localhost", "/", h=> {
            h.Username("guest");
            h.Password("guest");
        }); 
        cfg.ReceiveEndpoint("search-auction-created", e=> {
            e.UseMessageRetry(r => r.Interval(5, 5)); 
            e.ConfigureConsumer<AuctionCreatedConsumer>(context);
        }); 
        cfg.ConfigureEndpoints(context); 
        
    }); 
}); 

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); 


// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpClient<AuctionServiceHttpClient>().AddPolicyHandler(GetPolicy()); 

// builder.Services.AddHttp
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Lifetime.ApplicationStarted.Register(async () => {
    try
{
    await DbInitializer.InitDb(app);

}
catch (Exception ex)
{
    Console.WriteLine("Error initializing database: " + ex.Message);


}

}); 



app.Run();


static IAsyncPolicy<HttpResponseMessage> GetPolicy()
 => HttpPolicyExtensions
        .HandleTransientHttpError()
        .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
        .WaitAndRetryForeverAsync(_ => TimeSpan.FromSeconds(3)); 

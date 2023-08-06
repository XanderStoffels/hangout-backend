using XanderStoffels.Background;
using XanderStoffels.Hubs;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors();
builder.Services.AddSignalR().AddMessagePackProtocol();
builder.Services.AddHostedService<VideoSwitcher>();

var app = builder.Build();

app.UseCors(options => options
    .WithOrigins("https://xanderapp.com", "https://www.xanderapp.com", "http://localhost:5173", "http://localhost:5173")
    .AllowAnyHeader().AllowAnyMethod().AllowCredentials());

app.MapGet("/", () => "Hello World!");

app.MapHub<VisitorHub>("visitor");

app.Run();

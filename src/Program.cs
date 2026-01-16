using DotNetEnv;
using ShareXe.Base.Converters;
using ShareXe.Base.Extensions;
using ShareXe.Base.Middleware;
using ShareXe.Modules.Auth.Extensions;
using ShareXe.Modules.Auth.Services;
using ShareXe.Modules.Minio.Extensions;
using System.Text.Json;
using System.Text.Json.Serialization;

Env.TraversePath().Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDatabaseConfig();
builder.Services.AddAutoInject();
builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.Converters.Add(new DateTimeOffsetIso8601Converter());
    });

builder.Services.AddCustomValidationResponse();
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        b => b.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

builder.Services.AddFirebaseAuthentication();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerConfig(); // Sẽ nhận diện được [Authorize] từ Firebase

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddMinioConfig();

builder.Services.AddHttpClient<AuthService>();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddHttpClient("FirebaseClient")
        .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        });
}

var app = builder.Build();

app.WaitForDatabase();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ShareXe API v1");
        c.DisplayRequestDuration();
    });
}

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
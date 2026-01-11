using ShareXe.Base.Middleware;
using DotNetEnv;
using ShareXe.Base.Extensions;

Env.TraversePath().Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDatabaseConfig();
builder.Services.AddAutoInject();
builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        b => b.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.WaitForDatabase();

app.UseMiddleware<GlobalExceptionHandler>();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAll");
app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();

app.Run();

using Microsoft.OpenApi;
using QuestsInGame_WebAPI_Project.Interfaces;
using QuestsInGame_WebAPI_Project.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Quest in Games API", Version = "v1" });
});

builder.Services.AddScoped<ICharactersService, CharactersService>();
builder.Services.AddScoped<IGuildsService, GuildsService>();
builder.Services.AddScoped<IQuestService, QuestService>();
builder.Services.AddScoped<IQuestCompletionService, QuestCompletionService>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwagger();
    
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Quest in Games API V1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

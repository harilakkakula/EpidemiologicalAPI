using Epidemiological.DAL.Interface;
using Epidemiological.DAL.Services;
using Epidemiological.DAL.Util;
using EpidemiologicalAPI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<StoreDatabaseSettings>(
    builder.Configuration.GetSection("StoreDatabase"));
builder.Services.Configure<APIIntegrationDetails>(
    builder.Configuration.GetSection("APIIntegrationDetails"));

builder.Services.AddScoped<IWeeklyInfectusDiseases, WeeklyInfectusDiseasesImpl>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.

// Enable CORS globally
app.UseCors("AllowAll");

app.UseSwagger();
    app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

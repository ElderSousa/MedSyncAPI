
using System.Reflection;
using System.Security.Policy;
using System.Text.Json.Serialization;
using Asp.Versioning;
using CroosCutting.MS_AuthenticationAutorization.IoC;
using MedSync.CrossCutting.Data;
using MedSync.CrossCutting.IoC;
using MedSync.CrossCutting.Middlewares;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

//Configura��o de versionamento
builder.Services.AddApiVersioning(v =>
{
    v.DefaultApiVersion = new ApiVersion(1.0);
    v.AssumeDefaultVersionWhenUnspecified = true;
    v.ReportApiVersions = true; 
    v.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new QueryStringApiVersionReader()
    );

}).AddApiExplorer(options => {
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.InjectDependency();
builder.Services.InjectDataBase();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//Configura��o de documenta��o do swagger.
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "MedSyncApi",
        Description = "Agendamento de Consultas m�dicas"
    });

    c.AddServer(new OpenApiServer { Url = "http://localhost:8080" });

    //Configura��o para coment�rios xml com descri��o dos endpoints na controller
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,xmlFilename));
});

builder.Services.AddCorsPolicy(builder.Environment);

var app = builder.Build();

app.UseForwardedHeaders(new ForwardedHeadersOptions 
{ 
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCors("Development");
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

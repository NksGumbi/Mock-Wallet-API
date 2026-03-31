using Wallet.DTO.DataObjects;
using Serilog;
using Microsoft.OpenApi.Models;
using Wallet.BL.BLL.Interfaces;
using Wallet.BL.BLL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Wallet.API.Authenticate;
using Serilog.Events;
using System.Collections.ObjectModel;
using System.Data;
using Serilog.Sinks.MSSqlServer;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Add Serilog & ElasticSearch
Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
                .WriteTo.File(builder.Configuration["AppSettings:LogPath"], rollingInterval: RollingInterval.Day)
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .WriteTo.Console()
            .WriteTo.Debug()
            .WriteTo.MSSqlServer(
                connectionString: config.GetConnectionString("DBConnection"),
                sinkOptions: new MSSqlServerSinkOptions
                {
                    TableName = "Logs",
                    AutoCreateSqlTable = true
                },
                columnOptions: new ColumnOptions
                {
                    AdditionalColumns = new Collection<SqlColumn>
                    {
                        new SqlColumn { ColumnName = "UserId", DataType = SqlDbType.NVarChar, DataLength = 128 },
                        new SqlColumn { ColumnName = "MethodName", DataType = SqlDbType.NVarChar, DataLength = 256 },
                        new SqlColumn { ColumnName = "IsBO", DataType = SqlDbType.Bit },
                        new SqlColumn { ColumnName = "IsSignificant", DataType = SqlDbType.Bit }
                    }
                }
            )
            .CreateLogger();

// Add services to the container.

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.Configure<ConnectionStrings>(builder.Configuration.GetSection("ConnectionStrings"));

builder.Services.AddSingleton<ApiKeyAuthorizationFilter>();
builder.Services.AddSingleton<IApiKeyValidator, ApiKeyValidator>();

builder.Services.AddHttpClient();
builder.Services.AddScoped<ITransaction, Transaction>();
builder.Services.AddScoped<ILogs, Logs>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Wallet API UAT", Version = "v1" });

    options.AddSecurityDefinition("apiKey", new OpenApiSecurityScheme
    {
        Description = "Wallet API KEY Authorization",
        Name = "X-API-Key",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "apiKey"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
    builder =>
    {
        builder.AllowAnyHeader().SetIsOriginAllowed(origin => true).AllowAnyMethod().AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();

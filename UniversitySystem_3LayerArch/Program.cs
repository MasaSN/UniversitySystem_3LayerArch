using System.Text;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;
using University.Core.services;
using University.Data.Context;
using University.Data.Entities.Identity;
using University.Data.Reposetories;
using UniversitySystem_3LayerArch.Configurations;
using Microsoft.IdentityModel.Tokens;
using UniversitySystem_3LayerArch.Helpers;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Please insert JWT with Bearer into field",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });
    c.AddSecurityRequirement(
        new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<UniversityContext>(options =>
    options.UseSqlServer(connectionString));

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores
    <UniversityContext>().AddDefaultTokenProviders();


builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(container =>

    {   
        container.RegisterType<UniversityContext>().AsSelf().InstancePerLifetimeScope();
        container.RegisterType<StudentRepository>().As<IStudentRepository>().InstancePerLifetimeScope();  
        container.RegisterType<StudentService>().As<IStudentService>().InstancePerLifetimeScope();
        container.RegisterType<CourseRepository>().As<IcourseRepository>().InstancePerLifetimeScope();
        container.RegisterType<CourseServeice>().As<ICourseService>().InstancePerLifetimeScope();
        container.RegisterType<AuthService>().As<IauthService>().InstancePerLifetimeScope();
        container.RegisterType<JwtTokenHelper>().As<IjwtTokenHelper>().InstancePerLifetimeScope();
    }
    
);

builder.Host.UseSerilog();

//add authentications
var jwtsettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.Configure<JwtSettings>(jwtsettings);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    var secretkey = Encoding.UTF8.GetBytes(jwtsettings["SecretKey"]);
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtsettings["Issuer"],
        ValidAudience = jwtsettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(secretkey)
    };
});


// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseDeveloperExceptionPage();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();

//tested on post man and connected successfully
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Serilog;
using University.Core.services;
using University.Data.Context;
using University.Data.Reposetories;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(container =>

    {   
        container.RegisterType<UniversityContext>().AsSelf().InstancePerLifetimeScope();
        container.RegisterType<StudentRepository>().As<IStudentRepository>().InstancePerLifetimeScope();  
        container.RegisterType<StudentService>().As<IStudentService>().InstancePerLifetimeScope();
        container.RegisterType<CourseRepository>().As<IcourseRepository>().InstancePerLifetimeScope();
        container.RegisterType<CourseServeice>().As<ICourseService>().InstancePerLifetimeScope();
    }
    
);

builder.Host.UseSerilog();
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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

//tested on post man and connected successfully
using Microsoft.EntityFrameworkCore;
using TwseApp.API.AutoMapper;
using TwseApp.API.Data;
using TwseApp.API.Repository;
using TwseApp.API.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<TwseDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("TwseDbConnectionString")));



#region Dependencies Injection

builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IBranchCompanyService, BranchCompanyService>();
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();

#endregion

#region AutoMapper

builder.Services.AddAutoMapper(typeof(MappingProfile));

#endregion


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
app.UseAuthorization();

app.MapControllers();

app.Run();

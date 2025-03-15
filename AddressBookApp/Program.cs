using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Context;
using FluentValidation.AspNetCore;
using ModelLayer.Validator;
using FluentValidation;
using AutoMapper;
using BusinessLayer.Mapping;
using BusinessLayer.Interface;
using BusinessLayer.Service;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;
using Microsoft.AspNetCore.Identity;
using Middleware.TokenGeneration;
using Middleware.PasswordHashing;
using Microsoft.AspNetCore.Http;
using Middleware.SMTP;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAddressBookBL, AddressBookBL>();
builder.Services.AddScoped<IAddressBookRL, AddressBookRL>();
builder.Services.AddScoped<IUserAuthenticationBL, UserAuthenticationBL>();
builder.Services.AddScoped<IUserAuthenticationRL, UserAuthenticationRL>();
builder.Services.AddSingleton<Jwt>();
builder.Services.AddScoped<PasswordHasher>();
builder.Services.AddScoped<IEmailServices, EmailServices>();


// Add services to the container
builder.Services.AddControllers();

//Add swagger 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<AddressBookValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UserValidator>();

// registered FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<AddressBookValidator>();

// Manually register AutoMapper
var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new AutoMapperProfile());
});

IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

// configuring database
var connectionString = builder.Configuration.GetConnectionString("SqlConnection");
builder.Services.AddDbContext<AddressBookDBContext>(options =>
    options.UseSqlServer(connectionString));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
builder.Services.AddAuthorization();    
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
using api.Authorization;
using api.Data;
using api.Data.Services;
using api.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using api.Data.Entities;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authorization;
using api.Authorization.Model;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<StoreContext> (options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentity<RegisteredUser, IdentityRole>()
    .AddEntityFrameworkStores<StoreContext>()
    .AddDefaultTokenProviders(); 

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidAudience = builder.Configuration["JWT:ValidAudience"],
            ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"])),
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
        };
    });

builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<ICommentService, CommentService>();
builder.Services.AddTransient<IOrderService, OrderService>();
//builder.Services.AddTransient<ITokenManager, TokenManager>(); // remove ?
builder.Services.AddTransient<IJwtTokenService, JwtTokenService>();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddScoped<AuthDbSeeder>();

builder.Services.AddAuthorization(options => 
{
    options.AddPolicy(PolicyNames.ResourceOwner, policy => policy.Requirements.Add(new ResourceOwnerRequirement()));
});

builder.Services.AddSingleton<IAuthorizationHandler, ResourceOwnerAuthorizationHandler>();  

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Bearer Authentication with JWT Token",
        Type = SecuritySchemeType.Http
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();
app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "APP API");
    c.RoutePrefix = String.Empty;
});

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors(opt => {
    opt.AllowAnyHeader().AllowAnyMethod().SetIsOriginAllowed(origin => true).AllowCredentials().WithOrigins("http://localhost:3000");
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

//using var scope = app.Services.CreateScope();
//var dbContext = scope.ServiceProvider.GetRequiredService<StoreContext>();
//dbContext.Database.Migrate();

var dbSeeder = app.Services.CreateScope().ServiceProvider.GetRequiredService<AuthDbSeeder>();
await dbSeeder.SeedAsync();

app.Run();

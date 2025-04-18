using System.Text;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.ExtensionMethods.Register;
using Infrastructure.Interfaces;
using Infrastructure.Interfaces.Account;
using Infrastructure.Interfaces.Gallery;
using Infrastructure.Interfaces.IServices;
using Infrastructure.Interfaces.VideoReview;
using Infrastructure.Profiles;
using Infrastructure.Repositories;
using Infrastructure.Seed;
using Infrastructure.Services;
using Infrastructure.Services.Memory;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddHttpContextAccessor();

var allowedOrigins = new List<string>
{
    "http://kavsaracademy.tj",
    "http://www.kavsaracademy.tj",
    "https://kavsaracademy.tj",
    "https://www.kavsaracademy.tj",
    "http://37.27.249.153:5173", 
    "http://localhost:5173"
};
if (builder.Environment.IsDevelopment())
{
    allowedOrigins.Add("http://localhost:5173");
}

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(allowedOrigins.ToArray())
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});


builder.Services.AddRegisterService(builder.Configuration);
builder.Services.AddIdentity<User, IdentityRole<int>>()
    .AddEntityFrameworkStores<DataContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<IAccountService>(sp =>
    new AccountService(
        sp.GetRequiredService<UserManager<User>>(),
        sp.GetRequiredService<RoleManager<IdentityRole<int>>>(),
        sp.GetRequiredService<IConfiguration>(),
        builder.Environment.WebRootPath
    ));

builder.Services.AddScoped<IUserRepository, UserRepository>();

var uploadPath = builder.Configuration.GetValue<string>("UploadPath") ?? "wwwroot";
builder.Services.AddScoped<IUserService>(sp =>
    new UserService(
        sp.GetRequiredService<IUserRepository>(),
        sp.GetRequiredService<IMapper>(),
        sp.GetRequiredService<IHttpContextAccessor>(),
        sp.GetRequiredService<UserManager<User>>(),
        uploadPath
    ));

builder.Services.AddScoped<IChooseUsService>(sp =>
    new ChooseUsService(
        sp.GetRequiredService<IChooseUsRepository>(),
        sp.GetRequiredService<IMapper>(),
        uploadPath
    ));

builder.Services.AddScoped<IVideoReviewService>(sp =>
    new VideoReviewService(
        sp.GetRequiredService<DataContext>(),
        sp.GetRequiredService<IRedisMemoryCache>(),
        uploadPath
    ));

builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<ICourseService>(sp =>
    new CourseService(
        sp.GetRequiredService<ICourseRepository>(),
        uploadPath
    ));

builder.Services.AddScoped<IColleagueRepository, ColleagueRepository>();
builder.Services.AddScoped<IColleagueService>(sp =>
    new ColleagueService(
        sp.GetRequiredService<IColleagueRepository>(),
        uploadPath
    ));

builder.Services.AddScoped<INewsService>(sp =>
    new NewsService(
        sp.GetRequiredService<INewsRepository>(),
        sp.GetRequiredService<IRedisMemoryCache>(),
        uploadPath
    ));

builder.Services.AddScoped<IGalleryRepository, GalleryRepository>();
builder.Services.AddScoped<IGalleryService>(sp =>
    new GalleryService(
        sp.GetRequiredService<IGalleryRepository>(),
        sp.GetRequiredService<IRedisMemoryCache>(),
        uploadPath
    ));

builder.Services.AddScoped<IBannerService>(sp =>
    new BannerService(
        sp.GetRequiredService<IBannerRepository>(),
        builder.Environment.WebRootPath
    )
);


builder.Services.AddAutoMapper(typeof(EntityProfile));
builder.Services.AddMemoryCache();


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("Error"))
        )
    };
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Введите JWT через Bearer",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

builder.Services.AddControllers();

var app = builder.Build();


try
{
    using var scope = app.Services.CreateScope();
    var serviceProvider = scope.ServiceProvider;
    var dataContext = serviceProvider.GetRequiredService<DataContext>();
    await dataContext.Database.MigrateAsync();
    Console.WriteLine("Database migrated successfully.");
    var seeder = serviceProvider.GetRequiredService<SeedData>();
    await seeder.SeedRole();
    await seeder.SeedUser();
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}


app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")
    ),
    RequestPath = ""
});


app.UseCors("AllowFrontend");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Kavsar Academy API v1"));
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});


app.Run();
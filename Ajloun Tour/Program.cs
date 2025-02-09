using Ajloun_Tour;
using Ajloun_Tour.Implementations;
using Ajloun_Tour.Models;
using Ajloun_Tour.Reposetories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("YourConnectionString")));


//// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});



builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.MaxDepth = 128;
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    });


builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 10 * 1024 * 1024;
});


//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Other configurations...

    c.OperationFilter<FileUploadOperationFilter>();
});
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


//// Add authentication services
var key = Encoding.ASCII.GetBytes(builder.Configuration["JwtSettings:Secret"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };
});






//scoped here
builder.Services.AddScoped<IApplicationBuilder, ApplicationBuilder>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<INewsLattersRepository, NewsLattersRepository>();


builder.Services.AddAuthorization();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("AllowAllOrigins");
app.MapControllers();

app.Run();





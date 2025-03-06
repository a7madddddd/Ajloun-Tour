using Ajloun_Tour;
using Ajloun_Tour.Implementations;
using Ajloun_Tour.Models;
using Ajloun_Tour.Reposetories;
using Ajloun_Tour.Repositories.Implementations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("YourConnectionString")));
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");


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


builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 104857600; 
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

// Add JWT settings configuration
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

// Get JWT settings
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
if (string.IsNullOrEmpty(jwtSettings?.Secret))
{
    throw new InvalidOperationException("JWT Secret is not configured");
}

var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);

// Add authentication configuration (only once)
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
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IToursRepository, ToursRepository>();
builder.Services.AddScoped<INewsLattersRepository, NewsLattersRepository>();
builder.Services.AddScoped<IContactRepository, ContactRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IAdminsRepository, AdminsRepository>();
builder.Services.AddScoped<IProjectsRepository, ProjectsRepository>();
builder.Services.AddScoped<ITestomonialsRepository, TestomonialsRepository>();
builder.Services.AddScoped<IReviewsRepository, ReviewsRepository>();
builder.Services.AddScoped<IToursOffersRepository, ToursOffersRepository>();
builder.Services.AddScoped<IToursPackagesRepository, ToursPackagesRepository>();
builder.Services.AddScoped<IBookingOptionRepository, BookingOptionRepository>();
builder.Services.AddScoped<IBookingOptionsSelectionRepository, BookingOptionsSelectionRepository>();
builder.Services.AddScoped<IToursProgramRepository, ToursProgramRepository>();
builder.Services.AddScoped<ITourCartRepository, TourCartRepository>();
builder.Services.AddScoped<IPackagesRepository, PackagesRepository>();
builder.Services.AddScoped<IOffersRepository, OffersRepository>();
builder.Services.AddScoped<ITourProgramServiceRepository, TourProgramServiceRepository>();
builder.Services.AddScoped<IProgramRepository, ProgramRepository>();
builder.Services.AddScoped<IPackProgramServiceRepository, PackProgramServiceRepository>();
builder.Services.AddScoped<IPackageProgramRepository, PackageProgramRepository>();
builder.Services.AddScoped<IOffersProgramRepository, OffersProgramRepository>();
builder.Services.AddScoped<IOfferProgramServiceRepository, OfferProgramServiceRepository>();
builder.Services.AddScoped<ICartItemRepository, CartItemRepository>();






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
app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.MapControllers();

app.Run();





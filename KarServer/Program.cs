using KarServer.Data;
using KarServer.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http; // For CookieSecurePolicy
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using KarServer.Configuration;

var builder = WebApplication.CreateBuilder(args);

// **Add services to the container** 
builder.Services.AddControllers();

// **Configure the database context with PostgreSQL**
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// **Add Identity with default UI**
builder.Services.AddIdentity<AppUser, IdentityRole> ()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// **Configure cookie authentication settings**
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    options.LoginPath = "/Identity/Account/Login";
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest; // SecurePolicy set on Cookie object
});

// **Add JWT bearer authentication for APIs**
builder.Services.AddAuthentication()
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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

// **Add Razor Pages support**
builder.Services.AddRazorPages();

// **Add KaraokeSettings configuration**
builder.Services.Configure<KaraokeSettings>(builder.Configuration.GetSection("KaraokeSettings"));

// **Add Swagger for API documentation and testing**
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// **Build the application**
var app = builder.Build();

// **Configure the HTTP request pipeline**
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// **Redirect HTTP requests to HTTPS**
app.UseHttpsRedirection();

// **Serve static files**
app.UseStaticFiles();

// **Enable routing**
app.UseRouting();

// **Enable authentication and authorization**
app.UseAuthentication();
app.UseAuthorization();

// **Map endpoints**
app.MapControllers();
app.MapRazorPages();

//Seed Roles
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var roles = new[] { "Regular User", "User Manager", "Song Manager", "Event Manager", "Application Manager" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}

// **Start the application**
app.Run();
using TVGuide.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using TVGuide.Areas.Identity.Data;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.Google;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Database") ?? throw new InvalidOperationException("Connection string 'TVGuideContextConnection' not found.");

// Add services to the container.
builder.Services.AddScoped<IChannelRepository,ChannelRepository>();
builder.Services.AddDbContext<ApplicationDbContext>();

builder.Services.AddIdentity<TVGuideUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireNonAlphanumeric = false;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication().AddGoogle(options =>
{
    options.ClientId = builder.Configuration["Google:ClientId"];
    options.ClientSecret = builder.Configuration["Google:ClientSecret"];
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("IsAdmin", policy => policy.RequireRole("Admin"));
    options.AddPolicy("CanManageChannels", policy => policy.RequireClaim("Permission", "ManageChannels"));
});

builder.Services.AddScoped<IUserClaimsPrincipalFactory<TVGuideUser>, ApplicationUserClaimPrincipalsFactory>();

builder.Services.AddControllersWithViews();
builder.Services.AddMvc().AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix);

builder.Services.AddLocalization(options => { options.ResourcesPath = "Languages"; });

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new List<CultureInfo>
    {
        new CultureInfo("fr"),
        new CultureInfo("ar")
    };
    options.DefaultRequestCulture = new RequestCulture("fr");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

var app = builder.Build();



//PIPELINE
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseDeveloperExceptionPage();

app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();

app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();


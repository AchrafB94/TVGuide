using TVGuide.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Database") ?? throw new InvalidOperationException("Connection string 'TVGuideContextConnection' not found.");

// Add services to the container.
builder.Services.AddScoped<IChannelRepository,ChannelRepository>();
builder.Services.AddScoped<IFavoriteChannelRepository,FavoriteChannelRepository>();
builder.Services.AddDbContext<ChannelContext>();

builder.Services.AddDefaultIdentity<TVGuideUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireNonAlphanumeric = false;
})
    .AddEntityFrameworkStores<ChannelContext>();

builder.Services.AddControllersWithViews();

await ProgrammeContext.Setup(builder);

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//}

app.UseDeveloperExceptionPage();

app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();


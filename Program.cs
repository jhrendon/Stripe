using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Stripe;
using StripeApi.Data;
using StripeApi.Data.InMemory;
using StripeApi.UseCases.Products;
using StripeApi.UsesCases.Products;
using StripeApi.UsesCases.ShoppingCart;
using StripeApi.UsesCases.user;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<StripeApiSettings>(builder.Configuration);
builder.Services.Configure<StripeApiSettings>(options => {
    options.ConnectionString = builder.Configuration.GetConnectionString("PostgresDBConection") ?? throw new InvalidOperationException("Connection string 'PostgresDBConection' not found.");
    options.appStorageConnectionString = builder.Configuration.GetConnectionString("AppStorageConnection") ?? throw new InvalidOperationException("Connection string 'AppStorageConnection' not found.");
    options.StripeApiKey = builder.Configuration["StripeApiKey"] ?? throw new InvalidOperationException("StripeApiKey not found");
    options.StripeWebhookSecret = builder.Configuration["StripeWebhookSecret"] ?? throw new InvalidOperationException("StripeWebhookSecret not found");



    //options.appStorageConnectionString.
});


var connectionString = builder.Configuration.GetConnectionString("PostgresDBConection") ?? throw new InvalidOperationException("Connection string 'PostgresDBConection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<AddProduct>();
builder.Services.AddScoped<GetProducts>();


builder.Services
    .AddScoped<AddProduct>()
    .AddScoped<GetProducts>()
    .AddSingleton<IInMemoryShoppingCart, InMemoryShoppingCart>()
    .AddScoped<GetShoppingCart>()
    .AddScoped<AddToShoppingCart>()
    .AddScoped<SetPremium>()
    .AddScoped<RemovePremium>()
    .AddScoped<CancelSubscription>()
    .AddScoped<SetPremiumEnd>();


StripeConfiguration.ApiKey = builder.Configuration["StripeApiKey"];

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();
app.Run();

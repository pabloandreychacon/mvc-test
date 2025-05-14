using DataAccess.AppContext;
using mvc_test;
using mvc_test.Data;

var builder = WebApplication.CreateBuilder(args);

// Register DapperContext as a singleton service for database access
builder.Services.AddSingleton<DbContext>();

// Register ExpensesDataAccess for dependency injection
builder.Services.AddScoped<ExpensesDataAccess>();

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure DatabaseHelper with the connection string from configuration
DatabaseHelper.Configure(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
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

DatabaseHelper.Initialise();

app.Run();

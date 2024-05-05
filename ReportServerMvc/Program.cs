
using fyiReporting.ReportServerMvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSystemWebAdapters();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHostedService<StartupWorker>();
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();
app.UseSystemWebAdapters();

app.UseSession();

app.MapRazorPages();
app.MapDefaultControllerRoute();


app.Run();

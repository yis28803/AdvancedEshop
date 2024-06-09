var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddSession();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseSession();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


/*app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");*/


/*endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}");*/

app.UseEndpoints(endpoints =>
{
    /*endpoints.MapControllerRoute(
        name: "admin",
        pattern: "Admin/{action=Login}",
        defaults: new { controller = "Admin" });*/
    // Đường dẫn cụ thể cho trang đăng nhập Account2
    endpoints.MapControllerRoute(
        name: "account2",
        pattern: "Account2/{action=Login}",
        defaults: new { controller = "Account2" });

    // Đường dẫn cụ thể cho trang đăng nhập Admin
    endpoints.MapControllerRoute(
        name: "admin",
        pattern: "Admin/{action=Login}",
        defaults: new { controller = "Admin" });

    // Đường dẫn mặc định cho các controller và action khác
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});
app.Run();

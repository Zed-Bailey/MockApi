using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MockApi.Data;
using MockApi.Services;
using MudBlazor;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);




// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<DataService>();

// setup api service singleton, stores logs and the api status 
var apiService = new ApiService();
builder.Services.AddSingleton(apiService);

// Configuring the snackbar documentation
// https://mudblazor.com/components/snackbar#configuration
builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft;

    config.SnackbarConfiguration.PreventDuplicates = false;
    config.SnackbarConfiguration.NewestOnTop = false;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 5000;
    config.SnackbarConfiguration.HideTransitionDuration = 500;
    config.SnackbarConfiguration.ShowTransitionDuration = 500;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();



var app = builder.Build();
// middle ware documentation
// https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view=aspnetcore-6.0

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}



app.Use(async (ctx, next) =>
{
    apiService.Host = ctx.Request.Host.ToString();
    
    
    // check if the path contains the api endpoint, checks if the api has been enabled, blocks request if it hasn't returning a 404 response
    if (ctx.Request.Path.ToString().Contains("/api/data") )
    {
        if(apiService.ApiEnabled is false)
        {
            ctx.Response.StatusCode = 404;
            //MARK: return json response?
            await ctx.Response.WriteAsync("Api not enabled");
            return;
        }
       
        // add a log
        apiService.AddLog(ctx.Request);
    }
    
    await next.Invoke();
});

app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();


app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

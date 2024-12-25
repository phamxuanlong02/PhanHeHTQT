using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddDbContext<C500Hemis.Models.HemisContext>(options =>
//    options.UseSqlServer("Server=tcp:c500.database.windows.net,1433;Database=dbHemisC500;User Id=c500;Password=@Abc1234"));

// Add services to the container.
builder.Services.AddControllersWithViews().AddMvcOptions(options => {
    options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(_ => "Field is required 1");
    options.ModelBindingMessageProvider.SetAttemptedValueIsInvalidAccessor((a, b) => $"{a} không hợp lệ cho {b}");
    options.ModelBindingMessageProvider.SetMissingBindRequiredValueAccessor((a) => "Field is required 3");
    options.ModelBindingMessageProvider.SetMissingKeyOrValueAccessor(() => "Field is required 4");
    options.ModelBindingMessageProvider.SetMissingRequestBodyRequiredValueAccessor(() => "Field is required 5");
    options.ModelBindingMessageProvider.SetNonPropertyAttemptedValueIsInvalidAccessor((a) => "Field is required 6");
    options.ModelBindingMessageProvider.SetNonPropertyUnknownValueIsInvalidAccessor(() => "Field is required 7");
    options.ModelBindingMessageProvider.SetNonPropertyValueMustBeANumberAccessor(() => "Field is required 8");
    options.ModelBindingMessageProvider.SetUnknownValueIsInvalidAccessor((a) => "Field is required 9");
    options.ModelBindingMessageProvider.SetValueIsInvalidAccessor((a) => "Field is required 9");
    options.ModelBindingMessageProvider.SetValueMustBeANumberAccessor((a) => "Field is required 10");
    options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor((a) => "Field is required 11");
});

builder.Services.AddHttpClient<PhanHeHTQT.API.ApiServices>();
var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
     name: "default",
     pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
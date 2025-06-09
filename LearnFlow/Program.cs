var builder = WebApplication.CreateBuilder(args);

// Adiciona serviços MVC e sessão
builder.Services.AddControllersWithViews();
builder.Services.AddSession(); // <-- DEVE vir antes do builder.Build()

var app = builder.Build();

// Configuração do pipeline HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession(); // <-- Aqui é o lugar certo de usar a sessão no pipeline
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

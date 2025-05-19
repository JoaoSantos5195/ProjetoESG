using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LearnFlow.Models;
using Microsoft.AspNetCore.Components.Forms;

namespace LearnFlow.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    //VAI PARA HOME
    public IActionResult Index()
    {
        //instanciando a home model
        //criando o objeto
        HomeModel home = new HomeModel();
        home.Nome = "Usuário";
        home.Email = "EmailUser";
        return View(home);
    }
    
    //VAI PARA PÁGINA PRIVACY
    public IActionResult Sobre()
    {
        return View();
    }
    //VAI PARA MAPA
    public IActionResult Mapa(){
        return View();
    }
//VAI PARA LOGIN
    public IActionResult Login()
    {
        return View();
    }
    public IActionResult Perfil()
    {
        return View();
    }
//PÁGINA CADASTRO
    [HttpGet]
    public IActionResult Cadastro()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Cadastro(CadastroModel model)
    {
        string nome = model.Nome;
        string email = model.Email;
        string senha = model.Senha;
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

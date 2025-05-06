using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LearnFlow.Models;

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
        return View();
    }
    
    //VAI PARA PÁGINA PRIVACY
    public IActionResult Privacy()
    {
        return View();
    }
    //VAI PARA MAPA
    public IActionResult Mapa(){
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

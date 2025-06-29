using Microsoft.AspNetCore.Mvc;
using LearnFlow.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Http;

public class LoginController : Controller
{
    private string CaminhoArquivo => Path.Combine(Directory.GetCurrentDirectory(), "App_Data", "usuarios.json");

    [HttpGet]
    public IActionResult Index()
    {
        return View("~/Views/Home/Login.cshtml");
    }

    [HttpPost]
    public IActionResult Index(string email, string senha)
    {
        var usuarios = LerUsuarios();
        var usuarioValido = usuarios.Find(u => u.Email == email && u.Senha == senha);

        if (usuarioValido != null)
        {
            HttpContext.Session.SetString("usuario", usuarioValido.Email);
            return RedirectToAction("Index", "Home");
        }

        ViewBag.Mensagem = "Email ou senha incorretos.";
        return View("~/Views/Home/Login.cshtml");
    }

    private List<Usuario> LerUsuarios()
    {
        if (!System.IO.File.Exists(CaminhoArquivo))
            return new List<Usuario>();

        var json = System.IO.File.ReadAllText(CaminhoArquivo);
        return JsonConvert.DeserializeObject<List<Usuario>>(json) ?? new List<Usuario>();
    }
}

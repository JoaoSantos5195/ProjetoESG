using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LearnFlow.Models;
using LearnFlow.ViewModels;
using Microsoft.AspNetCore.Components.Forms;

namespace LearnFlow.Controllers;

public class HomeController : Controller
{
        private readonly IWebHostEnvironment _env;

        public HomeController(IWebHostEnvironment env)
        {
            _env = env;
        }

    //TRATAMENTO PADRÃO DE ERRO
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
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
    //VAI PARA PERFIL
    public IActionResult Perfil()
    {
        return View();
    }

    //PÁGINA CADASTRO
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

    //CRIAÇÃO DE MAPA


    [HttpGet]
    public IActionResult CriarMapa()
    {
        return View(new CriarMapa());
    }

    [HttpPost]
    public async Task<IActionResult> CriarMapa(CriarMapa model)
    {
        if (model.Imagem != null && model.Imagem.Length > 0)
        {
            // Gera o caminho físico onde a imagem será salva
            var nomeArquivo = Path.GetFileName(model.Imagem.FileName);
            var caminho = Path.Combine(_env.WebRootPath, "uploads", nomeArquivo);

            // Garante que a pasta exista
            Directory.CreateDirectory(Path.GetDirectoryName(caminho));

            // Salva a imagem
            using (var stream = new FileStream(caminho, FileMode.Create))
            {
                await model.Imagem.CopyToAsync(stream);
            }

            model.ImagemUrl = "/uploads/" + nomeArquivo;
        }

        return View(model);
    }

}

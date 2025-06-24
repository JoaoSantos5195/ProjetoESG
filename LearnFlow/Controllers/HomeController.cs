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
    //VAI PARA MAPAFechar ￼Editar Fase 1
    public IActionResult Mapa()
    {
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
    private static List<CriarFase> fasesDoMapa = new List<CriarFase>();
    private static CriarMapa mapaAtual = new CriarMapa();

    [HttpGet]
    public IActionResult CriarMapa()
    {
        ViewBag.Fases = fasesDoMapa; // Devolve as fases já salvas
        var viewModel = new MapaFaseViewModel
        {
            Mapa = new CriarMapa(),
            Fase = new CriarFase()
        };

        return View(viewModel);
    }

    [HttpPost]
    //TRATAMENTO DA IMAGEM ENVIADA, COLOCANDO-A NA PASTA UPLOADS
    public async Task<IActionResult> CriarMapa(MapaFaseViewModel model)
    {
        if (model.Mapa.Imagem != null && model.Mapa.Imagem.Length > 0)
        {
            var nomeArquivo = Path.GetFileName(model.Mapa.Imagem.FileName);
            var caminho = Path.Combine(_env.WebRootPath, "uploads", nomeArquivo);

            Directory.CreateDirectory(Path.GetDirectoryName(caminho));

            using (var stream = new FileStream(caminho, FileMode.Create))
            {
                await model.Mapa.Imagem.CopyToAsync(stream);
            }

            model.Mapa.ImagemUrl = "/uploads/" + nomeArquivo;
        }

        // Atualiza o mapaAtual
        mapaAtual = model.Mapa;

        return View("CriarMapa", new MapaFaseViewModel
        {
            Mapa = mapaAtual,
            Fase = new CriarFase()
        });
    }

    [HttpPost]
    public IActionResult CriarFase(MapaFaseViewModel model)
    {
        var fase = model.Fase;

        var faseExistente = fasesDoMapa.FirstOrDefault(f => f.IdFase == fase.IdFase);

        if (faseExistente != null)
        {
            faseExistente.TituloFase = fase.TituloFase;
            faseExistente.DescFase = fase.DescFase;
            faseExistente.LinkFase = fase.LinkFase;
        }
        else
        {
            fasesDoMapa.Add(fase);
        }

        ViewBag.Fases = fasesDoMapa;

        return View("CriarMapa", new MapaFaseViewModel
        {
            Mapa = mapaAtual,    // <- Aqui usa o mapaAtual guardado
            Fase = fase
        });
    }


}


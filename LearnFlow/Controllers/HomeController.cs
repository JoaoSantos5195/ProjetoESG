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
            return View();
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
    private static List<CriarFase> fasesDoMapa = new(); // Guarda as fases criadas
    private static CriarMapa mapaAtual = new();         // Guarda o mapa

    private static int proximoId = 1; // Controla IDs únicos

    // GET
    public IActionResult CriarMapa()
    {
        var viewModel = new MapaFaseViewModel
        {
            Mapa = mapaAtual,
            TodasFases = fasesDoMapa
        };

        return View(viewModel);
    }

    // POST do formulário do mapa
    [HttpPost]
    public async Task<IActionResult> CriarMapa(MapaFaseViewModel model)
    {
        mapaAtual = model.Mapa;

        // Salvar imagem se houver
        if (model.Mapa.Imagem != null)
        {
            var nomeArquivo = Path.GetFileName(model.Mapa.Imagem.FileName);
            var caminho = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", nomeArquivo);
            Directory.CreateDirectory(Path.GetDirectoryName(caminho));
            using (var stream = new FileStream(caminho, FileMode.Create))
                await model.Mapa.Imagem.CopyToAsync(stream);

            mapaAtual.ImagemUrl = "/uploads/" + nomeArquivo;
        }

        return RedirectToAction("CriarMapa");
    }

    // POST de criação ou edição de uma fase
    [HttpPost]
    public IActionResult CriarFase(MapaFaseViewModel model)
    {
        var fase = model.Fase;

        if (fase.IdFase == 0)
        {
            // Nova fase
            fase.IdFase = proximoId++;
            fasesDoMapa.Add(fase);
        }
        else
        {
            // Edição de fase existente
            var faseExistente = fasesDoMapa.FirstOrDefault(f => f.IdFase == fase.IdFase);
            if (faseExistente != null)
            {
                faseExistente.TituloFase = fase.TituloFase;
                faseExistente.DescFase = fase.DescFase;
                faseExistente.LinkFase = fase.LinkFase;
            }
        }

        return RedirectToAction("CriarMapa");
    }

}


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
        // Seu modelo atual (para dados do perfil)
        var homeModel = new HomeModel();

        // Modelo do mapa (se existir)
        var mapaViewModel = new MapaFaseViewModel
        {
            Mapa = mapaAtual,
            TodasFases = fasesDoMapa
        };

        // Junta os dois modelos em um ViewBag dinâmico
        ViewBag.HomeModel = homeModel;
        ViewBag.MapaViewModel = mapaViewModel;

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

    // POST do formulário do mapa
    [HttpPost]
    public async Task<IActionResult> editMapa(MapaFaseViewModel model, string opcao, string novoValor, IFormFile novaImagem)
    {
        var mapa = model.Mapa;
        switch (opcao)
        {
            case "tituloMapa":
                mapaAtual.TituloMapa = novoValor;
                break;
            case "editDesc":
                mapaAtual.DescMapa = novoValor;
                break;
            case "editLink":
                mapaAtual.LinkMapa = novoValor;
                break;
            case "editFoto":
                if (novaImagem != null && novaImagem.Length > 0)
                {
                    var nomeArquivo = Path.GetFileName(novaImagem.FileName);
                    var caminho = Path.Combine(_env.WebRootPath, "uploads", nomeArquivo);
                    Directory.CreateDirectory(Path.GetDirectoryName(caminho));

                    using (var stream = new FileStream(caminho, FileMode.Create))
                    {
                        await novaImagem.CopyToAsync(stream);
                    }

                    mapaAtual.ImagemUrl = "/uploads/" + nomeArquivo;
                }
                break;
        }
        return RedirectToAction("CriarMapa");
    }
    [HttpPost]
    [HttpPost]
    public IActionResult DeletarFase(int idFase)
    {
        try
        {
            var faseParaDeletar = fasesDoMapa.FirstOrDefault(f => f.IdFase == idFase);

            if (faseParaDeletar != null)
            {
                // 1. Remove a fase
                fasesDoMapa.Remove(faseParaDeletar);

                // 2. Reorganiza os IDs (1, 2, 3...)
                for (int i = 0; i < fasesDoMapa.Count; i++)
                {
                    fasesDoMapa[i].IdFase = i + 1; // IDs começam em 1
                }

                // 3. Atualiza o próximoId para evitar conflitos
                proximoId = fasesDoMapa.Count + 1;

                TempData["Mensagem"] = "Fase deletada e IDs reorganizados!";
            }
            else
            {
                TempData["Erro"] = "Fase não encontrada.";
            }

            return RedirectToAction("CriarMapa");
        }
        catch (Exception ex)
        {
            TempData["Erro"] = "Erro ao deletar fase: " + ex.Message;
            return RedirectToAction("CriarMapa");
        }
    }
    [HttpPost]
    public IActionResult ExcluirMapa()
    {
        try
        {
            // Limpa TODOS os dados do mapa
            mapaAtual = new CriarMapa {
                TituloMapa = "",
                DescMapa = "",
                LinkMapa = "",
                ImagemUrl = ""
            };
            
            fasesDoMapa.Clear();
            proximoId = 1;

            // Garante que a mensagem será exibida
            TempData["MensagemSucesso"] = "Mapa excluído com sucesso!";
            
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            TempData["MensagemErro"] = "Erro ao excluir mapa: " + ex.Message;
            return RedirectToAction("CriarMapa");
        }
    }
}

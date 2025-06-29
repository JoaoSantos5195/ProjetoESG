using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LearnFlow.Models;
using LearnFlow.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace LearnFlow.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _env;

        public HomeController(IWebHostEnvironment env)
        {
            _env = env;
        }

        public IActionResult Index()
        {
            var usuario = HttpContext.Session.GetString("usuario");

            if (string.IsNullOrEmpty(usuario))
            {
                return RedirectToAction("Index", "Login");
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Mapa()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Sobre()
        {
            var usuario = HttpContext.Session.GetString("usuario");

            if (string.IsNullOrEmpty(usuario))
            {
                return RedirectToAction("Index", "Login");
            }

            return View();
        }

        public IActionResult Perfil()
        {
            var usuario = HttpContext.Session.GetString("usuario");

            if (string.IsNullOrEmpty(usuario))
            {
                return RedirectToAction("Index", "Login");
            }

            ViewBag.Mensagem = null;

            var homeModel = new HomeModel();

            var mapaViewModel = new MapaFaseViewModel
            {
                Mapa = mapaAtual,
                TodasFases = fasesDoMapa
            };

            ViewBag.HomeModel = homeModel;
            ViewBag.MapaViewModel = mapaViewModel;

            return View();
        }

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

        private static List<CriarFase> fasesDoMapa = new();
        private static CriarMapa mapaAtual = new();
        private static int proximoId = 1;

        public IActionResult CriarMapa()
        {
            var viewModel = new MapaFaseViewModel
            {
                Mapa = mapaAtual,
                TodasFases = fasesDoMapa
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CriarMapa(MapaFaseViewModel model)
        {
            mapaAtual = model.Mapa;

            if (model.Mapa.Imagem != null)
            {
                var nomeArquivo = Path.GetFileName(model.Mapa.Imagem.FileName);
                var caminho = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", nomeArquivo);
                var dir = Path.GetDirectoryName(caminho);

                if (dir != null)
                {
                    Directory.CreateDirectory(dir);
                }

                await using var stream = new FileStream(caminho, FileMode.Create);
                await model.Mapa.Imagem.CopyToAsync(stream);

                mapaAtual.ImagemUrl = "/uploads/" + nomeArquivo;
            }

            return RedirectToAction("CriarMapa");
        }

        [HttpPost]
        public IActionResult CriarFase(MapaFaseViewModel model)
        {
            var fase = model.Fase;

            if (fase.IdFase == 0)
            {
                fase.IdFase = proximoId++;
                fasesDoMapa.Add(fase);
            }
            else
            {
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

        [HttpPost]
        public async Task<IActionResult> editMapa(MapaFaseViewModel model, string opcao, string novoValor, IFormFile? novaImagem)
        {
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
                        var dir = Path.GetDirectoryName(caminho);

                        if (dir != null)
                        {
                            Directory.CreateDirectory(dir);
                        }

                        await using var stream = new FileStream(caminho, FileMode.Create);
                        await novaImagem.CopyToAsync(stream);

                        mapaAtual.ImagemUrl = "/uploads/" + nomeArquivo;
                    }
                    break;
            }
            return RedirectToAction("CriarMapa");
        }

        [HttpPost]
        public IActionResult DeletarFase(int idFase)
        {
            try
            {
                var faseParaDeletar = fasesDoMapa.FirstOrDefault(f => f.IdFase == idFase);

                if (faseParaDeletar != null)
                {
                    fasesDoMapa.Remove(faseParaDeletar);

                    for (int i = 0; i < fasesDoMapa.Count; i++)
                    {
                        fasesDoMapa[i].IdFase = i + 1;
                    }

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
                mapaAtual = new CriarMapa
                {
                    TituloMapa = "",
                    DescMapa = "",
                    LinkMapa = "",
                    ImagemUrl = ""
                };

                fasesDoMapa.Clear();
                proximoId = 1;

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
}

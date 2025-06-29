using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using LearnFlow.Models;

namespace LearnFlow.Controllers
{
    public class CadastroController : Controller
    {
        private readonly string caminhoArquivo = Path.Combine(Directory.GetCurrentDirectory(), "App_Data", "usuarios.json");

        [HttpGet]
        public IActionResult Cadastro()
        {
            return View("~/Views/Home/Cadastro.cshtml");
        }

        [HttpPost]
        public IActionResult Cadastro(CadastroModel model)
        {
            // Validação do email
            if (string.IsNullOrEmpty(model.Email) || !model.Email.Contains("@"))
            {
                ViewBag.Erro = "Email inválido, deve conter '@'.";
                return View("~/Views/Home/Cadastro.cshtml", model);
            }

            List<CadastroModel> usuarios;

            // Lê usuários existentes
            if (System.IO.File.Exists(caminhoArquivo))
            {
                var json = System.IO.File.ReadAllText(caminhoArquivo);
                usuarios = JsonConvert.DeserializeObject<List<CadastroModel>>(json) ?? new List<CadastroModel>();
            }
            else
            {
                usuarios = new List<CadastroModel>();
            }

            usuarios.Add(model);

            // Cria a pasta App_Data se não existir
            Directory.CreateDirectory(Path.GetDirectoryName(caminhoArquivo)!);

            // Salva o JSON atualizado
            var jsonAtualizado = JsonConvert.SerializeObject(usuarios, Formatting.Indented);
            System.IO.File.WriteAllText(caminhoArquivo, jsonAtualizado);

            ViewBag.Sucesso = "Cadastro realizado com sucesso!";
            ModelState.Clear();

            return View("~/Views/Home/Cadastro.cshtml", new CadastroModel());
        }
    }
}

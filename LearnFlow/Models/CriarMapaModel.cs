using Microsoft.AspNetCore.Http;

namespace LearnFlow.Models
{
    public class CriarMapa
    {
        public IFormFile? Imagem { get; set; } // para receber o upload
        public string ImagemUrl { get; set; } = string.Empty; // caminho para exibir a imagem
        public string? TituloMapa { get; set; }
        public string? DescMapa { get; set; }
        public string LinkMapa { get; set; } = string.Empty;
        public int QntFases { get; set; }
    }

    public class CriarFase
    {
        public int? IdFase { get; set; }
        public string? TituloFase { get; set; }
        public string? DescFase { get; set; }
        public string LinkFase { get; set; } = string.Empty;
    }
}

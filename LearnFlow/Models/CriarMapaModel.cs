namespace LearnFlow.Models;

public class CriarMapa
{
    public IFormFile Imagem { get; set; } // para receber o upload
    public string ImagemUrl { get; set; } // caminho para exibir a imagem
    public string? TituloMapa { get; set; }
    public string? DescMapa { get; set; }
    public string LinkMapa { get; set; }
    public int QntFases { get; set; }
}
using LearnFlow.Models;
namespace LearnFlow.ViewModels;

public class MapaFaseViewModel
{
    public CriarMapa Mapa { get; set; }
    public CriarFase Fase { get; set; } = new CriarFase(); // Fase sendo editada/criada
    public List<CriarFase> TodasFases { get; set; } = new List<CriarFase>(); // Lista de todas as fases
}

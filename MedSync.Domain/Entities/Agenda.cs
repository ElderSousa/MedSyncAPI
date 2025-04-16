using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata.Ecma335;

namespace MedSync.Domain.Entities;

public class Agenda : BaseModel
{
    public Guid MedicoId { get; set; }
    public DayOfWeek DiaSemana { get; set; }
    public DateTime DataDisponivel { get; set; }
   
    public Medico Medico { get; set; } = new();
    public List<Horario> Horarios { get; set; } = new();

    [NotMapped]
    public bool ValidacaoCadastrar { get; set; }
}


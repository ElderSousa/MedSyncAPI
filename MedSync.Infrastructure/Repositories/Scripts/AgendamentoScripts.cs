namespace MedSync.Infrastructure.Repositories.Scripts;

public class AgendamentoScripts
{
    internal static readonly string SelectBase =
        @"
            SELECT 
                Id, 
                AgendaId,
                MedicoId,
                DataHora,
                DiaSemana,
                CriadoEm,
                CriadoPor,
                ModificadoEm,
                MoficadoPor
            FROM
                agendamentos
            WHERE
                ExcluidoEm IS NULL
        ";

    internal static readonly string Insert =
        @"
            INSET INTO agendamento(
                Id, 
                AgendaId,
                MedicoId,
                DataHora,
                DiaSemana,
                CriadoEm,
                CriadoPor,
                ModificadoEm,
                MoficadoPor
            )VALUES(
                @Id, 
                @AgendaId,
                @MedicoId,
                @DataHora,
                @DiaSemana,
                @CriadoEm,
                @CriadoPor,
                @ModificadoEm,
                @MoficadoPor
            )             
        ";

    internal static readonly string WhereId =
        @"
            AND Id = @Id
        ";

    internal static readonly string WhereAgendaId =
        @"
            AND AgendaId = @AgendaId
        ";

    internal static readonly string WhereMedicoId =
        @"
            AND MedicoId = @MedicoId
        ";

    internal static readonly string Update =
        @"
            UPDATE SET agendas
                DataHora = @DataHora,
                DiaSemana = @DiaSemana,  
                ModificadoEm = @ModificadoEm,
                MoficadoPor = @MoficadoPor
            WHERE
                Id = @Id
                AND ExcluidoEm IS NULL
        ";
    
    internal static readonly string Delete =
        @"
            UPDATE SET agendas
                ModificadoEm = @ModificadoEm,
                MoficadoPor = @MoficadoPor,
                ExcluidoEm = @ExcluidoEm
            WHERE
                Id = @Id
                AND ExcluidoEm IS NULL
        "; 
    internal static readonly string periodo =
        @"
            SELECT 
                COUNT(*)
            FROM 
                agendas
            WHERE
                DataHora = @DataHora
                AND DiaSemana = @DiaSemana
                AND ExcluidoEm IS NULL
        ";
    
    internal static readonly string Existe =
        @"
            SELECT 
                COUNT(*)
            FROM 
                agendas
            WHERE
                Id = @Id
                AND ExcluidoEm IS NULL
        ";

    
}

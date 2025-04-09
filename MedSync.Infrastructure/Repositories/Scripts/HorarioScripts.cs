namespace MedSync.Infrastructure.Repositories.Scripts;

public class HorarioScripts
{
    internal static readonly string SelectBase =
        @"
            SELECT
                Id, 
                AgendaId,
                HorarioInicial,
                HorarioFinal,
                CriadoEm,
                CriadoPor,
                ModificadoEm,
                ModificadoPor
            FROM
                horarios
            WHERE
                ExcluidoEm IS NULL
        ";  
    
    internal static readonly string Insert =
        @"
           INSERT INTO horarios(
                Id, 
                AgendaId,
                HorarioInicial,
                HorarioFinal,
                CriadoEm,
                CriadoPor,
                ModificadoEm,
                ModificadoPor
           )VALUES(
                @Id, 
                @AgendaId,
                @HorarioInicial,
                @HorarioFinal,
                @CriadoEm,
                @CriadoPor,
                @ModificadoEm,
                @ModificadoPor
           )
        ";
    
    internal static readonly string Update =
        @"
            UPDATE horarios SET
                HorarioInicial = @HorarioInicial,
                HorarioFinal = @HorarioFinal,
                ModificadoEm = @ModificadoEm
            WHERE
                Id = @Id
                AND ExcluidoEm IS NULL
        "; 
    
    internal static readonly string Delete =
        @"
            UPDATE horarios SET
                ModificadoEm = @ModificadoEm,
                ExcluidoEm = @ModificadoEm
            WHERE
                Id = @Id
                AND ExcluidoEm IS NULL
        ";

    internal static readonly string Existe =
        @"
            SELECT
                COUNT(*)
            FROM
                horarios
            WHERE
                Id = @Id
                AND ExcluidoEm IS NULL
        ";
    
    internal static readonly string WhereId =
        @"
            AND Id = @Id
        ";
    
    internal static readonly string WhereAgendaId =
        @"
            AND AgendaId = @AgendaId
        ";


    internal static readonly string HorarioPeriodoExiste =
       @"
            SELECT 
                COUNT(*) AS Total
            FROM
                horarios
            WHERE
                HorarioInicial <= @HorarioInicial
                    AND HorarioFinal >= @HorarioFinal
                    AND ExcluidoEm IS NULL
        ";

}

namespace MedSync.Infrastructure.Repositories.Scripts;

public class HorarioScripts
{
    internal static readonly string SelectBase =
        @"
            SELECT
                Id, 
                AgendaId,
                Hora,
                Agendado,
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
                Hora,
                Agendado,
                CriadoEm,
                CriadoPor,
                ModificadoEm,
                ModificadoPor
           )VALUES(
                @Id, 
                @AgendaId,
                @Hora,
                @Agendado,
                @CriadoEm,
                @CriadoPor,
                @ModificadoEm,
                @ModificadoPor
           )
        ";

    internal static readonly string Update =
        @"
            UPDATE horarios SET
                Hora = @Hora,
                Agendado = @Agendado,
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


    internal static readonly string HorarioExiste =
       @"
            SELECT 
                COUNT(*)
            FROM
                horarios
            WHERE
                Hora = @Hora
                    AND Agendado = 0
                    AND ExcluidoEm IS NULL
        ";

    internal static readonly string Status =
        @"
            UPDATE horarios SET
                Agendado = @Agendado,
                ModificadoEm = @ModificadoEm
            WHERE
                Id = @Id
                AND ExcluidoEm IS NULL
        ";

    internal static readonly string WhereAgendadoFalse =
        @"
            AND Agendado = 0
        ";

    internal static readonly string WhereHoraIntervaloInvalido =
        @"
            SELECT 
                COUNT(*)
            FROM
                horarios
            WHERE
                ABS(TIMESTAMPDIFF(MINUTE,
                            STR_TO_DATE(Hora, '%H:%i:%s'),
                            STR_TO_DATE(@Hora, '%H:%i:%s'))) < 20;
        ";

}

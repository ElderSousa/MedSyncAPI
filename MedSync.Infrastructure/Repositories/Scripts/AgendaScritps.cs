namespace MedSync.Infrastructure.Repositories.Scripts;

public class AgendaScritps
{
    internal static readonly string SelectBase =
        @"
             SELECT
                a.Id,
                a.MedicoId,
                a.DiaSemana,
                a.DataDisponivel,
                a.CriadoEm,
                a.CriadoPor,
                a.ModificadoEm,
                a.ModificadoPor,
                m.Id, 
                m.PessoaId,
                m.CRM,
                m.Especialidade,
                m.CriadoEm,
                m.CriadoPor,
                m.ModificadoEm,
                m.ModificadoPor,
                p.Id,
                p.Nome,
                p.CPF,
                p.RG,
                p.Sexo,
                p.DataNascimento,
                p.Email,
                p.CriadoEm,
                p.CriadoPor,
                p.ModificadoEm,
                p.ModificadoPor,
                t.Id,
	            t.PacienteId,
	            t.MedicoId,
	            t.Numero,
	            t.Tipo,
	            t.CriadoEm,
	            t.CriadoPor,
	            t.ModificadoEm,
	            t.ModificadoPor,
                h.Id, 
                h.AgendaId,
                h.HorarioInicial,
                h.HorarioFinal,
                h.CriadoEm,
                h.CriadoPor,
                h.ModificadoEm,
                h.ModificadoPor
            FROM
                agendas a INNER JOIN
                medicos m ON m.Id = a.MedicoId
	                AND m.ExcluidoEm IS NULL INNER JOIN
                pessoas p ON p.Id = m.PessoaId
                    AND p.ExcluidoEm IS NULL INNER JOIN
	            telefones t ON t.MedicoId = m.Id
                    AND t.ExcluidoEm IS NULL INNER JOIN
                horarios h ON h.AgendaId = a.Id
                    AND h.ExcluidoEm IS NULL
            WHERE
                a.ExcluidoEm IS NULL
        ";

    internal static readonly string Insert =
        @"
            INSERT INTO agendas(
                Id,
                MedicoId,
                DiaSemana,
                DataDisponivel,
                Agendado,
                CriadoEm,
                CriadoPor,
                ModificadoEm,
                ModificadoPor
            )VALUES(
                @Id,
                @MedicoId,
                @DiaSemana,
                @DataDisponivel,
                @Agendado,
                @CriadoEm,
                @CriadoPor,
                @ModificadoEm,
                @ModificadoPor
            )
        ";

    internal static readonly string Update =
        @"
            UPDATE agendas SET
                DataDisponivel = @DataDisponivel,
                DiaSemana = @DiaSemana,
                ModificadoEm = @ModificadoEm
            WHERE
                Id = @Id
                AND ExcluidoEm IS NULL
        ";

    internal static readonly string Delete =
        @"
            UPDATE agendas SET
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
                agendas
            WHERE 
                Id = @Id
                AND ExcluidoEm IS NULL
        ";

    internal static readonly string DataHoraExiste =
        @"
            SELECT
                COUNT(*)
            FROM 
                agendas
            WHERE 
                DataDisponivel = @DataDisponivel
                AND DiaSemana = @DiaSemana
                AND Agendado = 0
                AND ExcluidoEm IS NULL
        ";

    internal static readonly string WhereId =
        @"
            AND a.Id = @Id
        ";

    internal static readonly string WhereMedicoId =
        @"
            AND a.MedicoId = @MedicoId
        ";

}

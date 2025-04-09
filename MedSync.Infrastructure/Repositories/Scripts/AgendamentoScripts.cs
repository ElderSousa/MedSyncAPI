namespace MedSync.Infrastructure.Repositories.Scripts;

public class AgendamentoScripts
{
    internal static readonly string SelectBase =
        @"
             SELECT 
	            ag.Id, 
                ag.AgendaId,
                ag.MedicoId,
                ag.PacienteId,
                ag.DataAgendamento AS AgendadoPara,
                ag.DiaSemana,
                ag.Horario,
                ag.Tipo,
                ag.Status,
                ag.CriadoEm,
                ag.CriadoPor,
                ag.ModificadoEm,
                ag.ModificadoPor,
                a.Id,
                a.MedicoId,
                a.DiaSemana,
                a.DataDisponivel,
                a.Horario,
                a.CriadoEm,
                a.CriadoPor,
                a.ModificadoEm,
                a.ModificadoPor,
                pa.Id,
                pa.PessoaId,
                pa.CriadoEm,
                pa.CriadoPor,
                pa.ModificadoEm,
                pa.ModificadoPor,
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
                p2.Id,
                p2.Nome,
                p2.CPF,
                p2.RG,
                p2.Sexo,
                p2.DataNascimento,
                p2.Email,
                p2.CriadoEm,
                p2.CriadoPor,
                p2.ModificadoEm,
                p2.ModificadoPor,
                t.Id,
                t.PacienteId,
                t.MedicoId,
                t.Numero,
                t.Tipo,
                t.CriadoEm,
                t.CriadoPor,
                t.ModificadoEm,
                t.ModificadoPor,
                e.Id,
                e.PacienteId,
                e.MedicoId,
                e.Logradouro,
                e.Numero,
                e.Complemento,
                e.Bairro,
                e.Cidade,
                e.Estado,
                e.CEP,
                e.CriadoEm,
                e.CriadoPor,
                e.ModificadoEm,
                e.ModificadoPor
            FROM
	            agendamentos ag INNER JOIN
                agendas a ON a.Id = ag.AgendaId
                    AND ag.ExcluidoEm IS NULL INNER JOIN
                pacientes pa ON pa.Id = ag.PacienteId
                    AND pa.ExcluidoEm IS NULL INNER JOIN
                medicos m ON m.Id = ag.MedicoId
                    AND m.ExcluidoEm IS NULL INNER JOIN
                pessoas p ON p.Id = pa.PessoaId
                    AND p.ExcluidoEm IS NULL INNER JOIN
                pessoas p2 ON p2.Id = m.PessoaId
                    AND p2.ExcluidoEm IS NULL INNER JOIN
                telefones t ON t.PacienteId = pa.Id
                    OR t.MedicoId = m.Id
                    AND t.ExcluidoEm IS NULL INNER JOIN
                enderecos e ON e.PacienteId = pa.Id
                    AND e.ExcluidoEm IS NULL
            WHERE
                a.ExcluidoEm IS NULL
        ";

    internal static readonly string Insert =
        @"
            INSET INTO agendamento(
                Id, 
                AgendaId,
                MedicoId,
                PacienteId,
                DataAgendamento,
                DiaSemana,
                Horario,
                Tipo,
                Status,
                CriadoEm,
                CriadoPor,
                ModificadoEm,
                MoficadoPor
            )VALUES(
                @Id, 
                @AgendaId,
                @MedicoId,
                @PacienteId,
                @DataAgendamento,
                @DiaSemana,
                @Horario,
                @Tipo,
                @Status,
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
                DataAgendamento = @DataAgendamento,
                DiaSemana = @DiaSemana,
                Horario = @Horario,
                Tipo = @Tipo,
                Status = @Status,
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
                DataAgendamento = @DataAgendamento
                AND DiaSemana = @DiaSemana
                AND Horario = @Horario
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

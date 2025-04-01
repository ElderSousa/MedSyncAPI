namespace MedSync.Infrastructure.Repositories.Scripts;

public class AgendamentoScritps
{
    internal static readonly string SelectBase =
        @"
             SELECT
                a.Id,
                a.PacienteId,
                a.MedicoId,
                a.AgendadoPara,
                a.Status,
                a.Observacao,
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
                agendamentos a INNER JOIN
                pacientes pa ON pa.Id = a.PacienteId 
	                    AND pa.ExcluidoEm IS NULL INNER JOIN
                medicos m ON m.Id = a.MedicoId
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
            INSERT INTO agendamentos(
                Id,
                PacienteId,
                MedicoId,
                AgendadoPara,
                Status,
                Observacao,
                CriadoEm,
                CriadoPor,
                ModificadoEm,
                ModificadoPor
            )VALUES(
                @Id,
                @PacienteId,
                @MedicoId,
                @AgendadoPara,
                @Status,
                @Observacao,
                @CriadoEm,
                @CriadoPor,
                @ModificadoEm,
                @ModificadoPor
            )
        ";

    internal static readonly string Update =
        @"
            UPDATE agendamentos SET
                AgendadoPara = @AgendadoPara,
                Status = @Status,
                Observacao = @Observacao,
                TipoAgendamento = @TipoAgendamento,
                ModificadoEm = @ModificadoEm
            WHERE
                Id = @Id
                AND ExcluidoEm IS NULL
        ";

    internal static readonly string Delete =
        @"
            UPDATE agendamentos SET
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
                agendamentos
            WHERE 
                Id = @Id
                AND ExcluidoEm IS NULL
        ";

    internal static readonly string AgendadoParaExiste =
        @"
            SELECT
                COUNT(*)
            FROM 
                agendamentos
            WHERE 
                AgendadoPara = @AgendadoPara
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
    
    internal static readonly string WherePacienteId =
        @"
            AND a.PacienteId = @PacienteId
        ";
}

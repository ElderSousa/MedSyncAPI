namespace MedSync.Infrastructure.Repositories.Scripts;

public class TelefoneScripts
{
    internal static readonly string SelectBase =
        @"
            SELECT 
                Id,
                PacienteId,
                MedicoId,
                Numero,
                Tipo,
                CriadoEm,
                CriadoPor,
                ModificadoEm,
                ModificadoPor
            FROM
                telefones
            WHERE
                ExcluidoEm IS NULL    
        ";

    internal static readonly string Insert =
        @"
            INSERT INTO telefones(
                Id,
                PacienteId,
                MedicoId,
                Numero,
                Tipo,
                CriadoEm,
                CriadoPor,
                ModificadoEm,
                ModificadoPor
            )VALUES(
                @Id,
                @PacienteId,
                @MedicoId,
                @Numero,
                @Tipo,
                @CriadoEm,
                @CriadoPor,
                @ModificadoEm,
                @ModificadoPor
            )
        ";

    internal static readonly string WhereId =
        @"
            AND Id = @Id
        ";

    internal static readonly string WhereMedicoId =
        @"
            AND MedicoId = @MedicoId
        ";
    
    internal static readonly string WherePcienteId =
        @"
            AND PacienteId = @PacienteId
        ";

    internal static readonly string WhereNumero =
        @"
            AND Numero = @Numero
        ";

    internal static readonly string Update =
        @"
            UPDATE telefones SET
                Numero = @Numero,
                Tipo = @Tipo,
                ModificadoEm = @ModificadoEm,
                ModificadoPor = @ModificadoPor
            WHERE
                Id = @Id
                AND ExcluidoEm IS NULL
        ";
    internal static readonly string Delete =
        @"
            UPDATE telefones SET
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
                telefones
            WHERE
                Id = @Id
                AND ExcluidoEm IS NULL
        ";
}

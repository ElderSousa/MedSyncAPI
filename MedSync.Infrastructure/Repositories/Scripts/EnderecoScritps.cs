namespace MedSync.Infrastructure.Repositories.Scripts;

public class EnderecoScritps
{
    internal static readonly string SelectBase =
        @"
            SELECT
               Id, 
               PacienteId,
               MedicoId,
               Logradouro,
               Numero,
               Complemento,
               Bairro,
               Cidade,
               Estado,
               CEP,
               CriadoEm,
               CriadoPor,
               ModificadoEm,
               ModificadoPor
            FROM
                enderecos
            WHERE
                ExcluidoEm IS NULL      
        ";

    internal static readonly string Insert =
        @"
            INSERT INTO enderecos(
                Id, 
                PacienteId,
                MedicoId,
                Logradouro,
                Numero,
                Complemento,
                Bairro,
                Cidade,
                Estado,
                CEP,
                CriadoEm,
                CriadoPor,
                ModificadoEm,
                ModificadoPor
            )VALUES(
                @Id, 
                @PacienteId,
                @MedicoId,
                @Logradouro,
                @Numero,
                @Complemento,
                @Bairro,
                @Cidade,
                @Estado,
                @CEP,
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

    internal static readonly string WhereCEP =
        @"
            AND CEP = @CEP
        ";

    internal static readonly string Update =
        @"
            UPDATE enderecos SET
                Logradouro = @Logradouro,
                Numero = @Numero,
                Complemento = @Complemento,
                Bairro = @Bairro,
                Cidade = @Cidade,
                Estado = @Estado,
                CEP = @CEP,
                ModificadoEm = @ModificadoEm,
                ModificadoPor = @ModificadoPor

            WHERE
                Id = @Id
                AND ExcluidoEm IS NULL
        ";

    internal static readonly string Delete =
        @"
            UPDATE enderecos SET
               
                ModificadoEm = @ModificadoEm,
                ExcluidoEm  = @ModificadoEm
            WHERE 
                Id = @Id
                AND ExcluidoEm IS NULL
        ";

    internal static readonly string Existe =
        @"
            SELECT
                COUNT(*)
            FROM
                enderecos
            WHERE
                Id = @Id
                AND ExcluidoEm IS NULL
                
        ";
}

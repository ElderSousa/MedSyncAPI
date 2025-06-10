Sistema de Agendamento Médico
Este projeto consiste em uma API RESTful desenvolvida com .NET para gerenciamento de agendamentos médicos. A aplicação foi construída seguindo princípios da Clean Architecture e boas práticas de desenvolvimento, com cobertura de testes unitários e uso de ferramentas modernas do ecossistema .NET.

🛠 Tecnologias Utilizadas
.NET 8
C#
xUnit
Moq
AutoMapper
FluentValidation
MySQL (via Docker)
Docker / Docker Compose
Clean Architecture
Swagger (documentação de API)
✅ Funcionalidades
Cadastro de pacientes e médicos
Criação e validação de agendamentos
Atualização de status de horários
Listagem de agendas e horários disponíveis
Testes unitários com validações de regras de negócio
API versionada e documentada via Swagger
▶️ Como Executar
Pré-requisitos
.NET 8 SDK instalado
Docker Desktop (ou engine Docker) configurado e rodando.
Visual Studio ou Visual Studio Code
Passos para execução com Docker Compose
Este projeto utiliza Docker Compose para orquestrar o ambiente de desenvolvimento, incluindo a API e o banco de dados MySQL.

Clonar o repositório:

Bash

git clone https://github.com/seuusuario/seu-projeto.git
cd seu-projeto/MedSync # Navegue para a pasta raiz da solução
(Ajuste seu-projeto/MedSync para o caminho correto onde está seu docker-compose.yml)

Configurar a Connection String da API:

Abra o arquivo src/API/appsettings.json (ou src/API/appsettings.Development.json).
Certifique-se de que a ConnectionStrings__DefaultConnection aponte para o nome do serviço do banco de dados no Docker Compose (db).
JSON

{
  "ConnectionStrings": {
    "DefaultConnection": "Server=db;Port=3306;Database=medsync_db;Uid=root;Pwd=123456;"
  },
  // ... outras configurações
}
Configurar o Script SQL de Inicialização do Banco de Dados:

Abra o arquivo db_image/SchemaMedSync.sql.
Verifique se a linha para o usuário root está presente no final do arquivo (antes do /*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;):
SQL

-- ADICIONAR ESTAS LINHAS NO FINAL DO SEU SchemaMedSync.sql
ALTER USER 'root'@'%' IDENTIFIED WITH mysql_native_password BY '123456';
FLUSH PRIVILEGES;
Esta linha garante a compatibilidade da autenticação do MySQL 8.0 com clientes como o DBeaver.
Iniciar o Ambiente Docker Compose:

No terminal, na pasta raiz da sua solução (MedSync/MedSync/ ou onde seu docker-compose.yml estiver), execute:
Bash

docker compose up --build -d
O comando --build garantirá que as imagens sejam construídas (ou reconstruídas se houver mudanças), e o -d fará com que os containers rodem em segundo plano.
Verificar o Status dos Containers:

Bash

docker ps
Você deverá ver medsync-api-instance e medsync-db-instance com status Up.

Acessar a API:

Abra seu navegador e acesse a documentação da API via Swagger: http://localhost:8080/swagger
Conectando ao Banco de Dados com DBeaver (ou similar)
Você pode usar uma ferramenta como o DBeaver para gerenciar seu banco de dados MySQL que está rodando no Docker.

Host: localhost
Porta: 3306
Banco de Dados: medsync_db
Usuário: root
Senha: 123456
Configurações Adicionais no DBeaver (se encontrar erro "Public Key Retrieval is not allowed"):
Nas propriedades da conexão (aba "Driver properties"):

allowPublicKeyRetrieval = TRUE
useSSL = FALSE
Parando os Serviços Docker
Para parar e remover os containers, redes e volumes criados pelo Docker Compose:

Bash

docker compose down --volumes
(O --volumes remove os volumes de dados, útil para um início limpo do DB.)

Executando os Testes
Bash

cd tests/NomeDoSeuProjeto.Tests
dotnet test
Estrutura do Projeto
src/
├── API/              # Camada de apresentação (controllers)
├── Application/      # Casos de uso e regras de negócio
├── Domain/           # Entidades e interfaces
├── CrossCutting/     # Serviços e configurações compartilhadas (ex: injeção de dependência, helpers)
├── Infrastructure/   # Implementações técnicas (banco de dados, serviços externos)
└── Tests/            # Testes unitários
db_image/             # Imagem Docker para o banco de dados
├── Dockerfile        # Define a imagem do MySQL
└── SchemaMedSync.sql # Script SQL para inicialização do banco de dados

Próximos Passos
Implementar autenticação e autorização via JWT
Configurar pipeline de integração contínua
Disponibilizar ambiente de homologação
Licença
Este projeto está licenciado sob a Licença MIT.


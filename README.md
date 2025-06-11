# 🏥 MedSync – Sistema de Agendamento Médico

API RESTful desenvolvida com **.NET 8** para gerenciamento de agendamentos médicos, construída com base na **Clean Architecture** e boas práticas modernas de desenvolvimento. A aplicação conta com **testes unitários**, **validações robustas**, **containerização com Docker**, e **documentação interativa via Swagger**.

---

## ⚙️ Tecnologias Utilizadas

- [.NET 8](https://dotnet.microsoft.com/)
- C#
- xUnit
- Moq
- AutoMapper
- FluentValidation
- MySQL (via Docker)
- Docker / Docker Compose
- Clean Architecture
- Swagger (Swashbuckle)

---

## ✅ Funcionalidades

- Cadastro de **pacientes** e **médicos**
- **Criação e validação** de agendamentos médicos
- Atualização de **status de horários**
- Listagem de **agendas** e **horários disponíveis**
- **Testes unitários** com validações de regras de negócio
- **API versionada** e documentada via Swagger

---

## ▶️ Como Executar

### ✅ Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/products/docker-desktop) instalado e em execução
- Visual Studio ou VS Code

---

### 📦 Passos para execução com Docker Compose

1. **Clonar o repositório**

```bash
git clone https://github.com/seuusuario/seu-projeto.git
cd seu-projeto/MedSync
```

> Certifique-se de ajustar o caminho conforme a estrutura do seu projeto.

---

2. **Configurar Connection String**

No arquivo `src/API/appsettings.json` (ou `appsettings.Development.json`), atualize a conexão com o banco:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=db;Port=3306;Database=medsync_db;Uid=root;Pwd=123456;"
  }
}
```

---

3. **Ajustar script de inicialização do banco**

Edite o arquivo `db_image/SchemaMedSync.sql` e adicione as linhas abaixo ao final:

```sql
ALTER USER 'root'@'%' IDENTIFIED WITH mysql_native_password BY '123456';
FLUSH PRIVILEGES;
```

> Isso garante compatibilidade de autenticação com MySQL 8+ para ferramentas como DBeaver.

---

4. **Subir os containers**

Na raiz do projeto (onde está o `docker-compose.yml`):

```bash
docker compose up --build -d
```

---

5. **Verificar se está rodando**

```bash
docker ps
```

Você verá `medsync-api-instance` e `medsync-db-instance` com status `Up`.

---

6. **Acessar a API**

Abra o navegador:

```
http://localhost:5000/swagger
```

---

## 📂 Acesso ao Banco de Dados (opcional)

Ferramenta sugerida: [DBeaver](https://dbeaver.io/)

**Parâmetros de conexão:**

- **Host**: `localhost`
- **Porta**: `3306`
- **Database**: `medsync_db`
- **Usuário**: `root`
- **Senha**: `123456`

**Configurações adicionais (se necessário):**

- `allowPublicKeyRetrieval = TRUE`
- `useSSL = FALSE`

---

## 🧪 Executando os Testes

```bash
cd tests/NomeDoSeuProjeto.Tests
dotnet test
```

---

## 🧱️ Estrutura do Projeto

```
src/
├── API/              # Camada de apresentação (Controllers, Swagger)
├── Application/      # Casos de uso, validações, DTOs, serviços de aplicação
├── Domain/           # Entidades e interfaces de domínio
├── CrossCutting/     # Injeção de dependência, helpers, configurações globais
├── Infrastructure/   # Acesso a dados, repositórios e serviços externos
└── Tests/            # Projetos de testes unitários
db_image/
├── Dockerfile        # Imagem customizada do MySQL
└── SchemaMedSync.sql # Script de criação e configuração inicial do banco
```

---

## 🐳 Arquivo `docker-compose.yml`

```yaml
services:
  db:
    build:
      context: ./db_image
      dockerfile: Dockerfile
    container_name: medsync-db-instance
    ports:
      - "3306:3306"
    environment:
      MYSQL_ROOT_PASSWORD: 123456
      MYSQL_DATABASE: medsync_db
      MYSQL_ROOT_AUTHENTICATION_PLUGIN: 'mysql_native_password'
    volumes:
      - medsync_db_data:/var/lib/mysql
    restart: unless-stopped

  medsync.api:
    build:
      context: .
      dockerfile: MedSync.Api/Dockerfile
    container_name: medsync-api-instance
    ports:
      - "5000:5000"
    environment:
      ASPNETCORE_URLS: http://+:5000
      ASPNETCORE_ENVIRONMENT: Development
      MYSQL_SERVER_MEDSYNC: db
      MYSQL_DB_MEDSYNC: medsync_db
      MYSQL_PORT_MEDSYNC: 3306
      MYSQL_USER_MEDSYNC: root
      MYSQL_PASSWORD: 123456
    depends_on:
      - db
    restart: unless-stopped

volumes:
  medsync_db_data:
```

---

## 📌 Próximos Passos

- 🔐 Implementar autenticação e autorização com **JWT**
- ⚙️ Criar **pipeline de CI/CD**
- 🌐 Disponibilizar ambiente de **homologação**
- 🔒 Adicionar proteção de rotas e roles (autorização por perfil)

---

## 📄 Licença

Este projeto está licenciado sob a [Licença MIT](LICENSE).


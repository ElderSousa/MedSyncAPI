# Sistema de Agendamento Médico

Este projeto consiste em uma API RESTful desenvolvida com .NET para gerenciamento de agendamentos médicos. A aplicação foi construída seguindo princípios da Clean Architecture e boas práticas de desenvolvimento, com cobertura de testes unitários e uso de ferramentas modernas do ecossistema .NET.

## Tecnologias Utilizadas

- .NET 8
- C#
- xUnit
- Moq
- AutoMapper
- FluentValidation
- SQL Server (ou MySQL)
- Clean Architecture

## Funcionalidades

- Cadastro de pacientes e médicos
- Criação e validação de agendamentos
- Atualização de status de horários
- Listagem de agendas e horários disponíveis
- Testes unitários com validações de regras de negócio

## Como Executar

### Pré-requisitos

- .NET 8 SDK instalado
- SQL Server ou MySQL configurado
- Visual Studio ou Visual Studio Code

### Passos para execução local

```bash
# Clonar o repositório
git clone https://github.com/seuusuario/seu-projeto.git
cd seu-projeto

# Restaurar os pacotes
dotnet restore

# Aplicar as migrações (caso utilize Entity Framework Core)
dotnet ef database update

# Executar o projeto
dotnet run
```

## Executando os Testes

```bash
cd tests/NomeDoSeuProjeto.Tests
dotnet test
```

## Estrutura do Projeto

```
src/
├── API/                # Camada de apresentação (controllers)
├── Application/        # Casos de uso e regras de negócio
├── Domain/             # Entidades e interfaces
├── CrossCutting/       # Serviços e configurações compartilhadas (ex: injeção de dependência, helpers)
├── Infrastructure/     # Implementações técnicas (banco de dados, serviços externos)
└── Tests/              # Testes unitários
```

## Próximos Passos

- Implementar autenticação e autorização via JWT
- Adicionar documentação via Swagger
- Configurar pipeline de integração contínua
- Disponibilizar ambiente de homologação

## Licença

Este projeto está licenciado sob a Licença MIT.


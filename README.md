# StayFlow

Sistema de gestão para hotéis e pousadas, com API em ASP.NET Core e interface em Blazor WebAssembly. Reúne cadastro de quartos e hóspedes, reservas, check-in, check-out e indicadores de ocupação e receita.

## Funcionalidades

- Consulta de disponibilidade por período e quantidade de hóspedes.
- Reservas com validação de sobreposição de datas e controle de concorrência otimista.
- Cotação por diária: adicional de 25% nas noites de sexta e sábado e desconto de 10% em estadias de pelo menos sete noites.
- Check-in, check-out e cancelamento com validação do estado da reserva.
- Cadastro de hotéis, hóspedes, quartos e recepcionistas, com perfis Manager e Receptionist.
- Isolamento dos dados por hotel.
- Dashboard com ocupação, ADR, RevPAR e previsão de ocupação para os próximos 14 dias.
- Interface e mensagens em português, inglês e espanhol.

## Tecnologias e organização

.NET 10, ASP.NET Core, Blazor WebAssembly, Entity Framework Core, SQLite e FluentValidation. A autenticação usa JWT em cookie HttpOnly e senhas com Argon2id. Os testes usam xUnit, Moq e NetArchTest.

```text
src/
  Backend/
    StayFlow.Api              Controllers, autenticação e configuração HTTP
    StayFlow.Application      Casos de uso, validações e mapeamentos
    StayFlow.Domain           Entidades, regras de negócio e interfaces
    StayFlow.Infrastructure   Persistência, repositórios, hashing e tokens
  Frontend/
    StayFlow.Web              Interface Blazor WebAssembly
  Shared/
    StayFlow.Communication    Contratos de requisição e resposta
    StayFlow.Exception        Exceções e mensagens localizadas
tests/
  StayFlow.UnitTests
  StayFlow.ArchitectureTests
```

O domínio concentra as regras de preços, reservas e indicadores. A aplicação coordena os casos de uso por meio de interfaces, enquanto a infraestrutura implementa o acesso a dados e os serviços externos. Os testes de arquitetura verificam as dependências entre camadas e as convenções do projeto.

## Executando localmente

Requer o SDK do .NET 10.

```bash
dotnet restore StayFlow.slnx
dotnet dev-certs https --trust
```

Inicie a API em um terminal:

```bash
dotnet run --project src/Backend/StayFlow.Api --launch-profile https
```

Em outro terminal, inicie a interface:

```bash
dotnet run --project src/Frontend/StayFlow.Web --launch-profile https
```

- Interface: https://localhost:7002
- API: https://localhost:7001
- Documentação dos endpoints: https://localhost:7001/scalar/v1
- Health check: https://localhost:7001/health

O SQLite é criado automaticamente na inicialização. Com `SeedDemoData` habilitado, a aplicação também inclui um hotel de demonstração.

### Acesso de demonstração

| Perfil | E-mail | Senha |
| --- | --- | --- |
| Manager | `manager@stayflow.dev` | `Demo@1234` |
| Receptionist | `frontdesk@stayflow.dev` | `Demo@1234` |

Essas contas e a chave JWT de exemplo são destinadas ao ambiente local. Para hospedar a aplicação, configure uma chave própria em `Jwt__SigningKey`, desative `SeedDemoData` e ajuste `Cors:AllowedOrigins` para o endereço da interface.

## Validação

```bash
dotnet build StayFlow.slnx --configuration Release
dotnet test StayFlow.slnx --configuration Release
dotnet format whitespace StayFlow.slnx --verify-no-changes --no-restore
```

A suíte cobre regras de reservas, preços e indicadores, validações, casos de uso, hashing de senhas e dependências entre camadas. O workflow do GitHub Actions executa build e testes em pushes na branch `main` e em pull requests.

## Escopo atual

O banco usa `EnsureCreated`; alterações de esquema ainda precisam de migrations. A suíte atual contém testes unitários e de arquitetura. Testes de integração HTTP, refresh tokens e tarifas sazonais ficam como evoluções do projeto.

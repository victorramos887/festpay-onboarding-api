# 🧪 Teste Técnico - Dev Fullstack (.NET/C#) - Festpay

## 🎯 Objetivo

Construir e manter uma api em .NET 9 utilizando o padrão CQRS afim de manter um sistema de contas e transações da Festpay. Utilizando dos métodos já existentes, construa a entidade de Transações e o seu respectivo CRUD.
A entidade deve herdar a entidade base e possuir os seguintes dados:

- **Conta de destino**
- **Conta de origem**
- **Valor**
- **Cancelada**

Deverá ser desenvolvido métodos para:

- **Buscar todas as transações**
- **Buscar uma transação pelo Id**
- **Inserir uma transação**
- **Cancelar uma transação**

---

**ATENÇÃO** - Não se esqueça de desenvolver os testes de domínio e testes de aplicação.

---

## 🧱 Critérios de Avaliação

- Separação das regras de domínio e regras de aplicação
- Estrutura e funcionalidade do código existente e do código redigido
- Uso correto da arquitetura definida no projeto
- Princípios SOLID
- Tratamento de exceções
- Código limpo e organizado

---

## 📤 Entrega

- Criar um fork do projeto e submetê-lo com as implementações
- Atualizar o README com:
  - Tecnologias utilizadas
  - Instruções para rodar o projeto
- As instruções para envio do projeto deverão seguir as orientações enviadas pelo recrutador.

---

## 🛠️ Tecnologias utilizadas

- .NET 9
- C#
- ASP.NET Core Web API
- Carter
- MediatR
- FluentValidation
- Entity Framework Core
- SQLite / InMemory para testes
- Swagger / OpenAPI
- xUnit

---

## 🚀 Como rodar o projeto

### Pré-requisitos

- .NET 9 SDK instalado

### Executar a aplicação

1. Restaurar os pacotes:
  - `dotnet restore`
2. Subir a API:
  - `dotnet run --project Festpay.Onboarding.Api`
3. Acessar o Swagger para testar os endpoints:
  - `/swagger`

### Executar os testes

1. Rodar os testes de aplicação:
  - `dotnet test tests/Festpay.Onboarding.Application.Tests/Festpay.Onboarding.Application.Tests.csproj`
2. Rodar os testes de domínio:
  - `dotnet test tests/Festpay.Onboarding.Domain.Tests/Festpay.Onboarding.Domain.Tests.csproj`

---

## ✅ O que foi implementado

- Entidade `Transaction`
- CRUD de transações
- Testes de domínio
- Testes de aplicação

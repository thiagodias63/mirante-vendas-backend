# Vendas API

API REST para registrar, consultar, atualizar e remover vendas. A aplicação usa .NET 10, ASP.NET Core Minimal APIs, Entity Framework Core e PostgreSQL.

## Pré-requisitos

- .NET SDK 10.x
- PostgreSQL acessível pela máquina onde a API será executada
- Banco de dados `vendas` e uma connection string válida

Confira a instalação do SDK com:

```powershell
dotnet --version
```

## Configuração

A API lê a connection string `DefaultConnection`. Configure-a por variável de ambiente para não manter credenciais no repositório:

```powershell
$env:ConnectionStrings__DefaultConnection = "Host=localhost;Port=5432;Database=vendas;Username=postgres;Password=<SUA_SENHA>"
```

O perfil local de execução usa o ambiente `Development` e a URL `http://localhost:5018`. A configuração de desenvolvimento em `Vendas.Api/appsettings.Development.json` é apenas um exemplo local; ajuste-a ou prefira a variável de ambiente acima.

## Restaurar, compilar e executar

Execute os comandos a partir da pasta que contém `Vendas.slnx`:

```powershell
dotnet restore Vendas.slnx
dotnet build Vendas.slnx
dotnet run --project Vendas.Api
```

Em ambiente de desenvolvimento, a documentação interativa do Swagger fica em [http://localhost:5018/swagger](http://localhost:5018/swagger), e o documento OpenAPI em [http://localhost:5018/openapi/v1.json](http://localhost:5018/openapi/v1.json).

## Banco de dados e migrations

As migrations do Entity Framework Core ficam em `Vendas.Infrastructure/Migrations`. Para aplicar as migrations pendentes ao banco configurado:

```powershell
dotnet ef database update --project Vendas.Infrastructure --startup-project Vendas.Api
```

Se o comando `dotnet ef` não estiver instalado, instale a ferramenta na versão usada pelo projeto:

```powershell
dotnet tool install --global dotnet-ef --version 10.0.12
```

## Testes

Os testes unitários usam xUnit e estão no projeto `Vendas.Tests`:

```powershell
dotnet test Vendas.Tests/Vendas.Tests.csproj
```

Para executar todos os testes da solução:

```powershell
dotnet test Vendas.slnx
```

## Endpoints

| Método   | Rota                    | Descrição                            |
| -------- | ----------------------- | ------------------------------------ |
| `POST`   | `/api/vendas`           | Registra uma venda                   |
| `GET`    | `/api/vendas`           | Lista vendas com filtros e paginação |
| `GET`    | `/api/vendas/{idVenda}` | Obtém uma venda pelo ID              |
| `PATCH`  | `/api/vendas/{idVenda}` | Atualiza parcialmente uma venda      |
| `DELETE` | `/api/vendas/{idVenda}` | Remove uma venda                     |

### Listar vendas

Os filtros são opcionais e combinados entre si:

- `produto`: busca parcial; use `*` como curinga, por exemplo `*camisa*`.
- `quantidade`: correspondência exata.
- `dataVenda`: correspondência exata, no formato `YYYY-MM-DD`.
- `_page`: página iniciando em zero; padrão `0`.
- `_size`: itens por página, de `1` a `100`; padrão `20`.
- `_order`: campo e direção (`asc` ou `desc`); padrão `DataVenda asc`. Campos aceitos: `produto`, `quantidade`, `dataVenda`, `precoUnitario` e `idVenda`.

Exemplo:

```text
GET /api/vendas?produto=*camisa*&quantidade=2&dataVenda=2026-10-08&_page=0&_size=20&_order=dataVenda%20asc
```

A resposta contém `data`, `page`, `size` e `totalItems`.

### Registrar uma venda

```json
{
  "produto": "camisa",
  "quantidade": 2,
  "precoUnitario": 150,
  "dataVenda": "2026-10-08"
}
```

### Atualizar parcialmente uma venda

Envie apenas os campos que deseja alterar:

```json
{
  "quantidade": 3
}
```

## Estrutura da solução

| Projeto                 | Responsabilidade                                             |
| ----------------------- | ------------------------------------------------------------ |
| `Vendas.Api`            | Endpoints HTTP, requests, respostas e inicialização da API   |
| `Vendas.Application`    | Serviços, queries, handlers e filtros dos casos de uso       |
| `Vendas.Domain`         | Entidades e contratos dos repositórios                       |
| `Vendas.Infrastructure` | Entity Framework Core, PostgreSQL, repositórios e migrations |
| `Vendas.Tests`          | Testes unitários xUnit                                       |

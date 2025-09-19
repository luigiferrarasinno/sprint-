# InvestmentApp - API REST para Gerenciamento de Investimentos

## 📋 Visão Geral

O **InvestmentApp** é uma API REST desenvolvida em **ASP.NET Core (.NET 9)** para gerenciar usuários, investimentos e a relação usuário-investimento. A aplicação utiliza **Oracle Database** como banco de dados e segue o padrão arquitetural **Controller → Service → Repository**.

## 🏗️ Arquitetura

```
InvestmentApp/
├── User/
│   ├── Controller/     # Controllers para endpoints de usuários
│   ├── Dao/           # Data Access Objects para operações específicas
│   ├── Dto/           # Data Transfer Objects
│   ├── Model/         # Modelo de dados do usuário
│   ├── Repository/    # Camada de acesso aos dados
│   └── Service/       # Lógica de negócio
├── Investimento/
│   ├── Controller/    # Controllers para endpoints de investimentos
│   ├── Dto/          # Data Transfer Objects
│   ├── Model/        # Modelo de dados do investimento
│   ├── Repository/   # Camada de acesso aos dados
│   └── Service/      # Lógica de negócio
├── Common/
│   ├── Controllers/  # Controllers compartilhados (UserInvestment)
│   ├── Middleware/   # Middleware personalizado
│   ├── Models/       # Modelos compartilhados
│   ├── Repository/   # Repositories compartilhados
│   └── Services/     # Serviços compartilhados
└── Data/            # DbContext e configurações do banco
```

## 🚀 Tecnologias Utilizadas

- **ASP.NET Core 9.0** - Framework web
- **Entity Framework Core** - ORM
- **Oracle Database** - Banco de dados
- **AutoMapper** - Mapeamento de objetos
- **Swashbuckle (Swagger)** - Documentação da API
- **Serilog** (implícito) - Logging

## 📊 Modelos de Dados

### User (Usuário)
```csharp
{
    "id": "guid",
    "nome": "string",
    "email": "string",
    "cpf": "string", 
    "dataNascimento": "datetime",
    "role": "Admin|User",
    "isActive": "boolean",
    "createdAt": "datetime",
    "updatedAt": "datetime"
}
```

### Investment (Investimento)
```csharp
{
    "id": "guid",
    "name": "string",
    "type": "string",
    "baseValue": "decimal",
    "expectedYieldPercent": "decimal",
    "riskLevel": "string",
    "description": "string",
    "isActive": "boolean",
    "createdAt": "datetime",
    "updatedAt": "datetime"
}
```

### UserInvestment (Investimento do Usuário)
```csharp
{
    "id": "guid",
    "userId": "guid",
    "investmentId": "guid",
    "amountInvested": "decimal",
    "units": "decimal",
    "purchaseDate": "datetime",
    "currentValue": "decimal",
    "status": "Ativo|Resgatado",
    "isActive": "boolean",
    "createdAt": "datetime",
    "updatedAt": "datetime"
}
```

## 🔧 Configuração

### 1. Pré-requisitos
- .NET 9.0 SDK
- Acesso ao Oracle Database (oracle.fiap.com.br:1521/ORCL)

### 2. Clonagem e Instalação
```bash
git clone <repository-url>
cd InvestmentApp
dotnet restore
```

### 3. Configuração do Banco de Dados

A connection string está configurada diretamente no código:
```csharp
"Data Source=oracle.fiap.com.br:1521/ORCL;User Id=RM98047;Password=201104;"
```

### 4. Executar Migrations
```bash
dotnet ef database update
```

### 5. Executar a Aplicação
```bash
dotnet run
```

A API estará disponível em:
- **HTTP**: `http://localhost:5020`
- **HTTPS**: `https://localhost:5021`  
- **Swagger UI**: `http://localhost:5020` (raiz da aplicação)

> **Nota**: A aplicação está configurada para rodar na porta 5020 (HTTP) por padrão.

## 📚 Endpoints da API

### 👥 Usuários (`/api/users`)

| Método | Endpoint | Descrição | Permissão |
|--------|----------|-----------|-----------|
| GET | `/api/users` | Lista todos os usuários | Admin only |
| GET | `/api/users/{id}` | Obtém usuário por ID | Admin ou próprio usuário |
| POST | `/api/users` | Cria novo usuário | Público |
| PUT | `/api/users/{id}` | Atualiza usuário | Admin ou próprio usuário |
| DELETE | `/api/users/{id}` | Remove usuário (soft delete) | Admin only |

### 💰 Investimentos (`/api/investimentos`)

| Método | Endpoint | Descrição | Permissão |
|--------|----------|-----------|-----------|
| GET | `/api/investimentos` | Lista todos os investimentos | Público |
| GET | `/api/investimentos/{id}` | Obtém investimento por ID | Público |
| POST | `/api/investimentos` | Cria novo investimento | Admin only |
| PUT | `/api/investimentos/{id}` | Atualiza investimento | Admin only |
| DELETE | `/api/investimentos/{id}` | Remove investimento | Admin only |

### 🔗 Investimentos do Usuário (`/api/users/{userId}/investimentos`)

| Método | Endpoint | Descrição | Permissão |
|--------|----------|-----------|-----------|
| GET | `/api/users/{userId}/investimentos` | Lista investimentos do usuário | Admin ou próprio usuário |
| POST | `/api/users/{userId}/investimentos` | Cria investimento do usuário | Admin ou próprio usuário |
| GET | `/api/users/{userId}/investimentos/{id}` | Obtém investimento específico | Admin ou próprio usuário |
| PUT | `/api/users/{userId}/investimentos/{id}` | Atualiza investimento | Admin ou próprio usuário |
| DELETE | `/api/users/{userId}/investimentos/{id}` | Remove investimento | Admin ou próprio usuário |

## 🔐 Sistema de Autorização

O sistema utiliza um controle simples baseado em roles via header HTTP:

- **Header**: `userId` (GUID do usuário fazendo a requisição)
- **Roles**: "Admin" ou "User"
- **Regras**:
  - Admins podem acessar todos os recursos
  - Usuários comuns só podem acessar seus próprios dados
  - Criação de usuários é pública

## 📋 Dados de Teste

### Usuário Admin
```json
{
    "email": "admin@investmentapp.com",
    "senha": "admin123",
    "role": "Admin"
}
```

### Usuário Teste
```json
{
    "email": "joao@teste.com", 
    "senha": "usuario123",
    "role": "User"
}
```

### Investimentos Pré-cadastrados
1. **Tesouro Direto - Selic 2029** (Renda Fixa, Baixo Risco)
2. **Fundo Multimercado XP** (Fundo, Médio Risco)
3. **FII Kinea Renda Imobiliária** (Fundo Imobiliário, Médio Risco)
4. **Ações VALE3** (Ação, Alto Risco)

## 🧪 Exemplos de Uso

### Criar Usuário
```bash
curl -X POST "http://localhost:5020/api/users" \
  -H "Content-Type: application/json" \
  -d '{
    "nome": "Maria Santos",
    "email": "maria@teste.com",
    "senha": "senha123",
    "cpf": "11122233344",
    "dataNascimento": "1990-03-15T00:00:00Z"
  }'
```

### Listar Investimentos
```bash
curl -X GET "http://localhost:5020/api/investimentos"
```

### Criar Investimento (Admin)
```bash
curl -X POST "http://localhost:5020/api/investimentos" \
  -H "Content-Type: application/json" \
  -H "userId: {ADMIN_USER_ID}" \
  -d '{
    "name": "CDB Banco XYZ",
    "type": "Renda Fixa", 
    "baseValue": 1000.00,
    "expectedYieldPercent": 11.5,
    "riskLevel": "Baixo",
    "description": "CDB com liquidez diária"
  }'
```

### Criar Investimento do Usuário
```bash
curl -X POST "http://localhost:5020/api/users/{USER_ID}/investimentos" \
  -H "Content-Type: application/json" \
  -H "userId: {USER_ID}" \
  -d '{
    "investmentId": "{INVESTMENT_ID}",
    "amountInvested": 5000.00,
    "units": 50.0,
    "purchaseDate": "2024-01-15T00:00:00Z",
    "currentValue": 5250.00
  }'
```

## 🐛 Tratamento de Erros

A API possui middleware global de tratamento de erros que retorna respostas padronizadas:

```json
{
    "statusCode": 400,
    "message": "Mensagem de erro detalhada",
    "details": ""
}
```

## 📖 Documentação

A documentação completa da API está disponível via **Swagger UI** na raiz da aplicação:
- **URL**: `http://localhost:5020`
- **JSON**: `http://localhost:5020/swagger/v1/swagger.json`

## 🔍 Logs

A aplicação utiliza o sistema de logging padrão do ASP.NET Core. Os logs incluem:
- Operações CRUD em todos os serviços
- Erros e exceções
- Tentativas de acesso não autorizado

## 🚀 Deploy

Para deploy em produção:

1. Configure a connection string apropriada no `appsettings.Production.json`
2. Execute as migrations: `dotnet ef database update`
3. Publique a aplicação: `dotnet publish -c Release`

## 📄 Licença

Este projeto foi desenvolvido para fins acadêmicos como parte do curso de desenvolvimento em C#.

---

**Desenvolvido por**: Investment App Team  
**Contato**: contato@investmentapp.com

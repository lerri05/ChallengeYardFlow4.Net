# 🏍️ YardFlow - Gestão Inteligente de Pátio de Motos  
**>>> ORGANIZE | LOCALIZE | FLUA <<<**

O **YardFlow** é uma API desenvolvida em **.NET 8** para controle de entrada, saída, localização, locação de motos e gerenciamento de usuários em pátios.  
Com autenticação via **API Key**, versionamento de API, monitoramento de saúde e serviços inteligentes, o YardFlow proporciona uma gestão moderna, segura e eficiente.

---

## 📌 Índice

- [🚀 Funcionalidades](#-funcionalidades)  
- [💻 Tecnologias](#-tecnologias)  
- [📋 Pré-requisitos](#-pré-requisitos)  
- [🔧 Instalação](#-instalação)  
- [🏃 Execução](#-execução)  
- [📘 Documentação da API](#-documentação-da-api)  
- [🗂 Estrutura](#-estrutura)  
- [🚧 Status da Aplicação](#-status-da-aplicação)  
- [👥 Autores](#-autores)

---

## 🚀 Funcionalidades

### 🏍️ Gerenciamento de Motos
- Registro de entrada e saída de motos no pátio  
- Consulta de status da moto (disponível, alugada, manutenção, etc.)  
- Localização das motos dentro do pátio

### 👤 Gerenciamento de Usuários
- Cadastro, atualização e exclusão de usuários  
- Controle de permissões (administrador, funcionário)  
- Autenticação e autorização via **API Key**

### 📅 Locações
- Cálculo automático do valor da locação com base no período informado  
- Histórico de locações por usuário e moto  

### 🧠 Serviços Inteligentes
- **PricingService:** cálculo inteligente de preços  
- **SentimentService:** análise de sentimentos e automação de respostas  
- **MlController:** integração com módulos de aprendizado de máquina

### 🩺 Monitoramento
- Endpoint `/health` para verificação de disponibilidade do sistema  
- Versionamento automático da API (v1, v2, etc.)

---

## 💻 Tecnologias

- [.NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)  
- ASP.NET Core  
- Entity Framework Core  
- Oracle Database  
- Swagger (OpenAPI)  
- API Key Authentication  
- Health Checks  
- xUnit (Testes Unitários e de Integração)  
- IDE: Visual Studio ou VS Code

---

## 📋 Pré-requisitos

- .NET 8 SDK instalado  
- Banco de Dados Oracle configurado  
- Editor de código (VS Code, Visual Studio, etc.)

---

## 🔧 Instalação

Clone o repositório:

````bash
git clone https://github.com/lerri05/ChallengeYardFlow.git
cd ChallengeYardFlow
````
Configure o arquivo appsettings.json com sua string de conexão Oracle:
"ConnectionStrings": {
  "DefaultConnection": "User Id=seu_usuario;Password=sua_senha;Data Source=seu_servidor"
},
"ApiKey": {
  "Key": "sua_chave_de_api_aqui"
}

Aplique as migrações:
dotnet ef database update

🏃 Execução
dotnet run

O Swagger será inicializado automaticamente em:
https://localhost:5050/swagger

📘 Documentação da API

A API pode ser testada diretamente via Swagger UI.
Use o cabeçalho X-API-Key para autenticação em endpoints protegidos.

🔑 Autenticação

| Cabeçalho | Exemplo                   |
| --------- | ------------------------- |
| X-API-Key | `12345-abcde-67890-fghij` |

🏍️ Motos /api/moto
| Método | Endpoint         | Descrição                               |
| ------ | ---------------- | --------------------------------------- |
| GET    | `/api/moto`      | Lista todas as motos cadastradas        |
| GET    | `/api/moto/{id}` | Retorna os dados de uma moto específica |
| POST   | `/api/moto`      | Cadastra uma nova moto                  |
| PUT    | `/api/moto/{id}` | Atualiza os dados de uma moto existente |
| DELETE | `/api/moto/{id}` | Remove uma moto do sistema              |

📅 Locações /api/locacoes
| Método | Endpoint                 | Descrição                                                            |
| ------ | ------------------------ | -------------------------------------------------------------------- |
| POST   | `/api/locacoes/calcular` | Calcula o valor da locação de uma moto com base nas datas informadas |

👤 Usuários /api/usuarios
| Método | Endpoint             | Descrição                                 |
| ------ | -------------------- | ----------------------------------------- |
| GET    | `/api/usuarios`      | Lista todos os usuários cadastrados       |
| GET    | `/api/usuarios/{id}` | Retorna os dados de um usuário específico |
| POST   | `/api/usuarios`      | Cadastra um novo usuário                  |
| PUT    | `/api/usuarios/{id}` | Atualiza os dados de um usuário existente |
| DELETE | `/api/usuarios/{id}` | Remove um usuário do sistema              |

🤖 Machine Learning /api/ml
| Método | Endpoint         | Descrição                                   |
| ------ | ---------------- | ------------------------------------------- |
| POST   | `/api/ml/testar` | Executa análises e testes com modelos de ML |

🩺 Health Check /health
| Método | Endpoint  | Descrição                                |
| ------ | --------- | ---------------------------------------- |
| GET    | `/health` | Retorna o status de funcionamento da API |

🗂 Estrutura
ChallengeYardFlow
├── Controllers
│   ├── AuthController.cs
│   ├── LocacoesController.cs
│   ├── MlController.cs
│   ├── MotoController.cs
│   └── UsuariosController.cs
├── Data
│   └── LocadoraContext.cs
├── Migrations
│   ├── 20250519011323_Inicial.cs
│   ├── 20250918223050_Usuario.cs
│   └── LocadoraContextModelSnapshot.cs
├── Modelo
│   ├── Locacao.cs
│   ├── Moto.cs
│   └── Usuario.cs
├── Services
│   ├── ApiKeyAuthenticationHandler.cs
│   ├── PricingService.cs
│   └── SentimentService.cs
├── Properties
│   └── launchSettings.json
├── Tests
│   ├── ChallengeYardFlow.IntegrationTests
│   └── ChallengeYardFlow.UnitTests
├── Program.cs
├── appsettings.json
└── README.md

🚧 Status da Aplicação
✅ Aplicação concluida

👥 Autores
| Nome                     | RM     | GitHub                                             |
| ------------------------ | ------ | -------------------------------------------------- |
| Fernanda Budniak de Seda | 558274 | [@Febudniak](https://github.com/Febudniak)         |
| Lucas Lerri de Almeida   | 554635 | [@lerri05](https://github.com/lerri05)             |
| Karen Marques dos Santos | 554556 | [@KarenMarquesS](https://github.com/KarenMarquesS) |




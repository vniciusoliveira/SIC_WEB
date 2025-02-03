API_SIC_WEB_ANGULAR
===================

Descrição    
---------
API REST desenvolvida em .NET 6.0 para dar suporte ao sistema SIC Web Angular. Esta API fornece endpoints para diversas funcionalidades do sistema, incluindo gestão de medidas disciplinares, consultas de notificações, cadastro de CID, e outras funcionalidades administrativas.

Tecnologias Utilizadas
---------------------
- .NET 6.0
- SQL Server
- EPPlus (para manipulação de arquivos Excel)
- Swashbuckle/Swagger (documentação da API)
- System.Data.SqlClient

Configuração do Ambiente
-----------------------
1. Clone o repositório
2. Copie o arquivo `appsettings.example.json` para `appsettings.json`
3. Atualize as configurações no `appsettings.json` com suas credenciais locais
4. Execute o projeto

Principais Funcionalidades
-------------------------
1. Gestão de Medidas Disciplinares
   - Consulta de medidas pendentes
   - Validação de assinaturas (Gerente, Empregado, Testemunha)
   - Geolocalização das assinaturas

2. Consulta de Notificações
   - Geração de relatórios em Excel
   - Exportação de dados

3. Cadastro de CID
   - Recebimento e processamento de dados CID

4. Sistema InovAI
   - Gerenciamento de arquivos e pastas
   - Operações CRUD em arquivos

5. Jurídico
   - Relatórios de chat
   - Exportação de dados em Excel

6. Premiação Indique Amigo
   - Exportação de dados em CSV

Configuração do Banco de Dados
-----------------------------
O sistema utiliza duas conexões principais configuradas no appsettings.json:
- DataBase: Conexão principal do SIC_WEB
- EPONTO: Conexão secundária para sistema de ponto

Requisitos do Sistema
--------------------
- .NET 6.0 SDK ou superior
- SQL Server
- Acesso às bases de dados configuradas
- Permissões de sistema adequadas para operações com arquivos

Segurança
---------
- Implementação de validações de acesso por matrícula
- Registro de IP e hostname nas operações críticas
- Validação de geolocalização para assinaturas

Estrutura do Projeto
-------------------
- Controllers: Controladores da API REST
- Services: Camada de serviços
- Models: Modelos de dados
- DAL: Camada de acesso a dados
- DTO: Objetos de transferência de dados
- Interfaces: Definições de contratos
- Utility: Classes utilitárias

Contato
-------
Para suporte ou dúvidas, entre em contato com a equipe de desenvolvimento.

Observações
-----------
- Mantenha as credenciais de banco de dados seguras
- Realize backups regulares
- Monitore os logs do sistema
- Mantenha as dependências atualizadas

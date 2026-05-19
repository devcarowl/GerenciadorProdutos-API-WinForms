# Gerenciador de Produtos com ASP.NET Core e WinForms

Desenvolvi este projeto como parte de um teste técnico para Programador Jr. Ele consiste em uma API RESTful integrada a um banco de dados local e uma interface de usuário para o gerenciamento completo de produtos e categorias.

 Tecnologias Utilizadas

- **Back-end:** ASP.NET Core Web API (.NET 8)
- **Front-end:** Windows Forms (.NET 8)
- **Banco de Dados:** SQLite (com Entity Framework Core)
- **Arquitetura:** Padrão Repositório para separação estrita de conceitos.

 Funcionalidades Implementadas

- **CRUD Completo de Produtos:** Cadastro, listagem, atualização e exclusão baseados em GridView.
- **Listagem Dinâmica:** Integração com o endpoint de Categorias para preenchimento automático de seletores (`ComboBox`).
- **Paginação de Dados:** Listagem de produtos consumindo parâmetros de paginação da API para maior eficiência.
- **Módulo de Relatórios e Estatísticas:** Endpoint dedicado que calcula o total de produtos, média de preços e valor total em estoque.
- **Regras de Negócio Avançadas:** - Tratamento automático de strings para armazenar o nome do produto sempre com a primeira letra maiúscula. - Bloqueio de preços menores ou iguais a zero direto na API.
- **Inicialização Automática:** Criação do banco de dados e execução de migrações de forma automática no início da aplicação.

 Como Executar o Projeto

1. Abra a solução `MinhaApiComSQLite.sln` no Visual Studio.
2. Certifique-se de que ambos os projetos (`MinhaApiComSQLite` e `MinhaApiComSQLite.WinForms`) estão configurados para iniciar simultaneamente nas propriedades da Solução (Vários projetos de inicialização).
3. Clique em **Iniciar**.
4. A API abrirá a interface do Swagger e, em seguida, a tela do Gerenciador de Produtos estará pronta para uso.
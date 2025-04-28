# API REST Rede Pescador

Descrição
O Rede Pescador é uma API RESTful que permite gerenciar dados relacionados a pescadores, suas redes e atividades de pesca. A aplicação foi desenvolvida utilizando ASP.NET Core (C#) como framework backend e PostgreSQL como banco de dados relacional.

## Funcionalidades Principais
Cadastro e autenticação de usuários (pescadores).
Registro de redes de pesca.
Gerenciamento de capturas e relatórios de pesca.
Consulta de estatísticas e histórico de atividades.

## Pré-requisitos
- Para executar esta aplicação localmente, você precisará dos seguintes componentes instalados:

- .NET SDK : Versão 8 ou superior.
- PostgreSQL : Servidor de banco de dados PostgreSQL instalado e configurado.
- Docker : Para Orquestrar os Contêineres da aplicação e do Banco de dados.

## Configuração do Ambiente

1. Clonar o Repositório

```bash
git clone https://github.com/seu-usuario/app-rede-pescador.git
cd app-rede-pescador
```

## Criar os Contêineres

```bash
docker-compose up
```
-A aplicação iniciará em http://localhost:5207/index.html
### Para finalizar o processo:

```bash
docker-compose down
```

## Executando a Aplicação

- Para iniciar a aplicação, execute o seguinte comando:

```bash
cd rede-pescador-api
dotnet run
```

Por padrão, a API estará disponível em http://localhost:5000 ou https://localhost:5001.

## Testes

Para executar os testes unitários da aplicação, use o seguinte comando:

```bash
rede-pescador-api
dotnet test
```

## Contribuição

Contribuições são bem-vindas! Se você deseja contribuir para este projeto, siga os passos abaixo:

- Faça um fork do repositório.
- Crie uma branch para sua feature (git checkout -b feature/nova-funcionalidade).
- Faça commit das suas alterações (git commit -m 'Adiciona nova funcionalidade').
- Envie suas alterações para o repositório remoto (git push origin feature/nova-funcionalidade).
- Abra um pull request no GitHub.


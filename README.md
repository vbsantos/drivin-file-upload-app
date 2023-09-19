# Email File Upload Project

## Requisitos

- HTML, CSS e Javascript
- JQuery
- Bootstrap
- Asp.net Core (C#)

## Problema

Desenvolver uma aplicação web em Asp.Net Core, dado o arquivo “emails.txt”, contendo uma série de endereços de e-mails.

A página vai receber o upload desse arquivo e gerar subarquivos contendo uma combinação de 5 e-mails cada, com seu título em ordem crescente (ex: 01.txt, 02.txt...) de acordo com o número de novos arquivos gerados.

Dentro dos novos arquivos gerados, não poderá haver repetições e nem e-mails inválidos.

Ao final você deve mostrar na pagina os links para download dos arquivos juntamente com seu respectivo conteúdo em tabelas. Pode ser usado uma tabela para cada lista.

Obs.: essa página deve ser responsiva.

## Como executar

- Utilizando **Docker Compose**:

```bash
docker-compose up
```

- Utilizando **Docker**:

```bash
docker build -t drivin_project_image -f Drivin/Dockerfile .
docker run --name drivin_project_container -p 5274:5274 drivin_project_image
```

- Utilizando **dotnet cli**:

```bash
dotnet restore
dotnet build
dotnet run --project Drivin/Drivin.csproj
```

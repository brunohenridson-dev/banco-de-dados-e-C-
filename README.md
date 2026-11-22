
# Cadastro com C# (.NET), HTML e MySQL

Aplicação web simples feita para praticar **ASP.NET Core Minimal API** em C#, envio de formulário com upload de foto (`multipart/form-data`) e persistência no **MySQL** via `MySqlConnector`.

## Como funciona
- `GET /`: lê e devolve `bancodedado.html`.
- `POST /api/cadastro`: recebe os campos do formulário, salva a imagem com `GUID` na pasta `guardarfoto` e insere o registo na tabela `usuarios`.

## Base de dados (MySQL)
```sql
CREATE DATABASE IF NOT EXISTS siteAtual;
USE siteAtual;

CREATE TABLE IF NOT EXISTS usuarios (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nome VARCHAR(150),
    email VARCHAR(150),
    waifu VARCHAR(150),
    jogo_favorito VARCHAR(150),
    descricao TEXT,
    foto_caminho VARCHAR(255)
);

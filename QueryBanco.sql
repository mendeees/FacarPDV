/* =====================================================
   CRIAÇÃO DO BANCO DE DADOS
   ===================================================== */
IF DB_ID('BD_FacarPDV') IS NOT NULL
BEGIN
    PRINT 'Banco de dados já existe. Usando o existente...';
END
ELSE
BEGIN
    CREATE DATABASE BD_FacarPDV;
    PRINT 'Banco de dados BD_FacarPDV criado com sucesso!';
END
GO

USE BD_FacarPDV;
GO

/* =====================================================
   TABELA: NivelUsuario
   ===================================================== */
IF OBJECT_ID('dbo.NivelUsuario', 'U') IS NOT NULL DROP TABLE dbo.NivelUsuario;
CREATE TABLE dbo.NivelUsuario
(
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Descricao VARCHAR(250) NOT NULL
);
GO

/* =====================================================
   TABELA: Usuarios
   ===================================================== */
IF OBJECT_ID('dbo.Usuario', 'U') IS NOT NULL DROP TABLE dbo.Usuario;
CREATE TABLE dbo.Usuario
(
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Nome VARCHAR(250) NOT NULL,
    Login VARCHAR(20) NOT NULL UNIQUE,
    Senha VARCHAR(10) NOT NULL,
    NivelId INT NOT NULL,
    CONSTRAINT FK_Usuarios_NivelUsuario_NivelId
        FOREIGN KEY (NivelId) REFERENCES dbo.NivelUsuario(Id)
        ON DELETE NO ACTION
);
GO

/* =====================================================
   TABELA: Produto
   ===================================================== */
IF OBJECT_ID('dbo.Produto', 'U') IS NOT NULL DROP TABLE dbo.Produto;
CREATE TABLE dbo.Produto
(
    ProdutoId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Nome VARCHAR(250) NOT NULL,
    Preco DECIMAL(18,2) NOT NULL,
    Descricao VARCHAR(500) NOT NULL
);
GO

/* =====================================================
   TABELA: Clientes
   ===================================================== */
IF OBJECT_ID('dbo.Cliente', 'U') IS NOT NULL DROP TABLE dbo.Cliente;
CREATE TABLE dbo.Cliente
(
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Nome VARCHAR(250) NOT NULL,
    CPF CHAR(11) NULL UNIQUE,
    Telefone VARCHAR(20) NULL,
    Email VARCHAR(100) NULL UNIQUE
);
GO

/* =====================================================
   TABELA: Estoque
   ===================================================== */
IF OBJECT_ID('dbo.Estoque', 'U') IS NOT NULL DROP TABLE dbo.Estoque;
CREATE TABLE dbo.Estoque
(
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    ProdutoId INT NOT NULL UNIQUE,
    Quantidade INT NOT NULL DEFAULT 0 CHECK (Quantidade >= 0),
    CONSTRAINT FK_Estoque_Produto FOREIGN KEY (ProdutoId)
        REFERENCES dbo.Produto(ProdutoId)
);
GO

/* =====================================================
   TABELA: Vendas
   ===================================================== */
IF OBJECT_ID('dbo.Venda', 'U') IS NOT NULL DROP TABLE dbo.Venda;
CREATE TABLE dbo.Venda
(
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    UsuarioId INT NULL,
    ClienteId INT NULL,
    DataEmissao DATETIME NOT NULL DEFAULT GETDATE(),
    ValorTotal DECIMAL(18,2) NOT NULL DEFAULT 0,
    CONSTRAINT FK_Vendas_Usuarios_UsuarioId
        FOREIGN KEY (UsuarioId) REFERENCES dbo.Usuario(Id)
        ON DELETE SET NULL,
    CONSTRAINT FK_Vendas_Clientes_ClienteId
        FOREIGN KEY (ClienteId) REFERENCES dbo.Cliente(Id)
        ON DELETE SET NULL
);
GO

/* =====================================================
   TABELA: ItensVenda
   ===================================================== */
IF OBJECT_ID('dbo.ItemVenda', 'U') IS NOT NULL DROP TABLE dbo.ItemVenda;
CREATE TABLE dbo.ItemVenda
(
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    VendaId INT NOT NULL,
    ProdutoId INT NOT NULL,
    Quantidade INT NOT NULL,
    PrecoUnitario DECIMAL(18,2) NOT NULL,
    Subtotal AS (Quantidade * PrecoUnitario) PERSISTED,
    CONSTRAINT FK_ItensVenda_Vendas_VendaId
        FOREIGN KEY (VendaId) REFERENCES dbo.Venda(Id)
        ON DELETE CASCADE,
    CONSTRAINT FK_ItensVenda_Produto_ProdutoId
        FOREIGN KEY (ProdutoId) REFERENCES dbo.Produto(ProdutoId)
        ON DELETE NO ACTION
);
GO

/* =====================================================
   INSERÇÃO DE DADOS INICIAIS
   ===================================================== */

-- Níveis de usuário
INSERT INTO NivelUsuario (Descricao)
VALUES ('Administrador'), ('Funcionário');

-- Usuário administrador
INSERT INTO Usuario (Nome, Login, Senha, NivelId)
VALUES ('Administrador do Sistema', 'admin', '123', 1);

-- Produtos iniciais (EPIs)
INSERT INTO Produto (Nome, Preco, Descricao)
VALUES
('Capacete de Segurança CA 498', 55.00, 'Capacete classe B com suspensão e ajuste de tamanho'),
('Luva Nitrílica Azul', 12.00, 'Luva nitrílica resistente a rasgos e produtos químicos leves'),
('Botina de Segurança Bico PVC', 89.90, 'Botina preta com bico PVC e solado antiderrapante');

-- Clientes
INSERT INTO Cliente (Nome, CPF, Telefone, Email)
VALUES
('Maria Silva', '12345678901', '(17)99999-9999', 'maria@email.com'),
('João Pereira', '98765432100', '(17)98888-8888', 'joao@email.com');

-- Estoque
INSERT INTO Estoque (ProdutoId, Quantidade)
VALUES (1, 20), (2, 50), (3, 15);

PRINT '✅ Banco de dados BD_FacarPDV criado e populado com sucesso!';
GO

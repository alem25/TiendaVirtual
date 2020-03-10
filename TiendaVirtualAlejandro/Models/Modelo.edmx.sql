
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 03/10/2020 17:08:20
-- Generated from EDMX file: D:\MiW-UPM\net\TiendaVirtualAlejandro\TiendaVirtualAlejandro\Models\Modelo.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [TiendaTarde];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_ClientePedido]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Pedido] DROP CONSTRAINT [FK_ClientePedido];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Cliente]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Cliente];
GO
IF OBJECT_ID(N'[dbo].[Pedido]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Pedido];
GO
IF OBJECT_ID(N'[dbo].[Producto]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Producto];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Cliente'
CREATE TABLE [dbo].[Cliente] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Nombre] nvarchar(max)  NOT NULL,
    [Apellidos] nvarchar(max)  NOT NULL,
    [Email] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Producto'
CREATE TABLE [dbo].[Producto] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Nombre] nvarchar(max)  NOT NULL,
    [Precio] decimal(18,2)  NOT NULL,
    [Foto] nvarchar(max)  NOT NULL,
    [Descripción] nvarchar(max)  NULL,
    [Category] int  NOT NULL,
    [Cantidad] int  NOT NULL
);
GO

-- Creating table 'Pedido'
CREATE TABLE [dbo].[Pedido] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Cliente_Id] int  NOT NULL
);
GO

-- Creating table 'Stock'
CREATE TABLE [dbo].[Stock] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Cantidad] int  NOT NULL,
    [Producto_Id] int  NOT NULL,
    [Pedido_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Cliente'
ALTER TABLE [dbo].[Cliente]
ADD CONSTRAINT [PK_Cliente]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Producto'
ALTER TABLE [dbo].[Producto]
ADD CONSTRAINT [PK_Producto]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Pedido'
ALTER TABLE [dbo].[Pedido]
ADD CONSTRAINT [PK_Pedido]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Stock'
ALTER TABLE [dbo].[Stock]
ADD CONSTRAINT [PK_Stock]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Cliente_Id] in table 'Pedido'
ALTER TABLE [dbo].[Pedido]
ADD CONSTRAINT [FK_ClientePedido]
    FOREIGN KEY ([Cliente_Id])
    REFERENCES [dbo].[Cliente]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ClientePedido'
CREATE INDEX [IX_FK_ClientePedido]
ON [dbo].[Pedido]
    ([Cliente_Id]);
GO

-- Creating foreign key on [Producto_Id] in table 'Stock'
ALTER TABLE [dbo].[Stock]
ADD CONSTRAINT [FK_StockProducto]
    FOREIGN KEY ([Producto_Id])
    REFERENCES [dbo].[Producto]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_StockProducto'
CREATE INDEX [IX_FK_StockProducto]
ON [dbo].[Stock]
    ([Producto_Id]);
GO

-- Creating foreign key on [Pedido_Id] in table 'Stock'
ALTER TABLE [dbo].[Stock]
ADD CONSTRAINT [FK_PedidoStock]
    FOREIGN KEY ([Pedido_Id])
    REFERENCES [dbo].[Pedido]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PedidoStock'
CREATE INDEX [IX_FK_PedidoStock]
ON [dbo].[Stock]
    ([Pedido_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------
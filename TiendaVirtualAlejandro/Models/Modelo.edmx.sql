
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 03/11/2020 16:30:21
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
IF OBJECT_ID(N'[dbo].[FK_PedidoProductoVendido]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductosVendidos] DROP CONSTRAINT [FK_PedidoProductoVendido];
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
IF OBJECT_ID(N'[dbo].[ProductosVendidos]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProductosVendidos];
GO
IF OBJECT_ID(N'[dbo].[ProductosAlmacen]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProductosAlmacen];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Cliente'
CREATE TABLE [dbo].[Cliente] (
    [ClienteId] int IDENTITY(1,1) NOT NULL,
    [Nombre] nvarchar(max)  NOT NULL,
    [Apellidos] nvarchar(max)  NOT NULL,
    [Email] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Pedido'
CREATE TABLE [dbo].[Pedido] (
    [PedidoId] int IDENTITY(1,1) NOT NULL,
    [Cliente_ClienteId] int  NOT NULL
);
GO

-- Creating table 'ProductosVendidos'
CREATE TABLE [dbo].[ProductosVendidos] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Nombre] nvarchar(max)  NOT NULL,
    [Descripcion] nvarchar(max)  NULL,
    [Foto] nvarchar(max)  NOT NULL,
    [Precio] decimal(18,0)  NOT NULL,
    [Cantidad] int  NOT NULL,
    [Categoria] int  NOT NULL,
    [Pedido_PedidoId] int  NOT NULL
);
GO

-- Creating table 'ProductosAlmacen'
CREATE TABLE [dbo].[ProductosAlmacen] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Nombre] nvarchar(max)  NOT NULL,
    [Descripcion] nvarchar(max)  NULL,
    [Foto] nvarchar(max)  NOT NULL,
    [Precio] decimal(18,0)  NOT NULL,
    [CantidadAlmacen] int  NOT NULL,
    [CantidadCarrito] int  NOT NULL,
    [Categoria] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [ClienteId] in table 'Cliente'
ALTER TABLE [dbo].[Cliente]
ADD CONSTRAINT [PK_Cliente]
    PRIMARY KEY CLUSTERED ([ClienteId] ASC);
GO

-- Creating primary key on [PedidoId] in table 'Pedido'
ALTER TABLE [dbo].[Pedido]
ADD CONSTRAINT [PK_Pedido]
    PRIMARY KEY CLUSTERED ([PedidoId] ASC);
GO

-- Creating primary key on [Id] in table 'ProductosVendidos'
ALTER TABLE [dbo].[ProductosVendidos]
ADD CONSTRAINT [PK_ProductosVendidos]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ProductosAlmacen'
ALTER TABLE [dbo].[ProductosAlmacen]
ADD CONSTRAINT [PK_ProductosAlmacen]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Cliente_ClienteId] in table 'Pedido'
ALTER TABLE [dbo].[Pedido]
ADD CONSTRAINT [FK_ClientePedido]
    FOREIGN KEY ([Cliente_ClienteId])
    REFERENCES [dbo].[Cliente]
        ([ClienteId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ClientePedido'
CREATE INDEX [IX_FK_ClientePedido]
ON [dbo].[Pedido]
    ([Cliente_ClienteId]);
GO

-- Creating foreign key on [Pedido_PedidoId] in table 'ProductosVendidos'
ALTER TABLE [dbo].[ProductosVendidos]
ADD CONSTRAINT [FK_PedidoProductoVendido]
    FOREIGN KEY ([Pedido_PedidoId])
    REFERENCES [dbo].[Pedido]
        ([PedidoId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PedidoProductoVendido'
CREATE INDEX [IX_FK_PedidoProductoVendido]
ON [dbo].[ProductosVendidos]
    ([Pedido_PedidoId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------
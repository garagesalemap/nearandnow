CREATE TABLE [dbo].[Sale] (
    [Id]          INT            NOT NULL IDENTITY,
    [Title]       NVARCHAR (50)  NOT NULL,
    [Description] NVARCHAR (150) NULL,
    [Address]     NVARCHAR (150) NOT NULL,
    [LatLong]     NVARCHAR (50)  NOT NULL,
    [SaleDate]    DATETIME       NOT NULL,
    [LinkUrl]     NVARCHAR (250) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


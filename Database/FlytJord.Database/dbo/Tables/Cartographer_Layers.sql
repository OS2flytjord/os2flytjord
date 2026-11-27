CREATE TABLE [dbo].[Cartographer_Layers] (
    [TableOwner]     VARCHAR (300) NOT NULL,
    [TableName]      VARCHAR (300) NOT NULL,
    [KeyColumn]      VARCHAR (300) NOT NULL,
    [GeometryColumn] VARCHAR (300) NOT NULL,
    [GeometryIndex]  VARCHAR (300) NULL,
    [StyleType]      VARCHAR (50)  NOT NULL,
    [StyleValue]     VARCHAR (MAX) NULL,
    [Epsg]           INT           NOT NULL,
    [MinX]           FLOAT (53)    NOT NULL,
    [MinY]           FLOAT (53)    NOT NULL,
    [MaxX]           FLOAT (53)    NOT NULL,
    [MaxY]           FLOAT (53)    NOT NULL
);


GO


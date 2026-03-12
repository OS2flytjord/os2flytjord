--Spatial index og metadata

--KortInfo Chartographer_Layers
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO
Drop  TABLE [dbo].[Cartographer_Layers]
go

CREATE TABLE [dbo].[Cartographer_Layers](
	[TableOwner] [varchar](300) NOT NULL,
	[TableName] [varchar](300) NOT NULL,
	[KeyColumn] [varchar](300) NOT NULL,
	[GeometryColumn] [varchar](300) NOT NULL,
	[GeometryIndex] [varchar](300) NULL,
	[StyleType] [varchar](50) NOT NULL,
	[StyleValue] [varchar](max) NULL,
	[Epsg] [int] NOT NULL,
	[MinX] [float] NOT NULL,
	[MinY] [float] NOT NULL,
	[MaxX] [float] NOT NULL,
	[MaxY] [float] NOT NULL
) ON [PRIMARY]
GO


delete from [Cartographer_Layers];
INSERT INTO [Cartographer_Layers]
([TableOwner],[TableName],[KeyColumn],[GeometryColumn],[GeometryIndex],[StyleType],[StyleValue],[Epsg],[MinX],[MinY],[MaxX],[MaxY]) 
VALUES
('dbo','Oprindelsessted','Id','Geom','','MapInfo_PrLayer','',25832,404829,6037940,946282,6409740)
GO

INSERT INTO [Cartographer_Layers]
([TableOwner],[TableName],[KeyColumn],[GeometryColumn],[GeometryIndex],[StyleType],[StyleValue],[Epsg],[MinX],[MinY],[MaxX],[MaxY]) 
VALUES
('dbo','Matrikel','Id','Geom','','MapInfo_PrLayer','',25832,404829,6037940,946282,6409740)
GO

INSERT INTO [Cartographer_Layers]
([TableOwner],[TableName],[KeyColumn],[GeometryColumn],[GeometryIndex],[StyleType],[StyleValue],[Epsg],[MinX],[MinY],[MaxX],[MaxY]) 
VALUES
('dbo','Modtageranlaeg','Id','Geom','','MapInfo_PrLayer','',25832,404829,6037940,946282,6409740)
GO

INSERT INTO [Cartographer_Layers]
([TableOwner],[TableName],[KeyColumn],[GeometryColumn],[GeometryIndex],[StyleType],[StyleValue],[Epsg],[MinX],[MinY],[MaxX],[MaxY]) 
VALUES
('dbo','MapModtagerAnlaeg','Eid','Geom','','MapInfo_PrLayer','',25832,404829,6037940,946282,6409740)
GO



SET ANSI_PADDING ON
GO
--Spatial Indexes
CREATE SPATIAL INDEX [SPIDX_Oprindelsessted_Geom] ON Oprindelsessted
(
	[Geom]
)USING  GEOMETRY_GRID 
WITH (
BOUNDING_BOX =(424358, 6033856, 796336, 6418559), GRIDS =(LEVEL_1 = MEDIUM,LEVEL_2 = MEDIUM,LEVEL_3 = MEDIUM,LEVEL_4 = MEDIUM), 
CELLS_PER_OBJECT = 16, PAD_INDEX  = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

CREATE SPATIAL INDEX [SPIDX_Matrikel] ON Matrikel
(
	[Geom]
)USING  GEOMETRY_GRID 
WITH (
BOUNDING_BOX =(424358, 6033856, 796336, 6418559), GRIDS =(LEVEL_1 = MEDIUM,LEVEL_2 = MEDIUM,LEVEL_3 = MEDIUM,LEVEL_4 = MEDIUM), 
CELLS_PER_OBJECT = 16, PAD_INDEX  = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO


CREATE SPATIAL INDEX [SPIDX_Modtageranlaeg_Geom] ON Modtageranlaeg
(
	[Geom]
)USING  GEOMETRY_GRID 
WITH (
BOUNDING_BOX =(424358, 6033856, 796336, 6418559), GRIDS =(LEVEL_1 = MEDIUM,LEVEL_2 = MEDIUM,LEVEL_3 = MEDIUM,LEVEL_4 = MEDIUM), 
CELLS_PER_OBJECT = 16, PAD_INDEX  = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO






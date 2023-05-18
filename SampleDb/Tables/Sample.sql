CREATE TABLE [dbo].[Sample]
(
	[Id1] INT NOT NULL ,
	[Id2] INT NOT NULL ,
	[Id3] INT NOT NULL ,
	Name VARCHAR(50),
	UpperName AS UPPER(Name),
	PRIMARY KEY(Id1, Id2, Id3)
)

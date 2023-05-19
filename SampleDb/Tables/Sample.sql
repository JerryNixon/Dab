CREATE TABLE [dbo].[Sample]
(
		[Id1] INT NOT NULL ,
		[Id2] INT NOT NULL ,
		[Id3] INT NOT NULL ,
		Name VARCHAR(50),
		UpperName AS UPPER(Name),
		[Weird Name] NCHAR(10) NULL, 
		[Super weird Name-here] NCHAR(10) NULL, 
		[Very % strange-like # name @ ] NCHAR(10) NULL, 
		lowercase INT,
		PRIMARY KEY(Id1, Id2, Id3)
)

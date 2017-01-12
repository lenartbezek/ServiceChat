CREATE TABLE [dbo].[Uporabnik] (
    [username] VARCHAR (50)  NOT NULL,
    [ime]      NVARCHAR (50) NULL,
    [priimek]  NVARCHAR (50) NOT NULL,
    [geslo]    VARCHAR (250) NOT NULL,
	  [admin]    BIT  NOT NULL DEFAULT 0,
    PRIMARY KEY CLUSTERED ([username] ASC)
    
CREATE TABLE [dbo].[Pogovor] (
    [id]       INT          IDENTITY (1, 1) NOT NULL,
    [username] VARCHAR (50) NOT NULL,
    [besedilo] TEXT         NULL,
    [time]     DATETIME     NOT NULL
    PRIMARY KEY CLUSTERED ([id] ASC)

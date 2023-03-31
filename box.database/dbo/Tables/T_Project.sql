CREATE TABLE [dbo].[T_Project] (
    [id]   INT            IDENTITY (1, 1) NOT NULL,
    [name] NVARCHAR (255) NOT NULL,
    [code] NVARCHAR (255) NOT NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [CK_Upper_Code] CHECK (upper([code])=([code]) collate Latin1_General_BIN2),
    CONSTRAINT [AK_Code] UNIQUE NONCLUSTERED ([code] ASC)
);


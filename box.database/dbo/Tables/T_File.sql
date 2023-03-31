CREATE TABLE [dbo].[T_File] (
    [id]          INT            IDENTITY (1, 1) NOT NULL,
    [filename]    NVARCHAR (255) NOT NULL,
    [project_id]  INT            NOT NULL,
    [is_active]   TINYINT        DEFAULT ((1)) NOT NULL,
    [create_time] DATETIME2 (7)  NULL,
    [update_time] DATETIME2 (7)  NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [fk_T_File_T_Project1] FOREIGN KEY ([project_id]) REFERENCES [dbo].[T_Project] ([id]),
    CONSTRAINT [AK_Filename] UNIQUE NONCLUSTERED ([filename] ASC)
);


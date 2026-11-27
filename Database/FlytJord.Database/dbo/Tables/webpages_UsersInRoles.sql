CREATE TABLE [dbo].[webpages_UsersInRoles] (
    [UserId] INT NOT NULL,
    [RoleId] INT NOT NULL,
    CONSTRAINT [PK__webpages__AF2760AD703EA55A] PRIMARY KEY CLUSTERED ([UserId] ASC, [RoleId] ASC),
    CONSTRAINT [fk_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[webpages_Roles] ([RoleId]),
    CONSTRAINT [fk_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[BrugerProfil] ([BrugerId])
);


GO
ALTER TABLE [dbo].[webpages_UsersInRoles] NOCHECK CONSTRAINT [fk_RoleId];


GO
ALTER TABLE [dbo].[webpages_UsersInRoles] NOCHECK CONSTRAINT [fk_UserId];


GO

CREATE NONCLUSTERED INDEX [IDX_webpages_UsersInRoles_1_FK]
    ON [dbo].[webpages_UsersInRoles]([UserId] ASC);


GO

CREATE NONCLUSTERED INDEX [IDX_webpages_UsersInRoles_2_FK]
    ON [dbo].[webpages_UsersInRoles]([RoleId] ASC);


GO


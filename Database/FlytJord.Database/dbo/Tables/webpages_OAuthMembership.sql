CREATE TABLE [dbo].[webpages_OAuthMembership] (
    [Provider]       NVARCHAR (30)  NOT NULL,
    [ProviderUserId] NVARCHAR (100) NOT NULL,
    [UserId]         INT            NOT NULL,
    CONSTRAINT [PK__webpages__F53FC0ED61F08603] PRIMARY KEY CLUSTERED ([Provider] ASC, [ProviderUserId] ASC)
);


GO


/****** Object:  Table [dbo].[webpages_Roles]    Script Date: 01/30/2013 21:20:51 ******/

INSERT INTO webpages_roles VALUES('MasterAdmin')
INSERT INTO webpages_roles VALUES('KommuneAdmin')
INSERT INTO webpages_roles VALUES('JordmodtagerAdmin')
INSERT INTO webpages_roles VALUES('Sagsbehandler')
INSERT INTO webpages_roles VALUES('Bogholder')
INSERT INTO webpages_roles VALUES('Miljømedarbejder')
INSERT INTO webpages_roles VALUES('Pladsmand')
INSERT INTO webpages_roles VALUES('Prøvetager')
INSERT INTO webpages_roles VALUES('Laboratorie')

/****** Object:  Table [dbo].[webpages_OAuthMembership]    Script Date: 01/30/2013 21:20:51 ******/
/****** Object:  Table [dbo].[webpages_Membership]    Script Date: 01/30/2013 21:20:51 ******/
INSERT [dbo].[webpages_Membership] ([UserId], [CreateDate], [ConfirmationToken], [IsConfirmed], [LastPasswordFailureDate], [PasswordFailuresSinceLastSuccess], [Password], [PasswordChangedDate], [PasswordSalt], [PasswordVerificationToken], [PasswordVerificationTokenExpirationDate]) VALUES (1, CAST(0x0000A15600E39F34 AS DateTime), NULL, 1, NULL, 0, N'ACj+NI8kfr/5ydcnoDD3pcY+75xJQ+2nf3QKRdrvL3b7qIQO0U020R0i8TrWXEQ+PA==', CAST(0x0000A15600E39F34 AS DateTime), N'', NULL, NULL)
/****** Object:  Table [dbo].[BrugerProfil]    Script Date: 01/30/2013 21:20:51 ******/
SET IDENTITY_INSERT [dbo].[BrugerProfil] ON
INSERT [dbo].[BrugerProfil] ([BrugerId], [BrugerNavn]) VALUES (1, N'admin@jordflytning.dk')
SET IDENTITY_INSERT [dbo].[BrugerProfil] OFF
/****** Object:  Table [dbo].[webpages_UsersInRoles]    Script Date: 01/30/2013 21:20:51 ******/
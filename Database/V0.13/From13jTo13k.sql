EXEC sp_rename 'Anmeldelse.BemaerkningTilAnmeldelse', 'AarsagAfvisning', 'COLUMN'
GO

EXEC sp_rename 'Anmeldelse.BemaerkningPaaAnmeldelse', 'BemaerkningTilAnmeldelse', 'COLUMN'
GO
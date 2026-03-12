CREATE TABLE NLogEntries(
          id int primary key not null identity(1,1),
          TimeStamp datetime2,
          Message nvarchar(max),
          level nvarchar(10),
          logger nvarchar(128))
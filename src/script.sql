BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250903162825_AddTestUnitAndCardAnswer'
)
BEGIN
    CREATE TABLE [TestCardAnswer] (
        [Id] uniqueidentifier NOT NULL,
        [TestCardId] uniqueidentifier NOT NULL,
        [UserId] int NOT NULL,
        CONSTRAINT [PK_TestCardAnswer] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_TestCardAnswer_TestCards_TestCardId] FOREIGN KEY ([TestCardId]) REFERENCES [TestCards] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_TestCardAnswer_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250903162825_AddTestUnitAndCardAnswer'
)
BEGIN
    CREATE TABLE [TestUnitAnswers] (
        [Id] uniqueidentifier NOT NULL,
        [Answer] nvarchar(max) NOT NULL,
        [TestUnitId] uniqueidentifier NOT NULL,
        [TestCardAnswerId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_TestUnitAnswers] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_TestUnitAnswers_TestCardAnswer_TestCardAnswerId] FOREIGN KEY ([TestCardAnswerId]) REFERENCES [TestCardAnswer] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_TestUnitAnswers_TestUnits_TestUnitId] FOREIGN KEY ([TestUnitId]) REFERENCES [TestUnits] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250903162825_AddTestUnitAndCardAnswer'
)
BEGIN
    CREATE INDEX [IX_TestCardAnswer_TestCardId] ON [TestCardAnswer] ([TestCardId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250903162825_AddTestUnitAndCardAnswer'
)
BEGIN
    CREATE INDEX [IX_TestUnitAnswers_TestCardAnswerId] ON [TestUnitAnswers] ([TestCardAnswerId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250903162825_AddTestUnitAndCardAnswer'
)
BEGIN
    CREATE INDEX [IX_TestUnitAnswers_TestUnitId] ON [TestUnitAnswers] ([TestUnitId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250903162825_AddTestUnitAndCardAnswer'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250903162825_AddTestUnitAndCardAnswer', N'8.0.16');
END;
GO

COMMIT;
GO


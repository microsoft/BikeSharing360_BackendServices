CREATE EXTERNAL DATA SOURCE [EventsReferenceData]
    WITH (
    TYPE = RDBMS,
    LOCATION = N'bikesshare360.database.windows.net',
    DATABASE_NAME = N'bikesharing-services-events',
    CREDENTIAL = [SQL_Credential]
    );


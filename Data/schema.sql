-- SQL script to initialise the database schema used by this project.
-- This file is used by the check_database_integrity.py script to create
-- a SQLite database for integrity testing.  It does not include the full
-- ASP.NET Identity schema but does contain tables for the core entities
-- defined in the Models folder.

CREATE TABLE IF NOT EXISTS Pages (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL,
    Description TEXT
);

CREATE TABLE IF NOT EXISTS Posts (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Title TEXT NOT NULL,
    Content TEXT NOT NULL,
    ImageUrl TEXT,
    PageId INTEGER NOT NULL,
    CreatedAt TEXT NOT NULL,
    FOREIGN KEY(PageId) REFERENCES Pages(Id)
);

CREATE TABLE IF NOT EXISTS ScheduledPosts (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Title TEXT NOT NULL,
    Content TEXT NOT NULL,
    ImageUrl TEXT,
    ScheduledFor TEXT NOT NULL,
    IsPublished INTEGER NOT NULL DEFAULT 0,
    PageId INTEGER NOT NULL,
    FOREIGN KEY(PageId) REFERENCES Pages(Id)
);

CREATE TABLE IF NOT EXISTS AnalyticsData (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    PostId INTEGER,
    PageId INTEGER,
    Reach INTEGER NOT NULL,
    Reactions INTEGER NOT NULL,
    Comments INTEGER NOT NULL,
    Shares INTEGER NOT NULL,
    RecordedAt TEXT NOT NULL,
    FOREIGN KEY(PostId) REFERENCES Posts(Id),
    FOREIGN KEY(PageId) REFERENCES Pages(Id)
);
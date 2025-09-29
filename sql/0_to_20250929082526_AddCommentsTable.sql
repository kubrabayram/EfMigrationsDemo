CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" TEXT NOT NULL CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY,
    "ProductVersion" TEXT NOT NULL
);

BEGIN TRANSACTION;
CREATE TABLE "Blogs" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Blogs" PRIMARY KEY AUTOINCREMENT,
    "Name" TEXT NOT NULL
);

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250927052955_InitialCreate', '9.0.0');

ALTER TABLE "Blogs" ADD "CreatedTimestamp" TEXT NULL;

UPDATE Blogs SET CreatedTimestamp = CURRENT_TIMESTAMP WHERE CreatedTimestamp IS NULL;

CREATE TABLE "ef_temp_Blogs" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Blogs" PRIMARY KEY AUTOINCREMENT,
    "CreatedTimestamp" TEXT NOT NULL,
    "Name" TEXT NOT NULL
);

INSERT INTO "ef_temp_Blogs" ("Id", "CreatedTimestamp", "Name")
SELECT "Id", "CreatedTimestamp", "Name"
FROM "Blogs";

COMMIT;

PRAGMA foreign_keys = 0;

BEGIN TRANSACTION;
DROP TABLE "Blogs";

ALTER TABLE "ef_temp_Blogs" RENAME TO "Blogs";

COMMIT;

PRAGMA foreign_keys = 1;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250927053050_AddBlogCreatedTimestamp', '9.0.0');

BEGIN TRANSACTION;
CREATE TABLE "Posts" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Posts" PRIMARY KEY AUTOINCREMENT,
    "Title" TEXT NOT NULL,
    "Content" TEXT NOT NULL,
    "CreatedAt" TEXT NOT NULL DEFAULT (CURRENT_TIMESTAMP),
    "BlogId" INTEGER NOT NULL,
    CONSTRAINT "FK_Posts_Blogs_BlogId" FOREIGN KEY ("BlogId") REFERENCES "Blogs" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_Posts_BlogId" ON "Posts" ("BlogId");

CREATE TABLE "ef_temp_Blogs" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Blogs" PRIMARY KEY AUTOINCREMENT,
    "CreatedTimestamp" TEXT NOT NULL DEFAULT (CURRENT_TIMESTAMP),
    "Name" TEXT NOT NULL
);

INSERT INTO "ef_temp_Blogs" ("Id", "CreatedTimestamp", "Name")
SELECT "Id", "CreatedTimestamp", "Name"
FROM "Blogs";

COMMIT;

PRAGMA foreign_keys = 0;

BEGIN TRANSACTION;
DROP TABLE "Blogs";

ALTER TABLE "ef_temp_Blogs" RENAME TO "Blogs";

COMMIT;

PRAGMA foreign_keys = 1;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250927055025_AddPostTable', '9.0.0');

BEGIN TRANSACTION;
CREATE TABLE "Comments" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Comments" PRIMARY KEY AUTOINCREMENT,
    "PostId" INTEGER NOT NULL,
    "Content" TEXT NOT NULL,
    "CreatedAt" TEXT NOT NULL DEFAULT (CURRENT_TIMESTAMP),
    CONSTRAINT "FK_Comments_Posts_PostId" FOREIGN KEY ("PostId") REFERENCES "Posts" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_Comments_PostId" ON "Comments" ("PostId");

CREATE TABLE "ef_temp_Posts" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Posts" PRIMARY KEY AUTOINCREMENT,
    "BlogId" INTEGER NOT NULL,
    "Content" TEXT NOT NULL,
    "CreatedAt" TEXT NOT NULL DEFAULT (CURRENT_TIMESTAMP),
    "Title" TEXT NOT NULL,
    CONSTRAINT "FK_Posts_Blogs_BlogId" FOREIGN KEY ("BlogId") REFERENCES "Blogs" ("Id") ON DELETE CASCADE
);

INSERT INTO "ef_temp_Posts" ("Id", "BlogId", "Content", "CreatedAt", "Title")
SELECT "Id", "BlogId", "Content", "CreatedAt", "Title"
FROM "Posts";

COMMIT;

PRAGMA foreign_keys = 0;

BEGIN TRANSACTION;
DROP TABLE "Posts";

ALTER TABLE "ef_temp_Posts" RENAME TO "Posts";

COMMIT;

PRAGMA foreign_keys = 1;

BEGIN TRANSACTION;
CREATE INDEX "IX_Posts_BlogId" ON "Posts" ("BlogId");

COMMIT;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250929082526_AddCommentsTable', '9.0.0');


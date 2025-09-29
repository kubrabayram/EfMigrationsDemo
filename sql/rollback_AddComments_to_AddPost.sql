BEGIN TRANSACTION;
DROP TABLE "Comments";

CREATE TABLE "ef_temp_Posts" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Posts" PRIMARY KEY AUTOINCREMENT,
    "BlogId" INTEGER NOT NULL,
    "Content" TEXT NOT NULL,
    "CreatedAt" TEXT NOT NULL,
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

DELETE FROM "__EFMigrationsHistory"
WHERE "MigrationId" = '20250929082526_AddCommentsTable';


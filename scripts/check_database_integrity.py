#!/usr/bin/env python3
"""
Create a SQLite database based on the provided schema.sql and perform an
integrity check.  This script verifies that the tables defined in
schema.sql exist and that SQLite reports the database as healthy.  The
script can be extended to insert sample data and perform more
comprehensive consistency checks.
"""
import os
import sqlite3
import sys

ROOT = os.path.abspath(os.path.join(os.path.dirname(__file__), '..'))
SCHEMA_PATH = os.path.join(ROOT, 'Data', 'schema.sql')
TEST_DB = os.path.join(ROOT, 'Data', 'test_facebook_manager.db')

REQUIRED_TABLES = {"Pages", "Posts", "ScheduledPosts", "AnalyticsData"}

def main() -> int:
    # Ensure any existing test database is removed
    if os.path.exists(TEST_DB):
        os.remove(TEST_DB)
    # Create a new database and apply the schema
    conn = sqlite3.connect(TEST_DB)
    try:
        with open(SCHEMA_PATH, 'r', encoding='utf-8') as f:
            conn.executescript(f.read())
        # Verify required tables exist
        cur = conn.cursor()
        cur.execute("SELECT name FROM sqlite_master WHERE type='table';")
        tables = {row[0] for row in cur.fetchall()}
        missing = REQUIRED_TABLES - tables
        if missing:
            print(f"Missing tables: {', '.join(missing)}")
            return 1
        # Run integrity check
        cur.execute("PRAGMA integrity_check;")
        result = cur.fetchone()[0]
        if result != 'ok':
            print(f"Integrity check failed: {result}")
            return 1
        print("Database integrity check passed and required tables are present.")
        return 0
    finally:
        conn.close()

if __name__ == '__main__':
    sys.exit(main())
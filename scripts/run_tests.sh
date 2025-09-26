#!/usr/bin/env bash
set -e
# Master script to run all project checks.  Executes basic syntax
# verification, database integrity check and a simple server startup test.

SCRIPT_DIR=$(dirname "$(realpath "$0")")
PROJECT_ROOT="$SCRIPT_DIR/.."

echo "Running syntax check..."
"$SCRIPT_DIR/run_syntax_check.py"

echo "Checking database integrity..."
"$SCRIPT_DIR/check_database_integrity.py"

echo "Starting static file server test..."
"$SCRIPT_DIR/run_server_test.sh"

echo "All tests passed."
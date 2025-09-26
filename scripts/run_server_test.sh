#!/usr/bin/env bash
set -e
# This script starts a simple static file server using Python's built in
# http.server module to simulate launching a local development server.  It
# then requests the site.css file to ensure static files are served
# correctly and finally shuts down the server.

ROOT_DIR=$(dirname "$(realpath "$0")")/..
WWWROOT="$ROOT_DIR/wwwroot"

PORT=8000

# Start the server in the background
python3 -m http.server "$PORT" --directory "$WWWROOT" &
SERVER_PID=$!

# Give the server a moment to start
sleep 1

# Perform a simple HTTP request to the CSS file
HTTP_STATUS=$(curl -o /dev/null -s -w "%{http_code}" "http://localhost:$PORT/css/site.css" || true)

# Shut down the server
kill $SERVER_PID

if [ "$HTTP_STATUS" = "200" ]; then
  echo "Server responded with status $HTTP_STATUS. Static file serving looks correct."
  exit 0
else
  echo "Unexpected HTTP status $HTTP_STATUS when retrieving site.css"
  exit 1
fi
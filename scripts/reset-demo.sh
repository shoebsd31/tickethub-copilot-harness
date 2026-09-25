#!/usr/bin/env bash
# Puts the repo back to the exact demo starting point. Safe to run between rehearsals.
# First stop the API (Ctrl+C) and the tickethub-db MCP server, or Windows may lock data/tickets.db.
set -euo pipefail
cd "$(dirname "$0")/.."

git checkout --force main        # leave any fallback branch
git reset --hard demo-start      # code, instruction files, and data/tickets.db
git clean -fd                    # new files the agent created (keeps bin/ and obj/ for a fast build)
dotnet build --nologo -v quiet   # warm build so the first `dotnet test` on stage is fast

echo "Reset complete. Now: close all chat sessions, then run 'MCP: List Servers' and restart tickethub-db."

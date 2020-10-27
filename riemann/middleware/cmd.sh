#!/bin/sh

set -m

(cd /app ; dotnet /app/ArqNetCore.dll) &

node /middleware/index.js


fg %1
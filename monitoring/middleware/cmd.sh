#!/bin/sh

set -m

RIEMANN_PORT_OR_DEFAULT=${RIEMANN_PORT:-5555}
APPLICATION_OR_DEFAULT=${APPLICATION_NAME:-UNKNOW}

echo "using rieman port: $RIEMANN_PORT_OR_DEFAULT"
echo "using rieman host: $RIEMANN_HOST"
echo "using application name: $APPLICATION_OR_DEFAULT"

(cd /app ; dotnet /app/ArqNetCore.dll) &

node /middleware/index.js  &

riemann-health -h $RIEMANN_HOST -p $RIEMANN_PORT_OR_DEFAULT -a application=$APPLICATION_OR_DEFAULT

riemann-net -h $RIEMANN_HOST -p $RIEMANN_PORT_OR_DEFAULT -a application=$APPLICATION_OR_DEFAULT


fg %1
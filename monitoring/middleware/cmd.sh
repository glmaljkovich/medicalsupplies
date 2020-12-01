#!/bin/sh

set -m

RIEMANN_PORT_OR_DEFAULT=${RIEMANN_PORT:-5555}
APPLICATION_NAME_OR_DEFAULT=${RIEMANN_APPLICATION_NAME:-UNKNOW}
if [[ -z "${RIEMANN_ATTRIBUTES}" ]]; then
  ATTRIBUTES_OR_DEFAULT="hostname=${HOSTNAME}"
else
  ATTRIBUTES_OR_DEFAULT="hostname=${HOSTNAME},${RIEMANN_ATTRIBUTES}"
fi

echo "using riemann port: $RIEMANN_PORT_OR_DEFAULT"
echo "using riemann host: $RIEMANN_HOST"
echo "using middleware logs enabled: $MIDDLEWARE_LOGS"
echo "using application name: $APPLICATION_NAME_OR_DEFAULT"

(cd /app ; dotnet /app/ArqNetCore.dll) &

node /middleware/index.js &

riemann-health -h $RIEMANN_HOST -p $RIEMANN_PORT_OR_DEFAULT -e $APPLICATION_NAME_OR_DEFAULT -a $ATTRIBUTES_OR_DEFAULT &

riemann-net -h $RIEMANN_HOST -p $RIEMANN_PORT_OR_DEFAULT -e $APPLICATION_NAME_OR_DEFAULT -a $ATTRIBUTES_OR_DEFAULT

fg %1
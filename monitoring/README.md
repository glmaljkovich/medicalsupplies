#### imagenes locales

docker run --rm -d \
 -p 5555:5555 \
 -p 5555:5555/udp \
 -p 5556:5556 \
 -e GRAPHITE_HOST=`hostname -i` \
 --name riemann riemann

docker run --rm -d \
 -p 4567:4567 \
 --name riemann-dashboard riemann-dashboard

#### imagenes de terceros

docker run --rm -d \
 -p 5555:5555 \
 -p 5555:5555/udp \
 -p 5556:5556 \
 --name riemann riemannio/riemann:0.3.6

docker run --rm -d \
 --name graphite \
 -p 9000:80 \
 -p 2003-2004:2003-2004 \
 -p 2023-2024:2023-2024 \
 -p 8125:8125/udp \
 -p 8126:8126 \
 graphiteapp/graphite-statsd:1.1.7-6

https://hub.docker.com/r/graphiteapp/graphite-statsd/
port 9000 web application
port 2003 data source
credentials root:root

docker run --rm -d \
 --name grafana \
 -p 3000:3000 \
 grafana/grafana:6.5.0

credentials admin:admin

grafana graphite config

URL: http://<host-ip>:<80 binding port>
Access: Server
BasicAutentication: true
WithCredentials: true
User: root
Password: root
Version: 1.1.7
Type: MetricTank

#currently it is necessary to get up Graphite storage to ensure Riemann collect metrics

https://gist.github.com/ilap/cb6d512694c3e4f2427f85e4caec8ad7

-1001418732022
1297526319:AAFRQ9kezUKQmSJiWCwQx9oBHVY6so_H8do
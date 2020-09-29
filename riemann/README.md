
docker run --rm -d -p 5555:5555 -p 5555:5555/udp -p 5556:5556 --name riemann riemannio/riemann:0.3.6

docker run --rm -d -p 4567:4567 --name riemann-dashboard riemann-dashboard
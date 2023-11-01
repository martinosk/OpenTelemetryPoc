docker-compose down
docker rm jaeger
docker rm zipkin
docker run -d --name jaeger -e COLLECTOR_OTLP_ENABLED=true -p 4317:4317 -p 16686:16686 jaegertracing/all-in-one:latest
docker run -d --name zipkin -p 9411:9411 openzipkin/zipkin
docker-compose up
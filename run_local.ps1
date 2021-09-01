docker rm jaeger
docker rm kafka
docker run -d --name jaeger -e COLLECTOR_ZIPKIN_HOST_PORT=:9411 -p 5775:5775/udp -p 6831:6831/udp -p 6832:6832/udp -p 5778:5778 -p 16686:16686 -p 14268:14268 -p 14250:14250 -p 9411:9411 jaegertracing/all-in-one:latest
docker run -d --name kafka -e ADVERTISED_PORT=:9092 -e ADVERTISED_HOST=:"172.17.194.113" -e AUTO_CREATE_TOPICS=:"true" -p 2181:2181 -p 9092:9092 spotify/kafka:latest
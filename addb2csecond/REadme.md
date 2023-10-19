docker run -it --rm -p 7054:7054  addb2csecond:latest
docker build -t addb2csecond .
dotnet dev-certs https --trust
dotnet dev-certs https
#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 7054

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
run dotnet dev-certs https --trust
WORKDIR /src
COPY ["addb2csecond.csproj", "."]
RUN dotnet restore "./addb2csecond.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "addb2csecond.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "addb2csecond.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
copy --from=publish /root/.dotnet/corefx/cryptography/x509stores/my/* /root/.dotnet/corefx/cryptography/x509stores/my/
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "addb2csecond.dll"]
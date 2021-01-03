#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
# https://medium.com/swlh/deploy-your-net-core-3-1-application-to-heroku-with-docker-eb8c96948d32
# EXPOSE 80
# EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["src/LinkedOut.Blazor/LinkedOut.Blazor.csproj", "src/LinkedOut.Blazor/"]
COPY ["src/LinkedOut.Application/LinkedOut.Application.csproj", "src/LinkedOut.Application/"]
COPY ["src/LinkedOut.Domain/LinkedOut.Domain.csproj", "src/LinkedOut.Domain/"]
COPY ["src/LinkedOut.Infrastructure/LinkedOut.Infrastructure.csproj", "src/LinkedOut.Infrastructure/"]
RUN dotnet restore "src/LinkedOut.Blazor/LinkedOut.Blazor.csproj"
COPY . .
WORKDIR "/src/src/LinkedOut.Blazor"
RUN dotnet build "LinkedOut.Blazor.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LinkedOut.Blazor.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LinkedOut.Blazor.dll"]
#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0-bullseye-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0-bullseye-slim AS build
WORKDIR /src
COPY ["src/Mall.WebApi/Mall.WebApi.csproj", "src/Mall.WebApi/"]
COPY ["src/Mall.Infrastructure/Mall.Infrastructure.csproj", "src/Mall.Infrastructure/"]
COPY ["src/Mall.Domain/Mall.Domain.csproj", "src/Mall.Domain/"]
COPY ["src/Mall.Domain.Shared/Mall.Domain.Shared.csproj", "src/Mall.Domain.Shared/"]
RUN dotnet restore "src/Mall.WebApi/Mall.WebApi.csproj"
COPY . .
WORKDIR "/src/src/Mall.WebApi"
RUN dotnet build "Mall.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Mall.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Mall.WebApi.dll"]

#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Mall.WebApi/Mall.WebApi.csproj", "src/Mall.WebApi/"]
COPY ["src/Mall.Infrastructure/Mall.Infrastructure.csproj", "src/Mall.Infrastructure/"]
COPY ["src/Mall.Domain/Mall.Domain.csproj", "src/Mall.Domain/"]
COPY ["src/Mall.Domain.Shared/Mall.Domain.Shared.csproj", "src/Mall.Domain.Shared/"]
RUN dotnet restore "src/Mall.WebApi/Mall.WebApi.csproj"
COPY . .
WORKDIR "/src/src/Mall.WebApi"
RUN dotnet build "Mall.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Mall.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Mall.WebApi.dll"]

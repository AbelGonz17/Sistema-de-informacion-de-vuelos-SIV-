FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SIV.Api/SIV.Presentation.csproj", "SIV.Api/"]
COPY ["SIV.Application/SIV.Application.csproj", "SIV.Application/"]
COPY ["SIV.Domain/SIV.Domain.csproj", "SIV.Domain/"]
COPY ["SIV.Infrastructure/SIV.Infrastructure.csproj", "SIV.Infrastructure/"]
COPY ["SIV.Persistence/SIV.Persistence.csproj", "SIV.Persistence/"]
RUN dotnet restore "./SIV.Api/SIV.Presentation.csproj"
COPY . .
WORKDIR "/src/SIV.Api"
RUN dotnet build "./SIV.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SIV.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SIV.Presentation.dll"]

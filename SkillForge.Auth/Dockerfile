FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 7222

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["SkillForge.Auth/", "SkillForge.Auth/"]
COPY ["SkillForge.Data/", "SkillForge.Data/"]

RUN ["dotnet", "restore", "SkillForge.Auth/SkillForge.Auth.csproj"]

RUN ["dotnet", "publish", "SkillForge.Auth/SkillForge.Auth.csproj", "-c", "Release", "-o", "/app/publish"]

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
COPY ["common.pfx", "./https/common.pfx"]
COPY ["SkillForge.Data/appsettings.Jwt.json", "./appsettings.Jwt.json"]

ENTRYPOINT ["dotnet", "SkillForge.Auth.dll"] 
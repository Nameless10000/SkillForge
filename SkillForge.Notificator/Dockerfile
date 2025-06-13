FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 7029

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["SkillForge.Notificator/", "SkillForge.Notificator/"]
COPY ["SkillForge.Data/", "SkillForge.Data/"]

RUN ["dotnet", "restore", "SkillForge.Notificator/SkillForge.Notificator.csproj"]

RUN ["dotnet", "publish", "SkillForge.Notificator/SkillForge.Notificator.csproj", "-c", "Release", "-o", "/app/publish"]

FROM base AS final

COPY --from=build /app/publish .

COPY ["common.pfx", "./https/common.pfx"]
COPY ["SkillForge.Data/*.json", "./"]

# Запускаем с указанием сертификата
ENTRYPOINT ["dotnet", "SkillForge.Notificator.dll"]
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app

EXPOSE 7090

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["SkillForge.Api/", "SkillForge.Api/"]
COPY ["SkillForge.Data/", "SkillForge.Data/"]

RUN ["dotnet", "restore", "SkillForge.Api/SkillForge.Api.csproj"]
RUN ["dotnet", "publish", "SkillForge.Api/SkillForge.Api.csproj", "-c", "Release", "-o", "/app/publish"]

FROM base AS final

COPY --from=build /app/publish .

COPY ["common.pfx", "./https/common.pfx"]
COPY ["SkillForge.Data/*.json", "./"]

ENTRYPOINT ["dotnet", "SkillForge.Api.dll"]

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app

EXPOSE 7134

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["SkillForge.Talks/", "SkillForge.Talks/"]
COPY ["SkillForge.Data/", "SkillForge.Data/"]

RUN ["dotnet", "restore", "SkillForge.Talks/SkillForge.Talks.csproj"]
RUN ["dotnet", "publish", "SkillForge.Talks/SkillForge.Talks.csproj", "-c", "Release", "-o", "/app/publish"]

FROM base AS final

COPY --from=build /app/publish .

COPY ["common.pfx", "./https/common.pfx"]
COPY ["SkillForge.Data/*.json", "./"]

ENTRYPOINT ["dotnet", "SkillForge.Talks.dll"]
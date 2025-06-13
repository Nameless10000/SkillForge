FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app

EXPOSE 7211

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["SkillForge.Products/", "SkillForge.Products/"]
COPY ["SkillForge.Data/", "SkillForge.Data/"]

RUN ["dotnet", "restore", "SkillForge.Products/SkillForge.Products.csproj"]

RUN ["dotnet", "publish", "SkillForge.Products/SkillForge.Products.csproj", "-c", "Release", "-o", "/app/publish"]

FROM base AS final
COPY --from=build /app/publish .
COPY ["common.pfx", "./https/common.pfx"]
COPY ["SkillForge.Data/*.json", "./"]

ENTRYPOINT ["dotnet", "SkillForge.Products.dll"]
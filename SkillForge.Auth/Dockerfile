FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app

# Копируем сертификат внутрь контейнера
COPY ../aspnetapp.pfx /https/aspnetapp.pfx

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY . .
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
# Открываем порт 443 для https
EXPOSE 443
# Запускаем с указанием сертификата
ENTRYPOINT ["dotnet", "SkillForge.Auth.dll"]
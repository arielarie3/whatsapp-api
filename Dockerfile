# שלב 1: בנייה וקימפול
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["WhatsappWeb.Api.csproj", "./"]
RUN dotnet restore "WhatsappWeb.Api.csproj"

COPY . .
RUN dotnet publish "WhatsappWeb.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# שלב 2: סביבת ריצה
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# הגדרות מניעת קריסת inotify
ENV DOTNET_HOSTBUILDER__RELOADCONFIGONCHANGE=false
ENV DOTNET_USE_POLLING_FILE_WATCHER=true

# הגדרת פורט ברירת מחדל
ENV PORT=5000
EXPOSE 5000

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "WhatsappWeb.Api.dll"]
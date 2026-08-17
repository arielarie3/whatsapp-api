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

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "WhatsappWeb.Api.dll"]
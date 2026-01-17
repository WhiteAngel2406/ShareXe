FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY ShareXe.csproj .
RUN dotnet restore
COPY . .
RUN dotnet build "ShareXe.csproj" -c Release -o /app/build
RUN dotnet publish "ShareXe.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS release
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000

ENTRYPOINT ["dotnet", "ShareXe.dll"]
FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY SignalrNotificationService/SignalrNotificationService.csproj SignalrNotificationService/
RUN dotnet restore SignalrNotificationService/SignalrNotificationService.csproj
COPY . .
WORKDIR /src/SignalrNotificationService
RUN dotnet build SignalrNotificationService.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish SignalrNotificationService.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "SignalrNotificationService.dll"]

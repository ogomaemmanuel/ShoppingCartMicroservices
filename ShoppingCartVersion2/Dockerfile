FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY *.sln ./
COPY ShoppingCartVersion2/ShoppingCartVersion2.csproj ShoppingCartVersion2/
RUN dotnet restore
COPY . .
WORKDIR /src/ShoppingCartVersion2
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ShoppingCartVersion2.dll"]

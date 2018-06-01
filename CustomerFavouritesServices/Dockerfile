FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY CustomerFavouritesServices/CustomerFavouritesServices.csproj CustomerFavouritesServices/
RUN dotnet restore CustomerFavouritesServices/CustomerFavouritesServices.csproj
COPY . .
WORKDIR /src/CustomerFavouritesServices
RUN dotnet build CustomerFavouritesServices.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish CustomerFavouritesServices.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "CustomerFavouritesServices.dll"]

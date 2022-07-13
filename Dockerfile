FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["HotelListing.API/HotelListing.API.csproj", "HotelListing.API/"]
RUN dotnet restore "HotelListing.API/HotelListing.API.csproj"
COPY . .
WORKDIR "/src/HotelListing.API"
RUN dotnet build "HotelListing.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HotelListing.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HotelListing.API.dll"]
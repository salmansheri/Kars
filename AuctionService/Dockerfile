FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /src 

EXPOSE 80

# Copy all .csproj files and restore as distinct layers. Use of te same COPY command 
# for all dockerfile in the project to take advantage of docker caching 


COPY Kars.sln Kars.sln

COPY AuctionService/AuctionService.csproj  /src/AuctionService/AuctionService.csproj
COPY SearchService/SearchService.csproj  /src/SearchService/SearchService.csproj
COPY IdentityService/IdentityService.csproj  /src/IdentityService/IdentityService.csproj
COPY GatewayService/GatewayService.csproj  /src/GatewayService/GatewayService.csproj
COPY Contracts/Contracts.csproj  /src/Contracts/Contracts.csproj

# Restore the solution
RUN dotnet restore Kars.sln

# Copy the app folders 
COPY AuctionService /src/AuctionService

COPY Contracts /src/Contracts

WORKDIR /src/AuctionService
RUN dotnet publish -c Release -o /app/out

# Build runtime image 
FROM mcr.microsoft.com/dotnet/aspnet:9.0

WORKDIR /app

COPY --from=build /app/out .

ENTRYPOINT [ "dotnet", "AuctionService.dll" ]

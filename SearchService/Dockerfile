FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /app

# Copy solution and project files
COPY Kars.sln ./
COPY ./SearchService/SearchService.csproj ./SearchService/
COPY ./Contracts/Contracts.csproj ./Contracts/

# Restore
RUN dotnet restore ./SearchService/SearchService.csproj

# Copy the rest of the source code
COPY . .

# Publish
RUN dotnet publish ./SearchService/SearchService.csproj -c Release -o /app/publish


# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime

WORKDIR /app
EXPOSE 80

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "SearchService.dll"]

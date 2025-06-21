FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

# Copy solution and project files
COPY Kars.sln ./
COPY ./IdentityService/IdentityService.csproj ./IdentityService/


# Restore
RUN dotnet restore ./IdentityService/IdentityService.csproj

# Copy the rest of the source code
COPY . .

# Publish
RUN dotnet publish ./IdentityService/IdentityService.csproj -c Release -o /app/publish


# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app
EXPOSE 80

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "IdentityService.dll"]

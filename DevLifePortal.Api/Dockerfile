FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy all project files maintaining directory structure
COPY DevLifePortal.Domain/DevLifePortal.Domain.csproj ./DevLifePortal.Domain/
COPY DevLifePortal.Application/DevLifePortal.Application.csproj ./DevLifePortal.Application/
COPY DevLifePortal.Infrastructure/DevLifePortal.Infrastructure.csproj ./DevLifePortal.Infrastructure/
COPY DevLifePortal.Api/DevLifePortal.Api.csproj ./DevLifePortal.Api/

# Restore dependencies for the main project (this will restore all referenced projects)
RUN dotnet restore ./DevLifePortal.Api/DevLifePortal.Api.csproj

# Copy all source code
COPY . .

# Publish the API project
RUN dotnet publish ./DevLifePortal.Api/DevLifePortal.Api.csproj -c Release -o /out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /out ./

EXPOSE 80
ENTRYPOINT ["dotnet", "DevLifePortal.Api.dll"]
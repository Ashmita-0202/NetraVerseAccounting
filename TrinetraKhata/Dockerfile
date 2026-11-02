#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TN.Web/TN.Web.csproj", "TN.Web/"]
COPY ["TN.Authentication.Application/TN.Authentication.Application.csproj", "TN.Authentication.Application/"]
COPY ["TN.Authentication.Domain/TN.Authentication.Domain.csproj", "TN.Authentication.Domain/"]
COPY ["TN.Shared.Domain/TN.Shared.Domain.csproj", "TN.Shared.Domain/"]
COPY ["TN.Authentication.Infrastructure/TN.Authentication.Infrastructure.csproj", "TN.Authentication.Infrastructure/"]
COPY ["TN.Shared.Infrastructure/TN.Shared.Infrastructure.csproj", "TN.Shared.Infrastructure/"]
COPY ["TN.Account.Domain/TN.Account.Domain.csproj", "TN.Account.Domain/"]
COPY ["TN.Inventory.Domain/TN.Inventory.Domain.csproj", "TN.Inventory.Domain/"]
RUN dotnet restore "./TN.Web/./TN.Web.csproj"
COPY . .
WORKDIR "/src/TN.Web"
RUN dotnet build "./TN.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TN.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseApp=false


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TN.Web.dll"]
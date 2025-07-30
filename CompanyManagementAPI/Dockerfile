FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Debug
WORKDIR /src
COPY ["CompanyManagementAPI.csproj", "./"]
RUN dotnet restore "CompanyManagementAPI.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "CompanyManagementAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

COPY --from=app/build /src /app
FROM base AS final
WORKDIR /app
#COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "dotnet run"]



# Set the major version of dotnet
ARG DOTNET_VERSION=8.0

# Build the app using the dotnet SDK
FROM "mcr.microsoft.com/dotnet/sdk:${DOTNET_VERSION}-azurelinux3.0" AS build
WORKDIR /build
ARG CI
ENV CI=${CI}
COPY ./script/docker-entrypoint.sh /app/docker-entrypoint.sh

## START: Restore Packages
ARG PROJECT_NAME="Dfe.PersonsApi"
COPY ./${PROJECT_NAME}.sln ./
COPY ./src/${PROJECT_NAME}.Application/${PROJECT_NAME}.Application.csproj         ./src/${PROJECT_NAME}.Application/
COPY ./src/${PROJECT_NAME}.Client/${PROJECT_NAME}.Client.csproj                   ./src/${PROJECT_NAME}.Client/
COPY ./src/${PROJECT_NAME}.Client/readme.md                                       ./src/${PROJECT_NAME}.Client/
COPY ./src/${PROJECT_NAME}.Domain/${PROJECT_NAME}.Domain.csproj                   ./src/${PROJECT_NAME}.Domain/
COPY ./src/${PROJECT_NAME}.Infrastructure/${PROJECT_NAME}.Infrastructure.csproj   ./src/${PROJECT_NAME}.Infrastructure/
COPY ./src/${PROJECT_NAME}.Utils/${PROJECT_NAME}.Utils.csproj                     ./src/${PROJECT_NAME}.Utils/
COPY ./src/PersonsApi/PersonsApi.csproj                                           ./src/PersonsApi/

COPY ./src/Tests/${PROJECT_NAME}.Application.Tests/${PROJECT_NAME}.Application.Tests.csproj   ./src/Tests/${PROJECT_NAME}.Application.Tests/
COPY ./src/Tests/${PROJECT_NAME}.Domain.Tests/${PROJECT_NAME}.Domain.Tests.csproj             ./src/Tests/${PROJECT_NAME}.Domain.Tests/
COPY ./src/Tests/${PROJECT_NAME}.Tests.Common/${PROJECT_NAME}.Tests.Common.csproj             ./src/Tests/${PROJECT_NAME}.Tests.Common/
COPY ./src/Tests/${PROJECT_NAME}.Tests.Integration/${PROJECT_NAME}.Tests.Integration.csproj   ./src/Tests/${PROJECT_NAME}.Tests.Integration/

RUN --mount=type=secret,id=github_token dotnet nuget add source --username USERNAME --password $(cat /run/secrets/github_token) --store-password-in-clear-text --name github "https://nuget.pkg.github.com/DFE-Digital/index.json"
RUN ["dotnet", "restore", "Dfe.PersonsApi.sln"]
## END: Restore Packages

COPY ./src/ ./
RUN ["dotnet", "publish", "PersonsApi", "-o", "/app"]

# Build a runtime environment
FROM "mcr.microsoft.com/dotnet/aspnet:${DOTNET_VERSION}-azurelinux3.0" AS final
WORKDIR /app
LABEL org.opencontainers.image.source="https://github.com/DFE-Digital/persons-api"
LABEL org.opencontainers.image.description="Persons API"

COPY --from=build /app /app
RUN ["chmod", "+x", "./docker-entrypoint.sh"]
USER $APP_UID

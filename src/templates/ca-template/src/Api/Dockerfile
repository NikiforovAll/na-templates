#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

ARG Configuration="Release"

ARG CACHEBUST=1
RUN echo "CACHEBUST: $CACHEBUST"

ENV DOTNET_CLI_TELEMETRY_OPTOUT=true \
    DOTNET_SKIP_FIRST_TIME_EXPERIENCE=true

WORKDIR /app
COPY ["src/Directory.Build.props", "src/"]
COPY ["src/Api/Api.csproj", "src/Api/"]
COPY ["src/Application/Application.csproj", "src/Application/"]
COPY ["src/Application.SharedKernel/Application.SharedKernel.csproj", "src/Application.SharedKernel/"]
COPY ["src/Domain/Domain.csproj", "src/Domain/"]
COPY ["src/Domain.SharedKernel/Domain.SharedKernel.csproj", "src/Domain/Domain.SharedKernel/"]
COPY ["src/Infrastructure/Infrastructure.csproj", "src/Infrastructure/"]
RUN dotnet restore "src/Api/Api.csproj"
COPY . .
WORKDIR "/app/src/Api"
RUN dotnet build "Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Api.csproj" -c Release -o /app/publish

FROM build as unittest
WORKDIR /app/tests/

FROM build as integrationtest
WORKDIR /app/tests/

FROM base AS final

WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NikiforovAll.CA.Template.Api.dll"]

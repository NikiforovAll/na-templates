#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

ARG Configuration="Release"

ARG CACHEBUST=1
RUN echo "CACHEBUST: $CACHEBUST"

ENV DOTNET_CLI_TELEMETRY_OPTOUT=true \
    DOTNET_SKIP_FIRST_TIME_EXPERIENCE=true
WORKDIR /src

COPY ["src/Directory.Build.props", "src/"]
COPY ["src/Worker/Worker.csproj", "src/Worker/"]
COPY ["src/Application/Application.csproj", "src/Application/"]
COPY ["src/Application.SharedKernel/Application.SharedKernel.csproj", "src/Application.SharedKernel/"]
COPY ["src/Domain/Domain.csproj", "src/Domain/"]
COPY ["src/Domain.SharedKernel/Domain.SharedKernel.csproj", "src/Domain/Domain.SharedKernel/"]
COPY ["src/Infrastructure/Infrastructure.csproj", "src/Infrastructure/"]

RUN dotnet restore "src/Worker/Worker.csproj"

COPY . .

RUN dotnet build "src/Worker/Worker.csproj" --configuration $Configuration -o /app/build

FROM build AS publish

ARG Configuration="Release"

RUN dotnet publish "src/Worker/Worker.csproj" --configuration $Configuration --output /app/publish

FROM base AS final

WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NikiforovAll.CA.Template.Worker.dll"]

#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /src
COPY ["Directory.Build.props", "src/"]
COPY ["Directory.Packages.props", "src/"]
COPY ["src/Gateway/Gateway.csproj", "src/Gateway/"]

RUN dotnet restore "src/Gateway/Gateway.csproj"
COPY . .
WORKDIR "/src/src/Gateway"
RUN dotnet build "Gateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Gateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NikiforovAll.GA.Template.Gateway.dll"]

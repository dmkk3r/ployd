FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base

LABEL org.opencontainers.image.source=https://github.com/dmkk3r/ployd
USER $APP_UID

WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ployd/ployd.csproj", "ployd/"]
COPY ["Modules.Ui/Modules.Ui.csproj", "Modules.Ui/"]
COPY ["Modules/Module.Destination/Module.Destination.csproj", "Modules/Module.Destination/"]
COPY ["Modules/Module.Resource/Module.Resource.csproj", "Modules/Module.Resource/"]
COPY ["Modules.Shared/Modules.Shared.csproj", "Modules.Shared/"]
COPY ["Modules/Module.Source.Contract/Module.Source.Contract.csproj", "Modules/Module.Source.Contract/"]
COPY ["Modules/Module.Resource.Contract/Module.Resource.Contract.csproj", "Modules/Module.Resource.Contract/"]
COPY ["Modules/Module.Destination.Contract/Module.Destination.Contract.csproj", "Modules/Module.Destination.Contract/"]
COPY ["Modules/Module.ReverseProxy/Module.ReverseProxy.csproj", "Modules/Module.ReverseProxy/"]
COPY ["Modules/Module.Webhook/Module.Webhook.csproj", "Modules/Module.Webhook/"]
RUN dotnet restore "ployd/ployd.csproj"
COPY . .
WORKDIR "/src/ployd"
RUN dotnet build "ployd.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ployd.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ployd.dll"]

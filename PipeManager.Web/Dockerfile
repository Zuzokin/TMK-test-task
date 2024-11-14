FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["PipeManager.Web/PipeManager.Web.csproj", "PipeManager.Web/"]
COPY ["PipeManager.Application/PipeManager.Application.csproj", "PipeManager.Application/"]
COPY ["PipeManager.Core/PipeManager.Core.csproj", "PipeManager.Core/"]
COPY ["PipeManager.DataAccess/PipeManager.DataAccess.csproj", "PipeManager.DataAccess/"]
RUN dotnet restore "PipeManager.Web/PipeManager.Web.csproj"
COPY . .
WORKDIR "/src/PipeManager.Web"
RUN dotnet build "PipeManager.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "PipeManager.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PipeManager.Web.dll"]

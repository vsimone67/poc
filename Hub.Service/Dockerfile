FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Hub.Service.csproj", ""]
RUN dotnet restore "./Hub.Service.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "Hub.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Hub.Service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Hub.Service.dll"]
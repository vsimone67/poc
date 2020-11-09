FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Correspondence.Service.csproj", ""]
RUN dotnet restore "./Correspondence.Service.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "Correspondence.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Correspondence.Service.csproj" -c Kubernetes -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Correspondence.Service.dll"]
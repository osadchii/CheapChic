FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Source/CheapChic.Api/CheapChic.Api.csproj", "Source/CheapChic.Api/"]
RUN dotnet restore "Source/CheapChic.Api/CheapChic.Api.csproj"
COPY . .
WORKDIR "/src/Source/CheapChic.Api"
RUN dotnet build "CheapChic.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CheapChic.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CheapChic.Api.dll"]

#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["WEB/WEB.csproj", "WEB/"]
COPY ["API/API.csproj", "API/"]
COPY ["Common/Common.csproj", "Common/"]
COPY ["Data/Data.csproj", "Data/"]
RUN dotnet restore "WEB/WEB.csproj"
COPY . .
WORKDIR "/src/WEB"
RUN dotnet build "WEB.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WEB.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WEB.dll"]
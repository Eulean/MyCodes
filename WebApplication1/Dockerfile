# FROM mcr.microsoft.com/dotnet/aspnet:8.0-nanoserver-1809 AS base
# WORKDIR /app
# EXPOSE 5000

# ENV ASPNETCORE_URLS=http://+:5000

# FROM mcr.microsoft.com/dotnet/sdk:8.0-nanoserver-1809 AS build
# ARG configuration=Release
# WORKDIR /src
# COPY ["WebApplication1/WebApplication1.csproj", "WebApplication1/"]
# RUN dotnet restore "WebApplication1\WebApplication1.csproj"
# COPY . .
# WORKDIR "/src/WebApplication1"
# RUN dotnet build "WebApplication1.csproj" -c $configuration -o /app/build

# FROM build AS publish
# ARG configuration=Release
# RUN dotnet publish "WebApplication1.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

# FROM base AS final
# WORKDIR /app
# COPY --from=publish /app/publish .
# ENTRYPOINT ["dotnet", "WebApplication1.dll"]


FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5000

ENV ASPNETCORE_URLS=http://+:5000

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["WebApplication1/WebApplication1.csproj", "WebApplication1/"]
RUN dotnet restore "WebApplication1/WebApplication1.csproj"
COPY . .
WORKDIR "/src/WebApplication1"
RUN dotnet build "WebApplication1.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebApplication1.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApplication1.dll"]
#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["OLT.Sample.CoreLib.WebApi/OLT.Sample.CoreLib.WebApi.csproj", "OLT.Sample.CoreLib.WebApi/"]
RUN dotnet restore "OLT.Sample.CoreLib.WebApi/OLT.Sample.CoreLib.WebApi.csproj"
COPY . .
WORKDIR "/src/OLT.Sample.CoreLib.WebApi"
RUN dotnet build "OLT.Sample.CoreLib.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OLT.Sample.CoreLib.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OLT.Sample.CoreLib.WebApi.dll"]
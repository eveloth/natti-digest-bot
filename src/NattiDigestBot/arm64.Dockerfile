FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0.103-bullseye-slim-arm64v8 AS build
WORKDIR /src
COPY ["./NattiDigestBot.csproj", "."]
RUN dotnet restore "./NattiDigestBot.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "NattiDigestBot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NattiDigestBot.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NattiDigestBot.dll"]

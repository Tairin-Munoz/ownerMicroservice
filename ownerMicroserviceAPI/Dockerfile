FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["ownerMicroserviceAPI/ownerMicroserviceAPI.csproj", "ownerMicroserviceAPI/"]
COPY ["ownerMicroservice.Application/ownerMicroservice.Application.csproj", "ownerMicroservice.Application/"]
COPY ["ownerMicroservice.Domain/ownerMicroservice.Domain.csproj", "ownerMicroservice.Domain/"]
COPY ["ownerMicroservice.Infrastructure/ownerMicroservice.Infrastructure.csproj", "ownerMicroservice.Infrastructure/"]

RUN dotnet restore "ownerMicroserviceAPI/ownerMicroserviceAPI.csproj"

COPY . .

WORKDIR "/src/ownerMicroserviceAPI"
RUN dotnet build "ownerMicroserviceAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ownerMicroserviceAPI.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ownerMicroserviceAPI.dll"]

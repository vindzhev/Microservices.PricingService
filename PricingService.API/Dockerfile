#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["PricingService/PricingService.API/PricingService.API.csproj", "PricingService/PricingService.API/"]
COPY ["PricingService/PricingService.Infrastrucutre/PricingService.Infrastrucutre.csproj", "PricingService/PricingService.Infrastrucutre/"]
COPY ["PricingService/PricingService.Application/PricingService.Application.csproj", "PricingService/PricingService.Application/"]
COPY ["PricingService/PricingService.Domain/PricingService.Domain.csproj", "PricingService/PricingService.Domain/"]
RUN dotnet restore "PricingService/PricingService.API/PricingService.API.csproj"
COPY . .
WORKDIR "/src/PricingService/PricingService.API"
RUN dotnet build "PricingService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PricingService.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PricingService.API.dll"]
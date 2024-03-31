#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Services/Identity/Identity.API/Identity.API.csproj", "src/Services/Identity/Identity.API/"]
COPY ["src/SharedKernel/Core.AspNetCore/Core.AspNet.csproj", "src/SharedKernel/Core.AspNetCore/"]
COPY ["src/SharedKernel/Core/Core.csproj", "src/SharedKernel/Core/"]
COPY ["src/SharedKernel/Core.Autofac/Core.Autofac.csproj", "src/SharedKernel/Core.Autofac/"]
COPY ["src/SharedKernel/Core.Kafka/Core.Kafka.csproj", "src/SharedKernel/Core.Kafka/"]
COPY ["src/SharedKernel/Core.MediaR/Core.MediaR.csproj", "src/SharedKernel/Core.MediaR/"]
COPY ["src/Services/Identity/Identity.Application/Identity.Application.csproj", "src/Services/Identity/Identity.Application/"]
COPY ["src/SharedKernel/Core.Contract/Core.Contract.csproj", "src/SharedKernel/Core.Contract/"]
COPY ["src/Services/Identity/Identity.Domain/Identity.Domain.csproj", "src/Services/Identity/Identity.Domain/"]
COPY ["src/Services/Identity/Identity.Infrastructure/Identity.Infrastructure.csproj", "src/Services/Identity/Identity.Infrastructure/"]
RUN dotnet restore "./src/Services/Identity/Identity.API/./Identity.API.csproj"
COPY . .
WORKDIR "/src/src/Services/Identity/Identity.API"
RUN dotnet build "./Identity.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Identity.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Identity.API.dll"]
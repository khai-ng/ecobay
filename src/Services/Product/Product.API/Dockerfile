#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Services/Product/Product.API/Product.API.csproj", "src/Services/Product/Product.API/"]
COPY ["src/SharedKernel/Core.AspNetCore/Core.AspNet.csproj", "src/SharedKernel/Core.AspNetCore/"]
COPY ["src/SharedKernel/Core/Core.csproj", "src/SharedKernel/Core/"]
COPY ["src/SharedKernel/Core.Autofac/Core.Autofac.csproj", "src/SharedKernel/Core.Autofac/"]
COPY ["src/SharedKernel/Core.Kafka/Core.Kafka.csproj", "src/SharedKernel/Core.Kafka/"]
COPY ["src/SharedKernel/Core.MediaR/Core.MediaR.csproj", "src/SharedKernel/Core.MediaR/"]
COPY ["src/SharedKernel/Core.MongoDB/Core.MongoDB.csproj", "src/SharedKernel/Core.MongoDB/"]
RUN dotnet restore "./src/Services/Product/Product.API/./Product.API.csproj"
COPY . .
WORKDIR "/src/src/Services/Product/Product.API"
RUN dotnet build "./Product.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Product.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Product.API.dll"]
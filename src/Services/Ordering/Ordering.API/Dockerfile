#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Services/Ordering/Ordering.API/Ordering.API.csproj", "src/Services/Ordering/Ordering.API/"]
COPY ["src/SharedKernel/Core.AspNetCore/Core.AspNet.csproj", "src/SharedKernel/Core.AspNetCore/"]
COPY ["src/SharedKernel/Core/Core.csproj", "src/SharedKernel/Core/"]
COPY ["src/SharedKernel/Core.Autofac/Core.Autofac.csproj", "src/SharedKernel/Core.Autofac/"]
COPY ["src/SharedKernel/Core.EntityFramework/Core.EntityFramework.csproj", "src/SharedKernel/Core.EntityFramework/"]
COPY ["src/SharedKernel/Core.Kafka/Core.Kafka.csproj", "src/SharedKernel/Core.Kafka/"]
COPY ["src/SharedKernel/Core.Marten/Core.Marten.csproj", "src/SharedKernel/Core.Marten/"]
COPY ["src/SharedKernel/Core.MediaR/Core.MediaR.csproj", "src/SharedKernel/Core.MediaR/"]
RUN dotnet restore "./src/Services/Ordering/Ordering.API/./Ordering.API.csproj"
COPY . .
WORKDIR "/src/src/Services/Ordering/Ordering.API"
RUN dotnet build "./Ordering.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Ordering.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ordering.API.dll"]
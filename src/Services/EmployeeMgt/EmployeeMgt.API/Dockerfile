#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 80
EXPOSE 81

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Services/EmployeeMgt/EmployeeMgt.API/EmployeeMgt.API.csproj", "src/Services/EmployeeMgt/EmployeeMgt.API/"]
COPY ["src/SharedKernel/Core.AspNetCore/Core.AspNet.csproj", "src/SharedKernel/Core.AspNetCore/"]
COPY ["src/SharedKernel/Core/Core.csproj", "src/SharedKernel/Core/"]
COPY ["src/SharedKernel/Core.Autofac/Core.Autofac.csproj", "src/SharedKernel/Core.Autofac/"]
COPY ["src/SharedKernel/Core.Kafka/Core.Kafka.csproj", "src/SharedKernel/Core.Kafka/"]
COPY ["src/Services/CommonServices/Core.Contract/Core.Contract.csproj", "src/Services/CommonServices/Core.Contract/"]
COPY ["src/SharedKernel/Core.MediaR/Core.MediaR.csproj", "src/SharedKernel/Core.MediaR/"]
COPY ["src/Services/EmployeeMgt/EmployeeMgt.Application/EmployeeMgt.Application.csproj", "src/Services/EmployeeMgt/EmployeeMgt.Application/"]
COPY ["src/SharedKernel/Core.EntityFramework/Core.EntityFramework.csproj", "src/SharedKernel/Core.EntityFramework/"]
COPY ["src/Services/EmployeeMgt/EmployeeMgt.Domain/EmployeeMgt.Domain.csproj", "src/Services/EmployeeMgt/EmployeeMgt.Domain/"]
COPY ["src/Services/EmployeeMgt/EmployeeMgt.Infrastructure/EmployeeMgt.Infrastructure.csproj", "src/Services/EmployeeMgt/EmployeeMgt.Infrastructure/"]
RUN dotnet restore "./src/Services/EmployeeMgt/EmployeeMgt.API/./EmployeeMgt.API.csproj"
COPY . .
WORKDIR "/src/src/Services/EmployeeMgt/EmployeeMgt.API"
RUN dotnet build "./EmployeeMgt.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./EmployeeMgt.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EmployeeMgt.API.dll"]
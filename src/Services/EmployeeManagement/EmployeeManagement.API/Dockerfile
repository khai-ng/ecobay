#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 80
EXPOSE 81

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Services/EmployeeManagement/EmployeeManagement.API/EmployeeManagement.API.csproj", "src/Services/EmployeeManagement/EmployeeManagement.API/"]
COPY ["src/SharedKernel/Core.AspNetCore/Core.AspNet.csproj", "src/SharedKernel/Core.AspNetCore/"]
COPY ["src/SharedKernel/Core/Core.csproj", "src/SharedKernel/Core/"]
COPY ["src/SharedKernel/Core.Autofac/Core.Autofac.csproj", "src/SharedKernel/Core.Autofac/"]
COPY ["src/Services/EmployeeManagement/EmployeeManagement.Application/EmployeeManagement.Application.csproj", "src/Services/EmployeeManagement/EmployeeManagement.Application/"]
COPY ["src/Services/EmployeeManagement/EmployeeManagement.Domain/EmployeeManagement.Domain.csproj", "src/Services/EmployeeManagement/EmployeeManagement.Domain/"]
COPY ["src/Services/EmployeeManagement/EmployeeManagement.Infrastructure/EmployeeManagement.Infrastructure.csproj", "src/Services/EmployeeManagement/EmployeeManagement.Infrastructure/"]
COPY ["src/Services/Identity/Identity.Application/Identity.Application.csproj", "src/Services/Identity/Identity.Application/"]
COPY ["src/Services/Identity/Identity.Domain/Identity.Domain.csproj", "src/Services/Identity/Identity.Domain/"]
RUN dotnet restore "./src/Services/EmployeeManagement/EmployeeManagement.API/./EmployeeManagement.API.csproj"
COPY . .
WORKDIR "/src/src/Services/EmployeeManagement/EmployeeManagement.API"
RUN dotnet build "./EmployeeManagement.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./EmployeeManagement.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EmployeeManagement.API.dll"]
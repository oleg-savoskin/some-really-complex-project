FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["SomeReallyComplexProject.ServiceTwo/SomeReallyComplexProject.ServiceTwo.csproj", "SomeReallyComplexProject.ServiceTwo/"]
COPY ["SomeReallyComplexProject.Integration/SomeReallyComplexProject.Integration.csproj", "SomeReallyComplexProject.Integration/"]
COPY ["SomeReallyComplexProject.Core/SomeReallyComplexProject.Core.csproj", "SomeReallyComplexProject.Core/"]
COPY ["SomeReallyComplexProject.Common/SomeReallyComplexProject.Common.csproj", "SomeReallyComplexProject.Common/"]
RUN dotnet restore "SomeReallyComplexProject.ServiceTwo/SomeReallyComplexProject.ServiceTwo.csproj"
COPY . .
WORKDIR "/src/SomeReallyComplexProject.ServiceTwo"
RUN dotnet build "SomeReallyComplexProject.ServiceTwo.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "SomeReallyComplexProject.ServiceTwo.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "SomeReallyComplexProject.ServiceTwo.dll"]
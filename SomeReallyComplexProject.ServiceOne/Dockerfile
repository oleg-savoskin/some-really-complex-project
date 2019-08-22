FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["SomeReallyComplexProject.ServiceOne/SomeReallyComplexProject.ServiceOne.csproj", "SomeReallyComplexProject.ServiceOne/"]
COPY ["SomeReallyComplexProject.Integration/SomeReallyComplexProject.Integration.csproj", "SomeReallyComplexProject.Integration/"]
COPY ["SomeReallyComplexProject.Core/SomeReallyComplexProject.Core.csproj", "SomeReallyComplexProject.Core/"]
COPY ["SomeReallyComplexProject.Common/SomeReallyComplexProject.Common.csproj", "SomeReallyComplexProject.Common/"]
COPY ["SomeReallyComplexProject.ServiceOne.Domain/SomeReallyComplexProject.ServiceOne.Domain.csproj", "SomeReallyComplexProject.ServiceOne.Domain/"]
COPY ["SomeReallyComplexProject.EntityFramework/SomeReallyComplexProject.EntityFramework.csproj", "SomeReallyComplexProject.EntityFramework/"]
RUN dotnet restore "SomeReallyComplexProject.ServiceOne/SomeReallyComplexProject.ServiceOne.csproj"
COPY . .
WORKDIR "/src/SomeReallyComplexProject.ServiceOne"
RUN dotnet build "SomeReallyComplexProject.ServiceOne.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "SomeReallyComplexProject.ServiceOne.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "SomeReallyComplexProject.ServiceOne.dll"]
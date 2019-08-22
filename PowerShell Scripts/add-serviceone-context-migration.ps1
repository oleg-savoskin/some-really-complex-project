param (	
    [Parameter(Mandatory=$True)] $name
)

Set-StrictMode -Version Latest

$Env:SERVICE_IDENTIFIER = "SomeServiceID"
$Env:ASPNETCORE_ENVIRONMENT = "Development"
$Env:SQL_SERVER = "(localdb)\mssqllocaldb"
$Env:SQL_DATABASE = "Northwind"
$Env:SQL_USERNAME = "sa"
$Env:SQL_PASSWORD = "dummy"
$Env:RABBIT_HOST = "127.0.0.1"
$Env:RABBIT_VHOST = "/"
$Env:RABBIT_PORT = "5672"
$Env:RABBIT_USERNAME = "guest"
$Env:RABBIT_PASSWORD = "guest"

dotnet ef migrations add $name `
    -p ../SomeReallyComplexProject.ServiceOne.Domain/SomeReallyComplexProject.ServiceOne.Domain.csproj `
    -s ../SomeReallyComplexProject.ServiceOne/SomeReallyComplexProject.ServiceOne.csproj `
	-o Infrastructure/Migrations `
    -c ServiceOneContext
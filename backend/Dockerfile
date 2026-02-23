# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["ContactsApp/ContactsApp.csproj", "ContactsApp/"]
RUN dotnet restore "ContactsApp/ContactsApp.csproj"

COPY . .
RUN dotnet publish "ContactsApp/ContactsApp.csproj" -c Release -o /app/publish

# Run stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "ContactsApp.dll"]
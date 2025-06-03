FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build

WORKDIR /app


COPY *.csproj ./

RUN dotnet restore


COPY . ./

WORKDIR /app


RUN dotnet publish -c Release -o out


FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS runtime

WORKDIR /app

COPY --from=build /app/out ./

ENTRYPOINT ["dotnet", "MySaleApp.Admin.UI.dll"]
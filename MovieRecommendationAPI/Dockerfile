FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY MovieRecommendationAPI.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o out

# RUN dotnet tool install --global dotnet-ef
# ENV PATH="$PATH:/root/.dotnet/tools"
# RUN dotnet ef database update --project MovieRecommendationAPI.csproj

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/out .

EXPOSE 8080

ENTRYPOINT ["dotnet", "MovieRecommendationAPI.dll"]
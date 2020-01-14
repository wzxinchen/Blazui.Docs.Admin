FROM registry.cn-shanghai.aliyuncs.com/wzyuchen/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM registry.cn-shanghai.aliyuncs.com/wzyuchen/sdk:3.1-buster AS build
WORKDIR /src
COPY . .
RUN dotnet restore "src/Blazui.Docs.Service/Blazui.Docs.Service.csproj"
WORKDIR "/src/src/Blazui.Docs.Service"
RUN dotnet build "Blazui.Docs.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Blazui.Docs.Service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Blazui.Docs.Service.dll"]
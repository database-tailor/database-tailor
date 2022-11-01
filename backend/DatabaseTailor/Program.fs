open Falco
open Falco.Routing
open Falco.HostBuilder
open Microsoft.AspNetCore.SignalR
open Microsoft.AspNetCore.DataProtection
open Microsoft.Extensions.Caching.StackExchangeRedis
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open AspNet.Security.OAuth.GitHub
open StackExchange.Redis

let configuration = configuration [||] { add_env }

let configureGitHubAuthentication: GitHubAuthenticationOptions -> unit =
  fun options ->
    options.ClientId <- configuration["Github_ClientId"]
    options.ClientSecret <- configuration["Github_ClientSecret"]

let configureRedisCache: RedisCacheOptions -> unit =
  fun options ->
    options.InstanceName <- "DatabaseTailor_Backend_Cache"
    options.Configuration <- configuration.GetConnectionString("Redis")

let configureServices (serviceCollection: IServiceCollection) =
  let redisConnectionString = configuration.GetConnectionString("Redis")
  let redis = ConnectionMultiplexer.Connect(redisConnectionString)

  serviceCollection
    .AddDataProtection()
    .PersistKeysToStackExchangeRedis(redis, "DataProtection-Keys")
  |> ignore

  serviceCollection.AddAuthentication().AddGitHub(configureGitHubAuthentication)
  |> ignore

  serviceCollection.AddStackExchangeRedisCache(configureRedisCache) |> ignore
  serviceCollection

webHost [||] {
  add_service configureServices
  endpoints [ get "/ping" (Response.ofPlainText "pong!") ]
}

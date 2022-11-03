open Falco
open Falco.Routing
open Falco.HostBuilder
open Microsoft.AspNetCore.DataProtection
open Microsoft.Extensions.Caching.StackExchangeRedis
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open AspNet.Security.OAuth.GitHub
open StackExchange.Redis
open Falco.Security
open Microsoft.AspNetCore.Authentication
open Microsoft.AspNetCore.Authentication.Cookies

let configuration = configuration [||] { add_env }

let authenticationOptions: AuthenticationOptions -> unit =
  fun authOptions ->
    authOptions.DefaultScheme <- CookieAuthenticationDefaults.AuthenticationScheme
    authOptions.DefaultChallengeScheme <- GitHubAuthenticationDefaults.AuthenticationScheme

let configureGitHubAuthentication: GitHubAuthenticationOptions -> unit =
  fun options ->
    options.ClientId <- configuration["Github_ClientId"]
    options.ClientSecret <- configuration["Github_ClientSecret"]
    options.SaveTokens <- true
    options.CallbackPath <- "/github-callback"
    options.Scope.Add("user:email")

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

  serviceCollection
    .AddAuthentication(authenticationOptions)
    .AddCookie()
    .AddGitHub(configureGitHubAuthentication)
  |> ignore

  serviceCollection.AddStackExchangeRedisCache(configureRedisCache) |> ignore
  serviceCollection

webHost [||] {
  add_service configureServices
  use_authentication
  use_authorization

  endpoints [
    get "/ping" (Response.ofPlainText "pong!")
    get
      "/github-auth"
      (Auth.challenge GitHubAuthenticationDefaults.AuthenticationScheme (AuthenticationProperties(RedirectUri = "/")))
  ]
}

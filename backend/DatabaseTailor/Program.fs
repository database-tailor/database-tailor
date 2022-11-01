open Falco
open Falco.Routing
open Falco.HostBuilder

let configuration = configuration [||] { add_env }

webHost [||] { endpoints [ get "/ping" (Response.ofPlainText "pong!") ] }

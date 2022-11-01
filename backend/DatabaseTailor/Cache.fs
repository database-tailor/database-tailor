module Cache

open Microsoft.Extensions.Caching.Distributed
open System
open System.Text.Json
open FsToolkit.ErrorHandling

let jsonSerializerOptions =
  JsonSerializerOptions(PropertyNameCaseInsensitive = true)

let setRecordAsync
  (recordKey: string)
  (recordValue: 'a)
  (absoluteExpiry: TimeSpan option)
  (slidingExpiration: TimeSpan option)
  (cache: IDistributedCache)
  =
  let options = DistributedCacheEntryOptions()

  let defaultTimespan = TimeSpan.FromMinutes(5)
  options.AbsoluteExpirationRelativeToNow <- absoluteExpiry |> Option.defaultValue defaultTimespan
  options.SlidingExpiration <- slidingExpiration |> Option.defaultValue defaultTimespan

  let recordValue = JsonSerializer.Serialize(recordValue, jsonSerializerOptions)
  cache.SetStringAsync(recordKey, recordValue, options)

let getRecordAsync<'a> (recordKey: string) (distributedCache: IDistributedCache) = task {
  let! jsonResult = distributedCache.GetStringAsync(recordKey)
  return jsonResult |> Option.ofNull |> Option.map JsonSerializer.Deserialize<'a>
}

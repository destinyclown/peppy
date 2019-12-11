using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Peppy.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Peppy.ServiceRegistry.Consul.Provider
{
    public class ConsulConfigurationProvider : ConfigurationProvider
    {
        private const string ConsulIndexHeader = "X-Consul-Index";
        private readonly string _path;
        private readonly HttpClient _httpClient;
        private readonly IReadOnlyList<Uri> _consulUrls;
        private readonly Task _configurationListeningTask;
        private int _consulUrlIndex; private int _failureCount;
        private int _consulConfigurationIndex;
        public ConsulConfigurationProvider(IEnumerable<Uri> consulUrls, string path)
        {
            _path = path;
            _consulUrls = consulUrls.Select(u => new Uri(u, $"v1/kv/{path}")).ToList(); 
            if (_consulUrls.Count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(consulUrls));
            }

            _httpClient = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip }, true);
            _configurationListeningTask = new Task(ListenToConfigurationChanges);
        }
        public override void Load() => LoadAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        private async Task LoadAsync()
        {
            Data = await ExecuteQueryAsync();
            if (_configurationListeningTask.Status == TaskStatus.Created)
                _configurationListeningTask.Start();
        }
        private async void ListenToConfigurationChanges()
        {
            while (true)
            {
                try
                {
                    if (_failureCount > _consulUrls.Count)
                    {
                        _failureCount = 0; await Task.Delay(TimeSpan.FromMinutes(1));
                    }

                    Data = await ExecuteQueryAsync(true);
                    OnReload();
                    _failureCount = 0;
                }
                catch (TaskCanceledException)
                {
                    _failureCount = 0;
                }
                catch
                {
                    _consulUrlIndex = (_consulUrlIndex + 1) % _consulUrls.Count;
                    _failureCount++;
                }
            }
        }

        private async Task<IDictionary<string, string>> ExecuteQueryAsync(bool isBlocking = false)
        {
            var requestUri = isBlocking ? $"?recurse=true&index={_consulConfigurationIndex}" : "?recurse=true";
            using var request = new HttpRequestMessage(HttpMethod.Get, new Uri(_consulUrls[_consulUrlIndex], requestUri));
            using var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            if (response.Headers.Contains(ConsulIndexHeader))
            {
                var indexValue = response.Headers.GetValues(ConsulIndexHeader).FirstOrDefault(); int.TryParse(indexValue, out _consulConfigurationIndex);
            }
            var tokens = JsonHelper.FromJsonList<ConsulConfigurationEnitiy>(await response.Content.ReadAsStringAsync());
            var keyValues = tokens
                .Select(k => new KeyValuePair<string, JToken>
                (
                  k.Key.Substring(_path.Length),
                  k.Value != null ? JToken.Parse(Encoding.UTF8.GetString(Convert.FromBase64String(k.Value))) : null
                ))
              .Where(v => !string.IsNullOrWhiteSpace(v.Key))
              .SelectMany(Flatten)
              .ToDictionary(v => ConfigurationPath.Combine(v.Key.Split('/')), v => v.Value, StringComparer.OrdinalIgnoreCase);
            return keyValues;
        }

        private static IEnumerable<KeyValuePair<string, string>> Flatten(KeyValuePair<string, JToken> tuple)
        {
            if (!(tuple.Value is JObject value))
            {
                if (tuple.Value is JArray values)
                {
                    for (int i = 0; i < values.Count; i++)
                    {
                        foreach (var item in Flatten(new KeyValuePair<string, JToken>($"{tuple.Key}:{i.ToString()}", values[i])))
                            yield
                                return item;
                    }
                }
                else
                {
                    var propertyKey = $"{tuple.Key}";
                    var str = tuple.Value.Value<string>();
                    yield
                        return new KeyValuePair<string, string>(propertyKey, str);
                }
            }
            else
            {
                foreach (var property in value)
                {
                    var propertyKey = $"{tuple.Key}/{property.Key}";
                    switch (property.Value.Type)
                    {
                        case JTokenType.Object:
                            foreach (var item in Flatten(new KeyValuePair<string, JToken>(propertyKey, property.Value)))
                                yield
                                    return item;
                            break;
                        case JTokenType.Array:
                            if (property.Value is JArray values)
                            {
                                for (int i = 0; i < values.Count; i++)
                                {
                                    foreach (var item in Flatten(new KeyValuePair<string, JToken>($"{propertyKey}:{i.ToString()}", values[i])))
                                        yield
                                            return item;
                                }
                            }
                            break;
                        default:
                            yield
                                return new KeyValuePair<string, string>(propertyKey, property.Value.Value<string>());
                            break;
                    }
                }
            }
        }
    }
}

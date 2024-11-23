// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Atlas.Web.App.Storages;

[ExcludeFromCodeCoverage]
internal sealed class LocalStorage(IJSInProcessRuntime jsRuntime) : ILocalStorage
{
    private readonly JsonSerializerOptions _options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
    };

    public T? GetItem<T>(string key)
    {
        string? json = jsRuntime.Invoke<string>("localStorage.getItem", key);

        return string.IsNullOrEmpty(json) ? default : JsonSerializer.Deserialize<T>(json, _options);
    }

    public void RemoveItem(string key) => jsRuntime.InvokeVoid("localStorage.removeItem", key);

    public void SetItem<T>(string key, T value)
    {
        string json = JsonSerializer.Serialize(value, _options);

        jsRuntime.InvokeVoid("localStorage.setItem", key, json);
    }
}

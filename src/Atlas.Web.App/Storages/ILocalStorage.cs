// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

namespace Atlas.Web.App.Storages;

public interface ILocalStorage
{
    T? GetItem<T>(string key);

    void RemoveItem(string key);

    void SetItem<T>(string key, T value);
}

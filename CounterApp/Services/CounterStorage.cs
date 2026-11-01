
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.Maui.Storage;
using CounterApp.Models;

namespace CounterApp.Services;

public static class CounterStorage
{
    public static void SaveCounters(IEnumerable<Counter> counters)
    {
        string json = JsonSerializer.Serialize(counters);
        Preferences.Default.Set("counters", json);
    }

    public static List<Counter> LoadCounters()
    {
        string json = Preferences.Default.Get("counters", "");

        if (json == "")
            return new List<Counter>();

        try
        {
            return JsonSerializer.Deserialize<List<Counter>>(json)
                   ?? new List<Counter>();
        }
        catch (JsonException)
        {
            return new List<Counter>();
        }
    }
}


using System;
using System.Collections.Generic;
using CounterApp.Models;
using CounterApp.Services;

namespace CounterApp.Views;

public partial class MainPage : ContentPage
{
    public List<Counter> Counters { get; set; } = new List<Counter>();

    public MainPage()
    {
        InitializeComponent();
        Counters = CounterStorage.LoadCounters();
        BindingContext = this;
        BindableLayout.SetItemsSource(countersLayout, Counters);
    }

    private async void AddCounter(object sender, EventArgs e)
    {
        string name = counterNameEntry.Text?.Trim() ?? "";

        if (name == "")
        {
            await DisplayAlert("Błąd", "Wpisz nazwę licznika!", "OK");
            return;
        }

        int start = 0;
        string text = startingValueEntry.Text ?? "";

        if (text.Trim() != "" && !int.TryParse(text, out start))
        {
            await DisplayAlert("Błąd", "Wpisz poprawną liczbę!", "OK");
            return;
        }

        Counters.Add(new Counter { Name = name, Value = start });

        RefreshCounters();
        CounterStorage.SaveCounters(Counters);

        counterNameEntry.Text = "";
        startingValueEntry.Text = "";
    }

    private void IncreaseCounter(object sender, EventArgs e)
    {
        ChangeCounter(sender, 1);
    }

    private void DecreaseCounter(object sender, EventArgs e)
    {
        ChangeCounter(sender, -1);
    }

    private void ChangeCounter(object sender, int amount)
    {
        if (sender is not Button button ||
            button.CommandParameter is not Counter counter)
            return;

        if (amount == 1 && counter.Value == int.MaxValue)
            return;

        if (amount == -1 && counter.Value == int.MinValue)
            return;

        counter.Value += amount;

        RefreshCounters();
        CounterStorage.SaveCounters(Counters);
    }

    private void RefreshCounters()
    {
        BindableLayout.SetItemsSource(countersLayout, null);
        BindableLayout.SetItemsSource(countersLayout, Counters);
    }
}

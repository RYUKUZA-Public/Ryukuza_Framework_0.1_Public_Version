using System.Collections.Generic;
using UnityEngine;

public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}

public class DataManager
{
    public Dictionary<int, Stat> StatsDict { get; private set; } = new Dictionary<int, Stat>();

    public void Init()
    {
        StatsDict = LoadJson<StatData, int, Stat>("StatData").MakeDict();
    }

    private Loader LoadJson<Loader, key, Value>(string path) where Loader : ILoader<key, Value>
    {
        // JsonデータなのでTextAsset
        // Jsonデータを丸ごとロード
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/{path}");
        // メモリにロード
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }
}
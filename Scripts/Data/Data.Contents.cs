using System;
using System.Collections.Generic;

#region [Stat]
[Serializable]
public class Stat
{
    // Jsonのデータ名と同じである必要があり
    public int level;
    public int maxHp;
    public int attack;
    public int totalExp;
}

[Serializable]
public class StatData : ILoader<int, Stat>
{
    public List<Stat> stats = new List<Stat>();
    
    public Dictionary<int, Stat> MakeDict()
    {
        Dictionary<int, Stat> dict = new Dictionary<int, Stat>();
    
        // List -> Dictionary
        foreach (Stat stat in stats)
            dict.Add(stat.level, stat);
        return dict;
    }
}
#endregion
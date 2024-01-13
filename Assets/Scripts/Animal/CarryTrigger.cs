using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Game.Tool.Singleton;

public class CarryTrigger : Singleton<CarryTrigger>
{
    public Dictionary<GameObject, int> backpack = new Dictionary<GameObject, int>();

    public void AddItem(GameObject itemPrefab)
    {
        if (backpack.ContainsKey(itemPrefab))
        {
            backpack[itemPrefab]++;
        }
        else
        {
            backpack.Add(itemPrefab, 1);
        }
    }
    
    public bool RemoveItem(GameObject itemPrefab)
    {
        if (backpack.ContainsKey(itemPrefab))
        {
            if (backpack[itemPrefab] > 1)
            {
                backpack[itemPrefab]--;
            }
            else
            {
                backpack.Remove(itemPrefab);
            }
            return true;
        }
        else
        {
            return false;
        }
    }

}

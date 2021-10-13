using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class DropWeight : MonoBehaviour
{
    /// <summary>
    /// Laver et List view og vælger en random værdi som skal "droppes" inden for en vis værdi
    /// </summary>
    /// <param name="_weightDict"></param>
    public void TempWeight(Dictionary<string, float> _weightDict) {
        float value = Random.value;

        List<string> keys = new List<string>();
        keys.AddRange(_weightDict.Where(w => w.Value >= value).Select(w => w.Key).ToList());

        if (keys.Count == 0) {
            return;
        }
        print($"{value} - {keys[Random.Range(0, keys.Count)]}");
    }
}
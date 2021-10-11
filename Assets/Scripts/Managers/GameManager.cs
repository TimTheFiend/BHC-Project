using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null; // Singleton

    public GameObject player;

    private void Awake() {
        #region Singleton Pattern
        if (instance == null) {
            instance = this;
        }
        else if (instance != this) {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        #endregion
    }

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

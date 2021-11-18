using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance = null;

    [Header("Debug vars")]
    public bool isDebug = true;
    public GameObject mob;
    public GameObject boss;

    [Header("Item templates")]
    public GameObject itemObject;

    [Header("Enemy drops")]
    public List<GameObject> pickupDrops = new List<GameObject>();

    [Header("Enemies")]
    public List<GameObject> enemies = new List<GameObject>();

    [Header("Room")]
    public List<UpgradeObject> itemUpgrades = new List<UpgradeObject>();
    public List<GameObject> bosses = new List<GameObject>();

    [Header("Exit")]
    public GameObject exitObject;

    private void Awake() {

        #region Singleton pattern

        if (instance == null) {
            instance = this;
        }
        else if (instance != this) {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        #endregion Singleton pattern
    }

    #region Room Handling

    /// <summary>
    /// Activates a room of type <see cref="RoomType.Normal"/>.
    /// </summary>
    /// <param name="activeRoom">RoomHolder</param>
    /// <param name="mobsInActiveRoom">The amount of mobs in the active room.</param>
    public void ActivateNormalRoom(Transform activeRoom, out int mobsInActiveRoom) {
        mobsInActiveRoom = 0;
        Transform player = GameManager.instance.player.transform;
        bool canUseDoors = GameManager.instance.canUseDoors;

        foreach (Transform obj in activeRoom) {
            if (obj.tag == "SpawnerMob" && canUseDoors == false) {
                if (isDebug) {
                    //Only spawns the same EnemyObject.
                    GameObject spawn = Instantiate(mob, obj.position, Quaternion.identity);
                    spawn.transform.SetParent(activeRoom);
                    spawn.gameObject.GetComponent<EnemyAttackController>().playerPosition = player.transform;
                    Destroy(obj.gameObject);
                }
                else {
                    Debug.LogError("Non-debug `ActivateNormalRoom` hasn't been implemented");
                }
                mobsInActiveRoom++;
            }
        }
    }

    /// <summary>
    /// Activates a room of type <see cref="RoomType.Boss"/>.
    /// </summary>
    /// <param name="activeRoom">RoomHolder</param>
    public void ActivateBossRoom(Transform activeRoom) {
        if (isDebug) {
            GameObject bossToSpawn = Instantiate(boss, activeRoom.position, Quaternion.identity);
            bossToSpawn.GetComponent<EnemyAttackController>().playerPosition = GameManager.instance.player.transform;
        }
        else {
            Debug.LogError("Non-debug `ActivateBossRoom` hasn't been implemented");
        }
    }

    /// <summary>
    /// Activates a room of type <see cref="RoomType.Boss"/>.
    /// </summary>
    /// <param name="activeRoom">RoomHolder</param>
    public void ActivateBossRoom(Transform activeRoom, out int mobsInActiveRoom) {
        mobsInActiveRoom = 1;
        if (isDebug) {
            GameObject bossToSpawn = Instantiate(boss, activeRoom.position, Quaternion.identity);
            bossToSpawn.GetComponent<EnemyAttackController>().playerPosition = GameManager.instance.player.transform;
        }
        else {
            Debug.LogError("Non-debug `ActivateBossRoom` hasn't been implemented");
        }
    }

    /// <summary>
    /// Activates a room of type <see cref="RoomType.Item"/>.
    /// </summary>
    /// <param name="activeRoom">RoomHolder</param>
    public void ActivateItemRoom(Transform activeRoom) {
        if (isDebug) {
            GameObject itemToSpawn = Instantiate(itemObject, activeRoom.position, Quaternion.identity);

            itemToSpawn.GetComponent<UpgradeEntity>().SetValues(itemUpgrades[0]);
        }
        else {
            Debug.LogError("Non-debug `ActivateItemRoom` hasn't been implemented");
        }
    }

    #endregion Room Handling

    #region Pickup drops

    /// <summary>
    /// Checks to see if a mob should drop an item.
    /// </summary>
    /// <param name="mobPosition">Position of the dead mob.</param>
    public void MobPickupDrop(Vector3 mobPosition) {
        Dictionary<GameObject, float> weight = weight = new Dictionary<GameObject, float>();
        if (isDebug) {
            foreach (GameObject pickup in pickupDrops) {
                weight.Add(pickup, 0.33f);
            }
        }

        GameObject _pickup = PickupWeight(weight);
        if (_pickup == null) {
            Debug.Log("No pickup");
            return;
        }

        Instantiate(_pickup, mobPosition, Quaternion.identity);
    }

    /// <summary>
    /// Helper method for MobPickupDrop
    /// </summary>
    /// <param name="dict"></param>
    /// <returns></returns>
    private GameObject? PickupWeight(Dictionary<GameObject, float> dict) {
        float value = Random.value;

        List<GameObject> gObjs = new List<GameObject>();
        gObjs.AddRange(dict.Where(x => x.Value >= value).Select(x => x.Key).ToList());

        if (gObjs.Count == 0) {
            return null;
        }

        return gObjs[Random.Range(0, gObjs.Count)];
    }

    #endregion Pickup drops

    /// <summary>
    /// Spawns an <see cref="ExitObject"/> after the boss of a dungeon has been killed.
    /// </summary>
    /// <param name="pos">The center position of the boss room.</param>
    public void SpawnDungeonExit(Vector3 pos) {
        GameObject exit = Instantiate(exitObject, pos, Quaternion.identity);
    }
}
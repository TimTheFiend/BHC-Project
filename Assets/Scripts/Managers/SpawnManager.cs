using System.Collections;
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

    [Header("Lists")]
    public List<GameObject> enemies = new List<GameObject>();
    public List<UpgradeObject> itemUpgrades = new List<UpgradeObject>();
    public List<GameObject> bosses = new List<GameObject>();

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

}
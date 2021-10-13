using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DungeonGenerator : MonoBehaviour
{
    public static DungeonGenerator Instance = null;

    [Header("Debug")]
    [Tooltip("Prints out additional data if `true`.")]
    public bool isDebug;
    public bool hasSeed;
    public int rngSeed;

    [Header("Generation variables")]
    [Range(1, 10)] public int dungeonLevel = 1;
    [Range(5, 10)] public int maxLayoutWidth = 6;
    [Range(5, 10)] public int maxLayoutHeight = 5;
    public readonly int _minAmountDeadendsPerFloor = 5;
    public int minAmountDeadends = 5;
    public int maxAmountRooms;

    [Header("Minimap variables")]
    public RoomObject startRoom;
    public List<RoomObject> minimapPositions = new List<RoomObject>();
    public Queue<RoomObject> minimapQueue = new Queue<RoomObject>();

    [Header("Generation Variables")]
    [SerializeField, Range(0.0f, 1.0f)]
    private float randomRoomGiveUp = 0.5f;
    private int generationAttempts = 5000;  //1 Million attempts. - Dr. Evil.

    #region Start / Awake

    private void Start() {
        if (isDebug) {
            //TODO
        }
    }

    private void Awake() {

        #region Singleton Pattern

        if (Instance == null) {
            Instance = this;
        }
        else if (Instance != this) {
            Destroy(this);
        }
        DontDestroyOnLoad(gameObject);

        #endregion Singleton Pattern
    }

    #endregion Start / Awake

    public void GenerateDungeon() {
        if (isDebug) {
            DebugStartGeneration();
            //NOTE: temp 
        }
        else {
            DebugStartGeneration();
            RoomDrawer.Instance.DrawDungeonRooms(minimapPositions);
        }
    }

    #region Starts Generation



    private void DebugStartGeneration() {
        for (int i = 0; i < generationAttempts; i++) {
            if (GenerateDungeonLayout()) {
                //Debug.ClearDeveloperConsole();
                print($"Generate: {maxAmountRooms}\tStartroom: {startRoom.ToString()}\tAttempts: {i}");
                foreach (RoomObject item in minimapPositions) {
                    print(item.DebugPrint());
                }
                break;
            }
        }
        if (minimapPositions.Count != maxAmountRooms - 1) {
           Debug.Log("didn't generate");
        }
    }

    #endregion Starts Generation

    #region Procedural Generation

    private bool GenerateDungeonLayout() {
        InitialiseVariables();
        while (true) {
            if (minimapQueue.Count == 0) {
                return false;
            }

            RoomObject currentRoom = minimapQueue.Dequeue();

            foreach (RoomObject newRoom in currentRoom.GetAdjacentRooms()) {
                //Random chance to stop
                if (Random.value > randomRoomGiveUp) {
                    //Room for more rooms
                    if (minimapPositions.Count < maxAmountRooms) {
                        //If inside bounds
                        if (newRoom.InsideBounds(maxLayoutWidth, maxLayoutHeight)) {
                            //Room isn't already in the list
                            if (!minimapPositions.Contains(newRoom)) {
                                if (HasNoAdjacentRooms(newRoom)) {
                                    currentRoom = AddRoomToLayout(newRoom, currentRoom);
                                    
                                }
                            }
                        }
                    }
                    else {
                        return true;
                    }
                }
            }
        }
    }

    #endregion Procedural Generation

    #region Helper functions

    /// <summary>
    /// Checks if a <see cref="RoomObject"/> can be placed in the given position.
    /// </summary>
    /// <param name="room">Room to be spawned</param>
    /// <returns></returns>
    private bool HasNoAdjacentRooms(RoomObject room) {
        int counter = -1; //-1 to balance the neighbour it came from

        foreach (RoomObject newRoom in room.GetAdjacentRooms()) {
            if (minimapPositions.Contains(newRoom)) {
                counter++;
            }
        }

        return counter < 1;
    }

    /// <summary>
    /// Resets the values of variables that are used in generation.
    /// </summary>
    private void InitialiseVariables() {
        /* Reset collections */
        minimapPositions.Clear();
        minimapQueue.Clear();
        /* Set seed */
        if (!hasSeed) {
            rngSeed = (int)System.DateTime.Now.Ticks;
        }
        Random.InitState(rngSeed);
        /* Get a new `startRoom` */
        startRoom = RoomObject.GetStartRoom(maxLayoutWidth, maxLayoutHeight);
        AddRoomToLayout(startRoom);
        /* Reset number of rooms to generate */
        GetMaxAmountRooms();
    }

    //NOTE: needs expansion.
    private void GetMaxAmountRooms() {
        minAmountDeadends = Random.Range(_minAmountDeadendsPerFloor, _minAmountDeadendsPerFloor + 1 + 1); //+1 +1 because exclusive, and for variation.
        maxAmountRooms = Mathf.RoundToInt(3.33f * dungeonLevel + minAmountDeadends);
    }

    #endregion Helper functions

    #region Add Room To Layout

    /// <summary>
    /// Adds a <see cref="RoomObject"/> to <see cref="minimapPositions"/> and <see cref="minimapQueue"/>, and gives it an index value.
    /// </summary>
    /// <param name="newRoom">Room to be added.</param>
    private void AddRoomToLayout(RoomObject newRoom) {
        newRoom.index = minimapPositions.Count;
        minimapPositions.Add(newRoom);
        minimapQueue.Enqueue(newRoom);
    }

    /// <summary>
    /// Sets and updates <see cref="DoorLayout"/> values in a new <see cref="RoomObject"/> and the object it spawned from.
    /// </summary>
    /// <param name="newRoom">Room to be added.</param>
    /// <param name="parentRoom">The parent room.</param>
    /// <returns>The updated parent room.</returns>
    private RoomObject AddRoomToLayout(RoomObject newRoom, RoomObject parentRoom) {
        /* Set DoorLayout */
        DoorLayout parent;

        newRoom.SetDoor(newRoom.GetDoorLayoutFromOffset(parentRoom, out parent), true);
        parentRoom.SetDoor(parent, true);

        minimapPositions[parentRoom.index] = parentRoom;

        //parentRoom.SetDoor(parent, true);

        AddRoomToLayout(newRoom);

        return parentRoom;
    }

    #endregion Add Room To Layout

    #region DrawGizmos

    private void OnDrawGizmos() {
        if (!isDebug) {
            return;
        }

        Gizmos.color = Color.white;
        for (int x = 0; x < maxLayoutWidth + 1; x++) {
            for (int y = 0; y < maxLayoutHeight + 1; y++) {
                if ((x + y) % 2 == 0) {
                    Gizmos.color = Color.white;
                }
                else {
                    Gizmos.color = Color.black;
                }
                Gizmos.DrawCube(new Vector3(x + 0.5f, y + 0.5f, 0f), new Vector3(1f, 1f, 0f));
            }
        }

        for (int i = 0; i < minimapPositions.Count; i++) {
            RoomObject room = minimapPositions[i];

            if (i == 0) {
                Gizmos.color = Color.magenta;
            }
            else {
                Gizmos.color = i % 2 == 0 ? Color.red : Color.blue;
            }
            Gizmos.DrawCube(new Vector3(room.x + 0.5f, room.y + 0.5f, 0f), Vector3.one);
        }
    }

    #endregion DrawGizmos
}
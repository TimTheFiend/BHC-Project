using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DungeonGenerator : MonoBehaviour
{
    public static DungeonGenerator instance = null;

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

    private int generationAttempts = 15000;

    #region Start / Awake

    private void Start() {
        if (isDebug) {
            //TODO
        }
    }

    private void Awake() {

        #region Singleton Pattern

        if (instance == null) {
            instance = this;
        }
        else if (instance != this) {
            Destroy(this);
        }
        DontDestroyOnLoad(gameObject);

        #endregion Singleton Pattern
    }

    #endregion Start / Awake

    /// <summary>
    /// Starts the process of generating a new dungeon layout.
    /// </summary>
    public void GenerateDungeon() {
        DebugStartGeneration();
    }

    /// <summary>
    /// Attempts to generate the dungeon.
    /// </summary>
    private void DebugStartGeneration() {
        for (int i = 0; i < generationAttempts; i++) {
            if (GenerateDungeonLayout()) {
                OnSuccessfulGeneration();
                return;
            }
        }
        Debug.Log("Couldn't generate layout.");
    }

    private void OnSuccessfulGeneration() {
        minimapQueue.Clear();

        RoomDrawer.Instance.DrawDungeonRooms(minimapPositions);
        GameManager.instance.SetCurrentPlayerPosition(startRoom);
    }

    #region Procedural Generation

    /// <summary>
    /// Generates a layout with certain criteria that has to be fulfilled in order to be valid.
    /// </summary>
    /// <returns><c>true</c> if the generation is valid for use; otherwise <c>false</c>.</returns>
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
}
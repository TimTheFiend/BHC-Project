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
    public int minAmountDeadends = 3;
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
                if (HasEnoughDeadends()) {
                    CompleteGeneration();
                }
            }
        }
    }

    /// <summary>
    /// When the generation is valid, this calls the last functions to start gameplay.
    /// </summary>
    private void CompleteGeneration() {
        AssignRoomTypes();

        RoomDrawer.Instance.DrawDungeonRooms(minimapPositions);
        GameManager.instance.SetCurrentPlayerPosition(startRoom);
    }

    #region Deadends assignment

    /// Make sure there's enough deadends
    private bool HasEnoughDeadends() {
        int deadendCounter = 0;
        foreach (RoomObject room in minimapPositions) {
            if (room.IsDeadEnd && room != startRoom) {
                deadendCounter++;
                //No reason to keep going if true
                if (deadendCounter >= minAmountDeadends) {
                    return true;
                }
            }
        }
        return deadendCounter >= minAmountDeadends;
    }

    /// <summary>
    /// Assigns certain rooms a new roomtype that are needed for a complet level.
    /// </summary>
    private void AssignRoomTypes() {
        //Reverse the list to get the room furthest away from start pos.
        List<RoomObject> reversedPos = minimapPositions.ToList();
        reversedPos.Reverse();

        Queue<RoomType> roomsNeeded = GetSpecialRoomsToAdd();

        //Assign start room
        RoomObject _startRoom = minimapPositions.SingleOrDefault(r => r.center == startRoom.center);
        _startRoom.type = RoomType.StartRoom;
        minimapPositions[_startRoom.index] = _startRoom;

        //Assign rest of special rooms
        for (int i = 0; i < reversedPos.Count; i++) {
            if (roomsNeeded.Count == 0) {
                break;
            }
            if (reversedPos[i].IsDeadEnd) {
                RoomObject deadEnd = reversedPos[i];
                deadEnd.type = roomsNeeded.Dequeue();
                minimapPositions[deadEnd.index] = deadEnd;
            }
        }
    }

    /// <summary>
    /// Temporary helper method for AssignRoomTypes
    /// </summary>
    private Queue<RoomType> GetSpecialRoomsToAdd() {
        Queue<RoomType> queue = new Queue<RoomType>();
        queue.Enqueue(RoomType.Boss);
        queue.Enqueue(RoomType.Item);

        return queue;
    }

    #endregion Deadends assignment

    #region Generating the layout.

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

    #endregion Generating the layout.

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
        //minAmountDeadends = Random.Range(_minAmountDeadendsPerFloor, _minAmountDeadendsPerFloor + 1 + 1); //+1 +1 because exclusive, and for variation.
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
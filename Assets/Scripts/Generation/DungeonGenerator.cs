using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public static DungeonGenerator Instance = null;

    [Header("Debug")]
    public bool isDebug = true;
    public bool hasSeed = false;
    public int rngSeed;

    [Header("Generation variables")]
    [Range(1, 10)] public int dungeonLevel = 1;
    [Range(5, 10)] public int maxLayoutWidth = 6;
    [Range(5, 10)] public int maxLayoutHeight = 5;
    public readonly int _minAmountDeadendsPerFloor = 5;
    public int minAmountDeadends = 5;
    public int maxAmountRooms;

    [Header("Minimap variables")]
    public List<RoomObject> minimapPositions = new List<RoomObject>();
    public Queue<RoomObject> minimapQueue = new Queue<RoomObject>();

    [Header("Generation Variables")]
    [SerializeField, Range(0.0f, 1.0f)]
    private readonly float randomRoomGiveUp = 0.5f;
    private int generationAttempts = 5000;

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

    public void StartGeneration() {
        if (isDebug) {
            DebugStartGeneration();
        }
    }

    private void DebugStartGeneration() {
        for (int i = 0; i < generationAttempts; i++) {
            InitialiseVariables();
            if (GenerateDungeonLayout()) {
                Debug.ClearDeveloperConsole();
                print($"Generate: {maxAmountRooms}\tDeadends: {minAmountDeadends}\tAttempts: {i}");
                foreach (RoomObject item in minimapPositions) {
                    print(item.DebugPrint());
                }
                break;
            }
        }
    }

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

    private bool GenerateDungeonLayout() {
        //AddRoomToLayout(GetStartRoom);
        AddRoomToLayout(RoomObject.StartRoom(maxLayoutWidth, maxLayoutHeight));
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

        return false;
    }

    private bool HasNoAdjacentRooms(RoomObject room) {
        int counter = -1; //-1 to balance the neighbour it came from

        foreach (RoomObject newRoom in room.GetAdjacentRooms()) {
            if (minimapPositions.Contains(newRoom)) {
                counter++;
            }
        }

        return counter < 1;
    }

    private void InitialiseVariables() {
        /* Reset collections */
        minimapPositions.Clear();
        minimapQueue.Clear();
        /* Set seed */
        if (!hasSeed) {
            rngSeed = (int)System.DateTime.Now.Ticks;
        }
        Random.InitState(rngSeed);
        /* Reset number of rooms to generate */
        GetMaxAmountRooms();
    }

    private void GetMaxAmountRooms() {
        minAmountDeadends = Random.Range(_minAmountDeadendsPerFloor, _minAmountDeadendsPerFloor + 1 + 1); //+1 +1 because exclusive, and for variation.
        maxAmountRooms = Mathf.RoundToInt(3.33f * dungeonLevel + minAmountDeadends);
    }

    private void AddRoomToLayout(RoomObject newRoom) {
        newRoom.index = minimapPositions.Count;
        minimapPositions.Add(newRoom);
        minimapQueue.Enqueue(newRoom);
    }

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
}
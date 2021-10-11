using System.Collections;
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
    public List<Vector2Int> minimapPositions = new List<Vector2Int>();
    public Queue<Vector2Int> minimapQueue = new Queue<Vector2Int>();

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
                print($"Generate: {maxAmountRooms}\tDeadends: {minAmountDeadends}\tAttempts: {i}");
                foreach (Vector2Int pos in minimapPositions) {
                    print($"room: {pos}");
                }
                break;
            }
        }
    }

    private bool GenerateDungeonLayout() {
        AddRoomToLayout(GetStartRoom);
        while (true) {
            if (minimapQueue.Count == 0) {
                return false;
            }

            Vector2Int currentRoom = minimapQueue.Dequeue();
            foreach (Vector2Int direction in GetCardinalDirections()) {
                //Random chance to stop
                if (Random.value > randomRoomGiveUp) {
                    //Room for more rooms
                    if (minimapPositions.Count < maxAmountRooms) {
                        Vector2Int newRoom = currentRoom + direction;
                        //If inside bounds
                        if (0 <= newRoom.x &&
                            newRoom.x <= maxLayoutWidth &&
                            0 <= newRoom.y &&
                            newRoom.y <= maxLayoutHeight) {
                            //Room isn't already in the list
                            if (!minimapPositions.Contains(newRoom)) {
                                if (HasNoAdjacentRooms(newRoom)) {
                                    AddRoomToLayout(newRoom);
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

    private bool HasNoAdjacentRooms(Vector2Int room) {
        int counter = -1; //-1 to balance the neighbour it came from

        foreach (Vector2Int direction in GetCardinalDirections()) {
            if (minimapPositions.Contains(room + direction)) {
                counter++;
            }
        }

        return counter < 1;
    }

    private IEnumerable<Vector2Int> GetCardinalDirections() {
        yield return Vector2Int.up;
        yield return Vector2Int.right;
        yield return Vector2Int.down;
        yield return Vector2Int.left;
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

    private void AddRoomToLayout(Vector2Int roomPos) {
        minimapPositions.Add(roomPos);
        minimapQueue.Enqueue(roomPos);
    }

    private Vector2Int GetStartRoom {
        get {
            //startRoom isn't in a corner, or directly next to a corner
            if (Random.value > 0.5f) {
                return new Vector2Int(Random.Range(2, maxLayoutWidth - 1), Random.value > 0.5f ? 0 : maxLayoutHeight);
            }
            return new Vector2Int(Random.value > 0.5f ? 0 : maxLayoutWidth, Random.Range(2, maxLayoutHeight - 1));
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DungeonLayout
{
    //Hardcoded from RoomDrawer
    private static int width = 16;
    private static int height = 12;

    private static Dictionary<DoorLayout, List<Vector2Int>> doorTilemapPositions;

    public static Dictionary<DoorLayout, Vector2> doorGameObjectPositions;

    static DungeonLayout() {

        #region Tilemap Doors

        doorTilemapPositions = new Dictionary<DoorLayout, List<Vector2Int>>();
        doorTilemapPositions.Add(DoorLayout.Up, new List<Vector2Int>() { new Vector2Int(0, 5), new Vector2Int(-1, 5) });
        doorTilemapPositions.Add(DoorLayout.Down, new List<Vector2Int>() { new Vector2Int(0, -6), new Vector2Int(-1, -6) });
        doorTilemapPositions.Add(DoorLayout.Left, new List<Vector2Int>() { new Vector2Int(-8, 0), new Vector2Int(-8, -1) });
        doorTilemapPositions.Add(DoorLayout.Right, new List<Vector2Int>() { new Vector2Int(7, 0), new Vector2Int(7, -1) });

        #endregion Tilemap Doors

        #region GameObject Doors

        doorGameObjectPositions = new Dictionary<DoorLayout, Vector2>();
        doorGameObjectPositions.Add(DoorLayout.Up, new Vector2(5.5f, 0f));
        doorGameObjectPositions.Add(DoorLayout.Down, new Vector2(-5.5f, 0f));
        doorGameObjectPositions.Add(DoorLayout.Right, new Vector2(0f, 7.5f));
        doorGameObjectPositions.Add(DoorLayout.Left, new Vector2(0f, -7.5f));

        #endregion GameObject Doors
    }

    //NOTE dårligt navn
    public static Vector3Int GetRoomCenterWorldPosition(RoomObject room) {
        return new Vector3Int(room.x * width, room.y * height, 0);
    }

    public static Vector3Int GetRoomCenterWorldPosition(RoomObject room, int xPos, int yPos) {
        Vector3Int v = GetRoomCenterWorldPosition(room);
        return new Vector3Int(xPos + v.x, yPos + v.y, 0);
    }

    public static Vector3 GetWorldPosition(Vector3 obj, RoomObject room) {
        return new Vector3(obj.x + (width * room.x), obj.y + (height * room.y));
    }

    public static List<Vector2Int> GetTilemapDoorPosition(RoomObject room) {
        List<Vector2Int> doorsToDraw = new List<Vector2Int>();

        if (room.doorLayout.HasFlag(DoorLayout.Up)) {
            doorsToDraw.AddRange(doorTilemapPositions[DoorLayout.Up]);
        }
        if (room.doorLayout.HasFlag(DoorLayout.Down)) {
            doorsToDraw.AddRange(doorTilemapPositions[DoorLayout.Down]);
        }
        if (room.doorLayout.HasFlag(DoorLayout.Left)) {
            doorsToDraw.AddRange(doorTilemapPositions[DoorLayout.Left]);
        }
        if (room.doorLayout.HasFlag(DoorLayout.Right)) {
            doorsToDraw.AddRange(doorTilemapPositions[DoorLayout.Right]);
        }

        return doorsToDraw;
    }
}

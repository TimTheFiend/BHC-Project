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

    /// <summary>
    /// Gets the center position of a room.
    /// </summary>
    /// <param name="room">The room</param>
    /// <returns>The center position</returns>
    public static Vector3Int GetRoomCenterWorldPosition(RoomObject room) {
        return new Vector3Int(room.x * width, room.y * height, 0);
    }

    /// <summary>
    /// Gets the center position of a room.
    /// </summary>
    /// <param name="room">the RoomObject</param>
    /// <param name="xPos"></param>
    /// <param name="yPos"></param>
    /// <returns></returns>
    public static Vector3Int GetRoomCenterWorldPosition(RoomObject room, int xPos, int yPos) {
        Vector3Int v = GetRoomCenterWorldPosition(room);
        return new Vector3Int(xPos + v.x, yPos + v.y, 0);
    }

    /// <summary>
    /// Gets the world position of an object within a room.
    /// </summary>
    /// <param name="obj">The object's position within the room prefab.</param>
    /// <param name="room">The room in-which to spawn.</param>
    /// <returns></returns>
    public static Vector3 GetWorldPosition(Vector3 obj, RoomObject room) {
        return new Vector3(obj.x + (width * room.x), obj.y + (height * room.y));
    }

    /// <summary>
    /// DEPRECATED.
    /// Gets the positions of doors to draw into a tilemap.
    /// </summary>
    /// <param name="room">Room to draw in</param>
    /// <returns>List of door positions to draw.</returns>
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

    /// <summary>
    /// Lazy implementation of where the doors are supposed to be placed within a room.
    /// </summary>
    /// <param name="room">Room to place doors in</param>
    /// <returns>List of door positions to spawn.</returns>
    public static List<Vector2> GetDoorPositions(RoomObject room) {
        List<Vector2> doorsToDraw = new List<Vector2>();
        float vertical = 5.5f;
        float horizontal = 7.5f;

        if (room.doorLayout.HasFlag(DoorLayout.Up)) {
            doorsToDraw.Add(new Vector2(0f, vertical));
        }
        if (room.doorLayout.HasFlag(DoorLayout.Down)) {
            doorsToDraw.Add(new Vector2(0f, -vertical));
        }
        if (room.doorLayout.HasFlag(DoorLayout.Left)) {
            doorsToDraw.Add(new Vector2(-horizontal, 0f));
        }
        if (room.doorLayout.HasFlag(DoorLayout.Right)) {
            doorsToDraw.Add(new Vector2(horizontal, 0f));
        }

        return doorsToDraw;
    }

    /// <summary>
    /// Gets the direction of which room the player is moving into.
    /// </summary>
    /// <param name="player">Player's position</param>
    /// <param name="currentRoom">Room the player is currently in.</param>
    /// <returns>Cardinal direction of movement.</returns>
    public static Vector2 GetActivatedDoorDirection(Vector3 player, RoomObject currentRoom) {
        float height = CameraManager.instance.totalHeight;
        float width = CameraManager.instance.totalWidth;

        Vector3 relativePos = new Vector3(player.x - (width * currentRoom.x), player.y - (height * currentRoom.y));

        bool isHorizontal = Mathf.Abs(relativePos.x) > Mathf.Abs(relativePos.y);
        bool isPositive = isHorizontal ? relativePos.x > 0f : relativePos.y > 0f;

        if (isHorizontal) {
            return isPositive ? Vector2.right : Vector2.left;
        }
        return isPositive ? Vector2.up : Vector2.down;
    }
}
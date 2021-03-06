using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct RoomObject
{
    //private Vector2Int center;

    private Vector2Int _center;
    public DoorLayout doorLayout;
    public RoomType type;

    public int index;

    public Vector2Int center {
        get { return _center; }
        set { _center = value; }
    }

    public int x => center.x;
    public int y => center.y;

    #region Constructors

    public RoomObject(int x, int y) {
        _center = new Vector2Int(x, y);
        doorLayout = DoorLayout.None;
        type = RoomType.Normal;

        index = -1;
    }

    public RoomObject(int x, int y, int index) : this(x, y) {
    }

    public RoomObject(Vector2 position) : this((int)position.x, (int)position.y) {
    }

    #endregion Constructors

    /// <summary>
    /// Prints out the objects position, and the position of the doors
    /// </summary>
    /// <returns>debug message.</returns>
    public string DebugPrint() {
        return $"{this}\tDoors: {doorLayout}";
    }

    /// <summary>
    /// Adds, or removes, a flag from <see cref="doorLayout"/>
    /// </summary>
    /// <param name="door"><see cref="DoorLayout"/> value</param>
    /// <param name="state">if <c>true</c> it's added; otherwise it's removed.</param>
    public void SetDoor(DoorLayout door, bool state) {
        switch (state) {
            case true:
                doorLayout |= door;
                break;

            case false:
                doorLayout &= ~door;
                break;
        }
    }

    /// <summary>
    /// Lazy way to check if the object only has 1 (one) door.
    /// </summary>
    public bool IsDeadEnd {
        get {
            if (((int)doorLayout) == 1 || ((int)doorLayout) == 2 || ((int)doorLayout) == 4 || ((int)doorLayout) == 8) {
                return true;
            }
            return false;
        }
    }

    /// <summary>
    /// Sets the <see cref="DoorLayout"/> value on both the new <see cref="RoomObject"/>, and the parent.
    /// </summary>
    /// <param name="other">The new room.</param>
    /// <param name="oppositeDoor">The door opposite, I.E. the parent's <see cref="DoorLayout"/></param>
    /// <returns></returns>
    public DoorLayout GetDoorLayoutFromOffset(RoomObject other, out DoorLayout oppositeDoor) {
        Vector2Int roomOffset = (this - other).center;
        DoorLayout door = DoorLayout.None;

        if (roomOffset == Vector2Int.up)
            door = DoorLayout.Up;
        else if (roomOffset == Vector2Int.right)
            door = DoorLayout.Right;
        else if (roomOffset == Vector2Int.down)
            door = DoorLayout.Down;
        else if (roomOffset == Vector2Int.left)
            door = DoorLayout.Left;

        oppositeDoor = GetOppositeDoor(door);
        return door;
    }

    /// <summary>
    /// Returns the door opposite of the parameter
    /// </summary>
    /// <param name="door">The door that you want the opposite of</param>
    /// <returns>The door directly opposite.</returns>
    public DoorLayout GetOppositeDoor(DoorLayout door) {
        switch (door) {
            case DoorLayout.Up:
                return DoorLayout.Down;

            case DoorLayout.Right:
                return DoorLayout.Left;

            case DoorLayout.Down:
                return DoorLayout.Up;

            case DoorLayout.Left:
                return DoorLayout.Right;

            default:
                Debug.LogException(new System.Exception("GetOppositeDoor() This error shouldn't happen"));
                return DoorLayout.None;
        }
    }

    /// <summary>
    /// Gets the potential cardinally adjacent <see cref="RoomObject"/>.
    /// </summary>
    /// <returns>A carindally adjacent room.</returns>
    public IEnumerable<RoomObject> GetAdjacentRooms() {
        yield return new RoomObject(center.x + 1, center.y);
        yield return new RoomObject(center.x - 1, center.y);
        yield return new RoomObject(center.x, center.y + 1);
        yield return new RoomObject(center.x, center.y - 1);
    }

    /// <summary>
    /// Checks if object is inside a layout's bounds.
    /// </summary>
    /// <param name="width">The layout's width</param>
    /// <param name="height">The layout's height</param>
    /// <returns><c>true</c> if inside; otherwise <c>false</c>.</returns>
    public bool InsideBounds(int width, int height) {
        return x >= 0 && x <= width && y >= 0 && y <= height;
    }

    public override string ToString() {
        return $"({center.x}, {center.y})";
    }

    #region Static functions

    /// <summary>
    /// Generates a start room for the player to spawn in.
    /// </summary>
    /// <param name="width">The layout's width.</param>
    /// <param name="height">The layout's height.</param>
    /// <returns>The starting room <see cref="RoomObject"/></returns>
    public static RoomObject GetStartRoom(int width, int height) {
        if (Random.value > 0.5f) {
            return new RoomObject(Random.Range(2, width - 1), Random.value > 0.5f ? 0 : height);
        }
        return new RoomObject(Random.value > 0.5f ? 0 : width, Random.Range(2, height - 1));
    }

    /// <summary>
    /// Change to the behavior of `==` comparisons.
    /// </summary>
    /// <param name="room1">A <see cref="RoomObject"/></param>
    /// <param name="room2">A different <see cref="RoomObject"/></param>
    /// <returns><see langword="true"/> if they're the same; otherwise <see langword="false"/>.</returns>
    public static bool operator ==(RoomObject room1, RoomObject room2) {
        return room1.center == room2.center;
    }

    /// <summary>
    /// Change to the behavior of `!=` comparisons.
    /// </summary>
    /// <param name="room1">A <see cref="RoomObject"/></param>
    /// <param name="room2">A different <see cref="RoomObject"/></param>
    /// <returns><see langword="true"/> if they're not the same; otherwise <see langword="false"/>.</returns>
    public static bool operator !=(RoomObject room1, RoomObject room2) {
        return !(room1.center == room2.center);
    }

    public static RoomObject operator -(RoomObject r1, RoomObject r2) {
        return new RoomObject(r2.x - r1.x, r2.y - r1.y);
    }

    #endregion Static functions

    #region Only here to avoid "implementation warnings"

    public override bool Equals(object obj) {
        return obj is RoomObject @object &&
               center.Equals(@object.center);
    }

    public override int GetHashCode() {
        return 1242221860 + center.GetHashCode();
    }

    #endregion Only here to avoid "implementation warnings"
}

/// <summary>
/// Indicates which positions the door(s) are located.
/// </summary>
[System.Flags]
public enum DoorLayout
{
    None = 0,
    Up = 1,
    Right = 2,
    Down = 4,
    Left = 8
}

/// <summary>
/// Specifies which type of room the <see cref="RoomObject"/> is going to be.
/// </summary>
public enum RoomType
{
    Null,
    StartRoom,
    Normal,
    Boss,
    Item
}
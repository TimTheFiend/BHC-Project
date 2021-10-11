using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct RoomObject
{
    private Vector2Int center;

    public RoomObject(int x, int y) {
        center = new Vector2Int(x, y);
    }

    public IEnumerable<RoomObject> GetAdjacentRooms() {
        yield return new RoomObject(center.x + 1, center.y);
        yield return new RoomObject(center.x - 1, center.y);
        yield return new RoomObject(center.x, center.y + 1);
        yield return new RoomObject(center.x, center.y - 1);
    }

    public static bool operator ==(RoomObject room1, RoomObject room2) {
        return room1.center == room2.center;
    }

    public static bool operator !=(RoomObject room1, RoomObject room2) {
        return !(room1.center == room2.center);
    }

    #region stuff

    public override bool Equals(object obj) {
        return obj is RoomObject @object &&
               center.Equals(@object.center);
    }

    public override int GetHashCode() {
        return 1242221860 + center.GetHashCode();
    }

    #endregion stuff
}
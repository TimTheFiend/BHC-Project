using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CurrentDungeon
{
    //Keeping track of player.
    public static RoomObject playerPosition;

    public static Transform activePlayerRoom;

    /// <summary>
    /// The entire dungeon.
    /// </summary>
    public static List<RoomObject> dungeonFloor;

    /// <summary>
    /// The rooms that the player has already visited.
    /// </summary>
    public static List<RoomObject> exploredRooms;

    static CurrentDungeon() {
        dungeonFloor = new List<RoomObject>();
        exploredRooms = new List<RoomObject>();
    }

    public static void UpdatePlayerPosition(RoomObject newRoom) {
        playerPosition = newRoom;
        activePlayerRoom = GameObject.Find(newRoom.ToString()).transform;

        if (!exploredRooms.Contains(playerPosition)) {
            exploredRooms.Add(playerPosition);
        }
    }

    public static bool IsRoomCompleted() {
        Debug.Log(activePlayerRoom.childCount);
        if (activePlayerRoom.childCount >= 2) {
            return false;
        }
        Debug.Log("All enemies are dead");
        GameObject.Find("Player").GetComponent<PlayerController>().canUseDoors = true;
        return true;
    }
}
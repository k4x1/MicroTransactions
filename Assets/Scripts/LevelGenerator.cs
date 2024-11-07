using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public RoomSO[] rooms;
    public ColorToPrefab[] colorMappings;
    public int maxRooms = 5;
    public float scale = 1.0f;
    public RoomSO startingRoom;

    public List<Room> generatedRooms = new List<Room>();
    public Dictionary<float, Room> roomPositions = new Dictionary<float, Room>();

    void Start()
    {
        rooms = rooms.Length == 0 ? Resources.LoadAll<RoomSO>("Rooms") : rooms;
        if (startingRoom != null)
            GenerateInitialRoom();
    }

    void GenerateInitialRoom() => GenerateRoom(startingRoom, Vector3.zero, 0);

    public void GenerateRoom(RoomSO roomSO, Vector3 position, int rotation)
    {
        if (generatedRooms.Count >= maxRooms) return;

        GameObject roomObj = new GameObject($"Room_{generatedRooms.Count}");

        roomObj.transform.position = position;
        roomObj.transform.parent = transform;

        Room room = roomObj.AddComponent<Room>();
        room.baseRoom = roomSO;
        room.scale = scale;
        room.colorMappings = colorMappings;
        room.position = position.z;
        room.levelGenerator = this;
        room.rotation = rotation;

        room.Initialize();
        room.GenerateRoom();

        generatedRooms.Add(room);
        roomPositions[position.z] = room;
    }

    public bool FindRoomWithEntrance(Vector2 requiredEntranceDirection, out RoomSO suitableRoom, out int rotation)
    {
        foreach (RoomSO roomSO in rooms)
        {
            for (int rot = 0; rot < 360; rot += 90)
            {
                if (GetRotatedDirections(roomSO.GetDirections(), rot).Any(dir => Vector2.Dot(dir, requiredEntranceDirection) > 0.99f))
                {
                    suitableRoom = roomSO;
                    rotation = rot;
                    return true;
                }
            }
        }

        suitableRoom = null;
        rotation = 0;
        return false;
    }

    private List<Vector2> GetRotatedDirections(List<Vector2> directions, int rotationDegrees)
        => directions.Select(dir => RotateDirection(dir, rotationDegrees)).ToList();

    private Vector2 RotateDirection(Vector2 dir, int rotationDegrees)
    {   
        float rad = rotationDegrees * Mathf.Deg2Rad;
        return new Vector2(
            dir.x * Mathf.Cos(rad) - dir.y * Mathf.Sin(rad),
            dir.x * Mathf.Sin(rad) + dir.y * Mathf.Cos(rad)
        ).normalized;
    }
}
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public RoomSO[] rooms; // Array of room scriptable objects
    public ColorToPrefab[] colorMappings;
    public int maxRooms = 5; // Example maximum number of rooms
    [SerializeField] float scale = 1.0f;

    private List<Room> generatedRooms = new List<Room>();

    void Start()
    {
        GenerateLevel();
    }

    void GenerateLevel()
    {
        // Initialize the level generation
        Vector2 currentPos = Vector2.zero;

        // For simplicity, start with the first room
        RoomSO startingRoom = rooms[0];
        GenerateRoom(startingRoom, currentPos);

        // Generate additional rooms
        for (int i = 1; i < maxRooms; i++)
        {
            // For this example, pick a random room
            RoomSO nextRoom = rooms[Random.Range(0, rooms.Length)];

            // Get possible exit directions from the last room
            Room lastRoom = generatedRooms[generatedRooms.Count - 1];
            Vector2 exitDirection = lastRoom.GetExitDirection();

            // Calculate the position for the next room
            currentPos += exitDirection * lastRoom.size * scale;

            // Generate the room at the new position
            GenerateRoom(nextRoom, currentPos);
        }
    }

    void GenerateRoom(RoomSO roomSO, Vector2 position)
    {
        // Instantiate a new Room GameObject
        GameObject roomObj = new GameObject("Room");
        roomObj.transform.position = new Vector3(position.x, 0, position.y);
        roomObj.transform.parent = transform;

        // Add the Room component
        Room room = roomObj.AddComponent<Room>();
        room.baseRoom = roomSO;
        room.scale = scale;
        room.colorMappings = colorMappings;

        // Generate the room
        room.GenerateRoom();

        // Add to the list of generated rooms
        generatedRooms.Add(room);
    }
}

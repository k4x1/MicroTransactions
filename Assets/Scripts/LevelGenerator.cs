using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class LevelGenerator : MonoBehaviour
{
    public Texture2D[] roomTextures;
    public ColorToPrefab[] colorMappings;
    public int maxRooms = 5;
    public float scale = 1.0f;

    public List<Room> generatedRooms = new List<Room>();
    public Dictionary<float, Room> roomPositions = new Dictionary<float, Room>();

    void Start()
    {
        if (roomTextures.Length == 0)
            roomTextures = Resources.LoadAll<Texture2D>("Rooms").ToArray();

        GenerateInitialRoom();
    }

    void GenerateInitialRoom() => GenerateRoom(Vector3.zero);

    public void GenerateRoom(Vector3 position)
    {
        if (generatedRooms.Count >= maxRooms) return;

        GameObject roomObj = new GameObject($"Room_{generatedRooms.Count}");
        roomObj.transform.position = position;
        roomObj.transform.parent = transform;

        Room room = roomObj.AddComponent<Room>();
        room.roomTextures = roomTextures;
        room.scale = scale;
        room.colorMappings = colorMappings;
        room.position = position.z;
        room.levelGenerator = this;

        // Choose a random room texture
        Texture2D chosenTexture = roomTextures[Random.Range(0, roomTextures.Length)];
        room.InitializeWithTexture(chosenTexture);

        room.GenerateRoom();

        generatedRooms.Add(room);
        roomPositions[position.z] = room;
    }
}

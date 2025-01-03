/// <summary>
/// Bachelor of Software Engineering
/// Media Design School
/// Auckland
/// New Zealand
/// (c) 2024 Media Design School
/// File Name : LevelGenerator.cs
/// Description : This class implements procedural level generation for the game.
///               It manages the creation of rooms, handles room textures and color mappings,
///               and provides functionality for resetting and regenerating levels.
/// Author : Kazuo Reis de Andrade
/// </summary>
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
        {
            roomTextures = Resources.LoadAll<Texture2D>("Rooms").ToArray();
        }

        // Ensure ObjectPooler instance exists
        if (ObjectPooler.Instance == null)
        {
            GameObject poolerObject = new GameObject("ObjectPooler");
            poolerObject.AddComponent<ObjectPooler>();
        }

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

        room.StartRoomGeneration();

        generatedRooms.Add(room);
        roomPositions[position.z] = room;
    }

    public void ResetGeneratedRooms()
    {
        foreach (Room room in generatedRooms)
        {
            foreach (Transform child in room.transform)
            {
                ObjectPooler.Instance.ReturnToPool(child.gameObject);
            }
            ObjectPooler.Instance.ReturnToPool(room.gameObject);
        }
        generatedRooms.Clear();
        roomPositions.Clear();

        GenerateInitialRoom();
    }
}
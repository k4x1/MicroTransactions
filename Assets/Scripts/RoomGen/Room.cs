/// <summary>
/// Bachelor of Software Engineering
/// Media Design School
/// Auckland
/// New Zealand
/// (c) 2024 Media Design School
/// File Name : Room.cs
/// Description : This class represents a procedurally generated room in the game.
///               It handles room initialization, tile generation, player detection,
///               and adjacent room generation for infinite level design.
/// Author : Kazuo Reis de Andrade
/// </summary>
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Room : MonoBehaviour
{
    public Texture2D[] roomTextures;
    public Vector2 size;
    public float position;
    public ColorToPrefab[] colorMappings;
    public float scale = 1.0f;
    public LevelGenerator levelGenerator;

    private Texture2D currentMap;
    private Vector2 offset;
    private Vector3 roomCenter;
    private float detectionRadius = 8f;
    private bool playerInRoom = false;
    private bool generated = false;
    private bool entered = false;

    public void InitializeWithTexture(Texture2D texture)
    {
        if (texture == null)
            throw new System.ArgumentException("No room texture assigned");

        currentMap = texture;
        levelGenerator = FindObjectOfType<LevelGenerator>();
        offset = new Vector2(currentMap.width * 0.5f, currentMap.height * 0.5f) * scale;
        roomCenter = transform.position;
        detectionRadius = (currentMap.width / 2) * scale;
    }

    void Update()
    {
        bool currentPlayerInRoom = IsPlayerInRoom();
        if (currentPlayerInRoom != playerInRoom)
        {
            if (playerInRoom)
            {
                OnPlayerExit();
            }
            else
            {
                OnPlayerEnter();
            }
        }
        playerInRoom = currentPlayerInRoom;
    }

    private void OnPlayerEnter()
    {
        if (entered) return;
        GenerateAdjacentRooms();
        GameManager.Instance.RestartTimer();
        GameManager.Instance.AddPoints();
        entered = true;
    }

    private void OnPlayerExit() { }

    public bool IsPlayerInRoom()
    {
        Transform playerTransform = GameManager.Instance.playerRef.transform;
        float sqrDistance = (roomCenter - playerTransform.position).sqrMagnitude;
        return sqrDistance <= detectionRadius * detectionRadius;
    }

    public IEnumerator GenerateRoomCoroutine()
    {
        float startTime = Time.realtimeSinceStartup;
        if (currentMap == null || colorMappings == null)
            throw new System.ArgumentNullException();

        int width = currentMap.width;
        int height = currentMap.height;
        Color32[] pixels = currentMap.GetPixels32();
        int batchSize = 100; // Number of tiles to process per frame

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                GenerateTile(x, y, pixels[y * width + x]);

                // Yield control every batchSize tiles
                if ((y * width + x) % batchSize == 0)
                {
                    yield return null;
                }
            }
        }

        float endTime = Time.realtimeSinceStartup;
        float generationTime = endTime - startTime;

        Debug.Log($"Room generation completed in {generationTime:F4} seconds");
    }

    public void StartRoomGeneration()
    {
        StartCoroutine(GenerateRoomCoroutine());
    }

    private void GenerateTile(int x, int y, Color32 pixColor)
    {
        if (pixColor.a == 0) return;

        var mapping = colorMappings.FirstOrDefault(cm => cm.color.Equals(pixColor));
        if (mapping.prefab != null)
        {
            Vector3 position = transform.position + new Vector3((x * scale) - offset.x, 0, (y * scale) - offset.y);

            GameObject pooledObject = ObjectPooler.Instance.GetPooledObject(mapping.prefab);
            if (pooledObject != null)
            {
                pooledObject.transform.position = position;
                pooledObject.transform.rotation = Quaternion.identity;
                pooledObject.transform.SetParent(transform);
                pooledObject.transform.localScale *= scale;
            }
        }
    }

    private void GenerateAdjacentRooms()
    {
        if (levelGenerator.generatedRooms.Count >= levelGenerator.maxRooms) return;

        Vector3 newPosition = transform.position + new Vector3(0, 0, currentMap.height * scale);

        if (!levelGenerator.roomPositions.ContainsKey(newPosition.z))
        {
            levelGenerator.GenerateRoom(newPosition);
        }

        generated = true;

        if (levelGenerator.generatedRooms.Count > levelGenerator.maxRooms - 1)
        {
            Room firstRoom = levelGenerator.generatedRooms[0];
            levelGenerator.generatedRooms.RemoveAt(0);
            levelGenerator.roomPositions.Remove(firstRoom.position);

            // Instead of destroying, deactivate and return to pool
            foreach (Transform child in firstRoom.transform)
            {
                ObjectPooler.Instance.ReturnToPool(child.gameObject);
            }
            firstRoom.gameObject.SetActive(false);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(roomCenter, detectionRadius);
    }
}

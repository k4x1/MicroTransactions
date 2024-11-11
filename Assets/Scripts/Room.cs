using UnityEngine;
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
        return Vector3.Distance(roomCenter, playerTransform.position) <= detectionRadius;
    }

    public void GenerateRoom()
    {
        float startTime = Time.realtimeSinceStartup;
        if (currentMap == null || colorMappings == null)
            throw new System.ArgumentNullException();

        int width = currentMap.width;
        int height = currentMap.height;
        Color32[] pixels = currentMap.GetPixels32();

        List<(int x, int y, GameObject prefab)> tilesToInstantiate = new List<(int x, int y, GameObject prefab)>();

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Color32 pixColor = pixels[y * width + x];
                if (pixColor.a != 0)
                {
                    var mapping = colorMappings.FirstOrDefault(cm => cm.color.Equals(pixColor));
                    if (mapping.prefab != null)
                    {
                        tilesToInstantiate.Add((x, y, mapping.prefab));
                    }
                }
            }
        }

        BatchInstantiate(tilesToInstantiate);

        float endTime = Time.realtimeSinceStartup;
        float generationTime = endTime - startTime;

        Debug.Log($"Room generation completed in {generationTime:F4} seconds");
    }

    void BatchInstantiate(List<(int x, int y, GameObject prefab)> tiles)
    {
        foreach (var tile in tiles)
        {
            Vector3 position = transform.position + new Vector3((tile.x * scale) - offset.x, 0, (tile.y * scale) - offset.y);
            var inst = GetObjectFromPool(tile.prefab, position);
            inst.transform.localScale *= scale;
        }
    }

    GameObject GetObjectFromPool(GameObject prefab, Vector3 position)
    {
        var mapping = colorMappings.FirstOrDefault(cm => cm.prefab == prefab);
        if (mapping.pool != null)
        {
            GameObject obj = mapping.pool.GetObject();
            obj.transform.position = position;
            return obj;
        }
        else
        {
            return Instantiate(prefab, position, Quaternion.identity, transform);
        }
    }
    void GenerateTile(int x, int y, Color32 pixColor)
    {
        if (pixColor.a == 0) return;

        var mapping = colorMappings.FirstOrDefault(cm => cm.color.Equals(pixColor));
        if (mapping.prefab != null)
        {
            Vector3 position = transform.position + new Vector3((x * scale) - offset.x, 0, (y * scale) - offset.y);
            var inst = Instantiate(mapping.prefab, position, Quaternion.identity, transform);
            inst.transform.localScale *= scale;
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
            Destroy(firstRoom.gameObject);
        }
    }
    public void CleanUp()
    {
        foreach (var mapping in colorMappings)
        {
            if (mapping.pool != null)
            {
                mapping.pool.ReturnAllObjects();
            }
        }
    }

    private void OnDestroy()
    {
        CleanUp();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(roomCenter, detectionRadius);
    }
}

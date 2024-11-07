using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Timeline;

public enum Direction { UP, DOWN, LEFT, RIGHT }

[CreateAssetMenu(fileName = "NewRoom", menuName = "ScriptableObjects/Room")]
public class RoomSO : ScriptableObject
{
    public Texture2D layout;
    public Direction[] directions;

    public List<Vector2> GetDirections()
    {
        return directions.Select(dir => dir switch
        {
            Direction.UP => Vector2.up,
            Direction.DOWN => Vector2.down,
            Direction.LEFT => Vector2.left,
            Direction.RIGHT => Vector2.right,
            _ => Vector2.zero
        }).ToList();
    }
}


public class Room : MonoBehaviour
{
    public RoomSO baseRoom;
    public Vector2 size;
    public float position;
    public ColorToPrefab[] colorMappings;
    public float scale = 1.0f;
    public LevelGenerator levelGenerator;
    public int rotation;

    private Texture2D map;
    private Vector2 offset;
    private Vector3 roomCenter;
    private const float detectionRadius = 8f;
    private bool playerInRoom = false;
    private bool generated = false;
    private bool playerEnteredRoom = false;
    public void Initialize()
    {
        map = baseRoom.layout ?? throw new System.ArgumentNullException(nameof(baseRoom));
        levelGenerator = FindObjectOfType<LevelGenerator>();
        offset = new Vector2(map.width * 0.5f, map.height * 0.5f) * scale;
        roomCenter = transform.position;
        transform.rotation = Quaternion.Euler(0, rotation, 0);
        
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
    protected virtual void OnPlayerEnter() => GenerateAdjacentRooms();

    protected virtual void OnPlayerExit() { }

    public bool IsPlayerInRoom()
    {
        Transform playerTransform = GameManager.Instance.playerRef.transform;
        return Vector3.Distance(roomCenter, playerTransform.position) <= detectionRadius;
    }

    public void GenerateRoom()
    {
        if (map == null || baseRoom == null || colorMappings == null)
            throw new System.ArgumentNullException();

        for (int x = 0; x < map.width; x++)
        {
            for (int y = 0; y < map.height; y++)
            {
                GenerateTile(x, y);
            }
        }
    }

    void GenerateTile(int x, int y)
    {
        Color pixColor = map.GetPixel(x, y);
        if (pixColor.a == 0) return;

        var mapping = colorMappings.FirstOrDefault(cm => cm.color.Equals(pixColor));
        if (mapping.prefab != null)
        {
            Vector2 rotatedPos = RotatePoint(new Vector2(x, y), rotation, map.width, map.height);
            Vector3 position = transform.position + new Vector3((rotatedPos.x * scale) - offset.x, 0, (rotatedPos.y * scale) - offset.y);
            var inst = Instantiate(mapping.prefab, position, Quaternion.Euler(0, rotation, 0), transform);
            inst.transform.localScale *= scale;
        }
    }

    private Vector2 RotatePoint(Vector2 point, int rotationDegrees, int width, int height)
    {
        Vector2 center = new Vector2(width / 2f, height / 2f);
        Vector2 dir = point - center;
        float rad = rotationDegrees * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);
        Vector2 rotatedDir = new Vector2(
            dir.x * cos - dir.y * sin,
            dir.x * sin + dir.y * cos
        );
        return center + rotatedDir;
    }
    private void GenerateAdjacentRooms()
    {
        if (levelGenerator.generatedRooms.Count >= levelGenerator.maxRooms) return;

        foreach (Vector2 exitDirection in GetExitDirections())
        {
            // Only generate rooms in the upward direction
            if (exitDirection != Vector2.up) continue;

            Vector3 newPosition = transform.position + new Vector3(exitDirection.x * map.width * scale, 0, exitDirection.y * map.height * scale);

            // Calculate the center of the new room
            Vector3 newRoomCenter = newPosition;

            if (!levelGenerator.roomPositions.ContainsKey(newRoomCenter.z))
            {
                RoomSO nextRoomSO;
                int roomRotation;
                if (levelGenerator.FindRoomWithEntrance(-exitDirection, out nextRoomSO, out roomRotation))
                {
                    levelGenerator.GenerateRoom(nextRoomSO, newRoomCenter, roomRotation);
                }
            }
        }
        generated = true;

        if (levelGenerator.generatedRooms.Count > levelGenerator.maxRooms-1)
        {
            Room firstRoom = levelGenerator.generatedRooms[0];
            levelGenerator.generatedRooms.RemoveAt(0);
            levelGenerator.roomPositions.Remove(firstRoom.position);
            Destroy(firstRoom.gameObject);
        }
    }


    public List<Vector2> GetExitDirections()
    {
        return baseRoom.GetDirections().Select(dir => RotateDirection(dir, rotation)).ToList();
    }

    private Vector2 RotateDirection(Vector2 dir, int rotationDegrees)
    {
        float rad = rotationDegrees * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);
        return new Vector2(
            dir.x * cos - dir.y * sin,
            dir.x * sin + dir.y * cos
        ).normalized;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(roomCenter, detectionRadius);
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum Direction
{
    UP, DOWN, LEFT, RIGHT
}
[CreateAssetMenu(fileName = "NewRoom", menuName = "ScriptableObjects/Room", order = 1)]
public class RoomSO : ScriptableObject 
{
    public Texture2D layout;
    public Direction[] directions;

    public List<Vector2> GetDirection()
    {
        List<Vector2> vecDirection = new List<Vector2>();
        foreach (Direction dir in directions)
        {
            switch (dir)
            {
                case Direction.UP:
                    vecDirection.Add(new Vector2(0, 1));
                    break;
                case Direction.DOWN:
                    vecDirection.Add(new Vector2(0, -1));
                    break;
                case Direction.LEFT:
                    vecDirection.Add(new Vector2(-1, 0));
                    break;
                case Direction.RIGHT:
                    vecDirection.Add(new Vector2(1, 0));
                    break;
                default:
                    break;
            }
        }
        return vecDirection;
    }

}

public class Room : MonoBehaviour
{
    public RoomSO baseRoom;
    public Vector2 size;
    private Vector2 position;
    private ColorToPrefab[] colorMappings;
    private bool PlayerInRoom;
    [SerializeField] float scale = 1.0f;
    Vector2 offset = new Vector2(16, 16);
    Texture2D map;

    void Start()
    {
        // Initialize the room's position
        position = new Vector2(transform.position.x, transform.position.z);
        colorMappings = GameManager.Instance.colorMappings;
        map = baseRoom.layout;
    }

    public bool GetPlayerInRoom()
    {
        if (GameManager.Instance.playerRef == null) return false;

        Vector2 playerPosition = new Vector2(GameManager.Instance.playerRef.transform.position.x, GameManager.Instance.playerRef.transform.position.z);

        return IsPointInRoom(playerPosition);
    }

    private bool IsPointInRoom(Vector2 point)
    {
        float halfWidth = size.x / 2f;
        float halfHeight = size.y / 2f;

        return (point.x >= position.x - halfWidth && point.x <= position.x + halfWidth &&
                point.y >= position.y - halfHeight && point.y <= position.y + halfHeight);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerInRoom = true;
            OnPlayerEnter();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerInRoom = false;
            OnPlayerExit();
        }
    }

    protected virtual void OnPlayerEnter() { }
    protected virtual void OnPlayerExit() { }

    void GenerateLevel()
    {
        offset = new Vector2(map.width / 2, map.height / 2);
        offset *= scale;
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

        if (pixColor.a == 0)
        {
            return;
            //blank pixel
        }

        foreach (ColorToPrefab colorMapping in colorMappings)
        {
            if (colorMapping.color.Equals(pixColor))
            {
                Vector3 position = new Vector3((x * scale) - offset.x, 0, (y * scale) - offset.y);
                var inst = Instantiate(colorMapping.prefab, position, Quaternion.identity, transform);
                inst.transform.localScale *= scale;
            }
        }


    }
}

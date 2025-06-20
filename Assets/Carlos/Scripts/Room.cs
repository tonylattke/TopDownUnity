using UnityEngine;

public class Room : MonoBehaviour
{

    [SerializeField] private GameObject topDoor, bottomDoor, leftDoor, rightDoor;

    public Vector2Int RoomIndex { get; set; }

    private Door[] doors;

    private void Awake()
    {
        doors = GetComponentsInChildren<Door>();
    }

    public Door GetDoor(Vector2Int dir)
    {
        foreach (Door door in doors)
        {
            if (door.Direction == dir)
                return door;
        }
        return null;
    }

    public void OpenDoor(Vector2Int direction)
    {
        if (direction == Vector2Int.up)
        {
            topDoor.SetActive(true);
        }
        else if (direction == Vector2Int.down)
        {
            bottomDoor.SetActive(true);
        }
        else if (direction == Vector2Int.left)
        {
            leftDoor.SetActive(true);
        }
        else if (direction == Vector2Int.right)
        {
            rightDoor.SetActive(true);
        }
    }

}

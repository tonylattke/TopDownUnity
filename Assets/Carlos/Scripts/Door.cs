using UnityEngine;

public class Door : MonoBehaviour, Interactuable
{
    [SerializeField] private Room thisRoom, nextRoom;
    public Room ThisRoom { get => thisRoom; set => thisRoom = value; }
    public Room NextRoom { get => nextRoom; set => nextRoom = value; }

    [SerializeField] private Vector2Int direction;
    public Vector2Int Direction { get => direction; set => direction = value; }

    public void Interactuar()
    {
        Debug.Log("Interacting with door");

        // Buscar la puerta opuesta en la habitación conectada

        if (nextRoom == null)
        {
            Debug.Log("Next room is not set");
            return;
        }
        if (direction == null)
        {
            Debug.Log("Direction is not set");
            return;
        }

        Door oppositeDoor = nextRoom.GetDoor(-direction);
        if (oppositeDoor == null)
        {
            Debug.Log("No opposite door found");
            return;
        }

        // Mover al jugador a esa puerta
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.Log("Player not found");
            return;
        }
        if (player != null)
        {
            Debug.Log("Player found: " + player.name);
            // para estar justo delante
            Vector3 offset = new Vector3(-direction.x, -direction.y, 0f);
            offset *= 1.1f; // esto es para evitar que choque... ajustarlo

            CharacterController cc = player.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;
            player.transform.position = oppositeDoor.transform.position + offset;
            if (cc != null) cc.enabled = true;

            //player.transform.position = oppositeDoor.transform.position + offset;
            Debug.Log("Teleporting player to: " + (oppositeDoor.transform.position + offset));
        }
    }
}

using System.Collections.Generic;
using Unity.Multiplayer.Center.Common;
using UnityEngine;

public class  RoomManager : MonoBehaviour
{

    [SerializeField] private GameObject roomPrefab;
    
    // numero máximo y mínimo de habitaciones
    [SerializeField] private int maxRooms = 15;
    [SerializeField] private int minRooms = 10;

    // tamaño de las habitaciones
    private int roomWidht = 19;
    private int roomHeight = 11;

    // tamaño del grid
    [SerializeField] private int gridSizeX = 10;
    [SerializeField] private int gridSizeY = 10;
    
    // lista de habitaciones generadas
    private List<GameObject> roomObjects = new List<GameObject>();

    // cola de habitaciones a generar
    private Queue<Vector2Int> roomQueue = new Queue<Vector2Int>();

    // grid de habitaciones, 
    // indica con 0 si la celda está vacía 
    // y con 1 si está ocupada por una habitación
    private int[,] roomGrid;

    // contador de habitaciones generadas
    private int roomCount;
    private bool generationComplete = false;
    


    private void Start()
    {
        roomGrid = new int[gridSizeX, gridSizeY];
        roomQueue = new Queue<Vector2Int>();

        // inciar la generación en mitad del grid
        Vector2Int initialRoomIndex = new Vector2Int(gridSizeX / 2, gridSizeX / 2);
        StartRoomGenerationFromRoom(initialRoomIndex);
    }

    private void Update()
    {
        // mientras haya habitaciones en la cola y no se haya alcanzado el máximo de habitaciones
        if (roomQueue.Count > 0 && roomCount < maxRooms && !generationComplete)
        {
            Vector2Int roomIndex = roomQueue.Dequeue();
            int gridX = roomIndex.x;
            int gridY = roomIndex.y;

            TryGenerateRoom(new Vector2Int(gridX - 1, gridY));
            TryGenerateRoom(new Vector2Int(gridX + 1, gridY));
            TryGenerateRoom(new Vector2Int(gridX, gridY + 1));
            TryGenerateRoom(new Vector2Int(gridX, gridY - 1));
        }
        else if (roomCount < minRooms && !generationComplete)
        {
            // si no se ha alcanzado el mínimo de habitaciones, regenerar
            Debug.Log("Regenerating rooms...");
            RegenerateRooms();
        }
        else if (!generationComplete)
        {
            // abrir puertas de todas las habitaciones generadas
            OpenDoorsForAllRooms();

            Debug.Log("Generation complete");
            generationComplete = true;
        }


    }

    private void RegenerateRooms()
    {
        // limpiar las habitaciones generadas
        roomObjects.ForEach(Destroy);
        roomObjects.Clear();
        roomQueue.Clear();
        roomCount = 0;
        generationComplete = false;

        // reiniciar el grid
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                roomGrid[x, y] = 0;
            }
        }

        Vector2Int initialRoomIndex = new Vector2Int(gridSizeX / 2, gridSizeY / 2);
        StartRoomGenerationFromRoom(initialRoomIndex);
    }

    // inicia la generación de habitaciones desde una habitación inicial
    private void StartRoomGenerationFromRoom(Vector2Int roomIndex)
    {
        roomQueue.Enqueue(roomIndex);
        int x = roomIndex.x;
        int y = roomIndex.y;
        roomGrid[x, y] = 1;
        roomCount++;
        GameObject initialRoom = Instantiate(roomPrefab, GetPositionFromGridIndex(roomIndex), Quaternion.identity);
        initialRoom.name = "Room-" + roomCount;
        initialRoom.GetComponent<Room>().RoomIndex = roomIndex;
        roomObjects.Add(initialRoom);
    }

    // intenta generar una habitación en la posición indicada
    private bool TryGenerateRoom(Vector2Int roomIndex)
    {
        int x = roomIndex.x;
        int y = roomIndex.y;


        // Evitar índices fuera del grid
        if (x < 0 || x >= gridSizeX || y < 0 || y >= gridSizeY) return false;

        // Evitar generar en una celda ya ocupada
        if (roomGrid[x, y] == 1) return false;

        // salir si se llegó al maximo de habitaciones
        if (roomCount >= maxRooms) return false;

        // damos 50% de probabilidad de no generar una habitación
        if (Random.value < 0.5f && roomIndex != Vector2Int.zero) return false;

        // evitar generar habitaciones adyacentes a más de una habitación
        if (CountAdjacentRooms(roomIndex) > 1) return false;

        // marcar la celda como ocupada y generar la habitacion
        roomQueue.Enqueue(roomIndex);
        roomGrid[x, y] = 1;
        roomCount++;

        GameObject newRoom = Instantiate(roomPrefab, GetPositionFromGridIndex(roomIndex), Quaternion.identity);
        newRoom.name = "Room-" + roomCount;
        newRoom.GetComponent<Room>().RoomIndex = roomIndex;
        roomObjects.Add(newRoom);

        return true;
    }

    // abre las puertas de todas las habitaciones generadas
    private void OpenDoorsForAllRooms()
    {
        foreach (GameObject room in roomObjects)
        {
            Room roomComponent = room.GetComponent<Room>();
            Vector2Int roomIndex = roomComponent.RoomIndex;
            OpenDoors(room, roomIndex.x, roomIndex.y);
        }
    }

    // abre las puertas de una habitación según las habitaciones adyacentes
    private void OpenDoors(GameObject room, int x, int y)
    {
        Room currentRoom = room.GetComponent<Room>();
        Vector2Int currentIndex = new Vector2Int(x, y);

        // Para cada dirección, si hay una habitación adyacente, abre puerta y asigna referencias
        TryOpenAndLinkDoor(currentRoom, currentIndex, Vector2Int.left);
        TryOpenAndLinkDoor(currentRoom, currentIndex, Vector2Int.right);
        TryOpenAndLinkDoor(currentRoom, currentIndex, Vector2Int.up);
        TryOpenAndLinkDoor(currentRoom, currentIndex, Vector2Int.down);
    }

    // intenta abrir y vincular una puerta entre la habitación actual y la vecina
    private void TryOpenAndLinkDoor(Room currentRoom, Vector2Int currentIndex, Vector2Int direction)
    {
        Vector2Int neighborIndex = currentIndex + direction;

        // Validar que esté dentro del grid
        if (neighborIndex.x < 0 || neighborIndex.x >= gridSizeX || neighborIndex.y < 0 || neighborIndex.y >= gridSizeY)
            return;

        // Verificar si hay habitación vecina
        if (roomGrid[neighborIndex.x, neighborIndex.y] != 1)
            return;

        // Obtener la habitación vecina
        GameObject neighborRoomObj = roomObjects.Find(r => r.GetComponent<Room>().RoomIndex == neighborIndex);
        if (neighborRoomObj == null) return;

        Room neighborRoom = neighborRoomObj.GetComponent<Room>();

        // Abrir puertas en ambas habitaciones
        currentRoom.OpenDoor(direction);
        neighborRoom.OpenDoor(-direction);

        // Obtener las puertas y asignar referencias
        Door doorFromCurrent = currentRoom.GetDoor(direction);
        Door doorFromNeighbor = neighborRoom.GetDoor(-direction);

        if (doorFromCurrent != null)
        {
            doorFromCurrent.ThisRoom = currentRoom;
            doorFromCurrent.NextRoom = neighborRoom;
            doorFromCurrent.Direction = direction;
        }

        if (doorFromNeighbor != null)
        {
            doorFromNeighbor.ThisRoom = neighborRoom;
            doorFromNeighbor.NextRoom = currentRoom;
            doorFromCurrent.Direction = -direction;


        }

/*
        Debug.Log($"Door from {currentRoom.name} to {neighborRoom.name} assigned.");
Debug.Log($"doorFromCurrent.NextRoom: {doorFromCurrent.NextRoom}");
            Debug.Log($"doorFromNeighbor.NextRoom: {doorFromNeighbor.NextRoom}");
*/
    }



    // ------------------
    // Métodos auxiliares
    // ------------------

    // cuenta las habitaciones adyacentes
    private int CountAdjacentRooms(Vector2Int roomIndex)
    {
        int x = roomIndex.x;
        int y = roomIndex.y;
        int count = 0;

        // Evitar índices fuera del grid
        if (x < 0 || x >= gridSizeX || y < 0 || y >= gridSizeY) return 0;

        if (x > 0 && roomGrid[x - 1, y] == 1) count++; // izquierda
        if (x < gridSizeX - 1 && roomGrid[x + 1, y] == 1) count++; // derecha
        if (y > 0 && roomGrid[x, y - 1] == 1) count++; // abajo
        if (y < gridSizeY - 1 && roomGrid[x, y + 1] == 1) count++; // arriba

        return count;
    }


    // convierte un índice de grid a una posición en el mundo
    private Vector3 GetPositionFromGridIndex(Vector2Int gridIndex)
    {
        int gridX = gridIndex.x;
        int gridY = gridIndex.y;
        return new Vector3(roomWidht * (gridX - gridSizeX / 2), roomHeight * (gridY - gridSizeY / 2));
    }

    private void OnDrawGizmos()
    {
        Color gizmoColor = new Color(0, 1, 1, 0.05f);
        Gizmos.color = gizmoColor;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
             Vector3 position = GetPositionFromGridIndex(new Vector2Int(x, y));   
             Gizmos.DrawWireCube(position, new Vector3(roomWidht, roomHeight, 1));
            }
        }
    }
}

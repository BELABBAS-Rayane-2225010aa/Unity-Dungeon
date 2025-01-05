using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public class Cell 
    {
        public bool[] status = new bool[4];
        public bool visited = false;
    }

    public struct Edge
    {
        public Vector2Int from;
        public Vector2Int to;

        public Edge(Vector2Int from, Vector2Int to)
        {
            this.from = from;
            this.to = to;
        }
    }

    [System.Serializable]
    public class Rule 
    {
        public GameObject room;
        public Vector2Int minPosition;
        public Vector2Int maxPosition;
        public bool[] EntranceExistence = new bool[4];
        public bool isCorridor = false;
    }

    public GameObject weapon;
    public GameObject trophy;
    public GameObject teleporterPrefab;
    public GameObject enemy;
    public GameObject Boss;
    public GameObject hole;
    public GameObject chest;

    public Vector2Int size;
    public Vector3Int startVectorPos;
    public Vector2Int startPos = new Vector2Int(0, 0);
    public Vector2 offset;
    public Rule[] rooms;

    List<List<Cell>> board;
    RoomBehavior[,] roomGrid;

    // Start is called before the first frame update
    void Start(){
        GridGenerator();
        GridGenerator();
        GenerateDungeon();
        MarkLastCell();
        PopulateCells(weapon, trophy, teleporterPrefab, enemy, Boss, hole, chest, gameObject.name);
    }

    void GridGenerator() {
        board = new List<List<Cell>>();
        roomGrid = new RoomBehavior[size.x, size.y];

        for (int i = 0; i < size.x; i++) {
            board.Add(new List<Cell>());

            for (int j = 0; j < size.y; j++) {
                Cell newCell = new Cell();
                board[i].Add(newCell);
            }
        }
    }

    void GenerateDungeon() {
        // Generation of the blank dungeon
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                Cell currentCell = board[i][j];
                var roomPrefab = rooms[0].room;
                RoomBehavior newRoom = Instantiate(roomPrefab, 
                    new Vector3(j * offset.x + startVectorPos.x, startVectorPos.y, -i * offset.y + startVectorPos.z), 
                    roomPrefab.transform.rotation, transform).GetComponent<RoomBehavior>();
                newRoom.name = "Room" + i + "|" + j;
                roomGrid[i, j] = newRoom;
            }
        }

        // Call Prim's algorithm to generate the maze
        GenerateMaze();

        // Replace rooms based on EntranceExistence
        ReplaceRooms();
    }

    void GenerateMaze()
    {
        List<Edge> edges = new List<Edge>();
        HashSet<Vector2Int> visitedCells = new HashSet<Vector2Int>();

        Vector2Int startCell = new Vector2Int(Random.Range(0, size.x), Random.Range(0, size.y));
        visitedCells.Add(startCell);
        AddEdges(startCell, edges);

        while (edges.Count > 0)
        {
            Edge edge = edges[Random.Range(0, edges.Count)];
            edges.Remove(edge);

            if (!visitedCells.Contains(edge.to))
            {
                visitedCells.Add(edge.to);
                OpenDoor(edge.from, edge.to);
                AddEdges(edge.to, edges);
            }
        }

        // Update the visual representation of the dungeon
        UpdateDoors();
    }

    void AddEdges(Vector2Int cell, List<Edge> edges)
    {
        int x = cell.x;
        int y = cell.y;

        if (x > 0) edges.Add(new Edge(cell, new Vector2Int(x - 1, y))); // Left
        if (x < size.x - 1) edges.Add(new Edge(cell, new Vector2Int(x + 1, y))); // Right
        if (y > 0) edges.Add(new Edge(cell, new Vector2Int(x, y - 1))); // Down
        if (y < size.y - 1) edges.Add(new Edge(cell, new Vector2Int(x, y + 1))); // Up
    }

    void OpenDoor(Vector2Int from, Vector2Int to)
    {
        if (from.x == to.x)
        {
            if (from.y > to.y)
            {
                board[from.x][from.y].status[3] = true; // Open door to the left
                board[to.x][to.y].status[2] = true; // Open door to the right
            }
            else
            {
                board[from.x][from.y].status[2] = true; // Open door to the right
                board[to.x][to.y].status[3] = true; // Open door to the left
            }
        }
        else
        {
            if (from.x > to.x)
            {
                board[from.x][from.y].status[0] = true; // Open door to the top
                board[to.x][to.y].status[1] = true; // Open door to the bottom
            }
            else
            {
                board[from.x][from.y].status[1] = true; // Open door to the bottom
                board[to.x][to.y].status[0] = true; // Open door to the top
            }
        }
    }

    void UpdateDoors() {
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                Cell currentCell = board[i][j];
                UpdateRoom(currentCell, i, j);
            }
        }
    }

    void UpdateRoom(Cell cell, int i, int j){
        // Conversion du tableau de booléens en chaîne de caractères
        string statusValues = string.Join(", ", cell.status.Select(b => b.ToString()).ToArray());

        // Mettre à jour le visuel de la salle
        RoomBehavior room = roomGrid[i, j];
        room.UpdateRoom(cell.status);
    }

    void ReplaceRooms() {
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                Cell currentCell = board[i][j];
                GameObject roomPrefab = SelectRoomPrefab(currentCell.status);
                Vector3 position = new Vector3(j * offset.x + startVectorPos.x, startVectorPos.y, -i * offset.y + startVectorPos.z);
                Quaternion rotation = roomPrefab.transform.rotation;

                // Détruire l'ancienne salle
                Destroy(roomGrid[i, j].gameObject);

                // Instancier la nouvelle salle
                RoomBehavior newRoom = Instantiate(roomPrefab, position, rotation, transform).GetComponent<RoomBehavior>();
                newRoom.name = "Room" + i + "|" + j;
                newRoom.isCorridor = rooms.First(r => r.room == roomPrefab).isCorridor;
                roomGrid[i, j] = newRoom;

                // Mettre à jour le visuel de la salle
                newRoom.UpdateRoom(currentCell.status);
            }
        }
    }

    GameObject SelectRoomPrefab(bool[] status)
    {
        List<Rule> validRooms = new List<Rule>();

        foreach (var room in rooms)
        {
            bool isValid = true;
            for (int i = 0; i < 4; i++)
            {
                if (room.EntranceExistence[i] != status[i])
                {
                    isValid = false;
                    break;
                }
            }
            if (isValid)
            {
                validRooms.Add(room);
            }
        }

        if (validRooms.Count == 0)
        {
            return rooms[0].room; // Default room if no match found
        }

        return validRooms[Random.Range(0, validRooms.Count)].room;
    }

    void MarkLastCell()
    {
        // Mark the last room in the grid as the last cell
        RoomBehavior lastRoom = roomGrid[size.x - 1, size.y - 1];

        lastRoom.UpdateLastCell(true);
    }

    void PopulateCells(GameObject weapon, GameObject trophy, GameObject teleporterPrefab, GameObject Ennemy, GameObject Boss, GameObject Hole, GameObject Chest, string gameObjectName)
    {
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                RoomBehavior currentRoom = roomGrid[i, j];
                currentRoom.PopulateRoom(weapon, trophy, teleporterPrefab, Ennemy, Boss, Hole, Chest, gameObjectName);
            }
        }
    }
}
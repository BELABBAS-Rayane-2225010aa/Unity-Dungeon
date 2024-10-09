using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public class Cell 
    {
        public bool visited = false;
        public bool[] status = new bool[4];
    }

    [System.Serializable]
    public class Rule 
    {
        public GameObject room;
        public Vector2Int minPosition;
        public Vector2Int maxPosition;

        public bool[] EntranceExistence = new bool[4];
    }

    public Vector2Int size;
    public Vector3Int startVectorPos;
    public int startPos = 0;
    public Vector2 offset;
    public Rule[] rooms;

    List<Cell> board;

    // Start is called before the first frame update
    void Start()
    {
        MazeGenerator();
        GenerateDungeon();
    }

    void MazeGenerator()
    {
        board = new List<Cell>();

        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                board.Add(new Cell());
            }
        }

        int currentCell = startPos;
        Stack<int> path = new Stack<int>();
        int k = 0;

        while (k < 20)
        {
            k++;
            board[currentCell].visited = true;

            if (currentCell == board.Count - 1)
            {
                break;
            }

            // Check neighbors
            List<int> neighbors = CheckNeighbors(currentCell);

            if (neighbors.Count == 0)
            {
                if (path.Count == 0)
                {
                    break;
                }
                else
                {
                    currentCell = path.Pop();
                }
            }
            else
            {
                path.Push(currentCell);
                int newCell = neighbors[Random.Range(0, neighbors.Count)];

                // down or right
                if (newCell > currentCell)
                {
                    // right
                    if (newCell - 1 == currentCell)
                    {
                        board[currentCell].status[2] = true;
                        currentCell = newCell;
                        board[currentCell].status[3] = true;
                    }
                    // down
                    else
                    {
                        board[currentCell].status[1] = true;
                        currentCell = newCell;
                        board[currentCell].status[0] = true;
                    }
                }
                // up or left
                else
                {
                    // left
                    if (newCell + 1 == currentCell)
                    {
                        board[currentCell].status[3] = true;
                        currentCell = newCell;
                        board[currentCell].status[2] = true;
                    }
                    // up
                    else
                    {
                        board[currentCell].status[0] = true;
                        currentCell = newCell;
                        board[currentCell].status[1] = true;
                    }
                }
            }
        }
    }

    void GenerateDungeon()
    {
        // Initialiser la première cellule avec la salle pleine (position 0 de Rule[] rooms)
        Cell firstCell = board[0];
        firstCell.visited = true;
        var firstRoomPrefab = rooms[0].room;

        var firstRoom = Instantiate(firstRoomPrefab, startVectorPos, firstRoomPrefab.transform.rotation, transform).GetComponent<RoomBehavior>();
        firstRoom.UpdateRoom(firstCell.status);

        firstRoom.name = "Room 0, 0";

        // Générer le reste du donjon
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                if (i == 0 && j == 0) continue; // Skip the first cell

                Cell currentCell = board[i + j * size.x];
                if (currentCell.visited)
                {
                    List<Rule> availableRooms = new List<Rule>();
                    Rule corridorRoom = null;

                    foreach (var room in rooms)
                    {
                        bool match = true;
                        for (int k = 0; k < currentCell.status.Length; k++)
                        {
                            if (currentCell.status[k] && !room.EntranceExistence[k])
                            {
                                match = false;
                                break;
                            }
                        }
                        if (match)
                        {
                            // Vérifier si c'est une version de couloir
                            if ((currentCell.status[0] && currentCell.status[1]) || (currentCell.status[2] && currentCell.status[3]))
                            {
                                corridorRoom = room;
                            }
                            else
                            {
                                availableRooms.Add(room);
                            }
                        }
                    }

                    Rule selectedRoom = null;
                    if (corridorRoom != null)
                    {
                        selectedRoom = corridorRoom; // Prioriser la version de couloir
                    }
                    else if (availableRooms.Count > 0)
                    {
                        selectedRoom = availableRooms[Random.Range(0, availableRooms.Count)];
                    }

                    if (selectedRoom != null)
                    {
                        var roomPrefab = selectedRoom.room;

                        var newRoom = Instantiate(roomPrefab, new Vector3(i * offset.x + startVectorPos.x, startVectorPos.y, -j * offset.y + startVectorPos.z), roomPrefab.transform.rotation, transform).GetComponent<RoomBehavior>();
                        newRoom.UpdateRoom(currentCell.status);

                        newRoom.name = "Room " + roomPrefab.name;
                    }
                }
            }
        }
    }

    List<int> CheckNeighbors(int cell)
    {
        List<int> neighbors = new List<int>();

        //check up
        if(cell - size.x >= 0 && !board[cell - size.x].visited)
        {
            neighbors.Add(cell - size.x);
        }

        //check down
        if(cell + size.x < board.Count && !board[cell + size.x].visited)
        {
            neighbors.Add(cell + size.x);
        }

        //check right
        if((cell + 1) % size.x != 0 && !board[cell + 1].visited)
        {
            neighbors.Add(cell + 1);
        }

        //check left
        if(cell % size.x != 0 && !board[cell - 1].visited)
        {
            neighbors.Add(cell - 1);
        }

        return neighbors;
    }
}
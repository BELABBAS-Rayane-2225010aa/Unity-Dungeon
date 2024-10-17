using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public Vector2Int startPos = new Vector2Int(0,0);
    public Vector2 offset;
    public float noise = 0.5f;
    public Rule[] rooms;

    List<List<Cell>> board;
    RoomBehavior[,] roomGrid;

    // Start is called before the first frame update
    void Start()
    {
        /*
        To generate the dungeon we uses a homebrew random dungeon generator
        First we generate a grid of Cell of the size of the dungeon
        Then we generate a dungeon that fullfil the grid with the full room
        Then we choose the last room
        Finnaly we generate the doors and a path to the last room
        */
        GridGenerator();
        GenerateDungeon();
    }

    void GridGenerator(){
        board = new List<List<Cell>>();
        roomGrid = new RoomBehavior[size.x, size.y];

        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                board.Add(new Cell());
            }
        }
    }

    void GenerateDungeon(){
        // Generation of the blank dungeon
        // Generation of the last Cell
        // This Cell can be where ever it want but cannot be on the first position
        GameObject lastCell = new();
        int boardPos = Random.Range(1,board.Count - 1);
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                Cell currentCell = board[i + j * size.x];
                var roomPrefab = rooms[0].room;
                RoomBehavior newRoom;
                if (i + j * size.x == boardPos)
                {
                    lastCell = Instantiate(roomPrefab, new Vector3(i * offset.x + startVectorPos.x, startVectorPos.y, -j * offset.y + startVectorPos.z), roomPrefab.transform.rotation, transform);
                    newRoom = lastCell.GetComponent<RoomBehavior>();
                    newRoom.UpdateRoom(currentCell.status);
                    newRoom.UpdateLastCell(true);
                }
                else
                {
                    newRoom = Instantiate(roomPrefab, new Vector3(i * offset.x + startVectorPos.x, startVectorPos.y, -j * offset.y + startVectorPos.z), roomPrefab.transform.rotation, transform).GetComponent<RoomBehavior>();
                    newRoom.UpdateRoom(currentCell.status);
                }

                roomGrid[i, j] = newRoom;
            }
        }
        if (lastCell)
        {
            PathGenerator(lastCell);
        }
    }

    void PathGenerator(GameObject lastCell){
        int totalCells = size.x * size.y;
        Vector2Int currentPos = startPos;
        Vector2Int endPos = new Vector2Int((int)(lastCell.transform.position.x / offset.x), -(int)(lastCell.transform.position.z / offset.y));

        List<Vector2Int> unvisitedCells = new List<Vector2Int>();
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                unvisitedCells.Add(new Vector2Int(i, j));
            }
        }
        unvisitedCells.Remove(currentPos);


        List<Vector2Int> path = new List<Vector2Int>();
        path.Add(currentPos);

        while (unvisitedCells.Count > 0)
        {
            List<Vector2Int> neighbours = GetUnvisitedNeighbours(currentPos, unvisitedCells);

            if (neighbours.Count > 0)
            {
                Vector2Int nextPos;
                if (Random.value < noise)
                {
                    nextPos = neighbours[Random.Range(0,neighbours.Count)];
                }
                else
                {
                    nextPos = GetClosestNeighbourToTarget(neighbours, endPos);
                }

                MarkPathOnBoard(currentPos, nextPos);

                currentPos = nextPos;
                path.Add(currentPos);
                unvisitedCells.Remove(currentPos);
            }
            else
            {
                currentPos = path[path.Count - 2];
                path.RemoveAt(path.Count - 1);
            }
        }
    }

    List<Vector2Int> GetUnvisitedNeighbours(Vector2Int currentPos, List<Vector2Int> unvisitedCells)
    {
        List<Vector2Int> neighbours = new List<Vector2Int>();

        if (currentPos.x > 0 && unvisitedCells.Contains(new Vector2Int(currentPos.x - 1, currentPos.y)))
            neighbours.Add(new Vector2Int(currentPos.x - 1, currentPos.y));  // Gauche
        if (currentPos.x < size.x - 1 && unvisitedCells.Contains(new Vector2Int(currentPos.x + 1, currentPos.y)))
            neighbours.Add(new Vector2Int(currentPos.x + 1, currentPos.y));  // Droite
        if (currentPos.y > 0 && unvisitedCells.Contains(new Vector2Int(currentPos.x, currentPos.y - 1)))
            neighbours.Add(new Vector2Int(currentPos.x, currentPos.y - 1));  // Bas
        if (currentPos.y < size.y - 1 && unvisitedCells.Contains(new Vector2Int(currentPos.x, currentPos.y + 1)))
            neighbours.Add(new Vector2Int(currentPos.x, currentPos.y + 1));  // Haut

        return neighbours;
    }


    Vector2Int GetClosestNeighbourToTarget(List<Vector2Int> neighbours, Vector2Int target)
    {
        Vector2Int closest = neighbours[0];
        float minDist = Vector2Int.Distance(neighbours[0], target);

        foreach (var neighbour in neighbours)
        {
            float dist = Vector2Int.Distance(neighbour, target);
            if (dist < minDist)
            {
                minDist = dist;
                closest = neighbour;
            }
        }
        return closest;
    }

    void MarkPathOnBoard(Vector2Int from, Vector2Int to)
    {
        // Vérifie d'abord que les deux cellules sont valides
        if (!IsValidCell(from) || !IsValidCell(to)) return;

        Cell currentCell = board[from.x + from.y * size.x];
        Cell nextCell = board[to.x + to.y * size.x];

        // Gère les déplacements horizontaux
        if (from.x < to.x)  // Aller à droite
        {
            currentCell.status[2] = true;  // Porte droite ouverte
            nextCell.status[3] = true;     // Porte gauche ouverte
        }
        else if (from.x > to.x)  // Aller à gauche
        {
            currentCell.status[3] = true;  // Porte gauche ouverte
            nextCell.status[2] = true;     // Porte droite ouverte
        }
        // Gère les déplacements verticaux
        else if (from.y < to.y)  // Aller en haut
        {
            currentCell.status[0] = true;  // Porte haut ouverte
            nextCell.status[1] = true;     // Porte bas ouverte
        }
        else if (from.y > to.y)  // Aller en bas
        {
            currentCell.status[1] = true;  // Porte bas ouverte
            nextCell.status[0] = true;     // Porte haut ouverte
        }

        // Met à jour les objets visuels dans la scène (les salles)
        var roomFrom = GetRoomAtPosition(from);
        var roomTo = GetRoomAtPosition(to);

        if (roomFrom != null)
        {
            roomFrom.UpdateRoom(currentCell.status);
        }

        if (roomTo != null)
        {
            roomTo.UpdateRoom(nextCell.status);
        }
    }

    // Ajout d'une fonction utilitaire pour vérifier si une cellule est valide
    bool IsValidCell(Vector2Int position)
    {
        return position.x >= 0 && position.x < size.x && position.y >= 0 && position.y < size.y;
    }

    RoomBehavior GetRoomAtPosition(Vector2Int position){
        if (position.x >= 0 && position.x < size.x && position.y >= 0 && position.y < size.y)
        {
            return roomGrid[position.x,position.y];
        }
        return null;
    }
}
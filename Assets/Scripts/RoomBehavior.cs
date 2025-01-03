using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehavior : MonoBehaviour
{

    // 0 - UP, 1 - DOWN, 2 - RIGHT, 3 - LEFT
    public GameObject[] walls;
    public GameObject[] doors;

    public bool isLastCell = false;

    public void UpdateRoom(bool[] status){
        for(int i = 0; i < status.Length; i++){
            doors[i].SetActive(status[i]);
            walls[i].SetActive(!status[i]);
        }
    }

    public void UpdateLastCell(bool lastCell){
        isLastCell = lastCell;
    }

    // add object to the room randomly
    public void PopulateRoom(GameObject teleporterPrefab, Vector2 offset){
        // if lastCell, add teleporter
        if(isLastCell){
            GameObject teleporter = Instantiate(teleporterPrefab);
            teleporter.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        }

        // if not lastCell, add enemies, holes, chests, randomly
        // when adding enemies, check if the room is the first room, if it is, don't add enemies
        // when adding holes, do a path to the next rooms
        // when adding chests, add more enemies
        // cannot have holes and chests in the same room
        /*else{
            int random = Random.Range(0, 3);
            // add enemy to the room (random number)
            if(random == 0){
                // if the room is the first room, don't add enemies
                if(transform.position.x == 0 && transform.position.y == 0){
                    return;
                }
                // number of enemies to add
                int numEnemies = Random.Range(1, 4);
                for(int i = 0; i < numEnemies; i++){
                    GameObject enemy = Instantiate(Resources.Load("Prefabs/Enemy")) as GameObject;
                    // random position in the room
                    enemy.transform.position = new Vector3(transform.position.x + Random.Range(-4, 4), transform.position.y + Random.Range(-4, 4), 0);
                }
            }
            // add holes to the room
            else if(random == 1){
                // making all the floor tiles holes
                for(int i = 0; i < transform.childCount; i++){
                    GameObject floor = transform.GetChild(i).gameObject;
                    GameObject hole = Instantiate(Resources.Load("Prefabs/Hole")) as GameObject;
                    hole.transform.position = new Vector3(floor.transform.position.x, floor.transform.position.y, 0);
                }

                // path to the next rooms
                GameObject path = Instantiate(Resources.Load("Prefabs/Path")) as GameObject;
                path.transform.position = new Vector3(transform.position.x - 5, transform.position.y, 0);
            }
            // add a chest in the center of the room
            else{
                GameObject chest = Instantiate(Resources.Load("Prefabs/Chest")) as GameObject;
                chest.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
            }
        }*/
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehavior : MonoBehaviour
{

    // 0 - UP, 1 - DOWN, 2 - RIGHT, 3 - LEFT
    public GameObject[] walls;
    public GameObject[] doors;

    public bool isLastCell = false;
    public bool isCorridor = false;

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
    public void PopulateRoom(GameObject weapon, GameObject trophy, GameObject teleporterPrefab, GameObject Ennemy, GameObject Boss, GameObject Hole, GameObject Chest, string gameObjectName){
        // if lastCell, add teleporter + if 3 stage add trophy
        if(isLastCell){
            if (gameObjectName == "DungeonGenerator3")
            {
                GameObject trophyObject = Instantiate(trophy);
                trophyObject.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                trophyObject.name = "Trophy";
            }
            else
            {
                GameObject teleporter = Instantiate(teleporterPrefab);
                teleporter.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                teleporter.name = "Teleporter" + gameObjectName;
            }
        }

        // if not lastCell and not first, add enemies, holes, chests, randomly
        else {
            if (this.gameObject.name == "Room0|0")
            {
                // if first level, add weapon
                if (gameObjectName == "DungeonGenerator1")
                {
                    GameObject weaponObject = Instantiate(weapon);
                    weaponObject.transform.position = new Vector3(transform.position.x + 1.5f, transform.position.y + 1, transform.position.z);
                    weaponObject.name = "Weapon";
                }
                return;
            }
            // first choose if it is an enemy, hole, or chest room
            // 0-4: enemy, 5: nothing, 6-9: hole, 10: chest
            int randomRoomType = Random.Range(0, 11);
            // if isCorridor, only ennemy can be added
            if(isCorridor){
                int random = Random.Range(0, 2);
                if(random == 0){
                    return;
                }
                GameObject enemy = Instantiate(Ennemy);
                enemy.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                enemy.name = "Enemy";
                return;
            }
            if(randomRoomType <= 4){
                // add enemy
                int randomEnemy = Random.Range(0, 3);
                for(int i = 0; i < randomEnemy; i++){
                    GameObject enemy = Instantiate(Ennemy);
                    enemy.transform.position = new Vector3(transform.position.x + i, transform.position.y, transform.position.z + i);
                    enemy.name = "Enemy" + i;
                }
            }
            else if(randomRoomType <= 5){
                return;
            }
            else if(randomRoomType <= 9){
                // add hole
                GameObject hole = Instantiate(Hole);
                hole.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                hole.name = "Hole";
            }
            else{
                // add chest
                // if chest, add boss in the room
                GameObject boss = Instantiate(Boss);
                boss.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1);
                GameObject chest = Instantiate(Chest);
                chest.transform.position = new Vector3(transform.position.x, transform.position.y + .25f, transform.position.z);
                chest.name = "Chest";
            }
        }
    }
}

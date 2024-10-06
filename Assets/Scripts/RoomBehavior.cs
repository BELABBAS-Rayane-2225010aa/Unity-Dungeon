using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehavior : MonoBehaviour
{

    // 0 - UP, 1 - DOWN, 2 - RIGHT, 3 - LEFT
    public GameObject[] walls;
    public GameObject[] doors;

    public void UpdateRoom(bool[] status){
        for(int i = 0; i < status.Length; i++){
            doors[i].SetActive(status[i]);
            walls[i].SetActive(!status[i]);
        }
    }

}

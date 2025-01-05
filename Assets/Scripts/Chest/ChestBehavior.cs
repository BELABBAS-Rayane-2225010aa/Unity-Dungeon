using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestBehavior : MonoBehaviour
{
    public GameObject dropPrefab;

    private bool isNearChest = false;

    private GameObject Player;

    void Update()
    {
        if (isNearChest && Input.GetKeyDown(KeyCode.E) && Player.GetComponent<PlayerBehavior>().HasKey())
        {
            OpenChest();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player = other.gameObject;
            isNearChest = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isNearChest = false;
        }
    }

    void OpenChest()
    {
        // delete the key from the player
        Player.GetComponent<PlayerBehavior>().SetHasKey(false);
        this.GetComponent<Animator>().SetTrigger("Open");
        // wair for the animation to finish
        StartCoroutine(DropItems());
    }

    IEnumerator DropItems()
    {
        yield return new WaitForSeconds(this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        // Instantiate 3 dropPrefab at the position of the chest with an offset
        for (int i = 0; i < 3; i++)
        {
            Instantiate(dropPrefab, new Vector3(transform.position.x + Random.Range(-1,2),transform.position.y + .5f,transform.position.z + Random.Range(-1,2)), dropPrefab.transform.rotation);
        }
        Destroy(gameObject);
    }
}

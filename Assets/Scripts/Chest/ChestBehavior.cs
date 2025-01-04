using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestBehavior : MonoBehaviour
{
    public GameObject dropPrefab;

    private bool isNearChest = false;

    void Update()
    {
        if (isNearChest && Input.GetKeyDown(KeyCode.E))
        {
            OpenChest();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
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
            Instantiate(dropPrefab, new Vector3(transform.position.x + i,transform.position.y,transform.position.z + i), dropPrefab.transform.rotation);
        }
        Destroy(gameObject);
    }
}

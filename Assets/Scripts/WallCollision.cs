using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCollision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, .01f);

        foreach (Collider col in colliders)
        {
            if (col.tag == "Wall" && gameObject.tag != "Entrance")
            {
                Destroy(gameObject);
                return;
            }
        }

        if (gameObject.tag == "Entrance")
        {
            gameObject.GetComponent<Collider>().enabled = false;
        }
        else {
            GetComponent<Collider>().enabled = true;
        }
    }
}

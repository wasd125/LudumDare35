using UnityEngine;
using System.Collections;

public class CheckPoint : MonoBehaviour {

    void  OnTriggerEnter2D(Collider2D other)
    {       
        Debug.Log("Triggered");

        if (other.tag == "Player")
        {
            if (transform.position.x > LevelController.Instance.SpawnPosition.x)
                LevelController.Instance.SpawnPosition = transform.position;
            Debug.Log("Saved");
        }
    }
}

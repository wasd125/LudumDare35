using UnityEngine;
using System.Collections;

public class CheckPoint : MonoBehaviour {

    void  OnTriggerEnter2D(Collider2D other)
    {       

        if (other.tag == "Player")
        {
            if (transform.position.x > LevelController.Instance.SpawnPosition.x)
                LevelController.Instance.SpawnPosition = transform.position;
        }
    }
}

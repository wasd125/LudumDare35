using UnityEngine;
using System.Collections;

public class CheckPoint : MonoBehaviour {

    bool check = false;

    public bool endFlag = false;

    void  OnTriggerEnter2D(Collider2D other)
    {

        if (check) return;


        if (other.tag == "Player")
        {
            if (transform.position.x > LevelController.Instance.SpawnPosition.x)
                LevelController.Instance.SpawnPosition = transform.position;

            check = true;
            GetComponent<SpriteRenderer>().color = Color.white;

            if (endFlag)
            {
                LevelController.Instance.LevelCompleted();
            }
        }
    }
}

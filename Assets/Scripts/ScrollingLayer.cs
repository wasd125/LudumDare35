using UnityEngine;
using System.Collections;

public class ScrollingLayer : MonoBehaviour {

    public Sprite mySprite;

    public float Multiplier { get; private set; }

    public Vector3 MoveTowardsPos { get; private set; }

    public void InitLayer(float multiplier)
    {
        Multiplier = multiplier;
    }

    public void MoveToPosition(float distance)
    {
        MoveTowardsPos = new Vector3(transform.position.x + (distance * Multiplier), transform.position.y, transform.position.z);
    }

	// Update is called once per frame
	void Update () {

        if (transform.position != MoveTowardsPos)
        {
            transform.position = MoveTowardsPos;
        }

	}
}

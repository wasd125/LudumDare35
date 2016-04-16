using UnityEngine;
using System.Collections;

public class CameraFollower : MonoBehaviour {

    public GameObject Border;

    private Camera myCam;

    private Vector3 lastFramePosition;

    public Transform targetToFollow;

    public float xOffset { get; private set; }

    public float minXPos;

    // Use this for initialization
    void Start () {

        myCam = Camera.main;
        lastFramePosition = transform.position;

        Camera cam = Camera.main;
        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;

        minXPos = Border.transform.position.x - Border.GetComponent<BoxCollider2D>().bounds.size.x / 2 + width * 1.5f;
        Debug.Log(minXPos);
    }
	
	// Update is called once per frame
	void Update () {

        xOffset = myCam.transform.position.x - targetToFollow.position.x;

        if (Mathf.Abs(xOffset) > 0)
        {
            float followSpeed = 2f;


            if (Mathf.Abs(xOffset) > 3.5f)
            {
                followSpeed = 8f;
            }
            else if (Mathf.Abs(xOffset) > 2f)
            {
                followSpeed = 4f;
            }

            Vector3 goToPosition = new Vector3(targetToFollow.position.x, myCam.transform.position.y, myCam.transform.position.z);


            myCam.transform.position = Vector3.MoveTowards(myCam.transform.position, goToPosition, followSpeed * Time.deltaTime);
           
        }

        if (myCam.transform.position.x < minXPos)
            myCam.transform.position = new Vector3(minXPos, myCam.transform.position.y, myCam.transform.position.z);
        lastFramePosition = transform.position;
	}
}

using UnityEngine;
using System.Collections;

public class CameraFollower : MonoBehaviour {

    private Camera myCam;

    private Vector3 lastFramePosition;

    public Transform targetToFollow;

    // Use this for initialization
    void Start () {
        myCam = Camera.main;
        lastFramePosition = transform.position;
    }
	
	// Update is called once per frame
	void Update () {

        float xOffset = myCam.transform.position.x - targetToFollow.position.x;

        if (Mathf.Abs(xOffset) > 0)
        {
            float followSpeed = 2f;


            if (Mathf.Abs(xOffset) > 5f)
            {
                followSpeed = 8f;
            }
            else if (Mathf.Abs(xOffset) > 3f)
            {
                followSpeed = 4f;
            }
            myCam.transform.position = Vector3.MoveTowards(myCam.transform.position, new Vector3(targetToFollow.position.x, myCam.transform.position.y, myCam.transform.position.z),followSpeed*Time.deltaTime );
        }

        lastFramePosition = transform.position;
	}
}

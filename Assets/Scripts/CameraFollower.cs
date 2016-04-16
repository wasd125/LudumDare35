using UnityEngine;
using System.Collections;

public class CameraFollower : MonoBehaviour {

    public static CameraFollower Instance;

    private Camera myCam;

    private Vector3 lastFramePosition;

    public Transform targetToFollow;

    public float xOffset { get; private set; }

    // Use this for initialization
    void Start () {
        // First we check if there are any other instances conflicting
        if (Instance == null)
        {
            // If that is the case, we destroy other instances
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        myCam = Camera.main;
        lastFramePosition = transform.position;
    }
	
	// Update is called once per frame
	void Update () {

        xOffset = myCam.transform.position.x - targetToFollow.position.x;

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

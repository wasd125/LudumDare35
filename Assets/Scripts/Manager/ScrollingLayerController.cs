using UnityEngine;
using System.Collections;

public class ScrollingLayerController : MonoBehaviour {


	public static ScrollingLayerController Instance;

	[SerializeField]
	public ScrollingLayer[] layers;

	private Camera myCam;
	private Vector3 lastFramePosition;
	private float lastFrameX;

	void Start () {
		if (Instance == null)
		{
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
		float currentFrameX = myCam.transform.position.x;

		float offsetX = currentFrameX - lastFrameX;

		foreach (ScrollingLayer item in layers) {

			item.MoveToPosition (offsetX);
			
		}


		lastFramePosition = transform.position;
		lastFrameX = currentFrameX;
	}
}

using UnityEngine;
using System.Collections;

public class ScrollingLayerController : MonoBehaviour {


	[SerializeField]
	public ScrollingLayer[] layers;

	private Camera myCam;
	private float lastFrameX;

	void Start () {


		myCam = Camera.main;
		lastFrameX = myCam.transform.position.x;

	}
	
	// Update is called once per frame
	void Update () {
		float currentFrameX = myCam.transform.position.x;

		float offsetX = currentFrameX - lastFrameX;

		foreach (ScrollingLayer item in layers) {

			item.MoveToPosition (offsetX);
			
		}
		lastFrameX = currentFrameX;
	}
}

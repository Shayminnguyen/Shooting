using UnityEngine;
using System.Collections;

public class TKCameraResizer : MonoBehaviour {
	public float deviceHalfHeight = 540.0f;

	void Awake(){
		Camera.main.orthographicSize = deviceHalfHeight;
	}
}
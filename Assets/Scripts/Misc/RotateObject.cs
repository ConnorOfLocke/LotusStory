using UnityEngine;
using System.Collections;

public class RotateObject : MonoBehaviour {
	
	public float RotateXSpeed = 0;
	public float RotateYSpeed = 0;
	public float RotateZSpeed = 0;
	
	// Update is called once per frame
	void Update () {
		
		transform.Rotate(new Vector3(RotateXSpeed * Time.deltaTime,
		                             RotateYSpeed * Time.deltaTime,
		                             RotateZSpeed * Time.deltaTime) );
		
	}
}

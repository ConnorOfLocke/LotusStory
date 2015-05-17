using UnityEngine;
using System.Collections;

public class SwitchMovement : MonoBehaviour {
	
	public bool Active;
	public Vector3 ActivePosition;
	public Vector3 InactivePosition;
	public float TimeToSwitch;
	
	private Vector3 SmoothVelocity;
	
	
	void Update () 
	{
		if (Active && transform.localPosition != ActivePosition)
			transform.localPosition = Vector3.SmoothDamp(transform.localPosition, ActivePosition, ref SmoothVelocity, TimeToSwitch);
		else if (!Active && transform.localPosition != InactivePosition)
			transform.localPosition = Vector3.SmoothDamp(transform.localPosition, InactivePosition, ref SmoothVelocity, TimeToSwitch);
	}
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UISwitchMovement : MonoBehaviour {

	public bool Active;
	public Vector3 ActivePosition;
	public Vector3 InactivePosition;
	public float TimeToSwitch;
	
	private RectTransform AttachedTransform;
	private Vector3 SmoothVelocity;

	void Start ()
	{
		AttachedTransform = GetComponent<RectTransform>();
	}
	
	void Update () {
		if (Active && AttachedTransform.localPosition != ActivePosition)
			AttachedTransform.localPosition = Vector3.SmoothDamp(InactivePosition, ActivePosition, ref SmoothVelocity, TimeToSwitch);
		else if (Active && AttachedTransform.localPosition != InactivePosition)
			AttachedTransform.localPosition = Vector3.SmoothDamp(ActivePosition, InactivePosition, ref SmoothVelocity, TimeToSwitch);
	}
}

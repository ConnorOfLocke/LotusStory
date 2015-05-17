using UnityEngine;
using System.Collections;

public class FollowMeCam : MonoBehaviour {

	public GameObject ObjectToFollow = null;

	public float TimeToMove = 0.5f;
	public float MouseSpring = 1.0f;
	public float CutoffMouseMagnitude = 0.8f;

	private Vector3 StartDistance;
	private Vector3 SmoothVelocity;
	
	public PlayerContoller AttachedController = null;

	// Use this for initialization
	void Start () {
		if (ObjectToFollow != null)
		{
			StartDistance = this.transform.position - ObjectToFollow.transform.position;
		}
		else
			Debug.Log("FollowMeCam Script : Object To Follow not set");
	}
	
	// Update is called once per frame
	void Update () {
	
		Vector3 PlaceToBe = ObjectToFollow.transform.position + StartDistance;
		if (AttachedController != null)
		{
			if (AttachedController.SelectedObject != null)
			{
				Vector3 AveragePosition = (ObjectToFollow.transform.position + AttachedController.SelectedObject.transform.position) * 0.75f;
				PlaceToBe = AveragePosition + StartDistance;
			}
		}
		
		//Mouse Spring effect
		Vector2 MouseInput = (  new Vector2(Input.mousePosition.x, Input.mousePosition.y));
														
		Vector2 MouseInputVectorFromCenter = Vector2.zero;
		if (MouseInput.magnitude != 0)
			MouseInputVectorFromCenter = new Vector2( (MouseInput.x - Screen.currentResolution.width * 0.5f) / Screen.currentResolution.width,
													  (MouseInput.y - Screen.currentResolution.height * 0.5f) / Screen.currentResolution.height) * 2.0f;
							
												
		if (Mathf.Abs( MouseInputVectorFromCenter.x) > CutoffMouseMagnitude)																	
			PlaceToBe.x += MouseInputVectorFromCenter.x * MouseSpring;
			
		if (Mathf.Abs(MouseInputVectorFromCenter.y) > CutoffMouseMagnitude)																	
			PlaceToBe.z += MouseInputVectorFromCenter.y * MouseSpring;
		
		
		transform.position = Vector3.SmoothDamp(transform.position, PlaceToBe, ref SmoothVelocity, TimeToMove);
		
	
	}
}

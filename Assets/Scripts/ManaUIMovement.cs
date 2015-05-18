using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ManaUIMovement : MonoBehaviour {

	public bool Active;
	public float ActiveYPosition;
	public float InactiveYPosition;
	public float TimeToSwitch;
	
	private GameObject CameraInScene;
	private RectTransform AttachedTransform;
	private Vector3 SmoothVelocity;
	
	void Start ()
	{
		CameraInScene = GameObject.FindGameObjectWithTag("MainCamera");
		AttachedTransform = GetComponent<RectTransform>();
	}
	
	void Update () {

		//rotate the current position towards the camera
		float DistanceFromParent = Vector2.Distance(new Vector2(AttachedTransform.parent.position.x, AttachedTransform.parent.position.z),
		                                            new Vector2(AttachedTransform.position.x, AttachedTransform.position.z));
		
		Vector3 DirectionToCamera = new Vector3(CameraInScene.transform.position.x - AttachedTransform.parent.position.x,   0,
		                                        CameraInScene.transform.position.z - AttachedTransform.parent.position.z);
		
		if (DirectionToCamera != Vector3.zero)
		{
			AttachedTransform.position = AttachedTransform.parent.transform.position + DirectionToCamera.normalized * DistanceFromParent;
			
			Quaternion newRotation = Quaternion.identity;
			newRotation.SetLookRotation(-DirectionToCamera);
			
			AttachedTransform.rotation = newRotation;
			//AttachedTransform.rotation.SetLookRotation(DirectionToCamera);
			
		}
		
		Vector3 ActivePos = new Vector3(AttachedTransform.position.x, ActiveYPosition, AttachedTransform.position.z);
		Vector3 InactivePos = new Vector3(AttachedTransform.position.x, InactiveYPosition, AttachedTransform.position.z);

		
		if (Active)
			AttachedTransform.position = Vector3.SmoothDamp(InactivePos, ActivePos, ref SmoothVelocity, TimeToSwitch);
		else
			AttachedTransform.position = Vector3.SmoothDamp(ActivePos, InactivePos, ref SmoothVelocity, TimeToSwitch);

		
		
	}
}

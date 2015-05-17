using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ParentedLookAtTheCamera : MonoBehaviour {

	private GameObject CameraInScene;
	private RectTransform AttachedTransform;

	// Use this for initialization
	void Start () {
	
		CameraInScene = GameObject.FindGameObjectWithTag("MainCamera");
		AttachedTransform = GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
		float DistanceFromParent = Vector2.Distance(new Vector2(AttachedTransform.parent.position.x, AttachedTransform.parent.position.z),
		                                            new Vector2(AttachedTransform.position.x, AttachedTransform.position.z));
		
		Vector3 DirectionToCamera = new Vector3(AttachedTransform.parent.position.x - CameraInScene.transform.position.x, 0,
		                                        AttachedTransform.parent.position.z - CameraInScene.transform.position.z);
		
		if (DirectionToCamera != Vector3.zero)
		{
			AttachedTransform.localPosition = new Vector3(0, AttachedTransform.localPosition.y, 0) + DirectionToCamera.normalized * DistanceFromParent;
			AttachedTransform.rotation.SetLookRotation(DirectionToCamera);
		}
	}
}

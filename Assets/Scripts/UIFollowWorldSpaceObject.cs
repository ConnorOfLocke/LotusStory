using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIFollowWorldSpaceObject : MonoBehaviour {

	public GameObject FollowObject;
	public Camera CameraInScene;
	private RectTransform attachedTransform;
	private Image attachedImage;

	void Start()
	{
		attachedTransform = GetComponent<RectTransform>(); 
	}

	// Update is called once per frame
	void Update () {
	
		if (FollowObject != null)
		{
			Vector3 ObjectCoords = CameraInScene.WorldToScreenPoint(FollowObject.transform.position);
			
			if (ObjectCoords.x > Screen.width - attachedTransform.rect.width)
				ObjectCoords.x = Screen.width - attachedTransform.rect.width;
			else if (ObjectCoords.x < 0 + attachedTransform.rect.width)
				ObjectCoords.x = 0 + attachedTransform.rect.width;
			
			if (ObjectCoords.y > Screen.height - attachedTransform.rect.height)
				ObjectCoords.y = Screen.height - attachedTransform.rect.height;
			else if (ObjectCoords.y < 0  + attachedTransform.rect.height)
				ObjectCoords.y = 0 + attachedTransform.rect.height;
			
			transform.position = ObjectCoords;
		}
		else
			Destroy(FollowObject);
	}
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIFollowWorldSpaceObject : MonoBehaviour {

	public GameObject FollowObject;
	public Camera CameraInScene;
	private RectTransform attachedTransform;

	void Start()
	{
		attachedTransform = GetComponent<RectTransform>(); 
	}

	// Update is called once per frame
	void Update () {
	
		if (FollowObject != null)
		{
			//first checks if the object is behind the camera
			Vector3 CheckVector = (FollowObject.transform.position - CameraInScene.transform.position).normalized;
			if (CheckVector.z < 0)
			{
				float xPos = ((CheckVector.x + 1) / 2) * Screen.width;
				//CheckVector.y = CheckVector.y + 1 / 2;
				
				if (xPos < attachedTransform.rect.width)
					xPos = attachedTransform.rect.width;
				else if (xPos > Screen.width - attachedTransform.rect.width)
					xPos = Screen.width - attachedTransform.rect.width;
					
				attachedTransform.position = new Vector3(xPos, 0 + attachedTransform.rect.height, 0);
			}
			else
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
				
				attachedTransform.position = new Vector3(ObjectCoords.x, ObjectCoords.y, 0);
			}
		}
		else
			Destroy(this.gameObject);
	}
}

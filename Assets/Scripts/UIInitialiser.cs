using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIInitialiser : MonoBehaviour {

	public Camera CameraInScene;
	public GameObject CollususInScene;
	public Canvas UICanvas;
	public GameObject TownIcon_Prefab;
	public GameObject Collusus_Prefab;
	
	public void InitialiseIcons( List<GameObject> TownHubs)
	{
		if (CollususInScene != null)
		{
			GameObject IconPointer = GameObject.Instantiate(Collusus_Prefab) as GameObject;
			IconPointer.transform.SetParent(UICanvas.transform);
			IconPointer.GetComponent<UIFollowWorldSpaceObject>().CameraInScene = CameraInScene;
			IconPointer.GetComponent<UIFollowWorldSpaceObject>().FollowObject = CollususInScene;
		}
		
		foreach( GameObject hub in TownHubs)
		{
			GameObject IconPointer = GameObject.Instantiate(TownIcon_Prefab) as GameObject;
			IconPointer.transform.SetParent(UICanvas.transform);
			IconPointer.GetComponent<UIFollowWorldSpaceObject>().CameraInScene = CameraInScene;
			IconPointer.GetComponent<UIFollowWorldSpaceObject>().FollowObject = hub;
		}
	}
	
}

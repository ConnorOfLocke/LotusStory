using UnityEngine;
using System.Collections;

public class FollowMeObject : MonoBehaviour {

	public GameObject FollowObject;
	private Vector3 InitialDistance;
	public float LerpSmooth = 1;

	// Use this for initialization
	void Start () {
		InitialDistance = transform.position - FollowObject.transform.position;
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.position = Vector3.Lerp(transform.position, FollowObject.transform.position + InitialDistance, LerpSmooth);
	}
}

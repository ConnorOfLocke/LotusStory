using UnityEngine;
using System.Collections;

public class FollowMeCam : MonoBehaviour {

	public GameObject ObjectToFollow = null;

	public float TimeToMove = 0.5f;

	private Vector3 StartDistance;
	private Vector3 SmoothVelocity;

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
		
		transform.position = Vector3.SmoothDamp(transform.position, PlaceToBe, ref SmoothVelocity, TimeToMove);
		
	
	}
}

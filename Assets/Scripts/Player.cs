using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public Camera followingCamera = null;
	public GameObject MouseLeftClickEffect;
	
	public float Speed = 5.0f;
	public float StopDistance = 1.0f;
	public float EaseOffDistance = 5.0f;
	
	public float RotatationDelta = 0.1f;
	
	private Plane GroundPlane;
	private Vector3 PositionToBe;
	private Quaternion RotationToBe;
	private Vector3 Velocity;

	private CharacterController AttachedContoller;

	// Use this for initialization
	void Start () {
		GroundPlane = new Plane( new Vector3(0, -1, 0), transform.position);
		if (followingCamera == null)
			Debug.Log("Player : FollowingCamera not set");
		
		AttachedContoller = GetComponent<CharacterController>();
		RotationToBe = transform.rotation;
		PositionToBe = transform.position;
			
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (followingCamera != null)
		{
			Ray mouseRay = followingCamera.ScreenPointToRay(Input.mousePosition);
			
			float RayDistance = 0.0f;
			if (GroundPlane.Raycast(mouseRay, out RayDistance))
			{
				if (Input.GetMouseButton(0))
				{
					PositionToBe = mouseRay.GetPoint(RayDistance);
					RotationToBe = Quaternion.LookRotation( PositionToBe - transform.position);
					
					if (MouseLeftClickEffect != null)
						GameObject.Instantiate(MouseLeftClickEffect, PositionToBe + new Vector3(0, 1, 0), Quaternion.identity);
				}
			}
		}
		
		transform.rotation = Quaternion.Slerp(transform.rotation, RotationToBe, RotatationDelta);
		
		float Distance = Vector3.Distance(transform.position, PositionToBe);
		if (Distance > StopDistance)
		{
			//Vector3 Direction = (PositionToBe - transform.position).normalized;
			Vector3 Direction = (transform.rotation * new Vector3(0, 0, 1));
			Vector3 Movement = Direction.normalized * Speed * Time.deltaTime;
			Movement.y = 0;
			
			//slows down near destination
			if (Distance < EaseOffDistance)
				Movement = Movement * Distance / EaseOffDistance;
			
			
			AttachedContoller.Move(Movement);
		}
		else
		{
			AttachedContoller.Move( Vector3.zero);
		}
		
		
		
		
		
	}
}

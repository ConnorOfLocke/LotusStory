using UnityEngine;
using System.Collections;

public class Wolf : MonoBehaviour
{
	public float DistanceToStartFollowing = 10.0f;
	public GameObject PlayerInScene = null;
	public float PathfindCheckTime = 2.0f;
	private float CurrentPathfindCheck = 0.0f;


	public float Speed = 5.0f;
	public float StopDistance = 1.0f;
	public float EaseOffDistance = 5.0f;
	
	public float RotatationDelta = 0.3f;
	
	public float LeanDegree = 20.0f;
	
	private Vector3 PositionToBe;
	private Quaternion RotationToBe;
	private Vector3 Velocity;
	
	private CharacterController AttachedContoller;

	// Use this for initialization
	void Start ()
	{
		AttachedContoller = GetComponent<CharacterController>();
		RotationToBe = transform.rotation;
		PositionToBe = transform.position;
		CurrentPathfindCheck = PathfindCheckTime;
	}

	public void SetTargetPosition(Vector3 Position)
	{
		RotationToBe = Quaternion.LookRotation( PositionToBe - transform.position);
		PositionToBe = new Vector3(Position.x, PositionToBe.y, Position.z);
		
	}

	// Update is called once per frame
	void Update ()
	{
		//goes towards player if close enough
		float Distance = Vector3.Distance(transform.position, PositionToBe);
	
		if (CurrentPathfindCheck >= PathfindCheckTime )
		{		
			SetTargetPosition(PlayerInScene.transform.position);
			CurrentPathfindCheck = 0;
		}
		else if (CurrentPathfindCheck < PathfindCheckTime)
			CurrentPathfindCheck += Time.deltaTime;
			
		//Rotate the things
		transform.rotation = Quaternion.Slerp(transform.rotation, RotationToBe, RotatationDelta);
		if ( Vector3.Distance(RotationToBe.eulerAngles, transform.rotation.eulerAngles) < 5)
			transform.rotation = RotationToBe;

		//lean the thing
		transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3( AttachedContoller.velocity.magnitude / Speed * LeanDegree, 0, 0));
		
		
		if (Distance > StopDistance)
		{
			//Vector3 Direction = (PositionToBe - transform.position).normalized;
			Vector3 Direction = (transform.rotation * new Vector3(0, 0, 1));
			Vector3 Movement = Direction.normalized * Speed * Time.deltaTime;
			Movement.y = 0;
			
			//slows down near destination
			if (Distance < EaseOffDistance)
			{
				Movement = Movement * Distance / EaseOffDistance;
			}
			
			AttachedContoller.Move(Movement);
		}
		else
		{
			AttachedContoller.Move( Vector3.zero);
		}
		


	}
}


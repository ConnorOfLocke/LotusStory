using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

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
	void Start () {
		
		AttachedContoller = GetComponent<CharacterController>();
		RotationToBe = transform.rotation;
		PositionToBe = transform.position;
	}
	
	public void SetTargetPosition(Vector3 Position)
	{
		PositionToBe = Position;
		RotationToBe = Quaternion.LookRotation( PositionToBe - transform.position);
	}
	
	public void SetTargetRotation(Quaternion newRotation)
	{
		RotationToBe = newRotation;
	}
	// Update is called once per frame
	void Update ()
	{
		float Distance = Vector3.Distance(transform.position, PositionToBe);
		
		//rotate towards wanted direction
		transform.rotation = Quaternion.Slerp(transform.rotation, RotationToBe, RotatationDelta);
		if ( Vector3.Distance(RotationToBe.eulerAngles, transform.rotation.eulerAngles) < 5)
			transform.rotation = RotationToBe;

		//leaneriser
		transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3( (AttachedContoller.velocity.magnitude / Speed) * LeanDegree, 0, 0));
		
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

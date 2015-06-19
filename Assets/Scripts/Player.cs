using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public float Speed = 5.0f;
	public float StopDistance = 1.0f;
	public float EaseOffDistance = 5.0f;
	
	public float RotatationDelta = 0.3f;
	
	public float LeanDegree = 20.0f;
	
	public PlayerContoller PlayerControllerInScene;
	
	public GameObject DamageEffect;
	
	private Vector3 PositionToBe;
	private Quaternion RotationToBe;
	private Vector3 Velocity;

	private CharacterController AttachedContoller;

	private float y_pos;
	private float LastDistance;

	// Use this for initialization
	void Start () {
		
		AttachedContoller = GetComponent<CharacterController>();
		RotationToBe = transform.rotation;
		PositionToBe = transform.position;
		y_pos = transform.position.y;
	}
	
	public void SetTargetPosition(Vector3 Position)
	{
		PositionToBe = new Vector3(Position.x, y_pos, Position.z);
		RotationToBe = Quaternion.LookRotation( (PositionToBe - transform.position).normalized);
		LastDistance = Vector3.Distance(transform.position, PositionToBe);
	}
	
	public void AntiAbsorb()
	{
		if (PlayerControllerInScene.CurrMinManaPower > 0)
		{
			PlayerControllerInScene.CurrMaxManaPower -= Time.deltaTime;
			PlayerControllerInScene.CurrMinManaPower -= Time.deltaTime;
		}
		else
		{
			PlayerControllerInScene.CurrMaxManaPower -= Time.deltaTime * 0.5f;
		}
		
		if ((int)(Time.realtimeSinceStartup * 100) % 5 < 1)
			GameObject.Instantiate(DamageEffect, transform.position, Quaternion.identity);
		
		FollowMeCam cam = FindObjectOfType<FollowMeCam>();
		if (cam != null)
			cam.AddShake(Time.deltaTime * 10.0f);
	}
	
	public void SetTargetRotation(Quaternion newRotation)
	{
		RotationToBe = newRotation;
	}
	// Update is called once per frame
	void Update ()
	{
	
		Debug.DrawLine(transform.position, PositionToBe, Color.cyan);
		float Distance = Vector3.Distance(transform.position, PositionToBe);
		
		//recalculate if not rotating enough
		if (LastDistance - Distance < 0)
		{
			transform.rotation = Quaternion.LookRotation( (PositionToBe - transform.position).normalized);
			RotationToBe = Quaternion.LookRotation( (PositionToBe - transform.position).normalized);
		}
		
		//rotate towards wanted direction
		transform.rotation = Quaternion.Slerp(transform.rotation, RotationToBe, RotatationDelta);
		
		if ( Vector3.Distance(RotationToBe.eulerAngles, transform.rotation.eulerAngles)< 10 && Distance > 10)
		{
			transform.rotation = Quaternion.LookRotation( (PositionToBe - transform.position).normalized);
			RotationToBe = transform.rotation;
		}
			
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
		
		//to make DAMN SURE NO FLOATYNESS OCCURS
		transform.position = new Vector3(transform.position.x, y_pos, transform.position.z);
		LastDistance = Distance;
	}
}

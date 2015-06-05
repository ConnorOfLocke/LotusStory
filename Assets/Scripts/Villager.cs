using UnityEngine;
using System.Collections;

public class Villager : MonoBehaviour {

	public GameObject TownHub = null;
	public Vector3 HubPosition;
	public CharacterController AttachedController;

	public float MaxDistanceFromHub = 100;
	
	public float Displacement = 10.0f;
	public float WanderDistance = 10.0f;
	public float FeelerAngle = 45.0f;
	public float RotatationDelta = 0.5f;

	public float Speed = 20.0f;

	public Vector3 NextIdlePosition;
	public float TimeIdling = 3.0f;
	public float IdleTimeJitter = 1.0f;
	public float StopDistance = 10.0f;
	private float CurIdleTime = 0;

	public float MaxTimeWandering = 10.0f;

	public bool Wandering = true;

	// Use this for initialization
	void Start () {
		if (TownHub != null)
			HubPosition = TownHub.transform.position;
		else 
			HubPosition = transform.position;

		HubPosition.y = transform.position.y;

		AttachedController = GetComponent<CharacterController>();
		Vector2 randPos = Random.insideUnitCircle * MaxDistanceFromHub;
		NextIdlePosition = HubPosition + new Vector3 (randPos.x, 0, randPos.y);
	}

	void Repath()
	{
		Vector2 randPos = Random.insideUnitCircle * MaxDistanceFromHub;
		NextIdlePosition = HubPosition + new Vector3 (randPos.x, 0, randPos.y);
		CurIdleTime = TimeIdling + Random.value * IdleTimeJitter;
		Wandering = false;
	}

	// Update is called once per frame
	void Update () {
	
		if (Wandering)
		{
			Vector3 CurVelocity = transform.rotation * -Vector3.forward;

			//Work out next Wander VelocityCurVelocity
			//Vector3 WanderCirclePos = transform.position + CurVelocity.normalized * WanderDistance;
			//Vector3 WanderPosition = WanderCirclePos + new Vector3 (Random.value * 2.0f - 1.0f, 0, Random.value * 2.0f - 1.0f) * Displacement;

			//Vector3 myPos = transform.position;
			Vector3 FinalDirectionVector = new Vector3(0,0,0);//(WanderCirclePos - myPos) + (WanderPosition - WanderCirclePos);

			Debug.DrawLine(transform.position + new Vector3(0, 2, 0), transform.position + FinalDirectionVector + new Vector3(0, 2, 0), Color.red);
			Debug.DrawLine(transform.position + new Vector3(0, 2, 0), transform.position + CurVelocity + new Vector3(0, 2, 0), Color.red);

			//Work out Feelers
			float CurrentAngle = Mathf.Atan2(CurVelocity.z, CurVelocity.x);

			float LeftAngle = CurrentAngle - FeelerAngle * 0.5f * Mathf.Deg2Rad;
			float RightAngle = CurrentAngle + FeelerAngle * 0.5f * Mathf.Deg2Rad;

			Vector3 LeftFeeler = new Vector3 (Mathf.Cos(LeftAngle), 0, Mathf.Sin(LeftAngle)) * WanderDistance;
			Vector3 RightFeeler = new Vector3 (Mathf.Cos(RightAngle), 0, Mathf.Sin(RightAngle)) * WanderDistance;

			for (int i = 0 ; i < 10; i++)
			{
				Debug.DrawLine( HubPosition + new Vector3(Mathf.Sin (i * (Mathf.PI * 2/ 10)), 0, Mathf.Cos(i * (Mathf.PI * 2/ 10))) 			 * MaxDistanceFromHub + new Vector3(0, 2, 0),
				               HubPosition + new Vector3(Mathf.Sin ((i + 1) * (Mathf.PI * 2/ 10)), 0, Mathf.Cos((i + 1) * (Mathf.PI * 2/ 10)))   * MaxDistanceFromHub + new Vector3(0, 2, 0),
				               Color.yellow);
				
			}

			Debug.DrawLine(transform.position + new Vector3(0, 5, 0), transform.position + FinalDirectionVector + new Vector3(0, 3, 0), Color.white);
			
			//Apply weightuing towards NextIdlePosition
			Vector3 NextIdlePositionDirection = NextIdlePosition - transform.position;
			FinalDirectionVector += (NextIdlePosition - transform.position).normalized;
			//if we are near enough to idle position stop a while
			if (NextIdlePositionDirection.magnitude < StopDistance)
				Repath();
			
			Debug.DrawLine(transform.position + new Vector3(0, 5, 0), transform.position + FinalDirectionVector + new Vector3(0, 3, 0), Color.cyan);


			Debug.DrawLine(transform.position + new Vector3(0, 2, 0), transform.position + LeftFeeler  + new Vector3(0, 2, 0), Color.green);
			Debug.DrawLine(transform.position + new Vector3(0, 2, 0), transform.position + RightFeeler + new Vector3(0, 2, 0), Color.green);

			//shy away from points further than the max Distance for the hub
			if (Vector3.Distance (transform.position + FinalDirectionVector * 2, HubPosition) > MaxDistanceFromHub &&
			    (Vector3.Distance (transform.position + LeftFeeler, HubPosition) < MaxDistanceFromHub))
			{
				FinalDirectionVector += LeftFeeler * 10;
			}
			else if (Vector3.Distance (transform.position + FinalDirectionVector * 2, HubPosition) > MaxDistanceFromHub &&
			    (Vector3.Distance (transform.position + RightFeeler, HubPosition) < MaxDistanceFromHub))
			{
				FinalDirectionVector += RightFeeler * 10;
			}
			else if (Vector3.Distance (transform.position + FinalDirectionVector * 2, HubPosition) > MaxDistanceFromHub)
			{
				FinalDirectionVector += LeftFeeler * 1000;
			}

			//shy away from any obstacles
			RaycastHit FrontRay;
			if (Physics.Raycast(transform.position, CurVelocity, out FrontRay, Speed, LayerMask.NameToLayer("DenizenCollision")))
			{
				if (! Physics.Raycast(transform.position, LeftFeeler, LeftFeeler.magnitude, LayerMask.NameToLayer("DenizenCollision")))
				{
					FinalDirectionVector += LeftFeeler * 10;
				}
				else if (! Physics.Raycast(transform.position, RightFeeler, LeftFeeler.magnitude, LayerMask.NameToLayer("DenizenCollision")))
		        {
					FinalDirectionVector += RightFeeler * 10;
				}
				else
				{
					FinalDirectionVector += LeftFeeler * 1000;
				}
			}

			//finally lerp rotate towards the final vector
			FinalDirectionVector.y = 0;
			FinalDirectionVector.Normalize();

			Quaternion FinalRotation = Quaternion.LookRotation(-FinalDirectionVector);
			transform.rotation = Quaternion.Slerp(transform.rotation, FinalRotation, RotatationDelta);

			AttachedController.Move( FinalDirectionVector * Speed * Time.deltaTime);
		}
		else
		{
			CurIdleTime -= Time.deltaTime;

			if (CurIdleTime <= 0)
			{
				Wandering = true;
			}

		}


	}
}

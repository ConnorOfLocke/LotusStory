using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
	public Vector3 Source;
	private Vector3 MidPoint;
	public Vector3 Destination;
	
	public float MaxHeight = 200.0f;
	
	public GameObject EffectOnArrival;
	public float TimeToHit = 2.0f;

	private float TimeSoFar = 0;

	void Start ()
	{
		float ThrowHeight = Mathf.Clamp( MaxHeight - Vector3.Distance(Source, Destination), 1, MaxHeight);
	
		MidPoint = ((Source + Destination) * 0.5f)  + new Vector3(0, ThrowHeight  ,0);
	}

	void Update ()
	{
		if (TimeSoFar < TimeToHit)
		{
			Vector3 P1 = Vector3.Lerp(Source, MidPoint, (TimeSoFar / TimeToHit));
			Vector3 P2 = Vector3.Lerp(MidPoint, Destination, (TimeSoFar / TimeToHit));
		
			transform.position = Vector3.Lerp(P1, P2, (TimeSoFar / TimeToHit));
			TimeSoFar += Time.deltaTime;	
		}
		else
		{
			GameObject.Instantiate(EffectOnArrival, transform.position, Quaternion.Euler(-90, 0, 0));
			Destroy(this.gameObject);
		}
	}
}


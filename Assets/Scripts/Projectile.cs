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
	public float GivenPower = 0;

	public float MaxScale = 3.0f;
	public float MinScale = 1.0f;
	public ParticleSystem AttachedParticleMeter;

	public Vector3 OriginalScale;

	void Start ()
	{
		float ThrowHeight = Mathf.Clamp( MaxHeight - Vector3.Distance(Source, Destination), 1, MaxHeight);
		MidPoint = ((Source + Destination) * 0.5f)  + new Vector3(0, ThrowHeight  ,0);
		OriginalScale = transform.localScale;
	}

	void Update ()
	{
		if (AttachedParticleMeter != null)
			AttachedParticleMeter.emissionRate = GivenPower * 20;
		
		if (MinScale < GivenPower)
		{
			if (GivenPower > MaxScale)
				transform.localScale = Vector3.Lerp( transform.localScale, OriginalScale * MaxScale, 0.5f );
			else
				transform.localScale = Vector3.Lerp( transform.localScale, OriginalScale * GivenPower, 0.5f );
		}
		else
			transform.localScale = OriginalScale * MinScale;
	
		if (TimeSoFar < TimeToHit)
		{
			Vector3 P1 = Vector3.Lerp(Source, MidPoint, (TimeSoFar / TimeToHit));
			Vector3 P2 = Vector3.Lerp(MidPoint, Destination, (TimeSoFar / TimeToHit));
		
			transform.position = Vector3.Lerp(P1, P2, (TimeSoFar / TimeToHit));
			TimeSoFar += Time.deltaTime;	
		}
		else
		{
			Explode();
		}
	}
	
	void Explode()
	{
		GameObject Explosion = GameObject.Instantiate(EffectOnArrival, transform.position, Quaternion.Euler(-90, 0, 0)) as GameObject;
		if (Explosion.GetComponent<SpellExplosion>() != null)
		{
			Explosion.GetComponent<SpellExplosion>().GivenPower = GivenPower;
		}
		Destroy(this.gameObject);
	}
	
	void OnTriggerEnter(Collider other )
	{
		if (other.gameObject.tag == "House")
		{
			Explode();
		}
		else if ( other.gameObject.tag == "Collusus")
		{
			Explode();
			if (other.gameObject.GetComponent<Collusus>() != null)
				other.gameObject.GetComponent<Collusus>().Health -= GivenPower;
		}
		else if (other.gameObject.tag == "Denizen")
		{
			Explode();
		}
		
	
	}
	
}


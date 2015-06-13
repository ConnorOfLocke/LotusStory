using UnityEngine;
using System.Collections;

public enum ePROJECTILE_TYPE
{
	TYPE_ARC,
	TYPE_STRAIGHT,
}

public abstract class Projectile : MonoBehaviour
{
	protected ePROJECTILE_TYPE Type = ePROJECTILE_TYPE.TYPE_ARC;

	public Vector3 Source;
	public Vector3 Destination;
	
	public GameObject EffectOnArrival;
	
	public float GivenPower = 0;

	public float MaxScale = 3.0f;
	public float MinScale = 1.0f;
	public ParticleSystem AttachedParticleMeter;

	public Vector3 OriginalScale;

	protected void Start ()
	{
		OriginalScale = transform.localScale;
	}

	virtual public void Update ()
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
		
	}
	
	abstract protected void Explode();
	
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
			Destroy(other.gameObject);
		}
		
	
	}
	
}


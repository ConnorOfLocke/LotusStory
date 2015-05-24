using UnityEngine;
using System.Collections;

public class SpellExplosion : MonoBehaviour
{
	public float GivenPower;
	private ParticleSystem AttachedParticleSystem;
	private SphereCollider AttachedCollider;
	private DestroyAfterTime AttachedDestroyAfterTime;
	
	private float StartRadius;

	// Use this for initialization
	void Start ()
	{
		AttachedParticleSystem = GetComponent<ParticleSystem>();
		AttachedCollider = GetComponent<SphereCollider>();
		AttachedDestroyAfterTime = GetComponent<DestroyAfterTime>();
		
		StartRadius = AttachedCollider.radius;
	}

	// Update is called once per frame
	void Update ()
	{
		if (GivenPower > AttachedCollider.radius)
			AttachedCollider.radius += (GivenPower) * Time.deltaTime;

	}
}


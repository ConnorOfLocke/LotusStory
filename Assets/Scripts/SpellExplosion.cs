using UnityEngine;
using System.Collections;

public class SpellExplosion : MonoBehaviour
{
	public float GivenPower;
	private ParticleSystem AttachedParticleSystem;
	private SphereCollider AttachedCollider;
	private DestroyAfterTime AttachedDestroyAfterTime;
	
	private float StartRadius;
	private Vector3 StartScale;
	private Color StartColor;
	// Use this for initialization
	void Start ()
	{
		AttachedParticleSystem = GetComponent<ParticleSystem>();
		AttachedCollider = GetComponent<SphereCollider>();
		AttachedDestroyAfterTime = GetComponent<DestroyAfterTime>();
		
		StartRadius = AttachedCollider.radius;
		StartScale = transform.localScale;
		if (StartScale.magnitude < GivenPower)
		{
			StartScale = Vector3.one * GivenPower;
		}
		
		if (renderer.material != null)
			StartColor = renderer.material.color;
			
		AttachedParticleSystem.startSpeed = GivenPower;
		AttachedParticleSystem.maxParticles = (int)(GivenPower * 30.0f);
		
		AttachedParticleSystem.Play();
	}

	// Update is called once per frame
	void Update ()
	{
		transform.localScale = Vector3.Lerp(StartScale, StartScale * GivenPower, AttachedDestroyAfterTime.TimeLeftRatio );

		if (renderer.material != null)
		{
			renderer.material.color = Color.Lerp(StartColor, Color.clear, AttachedDestroyAfterTime.TimeLeftRatio );
		}


	}
}


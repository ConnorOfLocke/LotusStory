using UnityEngine;
using System.Collections;

public class Projectile_Straight : Projectile
{
	public float Speed;
	public float TimeToExist = 10;
	
	private Vector3 Direction;
	private float curTimerSinceStart = 0;
	
	override public void Start ()
	{
		base.Start();
		Direction = (Destination - Source).normalized;
		transform.rotation = Quaternion.LookRotation(Direction);
		
		Speed = Speed + Speed * GivenPower;
	}

	// Update is called once per frame
	override public void Update ()
	{
		base.Update();
	
		transform.position += Direction * Speed * Time.deltaTime;
	
		curTimerSinceStart += Time.deltaTime;
		if (curTimerSinceStart > TimeToExist)
			Destroy(this.gameObject);	
		
	}
	
	override protected void Explode()
	{
		GameObject Explosion = GameObject.Instantiate(EffectOnArrival, transform.position, Quaternion.Euler(-90, 0, 0)) as GameObject;
		if (Explosion.GetComponent<SpellExplosion>() != null)
		{
			Explosion.GetComponent<SpellExplosion>().GivenPower = GivenPower;
		}
		Destroy(this.gameObject);
	}
}


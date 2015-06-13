using UnityEngine;
using System.Collections;

public class Projectile_Arc : Projectile
{
	private Vector3 MidPoint;
	public float MaxHeight = 200.0f;
	public float TimeToHit = 2.0f;
	
	private float TimeSoFar = 0;
	
			// Use this for initialization
		void Start ()
		{
			base.Start();
			float ThrowHeight = Mathf.Clamp( MaxHeight - Vector3.Distance(Source, Destination), 1, MaxHeight);
			MidPoint = ((Source + Destination) * 0.5f)  + new Vector3(0, ThrowHeight, 0);
			Type = ePROJECTILE_TYPE.TYPE_ARC;
		}
	
		// Update is called once per frame
		public override void Update()
		{
			base.Update();
	
			if (TimeSoFar < TimeToHit)
			{
				Vector3 P1 = Vector3.Lerp(Source, MidPoint, (TimeSoFar / TimeToHit));
				Vector3 P2 = Vector3.Lerp(MidPoint, Destination, (TimeSoFar / TimeToHit));
				
				Vector3 LastPos = transform.position;
				transform.position = Vector3.Lerp(P1, P2, (TimeSoFar / TimeToHit));
				
				Vector3 Direction = (transform.position - LastPos).normalized;
				this.transform.localRotation = Quaternion.LookRotation(Direction);
				TimeSoFar += Time.deltaTime;	
			}
			else
			{
				Explode();
			}
	
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


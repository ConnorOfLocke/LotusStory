using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIMoveAbout : MonoBehaviour {

	public float Jitter = 1.0f;
	public float MoveSpeed = 1.0f;
	
	private Vector3 NextSpot;
	private Vector3 OriginalSpot;

	void Start()
	{
		OriginalSpot = GetComponent<RectTransform>().localPosition;
		
		Vector2 RandomSpot = Random.insideUnitCircle;
		NextSpot = OriginalSpot + new Vector3(RandomSpot.x, RandomSpot.y, OriginalSpot.z) * Jitter;
	}

	void Update () 
	{
		float Distance = Vector3.Distance(NextSpot, GetComponent<RectTransform>().localPosition);
		
		if (Distance < MoveSpeed * Time.deltaTime)
		{
			Vector2 RandomSpot = Random.insideUnitCircle;
			NextSpot = OriginalSpot + new Vector3(RandomSpot.x, RandomSpot.y, OriginalSpot.z);
		}
		
		Vector3 Movement = (NextSpot - GetComponent<RectTransform>().localPosition).normalized * MoveSpeed * Time.deltaTime;
		GetComponent<RectTransform>().localPosition += Movement;
	
	}
}

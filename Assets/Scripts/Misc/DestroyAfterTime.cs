using UnityEngine;
using System.Collections;

public class DestroyAfterTime : MonoBehaviour {

	public float TimeToExist;
	private float CurrentTimeExisting = 0;
	
	public float TimeLeft
	{
		get {return this.CurrentTimeExisting; }
	}
	
	public float TimeLeftRatio
	{
		get {return this.CurrentTimeExisting / this.TimeToExist; }
	}
	
	// Update is called once per frame
	void Update () {
	
		CurrentTimeExisting += Time.deltaTime;
		if (CurrentTimeExisting > TimeToExist)
		{
			Destroy(this.gameObject);
		}
	}
}

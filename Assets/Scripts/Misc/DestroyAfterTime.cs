using UnityEngine;
using System.Collections;

public class DestroyAfterTime : MonoBehaviour {

	public float TimeToExist;
	private float CurrentTimeExisting = 0;
	
	// Update is called once per frame
	void Update () {
	
		CurrentTimeExisting += Time.deltaTime;
		if (CurrentTimeExisting > TimeToExist)
		{
			Destroy(this.gameObject);
		}
	}
}

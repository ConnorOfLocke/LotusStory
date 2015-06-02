using UnityEngine;
using System.Collections;

public class CreatureSpawner : MonoBehaviour {

	public GameObject SpawnedCreature;
	public HexChunk AttachedHexChunk;
	public GameObject PlayerInScene;

	public float SpawnDelay = 20.0f;
	public float SpawnJitter = 5.0f;

	private float Timer = 0;
	private float TimeToNextSpanwn;

	// Use this for initialization
	void Start () {
		TimeToNextSpanwn = SpawnDelay + Random.value * SpawnJitter;
	}
	
	// Update is called once per frame
	void Update () {
	
		if (AttachedHexChunk.HexMana > 0.75f)
		{
			Timer += Time.deltaTime;
			if (Timer > TimeToNextSpanwn)
			{
				GameObject NewCreature = GameObject.Instantiate( SpawnedCreature, transform.position, Quaternion.identity ) as GameObject;
				if (NewCreature.GetComponent<Wolf>() != null)
				{
					NewCreature.GetComponent<Wolf>().PlayerInScene = PlayerInScene;
				}	
				
				Timer = 0;
				TimeToNextSpanwn = SpawnDelay + Random.value * SpawnJitter;
			}
		}
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreatureSpawner : MonoBehaviour {

	public GameObject SpawnedCreature;
	public HexChunk AttachedHexChunk;
	public GameObject PlayerInScene;
	public int maxSpawnedCreatures = 4;

	public float SpawnDelay = 30.0f;
	public float SpawnJitter = 10.0f;

	private float Timer = 0;
	private float TimeToNextSpawn;
	private List<GameObject> SpawnedCreatures;

	// Use this for initialization
	void Start () {
		TimeToNextSpawn = SpawnDelay + Random.value * SpawnJitter;
		SpawnedCreatures = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
	
		if (AttachedHexChunk.HexMana > 0.75f)
		{
			Timer += Time.deltaTime;
			if (SpawnedCreatures.Count < maxSpawnedCreatures)
			{
				if (Timer > TimeToNextSpawn)
				{
					GameObject NewCreature = GameObject.Instantiate( SpawnedCreature, transform.position, Quaternion.identity ) as GameObject;
					if (NewCreature.GetComponent<Wolf>() != null)
					{
						NewCreature.GetComponent<Wolf>().PlayerInScene = PlayerInScene;
					}	
					
					SpawnedCreatures.Add(NewCreature);
					Timer = 0;
					TimeToNextSpawn = SpawnDelay + Random.value * SpawnJitter;
				}
			}
			else
			{
				//clear out deleted creatures
				for (int creatureIndex = 0; creatureIndex < SpawnedCreatures.Count; creatureIndex++)
				{
					if (SpawnedCreatures[creatureIndex] == null)
					{
						SpawnedCreatures.RemoveAt(creatureIndex);
					}
				}
			
			}
		}
	}
}

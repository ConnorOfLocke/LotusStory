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

	public float AddedAnger = 0.2f;

	private float Timer = 0;
	private float TimeToNextSpawn;
	private List<GameObject> SpawnedCreatures;

	private Pissed_O_Meter MeterInScene;
	// Use this for initialization
	void Start () {
		TimeToNextSpawn = SpawnDelay + Random.value * SpawnJitter;
		SpawnedCreatures = new List<GameObject>();
		MeterInScene = FindObjectOfType<Pissed_O_Meter>();
		
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		//for collision weirdness
		transform.position += Vector3.zero;
		
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
						NewCreature.GetComponent<AI_AttackPlayer>().PlayerInScene = PlayerInScene;
						NewCreature.GetComponent<Wolf>().WolfCave = this.gameObject;
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
	
	void OnDestroy()
	{
		if (MeterInScene != null)
			MeterInScene.AddAngerModifier(AddedAnger, "A wolf cave has been culled");
	}
}

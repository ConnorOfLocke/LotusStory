using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TownHub : MonoBehaviour
{
	public GameObject[] BuildingSets;
	private List<GameObject> CurrentBuildingSets;

	public float TimeToSpawn = 3.0f;
	public float TimeToSpawnJitter = 1.0f;
	private float CurrentTimeToSpawn = 0.0f;
	private int LastSpawnedSet = -1;

	public HexChunk AttachedChunk;
	public GameObject PlayerInScene;

	public GameObject[] SpawnedVillagers;
	public float VillagerSpawnDelay = 20.0f;
	public float VillagerSpawnJitter = 5.0f;
	private float CurVillagerSpawnTimer = 0.0f;

	public int MaxNumVillagers = 10;
	private int CurrentNumVillagers = 0;

	void Start ()
	{
		CurrentBuildingSets = new List<GameObject> ();
		CurVillagerSpawnTimer = VillagerSpawnDelay + Random.value * VillagerSpawnJitter;
	}
	
	void Update ()
	{ 
		//for collision weirdness
		transform.position += Vector3.zero;
		
		
		if (CurrentTimeToSpawn > TimeToSpawn)
		{
			SpawnTier();
			CurrentTimeToSpawn = Random.Range(0, 2 * TimeToSpawnJitter) - TimeToSpawnJitter;
			AttachedChunk.AbsorbMana();
		}
		else
			CurrentTimeToSpawn += Time.deltaTime;

		if (CurrentNumVillagers < MaxNumVillagers)
		{
			if (CurVillagerSpawnTimer <= 0)
				SpawnVillager ();
			else
				CurVillagerSpawnTimer -= Time.deltaTime;
		}

	}

	void SpawnVillager()
	{
		int randIndex = (int)(Random.value * SpawnedVillagers.Length);
	
		GameObject newVillager = GameObject.Instantiate(SpawnedVillagers[randIndex], transform.position, Quaternion.identity) as GameObject;

		//randomRotation to Start from
		Vector2 RandDir = Random.insideUnitCircle;
		newVillager.transform.rotation = Quaternion.LookRotation ( new Vector3(RandDir.x, 0, RandDir.y));

		newVillager.GetComponent<Villager> ().TownHub = this.gameObject;
		newVillager.GetComponent<AI_AttackPlayer>().PlayerInScene = PlayerInScene;
	
		CurVillagerSpawnTimer = VillagerSpawnDelay + Random.value * VillagerSpawnJitter;
		CurrentNumVillagers ++;
	}

	void SpawnTier()
	{
		int DamagedTier = -1;
		bool FoundDamagedBuilding = false;

		for (int i = 0; i < CurrentBuildingSets.Count && FoundDamagedBuilding == false ; i++, DamagedTier++)
		{
			for (int j = 0; j < CurrentBuildingSets[i].transform.childCount; j++)
			{
				GameObject building = CurrentBuildingSets[i].transform.GetChild(j).gameObject;
				if (building.GetComponent<TownBuilding>() != null)
				{
					if (! building.GetComponent<TownBuilding>().GetActivated())
					{
						FoundDamagedBuilding = true;
						building.GetComponent<TownBuilding>().Activate();
					}
				}
			}
		}

		//Sapwn next tier if nothing is broken
		if (!FoundDamagedBuilding && (LastSpawnedSet < BuildingSets.Length - 1))
		{
			LastSpawnedSet++;
			GameObject BuildingSet = GameObject.Instantiate(BuildingSets[LastSpawnedSet],
			                                             this.transform.position,
			                                             BuildingSets[LastSpawnedSet].transform.rotation) as GameObject;
			for (int i = 0; i < BuildingSet.transform.childCount; i++)
			{
				GameObject Building = BuildingSet.transform.GetChild(i).gameObject;
				if (Building.GetComponent<TownBuilding>() != null)
				{
					Building.GetComponent<TownBuilding>().RootHub = this.gameObject;
					Building.GetComponent<TownBuilding>().Activate();
				}
			}
			CurrentBuildingSets.Add(BuildingSet);
		}

	}
}


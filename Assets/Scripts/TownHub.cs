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
	
	void Start ()
	{
		CurrentBuildingSets = new List<GameObject> ();
	}
	
	void Update ()
	{
		if (CurrentTimeToSpawn > TimeToSpawn)
		{
			SpawnTier();
			CurrentTimeToSpawn = Random.Range(0, 2 * TimeToSpawnJitter) - TimeToSpawnJitter;
			AttachedChunk.AbsorbMana();
		}
		else
			CurrentTimeToSpawn += Time.deltaTime;
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


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HexGridGenerator : MonoBehaviour {

	public int grid_width;
	public int grid_height;

	public float hex_radius = 0.5f;

	public GameObject PlayerInScene;

	private List<HexChunk> BaseTerrainHexs;
	private List<float> BaseTerrainChances;
	private float OverallChance;
	private List<GameObject> SpecialObjects;
	private List<int> MinimumSpawns;
	private int totalNumMinimumSpawns = 0;
	
	private List<GameObject> generated_hex_chunks;
	
	public GameObject HexWall = null;
	public UIInitialiser UIInit;
	public Collusus CollususInScene;
	public Pissed_O_Meter PissedMeter;
	
	// Use this for initialization
	void Awake () 
	{
		InitHexGridSpawn();
		
		if (BaseTerrainHexs.Count != 0)
		{
			float hex_width = hex_radius * 2.0f * 3/4;
			float hex_height = hex_radius * Mathf.Sqrt(3) / 2.0f;
			
			List<GameObject> left_edge_hex_chunks  = new List<GameObject>();
			List<GameObject> right_edge_hex_chunks  = new List<GameObject>();
			List<GameObject> town_hubs  = new List<GameObject>();
			List<GameObject> creature_spawners  = new List<GameObject>();
			
			left_edge_hex_chunks = new List<GameObject>();
			right_edge_hex_chunks = new List<GameObject>();
			
			//First Pass for placeholders and positions
			int z_index = -1;
			for (float z = -(hex_radius * 2 * Mathf.Sqrt(3) / 2.0f); z <= (grid_height + 1) * hex_height * 2; z += hex_radius * 2 * Mathf.Sqrt(3) / 2.0f, z_index++)
			{
				bool jitter = false;
				int x_index = -1;
				for (float x = -hex_width; x <= (grid_width + 1) * hex_width; x += hex_width, x_index++)
				{
					if (!(x_index == -1 ||
					      z_index == -1 ||
					      x >= (grid_width * hex_width) ||
					      z >= (grid_height * hex_height * 2)))
					{
						GameObject newHex;
						if (jitter)
						{
							newHex = GameObject.Instantiate( new GameObject(),
							                                new Vector3(x, 0, z + hex_height * 0.5f),
							                                Quaternion.identity) as GameObject;
						}
						else 
						{
							newHex = GameObject.Instantiate( new GameObject(),
							                                new Vector3(x, 0, z - hex_height * 0.5f),
							                                Quaternion.identity) as GameObject;
						}					
						newHex.name = "TempHex";
						generated_hex_chunks.Add(newHex);
						jitter = !jitter;
					}
					else
					{
						//Spawns a Wall
						if (jitter)
							GameObject.Instantiate( HexWall,
							                       new Vector3(x, 0, z + hex_height * 0.5f),
							                       Quaternion.identity);	
						else
							GameObject.Instantiate( HexWall,
							                       new Vector3(x, 0, z - hex_height * 0.5f),
							                       Quaternion.identity);
						jitter = !jitter;
						
					}
				}						
			}
			
			//Second pass for Minimum Spawn hexs and spanwed specials and clearing placeholders
			for (int i = 0; i < MinimumSpawns.Count; i++)
			{
				for (int j = 0; j < MinimumSpawns[i]; j++)
				{
					int randHexIndex = (int)(Random.value * (float)generated_hex_chunks.Count);
					if ( generated_hex_chunks[randHexIndex].name == "TempHex")
					{
						//spawns a new hex at the position of the placeholder after deleting it
						Vector3 HexPos = generated_hex_chunks[randHexIndex].transform.position;
						DestroyImmediate(generated_hex_chunks[randHexIndex]);
						generated_hex_chunks[randHexIndex] = GameObject.Instantiate(BaseTerrainHexs[i].gameObject, HexPos, Quaternion.identity) as GameObject;
						
						if (SpecialObjects[i] != null)
						{
							GameObject newSpecial = null;
							generated_hex_chunks[randHexIndex].GetComponent<HexChunk>().SpawnSpecialObject( SpecialObjects[i], ref newSpecial);
							
							//special stuff for town hubs
							if (newSpecial.GetComponent<TownHub>() != null)
							{
								newSpecial.GetComponent<TownHub>().AttachedChunk = generated_hex_chunks[randHexIndex].GetComponent<HexChunk>();
								newSpecial.GetComponent<TownHub>().PlayerInScene = PlayerInScene;
								town_hubs.Add(newSpecial);
							}
							
							//special stuff for creature spawners
							if (newSpecial.GetComponent<CreatureSpawner>() != null)
							{
								newSpecial.GetComponent<CreatureSpawner>().AttachedHexChunk = generated_hex_chunks[randHexIndex].GetComponent<HexChunk>();
								newSpecial.GetComponent<CreatureSpawner>().PlayerInScene = PlayerInScene;
								creature_spawners.Add(newSpecial);
							}
						}
						
						generated_hex_chunks[randHexIndex].GetComponent<HexChunk>().ClearUnspawned();
						
						if (randHexIndex % grid_width == 0 )
						{
							left_edge_hex_chunks.Add(generated_hex_chunks[randHexIndex]);
						}
						else if (randHexIndex % grid_width == grid_width - 1)
						{
							right_edge_hex_chunks.Add(generated_hex_chunks[randHexIndex]);
						}
					}
					else
						j--;
				}
			}
			
			//Third pass of the rest of of the hexs and specials
			for (int i = 0; i < generated_hex_chunks.Count; i++)
			{
				if (generated_hex_chunks[i].name == "TempHex")
				{
					//randomly chooses what hex to spawn
					float HexChance = Random.value * OverallChance;
					for (int j = 0; j < BaseTerrainChances.Count; j++)
					{
						if (BaseTerrainChances[j] == -1)
							continue;
					
						if (HexChance < BaseTerrainChances[j])
						{
							HexChance = j;
							break;
						}
					}
					Vector3 HexPos = generated_hex_chunks[i].transform.position;
					DestroyImmediate(generated_hex_chunks[i]);
					generated_hex_chunks[i] = GameObject.Instantiate(BaseTerrainHexs[(int)HexChance].gameObject, HexPos, Quaternion.identity) as GameObject;
					
					if (SpecialObjects[(int)HexChance] != null)
					{
						GameObject newSpecial = null;
						generated_hex_chunks[i].GetComponent<HexChunk>().SpawnSpecialObject( SpecialObjects[(int)HexChance], ref newSpecial);
						
						//special stuff for town hubs
						if (newSpecial.GetComponent<TownHub>() != null)
						{
							newSpecial.GetComponent<TownHub>().AttachedChunk = generated_hex_chunks[i].GetComponent<HexChunk>();
							newSpecial.GetComponent<TownHub>().PlayerInScene = PlayerInScene;
							town_hubs.Add(newSpecial);
						}
						
						//special stuff for creature spawners
						if (newSpecial.GetComponent<CreatureSpawner>() != null)
						{
							newSpecial.GetComponent<CreatureSpawner>().AttachedHexChunk = generated_hex_chunks[i].GetComponent<HexChunk>();
							newSpecial.GetComponent<CreatureSpawner>().PlayerInScene = PlayerInScene;
							creature_spawners.Add(newSpecial);
						}
					}
					
					//
					
					generated_hex_chunks[i].GetComponent<HexChunk>().ClearUnspawned();
					
					if (i % grid_width == 0 )
					{
						left_edge_hex_chunks.Add(generated_hex_chunks[i]);
					}
					else if (i % grid_width == grid_width - 1)
					{
						right_edge_hex_chunks.Add(generated_hex_chunks[i]);
					}
				}
			}
			
			//finally pass through all needed hexs to various objects
			if (CollususInScene != null)
				CollususInScene.Initialise(left_edge_hex_chunks, right_edge_hex_chunks, town_hubs);
			
			if (UIInit != null)
				UIInit.InitialiseIcons(town_hubs, creature_spawners);
			
			if (PissedMeter != null)
				PissedMeter.Initialise(town_hubs);
		
		}
		else
		{
			Debug.Log("HexGridGenerator: No Hex's Given to generate with");
		}
	}

	private void InitHexGridSpawn()
	{
		//Gets all the attached HexChunk Components
		generated_hex_chunks = new List<GameObject>();	
		BaseTerrainHexs = new List<HexChunk>();
		BaseTerrainChances = new List<float>();
		SpecialObjects = new List<GameObject>();
		MinimumSpawns = new List<int>();
		
		OverallChance = 0;
		HexGridGeneratorChunk[] attachedChunks = GetComponents<HexGridGeneratorChunk>();
		foreach(HexGridGeneratorChunk chunk in attachedChunks)
		{
			BaseTerrainHexs.Add(chunk.BaseTerrain);
			SpecialObjects.Add( chunk.SpawnedSpecial);
			MinimumSpawns.Add( chunk.MinimumInstances);

			totalNumMinimumSpawns += chunk.MinimumInstances;	
			
			//Input chances
			OverallChance += chunk.SpawnChance;
			if (chunk.SpawnChance <= 0)
			{
				BaseTerrainChances.Add(-1);
			}
			else	
				BaseTerrainChances.Add( OverallChance );
			
		}
	}

	private void  SpawnHexGrids()
	{

		
		//check for edge pieces
		//if (x_index == 0)
		//	left_edge_hex_chunks.Add(newHex);
		//else if (x_index == grid_width - 1 )
		//	right_edge_hex_chunks.Add(newHex);
	}
	
	public List<GameObject> GetGeneratedChunks()
	{
		return generated_hex_chunks;
	}

	// Update is called once per frame
	void Update () {
	
	}
}

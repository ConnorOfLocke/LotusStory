﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HexGridGenerator : MonoBehaviour {

	public int grid_width;
	public int grid_height;

	public float hex_radius = 0.5f;

	private List<HexChunk> BaseTerrainHexs;
	private List<float> BaseTerrainChances;
	private float OverallChance;
	private List<List<GameObject>> SpecialObjects;
	public int numSpecials; 
	
	private List<GameObject> generated_hex_chunks;
	
	public GameObject HexWall = null;
	public UIInitialiser UIInit;

	// Use this for initialization
	void Awake () 
	{
		//Gets all the attached HexChunk Components
		generated_hex_chunks = new List<GameObject>();	
		BaseTerrainHexs = new List<HexChunk>();
		BaseTerrainChances = new List<float>();
		SpecialObjects = new List<List<GameObject>>();
		OverallChance = 0;
		
		HexGridGeneratorChunk[] attachedChunks = GetComponents<HexGridGeneratorChunk>();
		foreach(HexGridGeneratorChunk chunk in attachedChunks)
		{
			BaseTerrainHexs.Add(chunk.BaseTerrain);
			SpecialObjects.Add( new List<GameObject>());
			foreach (GameObject thing in chunk.SpawnedSpecials)
			{
				SpecialObjects[SpecialObjects.Count - 1].Add(thing);
			}
			
			//Input chances
			BaseTerrainChances.Add( OverallChance );
			OverallChance += chunk.SpawnChance;
		}
		
		if (BaseTerrainHexs.Count != 0)
		{
			float hex_width = hex_radius * 2.0f * 3/4;
			float hex_height = hex_radius * Mathf.Sqrt(3) / 2.0f;
		
			List<GameObject> hex_chunks_with_specials = new List<GameObject>();
		
			//First Pass for terrain	
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
						int randIndex = 0;
						
						float HexChance = Random.Range(0, OverallChance);
						for (int i = 0; i < BaseTerrainChances.Count; i++)
						{
							if (HexChance < BaseTerrainChances[i])
							{
								randIndex = i;
								break;
							}
						}
						
						if (jitter)
						{
							newHex = GameObject.Instantiate( BaseTerrainHexs[randIndex].gameObject,
							                                           new Vector3(x, 0, z + hex_height * 0.5f),
																		Quaternion.identity) as GameObject;	
						}
						else 
						{
							newHex = GameObject.Instantiate( BaseTerrainHexs[randIndex].gameObject,
							                                           		new Vector3(x, 0, z - hex_height * 0.5f),
							                                           		Quaternion.identity) as GameObject;
	                    }
	                    
	                    //sets the hex type if specials can spawn else -1
						if (newHex.GetComponent<HexChunk>().PlaceHolderSpecialObjects.Length > 0 && SpecialObjects[randIndex].Count > 0 )
	                   		newHex.GetComponent<HexChunk>().SetHexChunkType(randIndex);
	                   	else
	                   		newHex.GetComponent<HexChunk>().SetHexChunkType(-1);
	                   		
	                    
	                    if (newHex.GetComponent<HexChunk>().PlaceHolderSpecialObjects.Length != 0)
	                    {
							hex_chunks_with_specials.Add(newHex);
	                    }
	                    
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
	
			List<GameObject> TownList = new List<GameObject>();
	
			//Second Pass for the Specials
			for (int i = 0; i < numSpecials; i++)
			{
				int hexIndex = (int)(Random.value * hex_chunks_with_specials.Count);
	
				int HexChunkID = hex_chunks_with_specials[hexIndex].GetComponent<HexChunk>().GetHexChunkType();
				if (HexChunkID != -1)
				{
					int SpecialIndex = (int)(Random.value * SpecialObjects[HexChunkID].Count);
					
					GameObject SpanwnedObject = null;
					
					//if there are no placeholders in the hex chunk left remove it
					if (hex_chunks_with_specials[hexIndex].GetComponent<HexChunk>().SpawnSpecialObject(SpecialObjects[HexChunkID][SpecialIndex], ref SpanwnedObject))
					{
						hex_chunks_with_specials.RemoveAt(hexIndex);
						
						if (SpanwnedObject.GetComponent<TownHub>() != null)
							TownList.Add(SpanwnedObject);
						
					}
				}
				else
					i--;
			}
			
			//pass the townhubs to the UIInitialiser
			if (UIInit != null)
			{
				UIInit.InitialiseIcons(TownList);
			}
			
			
			//ThirdPass for getting rid of old placeholders
			for (int i = 0; i < generated_hex_chunks.Count; i++)
			{
				generated_hex_chunks[i].GetComponent<HexChunk>().ClearUnspawned();
			}
			
		}
		else
		{
			Debug.Log("No Hex Chunks included");
		}
	}

	public List<GameObject> GetGeneratedChunks()
	{
		return generated_hex_chunks;
	}

	// Update is called once per frame
	void Update () {
	
	}
}

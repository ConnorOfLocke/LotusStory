using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HexGridGenerator : MonoBehaviour {

	public int grid_width;
	public int grid_height;

	public float hex_radius = 0.5f;

	private List<HexChunk> BaseTerrainHexs;
	private List<List<GameObject>> InteractableObjects;
	public int numInteractables; 
	
	private List<GameObject> generated_hex_chunks;

	// Use this for initialization
	void Start () 
	{
		//Gets all the attached HexChunk Components
		generated_hex_chunks = new List<GameObject>();	
		
		BaseTerrainHexs = new List<HexChunk>();
		InteractableObjects = new List<List<GameObject>>();
		HexGridGeneratorChunk[] attachedChunks = GetComponents<HexGridGeneratorChunk>();
		foreach(HexGridGeneratorChunk chunk in attachedChunks)
		{
			BaseTerrainHexs.Add(chunk.BaseTerrain);
			InteractableObjects.Add( new List<GameObject>());
			foreach (GameObject thing in chunk.SpawnedInteractables)
			{
				InteractableObjects[InteractableObjects.Count - 1].Add(thing);
			}
		}
		
		if (BaseTerrainHexs.Count != 0)
		{
			float hex_width = hex_radius * 2.0f * 3/4;
			float hex_height = hex_radius * Mathf.Sqrt(3) / 2.0f;
		
			//First Pass for terrain	
			int z_index = 0;
			for (float z = 0; z < grid_height * hex_height; z += hex_radius * 2 * Mathf.Sqrt(3) / 2.0f, z_index++)
			{
				bool jitter = false;
				int x_index = 0;
				for (float x = 0; x < grid_width * hex_width; x += hex_width, x_index++)
				{
					GameObject newHex;
					int randIndex = (int)Random.Range(0, BaseTerrainHexs.Count);
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
                    
                    newHex.GetComponent<HexChunk>().SetHexChunkType(randIndex);
                    generated_hex_chunks.Add(newHex);
					jitter = !jitter;
				}						
			}
	
	
	
			//Second Pass for the Interactables
			for (int i = 0; i < numInteractables; i++)
			{
				int hexIndex = (int)(Random.value * generated_hex_chunks.Count);
				while (generated_hex_chunks[hexIndex].GetComponent<HexChunk>().PlaceHolderInteractableObjects.Length != 0)
					hexIndex = (int)(Random.value * generated_hex_chunks.Count);
	
				int HexChunkID = generated_hex_chunks[hexIndex].GetComponent<HexChunk>().GetHexChunkType();
				int InteractableIndex = (int)(Random.value * InteractableObjects[HexChunkID].Count);
				
				generated_hex_chunks[hexIndex].GetComponent<HexChunk>().SpawnInteractObject(InteractableObjects[HexChunkID][InteractableIndex]);
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

	List<GameObject> GetGeneratedChunks()
	{
		return generated_hex_chunks;
	}

	// Update is called once per frame
	void Update () {
	
	}
}

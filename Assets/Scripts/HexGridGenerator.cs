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
	
	public GameObject HexWall = null;

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
		
			List<GameObject> hex_chunks_with_interactables = new List<GameObject>();
		
			//First Pass for terrain	
			int z_index = -1;
			for (float z = -(hex_radius * 2 * Mathf.Sqrt(3) / 2.0f); z <= (grid_height + 1) * hex_height; z += hex_radius * 2 * Mathf.Sqrt(3) / 2.0f, z_index++)
			{
				bool jitter = false;
				int x_index = -1;
				for (float x = -hex_width; x <= (grid_width + 1) * hex_width; x += hex_width, x_index++)
				{
					if (!(x_index == -1 ||
						z_index == -1 ||
						x >= (grid_width * hex_width) ||
						z >= (grid_height * hex_height)))
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
	                    
	                    //sets the hex type if interactables can spawn else -1
						if (newHex.GetComponent<HexChunk>().PlaceHolderInteractableObjects.Length > 0 && InteractableObjects[randIndex].Count > 0 )
	                   		newHex.GetComponent<HexChunk>().SetHexChunkType(randIndex);
	                   	else
	                   		newHex.GetComponent<HexChunk>().SetHexChunkType(-1);
	                   		
	                    
	                    if (newHex.GetComponent<HexChunk>().PlaceHolderInteractableObjects.Length != 0)
	                    {
							hex_chunks_with_interactables.Add(newHex);
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
	
	
			//Second Pass for the Interactables
			for (int i = 0; i < numInteractables; i++)
			{
				int hexIndex = (int)(Random.value * hex_chunks_with_interactables.Count);
	
				int HexChunkID = hex_chunks_with_interactables[hexIndex].GetComponent<HexChunk>().GetHexChunkType();
				if (HexChunkID != -1)
				{
					int InteractableIndex = (int)(Random.value * InteractableObjects[HexChunkID].Count);
					
					//if there are no placeholders in the hex chunk left remove it
					if (hex_chunks_with_interactables[hexIndex].GetComponent<HexChunk>().SpawnInteractObject(InteractableObjects[HexChunkID][InteractableIndex]))
					{
						hex_chunks_with_interactables.RemoveAt(hexIndex);
					}
				}
				else
					i--;
				
				
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

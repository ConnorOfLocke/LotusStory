using UnityEngine;
using System.Collections;

public class HexGridGenerator : MonoBehaviour {

	public int grid_width;
	public int grid_height;

	public float hex_radius = 0.5f;

	public GameObject[] hex_chunks;
	private GameObject[] generated_hex_chunks;

	// Use this for initialization
	void Start () 
	{
		if (hex_chunks.Length != 0)
		{
			float hex_width = hex_radius * 2.0f * 3/4;
			float hex_height = hex_radius * Mathf.Sqrt(3) / 2.0f;
			
			int z_index = 0;
			for (float z = 0; z < grid_height * hex_height; z += hex_radius * 2 * Mathf.Sqrt(3) / 2.0f, z_index++)
			{
				bool jitter = false;
				int x_index = 0;
				for (float x = 0; x < grid_width * hex_width; x += hex_width, x_index++)
				{
					int randIndex = (int)Random.Range(0, 4);
					if (jitter)
					{
						GameObject newHex = GameObject.Instantiate( hex_chunks[randIndex],
						                                           new Vector3(x, 0, z + hex_height * 0.5f),
																	Quaternion.identity) as GameObject;
					}
					else 
					{
						GameObject newHex = GameObject.Instantiate( hex_chunks[randIndex],
						                                           new Vector3(x, 0, z - hex_height * 0.5f),
						                                           Quaternion.identity) as GameObject;
                    }
					jitter = !jitter;
				}						
			}
		}
		else
		{
			Debug.Log("No Hex Chunks included");
		}
	}

	GameObject[] GetGeneratedChunks()
	{
		return generated_hex_chunks;
	}

	// Update is called once per frame
	void Update () {
	
	}
}

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
			generated_hex_chunks = new GameObject[grid_width * grid_height];

			for (float z = 0 ; z < grid_height; z++)
			{
				float width = hex_radius * 2 * 3/4;
				float height = width * Mathf.Sqrt(3) / 4;
				bool jitter = false;

				for (float x = 0; x < grid_width; x++)
				{
					float better_height = (jitter) ? 0 : height;

					GameObject newHex = Instantiate( hex_chunks[(int)Random.Range(0, 4)],
					                                 new Vector3(x * width , 0, z * width + better_height),
					                                 Quaternion.identity) as GameObject;
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

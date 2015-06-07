using UnityEngine;
using System.Collections;

public class HexGridGeneratorChunk : MonoBehaviour {

	public HexChunk BaseTerrain;
	public GameObject SpawnedSpecial = null;
	public int MinimumInstances;
	public float SpawnChance = 1.0f;
}

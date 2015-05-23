﻿using UnityEngine;
using System.Collections;

public class HexChunk : MonoBehaviour {

	public float FlavorPlacementJitter = 0.1f;
	public float FlavorRotationDegreeJitter = 5f;
	
	public float SpecialPlacementJitter = 0.1f;
	public float SpecialRotationDegreeJitter = 5f;
	
	public GameObject[] PlaceHolderFlavorObjects;
	public GameObject[] FlavorObjects;

	private int PlaceHolderObjectArraySize;
	public GameObject[] PlaceHolderSpecialObjects;

	private int HexChunkTypeID;
	
	// Use this for initialization
	void Start () {
	
		if (PlaceHolderFlavorObjects.Length == 0 ||
		    FlavorObjects.Length == 0 )
			Debug.Log("HexChunk: No PlaceHolderObjects or FlavorObjects Given");
		else
			SpawnFlavorObjects();
			
		PlaceHolderObjectArraySize = PlaceHolderSpecialObjects.Length;
	}
	
	private void SpawnFlavorObjects()
	{
		for (int i = 0; i < PlaceHolderFlavorObjects.Length; i++)
		{
			Vector3 ObjectPosition = PlaceHolderFlavorObjects[i].transform.position;
			Vector3 ObjectRotation = PlaceHolderFlavorObjects[i].transform.rotation.eulerAngles;
			Destroy(PlaceHolderFlavorObjects[i].gameObject);
			
			int randomIndex = (int)Random.Range(0, FlavorObjects.Length);
			
			Vector3 randomJitter = Random.insideUnitSphere * FlavorPlacementJitter;
			randomJitter.y = 0;
			
			Quaternion newRotation = Quaternion.Euler( ObjectRotation + new Vector3(0 ,Random.Range(0, FlavorRotationDegreeJitter * 2) - FlavorRotationDegreeJitter , 0));
			
			GameObject.Instantiate( FlavorObjects[randomIndex], ObjectPosition + randomJitter, newRotation);
		}
	}
	
	public void SetHexChunkType(int a_HexChunkTypeID)
	{
		HexChunkTypeID = a_HexChunkTypeID;
	}
	
	public int GetHexChunkType()
	{
		return HexChunkTypeID;
	}
	
	public bool SpawnSpecialObject(GameObject Special)
	{
		int SpecialIndex = (int)(Random.value * PlaceHolderObjectArraySize);
		
		if (PlaceHolderSpecialObjects[SpecialIndex] != null)
		{
			//gets position then destroys it
			Vector3 ObjectPosition = PlaceHolderSpecialObjects[SpecialIndex].transform.position;
			Vector3 ObjectRotation = PlaceHolderSpecialObjects[SpecialIndex].transform.rotation.eulerAngles;
			DestroyImmediate(PlaceHolderSpecialObjects[SpecialIndex].gameObject ,true);
			PlaceHolderSpecialObjects[SpecialIndex] = null;
		
			//moves it to the back of the array
			for (int i = SpecialIndex + 1; i < PlaceHolderSpecialObjects.Length; i++)
			{
				PlaceHolderSpecialObjects[i - 1] = PlaceHolderSpecialObjects[i];
			}
			PlaceHolderObjectArraySize -= 1;
			
			Vector3 randomJitter = Random.insideUnitSphere * SpecialPlacementJitter;
			randomJitter.y = 0;
			
			Quaternion newRotation = Quaternion.Euler( ObjectRotation + new Vector3(0 ,Random.Range(0, SpecialRotationDegreeJitter * 2) - SpecialRotationDegreeJitter , 0));
			
			GameObject.Instantiate( Special, ObjectPosition + randomJitter, newRotation);
		}
		
		return (PlaceHolderObjectArraySize <= 0);
	}
	
	public void ClearUnspawned()
	{
		for (int i = 0; i < PlaceHolderSpecialObjects.Length; i++)
		{
			if (PlaceHolderSpecialObjects[i] != null)
			{
				DestroyImmediate(PlaceHolderSpecialObjects[i], true);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
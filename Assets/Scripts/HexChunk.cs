using UnityEngine;
using System.Collections;

public class HexChunk : MonoBehaviour {

	public float FlavorPlacementJitter = 0.1f;
	public float FlavorRotationDegreeJitter = 5f;
	
	public float InteractablePlacementJitter = 0.1f;
	public float InteractableRotationDegreeJitter = 5f;
	
	public GameObject[] PlaceHolderFlavorObjects;
	public GameObject[] FlavorObjects;

	public GameObject[] PlaceHolderInteractableObjects;

	private int HexChunkTypeID;
	
	// Use this for initialization
	void Start () {
	
		if (PlaceHolderFlavorObjects.Length == 0 ||
		    FlavorObjects.Length == 0 )
			Debug.Log("HexChunk: No PlaceHolderObjects or FlavorObjects Given");
		else
			SpawnFlavorObjects();
			
		//if (PlaceHolderInteractableObjects.Length == 0 ||
		//    InteractableObjects.Length == 0 )
		//	Debug.Log("HexChunk: No PlaceHolderObjects or FlavorObjects Given");
		//else
		//	SpawnInteractObjects();
		
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
	
	public void SpawnInteractObject(GameObject Interactable)
	{
		int InteractableIndex = (int)(Random.value * PlaceHolderInteractableObjects.Length);
		
		if (PlaceHolderInteractableObjects[InteractableIndex] != null)
		{
			Vector3 ObjectPosition = PlaceHolderInteractableObjects[InteractableIndex].transform.position;
			Vector3 ObjectRotation = PlaceHolderInteractableObjects[InteractableIndex].transform.rotation.eulerAngles;
			Destroy(PlaceHolderInteractableObjects[InteractableIndex].gameObject);
			PlaceHolderInteractableObjects[InteractableIndex] = null;
				
			Vector3 randomJitter = Random.insideUnitSphere * InteractablePlacementJitter;
			randomJitter.y = 0;
			
			Quaternion newRotation = Quaternion.Euler( ObjectRotation + new Vector3(0 ,Random.Range(0, InteractableRotationDegreeJitter * 2) - InteractableRotationDegreeJitter , 0));
			
			GameObject.Instantiate( Interactable, ObjectPosition + randomJitter, newRotation);
		}
	}
	
	public void ClearUnspawned()
	{
		for (int i = 0; i < PlaceHolderInteractableObjects.Length; i++)
		{
			if (PlaceHolderInteractableObjects[i] != null)
			{
				Destroy(PlaceHolderInteractableObjects[i]);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

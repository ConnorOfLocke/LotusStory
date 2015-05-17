using UnityEngine;
using System.Collections;

public class HexChunk : MonoBehaviour {

	public float FlavorPlacementJitter = 0.1f;
	public float FlavorRotationDegreeJitter = 5f;
	
	public float InteractablePlacementJitter = 0.1f;
	public float InteractableRotationDegreeJitter = 5f;
	
	public GameObject[] PlaceHolderFlavorObjects;
	public GameObject[] FlavorObjects;

	private int PlaceHolderObjectArraySize;
	public GameObject[] PlaceHolderInteractableObjects;

	private int HexChunkTypeID;
	
	// Use this for initialization
	void Start () {
	
		if (PlaceHolderFlavorObjects.Length == 0 ||
		    FlavorObjects.Length == 0 )
			Debug.Log("HexChunk: No PlaceHolderObjects or FlavorObjects Given");
		else
			SpawnFlavorObjects();
			
			
		PlaceHolderObjectArraySize = PlaceHolderInteractableObjects.Length;
			
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
	
	public bool SpawnInteractObject(GameObject Interactable)
	{
		int InteractableIndex = (int)(Random.value * PlaceHolderObjectArraySize);
		
		if (PlaceHolderInteractableObjects[InteractableIndex] != null)
		{
			//gets position then destroys it
			Vector3 ObjectPosition = PlaceHolderInteractableObjects[InteractableIndex].transform.position;
			Vector3 ObjectRotation = PlaceHolderInteractableObjects[InteractableIndex].transform.rotation.eulerAngles;
			Destroy(PlaceHolderInteractableObjects[InteractableIndex].gameObject);
			PlaceHolderInteractableObjects[InteractableIndex] = null;
		
			//moves it to the back of the array
			for (int i = InteractableIndex + 1; i < PlaceHolderInteractableObjects.Length; i++)
			{
				PlaceHolderInteractableObjects[i - 1] = PlaceHolderInteractableObjects[i];
			}
			PlaceHolderObjectArraySize -= 1;
			
			Vector3 randomJitter = Random.insideUnitSphere * InteractablePlacementJitter;
			randomJitter.y = 0;
			
			Quaternion newRotation = Quaternion.Euler( ObjectRotation + new Vector3(0 ,Random.Range(0, InteractableRotationDegreeJitter * 2) - InteractableRotationDegreeJitter , 0));
			
			GameObject.Instantiate( Interactable, ObjectPosition + randomJitter, newRotation);
		}
		
		return (PlaceHolderObjectArraySize <= 0);
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

using UnityEngine;
using System.Collections;

public class HexChunk : MonoBehaviour {

	public float PlacementJitter = 0.1f;
	public float RotationDegreeJitter = 5f;
	public GameObject[] PlaceHolderFlavorObjects;
	public GameObject[] FlavorObjects;

	public GameObject[] PlaceHolderInteractableObjects;
	public GameObject[] InteractableObjects;

	// Use this for initialization
	void Start () {
	
		if (PlaceHolderFlavorObjects.Length == 0 ||
		    FlavorObjects.Length == 0 )
			Debug.Log("HexChunk: No PlaceHolderObjects or FlavorObjects Given");
		else
			SpawnFlavorObjects();
			
		if (PlaceHolderInteractableObjects.Length == 0 ||
		    InteractableObjects.Length == 0 )
			Debug.Log("HexChunk: No PlaceHolderObjects or FlavorObjects Given");
		else
			SpawnInteractObjects();
		
	}
	
	private void SpawnFlavorObjects()
	{
		for (int i = 0; i < PlaceHolderFlavorObjects.Length; i++)
		{
			Vector3 ObjectPosition = PlaceHolderFlavorObjects[i].transform.position;
			Vector3 ObjectRotation = PlaceHolderFlavorObjects[i].transform.rotation.eulerAngles;
			Destroy(PlaceHolderFlavorObjects[i].gameObject);
			
			int randomIndex = (int)Random.Range(0, FlavorObjects.Length);
			
			Vector3 randomJitter = Random.insideUnitSphere * PlacementJitter;
			randomJitter.y = 0;
			
			Quaternion newRotation = Quaternion.Euler( ObjectRotation + new Vector3(Random.Range(0, RotationDegreeJitter) ,0 ,Random.Range(0, RotationDegreeJitter)));
			
			GameObject.Instantiate( FlavorObjects[randomIndex], ObjectPosition + randomJitter, newRotation);
		}
	}
	
	private void SpawnInteractObjects()
	{
		for (int i = 0; i < PlaceHolderInteractableObjects.Length; i++)
		{
			Vector3 ObjectPosition = PlaceHolderInteractableObjects[i].transform.position;
			Vector3 ObjectRotation = PlaceHolderInteractableObjects[i].transform.rotation.eulerAngles;
			Destroy(PlaceHolderInteractableObjects[i].gameObject);
			
			int randomIndex = (int)Random.Range(0, InteractableObjects.Length);
			
			Vector3 randomJitter = Random.insideUnitSphere * PlacementJitter;
			randomJitter.y = 0;
			
			Quaternion newRotation = Quaternion.Euler( ObjectRotation + new Vector3(Random.Range(0, RotationDegreeJitter) ,0 ,Random.Range(0, RotationDegreeJitter)));
			
			GameObject.Instantiate( InteractableObjects[randomIndex], ObjectPosition + randomJitter, newRotation);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

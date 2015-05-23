using UnityEngine;
using System.Collections;

public class TownBuilding : MonoBehaviour
{
	private MeshRenderer AttachedMeshRenderer;
	private Collider[] AttachedColliders;
	private Vector3 ActivePosition;
	private Vector3 InactivePosition;

	public bool BuildingActive;
	public GameObject RootHub = null;

	// Use this for initialization
	void Start ()
	{
		AttachedMeshRenderer = GetComponent<MeshRenderer>();
		AttachedColliders = GetComponents<Collider>();
		
		ActivePosition = transform.position;
		transform.position += new Vector3(0,-100, 0);
		InactivePosition = transform.position;

	}

	public bool GetActivated() {return BuildingActive;}

	public void Activate()
	{
		AttachedMeshRenderer = GetComponent<MeshRenderer>();
		AttachedColliders = GetComponents<Collider>();

		AttachedMeshRenderer.enabled = true;
		foreach (Collider c in AttachedColliders)
			c.enabled = true;
			
		BuildingActive = true;

		//transform.position = ActivePosition;
	}

	public void Deactiavte()
	{
		//AttachedMeshRenderer = GetComponent<MeshRenderer>();
		//AttachedColliders = GetComponents<Collider>();

		BuildingActive = false;

		//AttachedMeshRenderer.enabled = false;
		//foreach (Collider c in AttachedColliders)
		//	c.enabled = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (BuildingActive)
			transform.position = Vector3.Lerp(transform.position, ActivePosition, 0.1f);
		else
			transform.position = Vector3.Lerp(transform.position, InactivePosition, 0.1f);
			
	}


}

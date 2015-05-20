using UnityEngine;
using System.Collections;

public class TownBuilding : MonoBehaviour
{
	private MeshRenderer AttachedMeshRenderer;
	private Collider[] AttachedColliders;
	private Vector3 StartPosition;

	public bool BuildingActive;

	// Use this for initialization
	void Start ()
	{
		AttachedMeshRenderer = GetComponent<MeshRenderer>();
		AttachedColliders = GetComponents<Collider>();
		StartPosition = transform.position;

	}

	public bool GetActivated() {return BuildingActive;}

	public void Activate()
	{
		AttachedMeshRenderer = GetComponent<MeshRenderer>();
		AttachedColliders = GetComponents<Collider>();

		AttachedMeshRenderer.enabled = true;
		foreach (Collider c in AttachedColliders)
			c.enabled = true;

		transform.position = StartPosition;
	}

	public void Deactiavte()
	{
		//AttachedMeshRenderer = GetComponent<MeshRenderer>();
		//AttachedColliders = GetComponents<Collider>();

		//AttachedMeshRenderer.enabled = false;
		//foreach (Collider c in AttachedColliders)
		//	c.enabled = false;
	}
	
	// Update is called once per frame
	void Update ()
	{

	}


}


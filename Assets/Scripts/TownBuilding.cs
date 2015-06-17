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

	public float AddedAnger = 0.003f;

	private Pissed_O_Meter MeterInScene;

	// Use this for initialization
	void Start ()
	{
		AttachedMeshRenderer = GetComponent<MeshRenderer>();
		AttachedColliders = GetComponents<Collider>();
		
		ActivePosition = transform.position;
		transform.position += new Vector3(0,-100, 0);
		InactivePosition = transform.position;
		
		MeterInScene = FindObjectOfType<Pissed_O_Meter>();

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
		
		FollowMeCam cam = FindObjectOfType<FollowMeCam>();
		if (cam != null)
			cam.AddShake(0.3f);

		if (MeterInScene != null)
			MeterInScene.AddAngerModifier(AddedAnger);
		
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


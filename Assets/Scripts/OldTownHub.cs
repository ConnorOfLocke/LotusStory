using UnityEngine;
using System.Collections;

public class OldTownHub : MonoBehaviour
{
	public GameObject[] Buildings;
	public int MaxTiers = 3;
	public float BuildingDelination = 5.0f;
	public float BuildingDelinationPerTier = 5.0f;

	public int FirstBuildingNumber = 2;
	public int BuildingAdditionPerTier = 2;
	public float PlacementAngleJitter = 5.0f;
	
	public float TimeToSpawn = 3.0f;
	public float TimeToSpawnJitter = 1.0f;
	private float CurrentTimeToSpawn = 0.0f;

	private int BuildingsNextTier;
	
	private GameObject[] InstancedBuildings;

	void Start ()
	{
		BuildingsNextTier = FirstBuildingNumber;

		InstancedBuildings = new GameObject[FirstBuildingNumber + MaxTiers * BuildingAdditionPerTier];

		int curBuildingNum = 0;
		for (int i = 0, curTierBuildingNum = 0; i < MaxTiers; i++, curTierBuildingNum += BuildingAdditionPerTier)
		{
			float angleInstance = 360.0f / (float)BuildingsNextTier;
			float StartAngle = Random.Range(0, 720) - 360;

			int RandBuildingIndex = (int)Random.Range(0, Buildings.Length);
			float Angle = StartAngle + i * angleInstance + Random.Range(0, 2 * PlacementAngleJitter) - PlacementAngleJitter;

			for (;curBuildingNum < curTierBuildingNum; curBuildingNum++)
			{
				//spawn the thing
				Vector3 BuildingPos = transform.position + new Vector3( Mathf.Cos(Angle * Mathf.Deg2Rad),0, Mathf.Sin(Angle * Mathf.Deg2Rad))* BuildingDelination;
				
				GameObject newBuilding = GameObject.Instantiate(Buildings[RandBuildingIndex], BuildingPos, Quaternion.identity) as GameObject;
				newBuilding.transform.position = new Vector3(10000, 100000, 10000);

				TownBuilding attachedBuildScript = newBuilding.GetComponent<TownBuilding>();
				if (attachedBuildScript != null)
					attachedBuildScript.Deactiavte();

				InstancedBuildings[curBuildingNum] = newBuilding;
				CurrentTimeToSpawn = 0;
			}
		}
	}
	
	void Update ()
	{
		CurrentTimeToSpawn += Time.deltaTime;

		if (CurrentTimeToSpawn >= TimeToSpawn)
		{
			if (CanExpand())
			{
				BuildingDelination += BuildingDelinationPerTier;
				BuildingsNextTier += BuildingAdditionPerTier;
			}
			else
			{
				//for getting the highest tier of building that have been spawned
				int MaxTier = -1;
				for (int i = 0; i < InstancedBuildings.Length; i++)
				{
					TownBuilding attachedBuildScript = InstancedBuildings[i].GetComponent<TownBuilding>();

					if (attachedBuildScript != null)
					{
						if (!attachedBuildScript.GetActivated())
						{
							attachedBuildScript.Activate();
							if (MaxTier == -1)
								MaxTier = (i - FirstBuildingNumber) / BuildingAdditionPerTier;
							else if (i < FirstBuildingNumber + MaxTier * BuildingAdditionPerTier)
								i = InstancedBuildings.Length;
						}
					}
				}
			}

		}
	}

	bool CanExpand()
	{
		foreach( GameObject building in InstancedBuildings)
		{
			TownBuilding attachedBuildScript = building.GetComponent<TownBuilding>();
			if (attachedBuildScript != null)
				if (attachedBuildScript.GetActivated())
					return true;
		}
		return false;
	}

}


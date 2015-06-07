using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TownHealthBarController : MonoBehaviour
{
	private List<GameObject> TownList;
	public HealthBar_Vertical AttachedHealthBar;

	private float OriginalTowns;

	public void Initialise( List<GameObject> a_TownList)
	{
		TownList = a_TownList;
		OriginalTowns = a_TownList.Count;
		
		AttachedHealthBar.CurrentHealthScale = (float)TownList.Count / OriginalTowns;
	}

	void Update ()
	{
		//check all the towns are still alive
		if (TownList != null)
		{
			for (int i = 0 ; i < TownList.Count; i++)
			{
				if (TownList[i] == null)
				{
					TownList.RemoveAt(i);
					i--;
				}
			}
			
			AttachedHealthBar.CurrentHealthScale = (float)TownList.Count / OriginalTowns;
		}
	}
}


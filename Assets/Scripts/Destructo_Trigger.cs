using UnityEngine;
using System.Collections;

public class Destructo_Trigger : MonoBehaviour {

	
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "House")
		{
			if (other.gameObject.GetComponent<TownBuilding>() != null)
				other.gameObject.GetComponent<TownBuilding>().Deactiavte();
			else if (other.gameObject.GetComponent<TownHub>() != null)
				Destroy(other.gameObject);
		}
	}
}

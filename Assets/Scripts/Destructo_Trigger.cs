using UnityEngine;
using System.Collections;

public class Destructo_Trigger : MonoBehaviour {

	public Collusus PossiblyAttachedCollusus;
	
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "House")
		{
			if (other.gameObject.GetComponent<TownBuilding>() != null)
			{
				other.gameObject.GetComponent<TownBuilding>().Deactiavte();
				if (PossiblyAttachedCollusus != null)
				{
					if (other.gameObject.GetComponent<TownBuilding>().RootHub != null)
					PossiblyAttachedCollusus.SetDetour( other.gameObject.GetComponent<TownBuilding>().RootHub.transform.position );
				}
			}
			else if (other.gameObject.GetComponent<TownHub>() != null)
			{
				Destroy(other.gameObject);
				FollowMeCam cam = FindObjectOfType<FollowMeCam>();
				if (cam != null)
					cam.AddShake(1.0f);
			}
			else if(other.tag == "Denizen")
			{
				Destroy(other.gameObject);
			}

		}
	}
}

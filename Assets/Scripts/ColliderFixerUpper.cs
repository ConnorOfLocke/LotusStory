using UnityEngine;
using System.Collections;

public class ColliderFixerUpper : MonoBehaviour
{
	void Update ()
	{
		//does nothing but make the attached collider give out collision calls
		transform.position += Vector3.zero;
	}
}


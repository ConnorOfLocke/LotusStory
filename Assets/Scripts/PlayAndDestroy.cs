using UnityEngine;
using System.Collections;

public class PlayAndDestroy : MonoBehaviour
{
	public AudioSource Audio;
	private float TimePlaying;
	private bool Activated = false;

	public void Activate()
	{
		TimePlaying = Audio.clip.length;
		Audio.Play();
		Activated = true;
	}

	// Update is called once per frame
	void Update ()
	{
		if (Activated)
		{
			if (TimePlaying <= 0)
			{
				Destroy(this.gameObject);
			}
		
			TimePlaying -= Time.deltaTime;
		}
	}
}


using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelLoader : MonoBehaviour {

	public GameObject[] DestroyOnLoad;
	public Image[] FadeOutAndDestroy;
	
	public float timeToFade = 2.0f;
	private float currentTimeLeft = 0.0f;

	private bool activated = false;
	
	void Start()
	{
		currentTimeLeft = timeToFade;
	}
	
	public void LOAD_EM_UP()
	{
		Application.LoadLevelAdditive("mainScene");
		
		foreach (GameObject thing in DestroyOnLoad)
		{
			Destroy(thing);
		}
		activated = true;
	}
	
	public void Update()
	{
		if (activated)
		{
			foreach(Image image in FadeOutAndDestroy)
				image.color = new Color(image.color.r, image.color.g, image.color.b, currentTimeLeft / timeToFade);
			
			if (currentTimeLeft < 0)
			{
				foreach(Image image in FadeOutAndDestroy)
					Destroy(image.gameObject);
				
				Destroy(this.gameObject);
			}
			else
				currentTimeLeft -= Time.deltaTime;
			
		}
		
		
	}
}

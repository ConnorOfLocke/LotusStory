using UnityEngine;
using System.Collections;

public class ProjectorLightFadeInAndExpand : MonoBehaviour {

	public Projector AttachedProjector;
	public Light AttachedLilght;

	public Color InactiveColor;
	public Color ActiveColor;

	public float InitialSize = 10;	
	public float MaxSize = 30;
	public float SizeIncrease = 1.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
		if (AttachedProjector.orthographicSize < MaxSize)
			AttachedProjector.orthographicSize += SizeIncrease * Time.deltaTime;
			
		AttachedProjector.material.color = Color.Lerp(InactiveColor, ActiveColor, (AttachedProjector.orthographicSize / MaxSize));
		
	}
}

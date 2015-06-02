using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBar_Vertical : MonoBehaviour
{
	public Image HealthBar;
	public Image BackgroundToScaleTo;
	
	public float CurrentHealthScale = 1.0f;
	public float LerpFactor = 0.2f;
	
	void Start ()
	{

	}

	void Update ()
	{
		float MaxManaScale = BackgroundToScaleTo.rectTransform.rect.height;
		float CurrentHealth = CurrentHealthScale * MaxManaScale;
		
		//shakey shakes
		if (Mathf.Abs(HealthBar.rectTransform.offsetMax.magnitude - new Vector2(HealthBar.rectTransform.offsetMax.x, CurrentHealth).magnitude) > 1.0f)
		{
			Vector2 jitter = Random.insideUnitSphere * 2;
			transform.position += new Vector3(jitter.x, jitter.y, 0);
		}
		
		HealthBar.rectTransform.offsetMax = Vector2.Lerp(	HealthBar.rectTransform.offsetMax,
		                                                 	new Vector2(HealthBar.rectTransform.offsetMax.x, CurrentHealth),
		                                               		LerpFactor);
		
		
	}
}


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
		
		HealthBar.rectTransform.offsetMax = Vector2.Lerp(	HealthBar.rectTransform.offsetMax,
		                                                 new Vector2(HealthBar.rectTransform.offsetMax.x, CurrentHealth),
		                                               		LerpFactor);
		
		
	}
}


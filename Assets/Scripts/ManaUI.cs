using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ManaUI : MonoBehaviour {

	public PlayerContoller AttachedPlayer;
	public Image BackgroundBarToScaleTo;
	public Image ManaBar;
	public Image CurrentSpellBar;
	public float LerpFactor = 0.2f;

	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
		float MaxManaScale = BackgroundBarToScaleTo.rectTransform.rect.width;

		float CurrMinManaPower = AttachedPlayer.CurrMinManaPower;
		float CurrMaxManaPower = AttachedPlayer.CurrMaxManaPower;
		float MaxManaPower = AttachedPlayer.MaxManaPower;
		
		float CurPower = AttachedPlayer.SpellHoldTime;
		
		float ManaLeftX = (CurrMinManaPower / MaxManaPower) * MaxManaScale;
		float ManaRightX = (CurrMaxManaPower / MaxManaPower) * MaxManaScale;
		
		float CurPowerRightX = (CurPower / MaxManaPower) * MaxManaScale;


		ManaBar.rectTransform.offsetMin = Vector2.Lerp(	ManaBar.rectTransform.offsetMin,
		                                                new Vector2(ManaLeftX, ManaBar.rectTransform.offsetMin.y),
		                                              	LerpFactor);
													  
		ManaBar.rectTransform.offsetMax = Vector2.Lerp(	ManaBar.rectTransform.offsetMax,
		                                               new Vector2(ManaRightX, ManaBar.rectTransform.offsetMax.y),
		                                               	LerpFactor);
		                                               	
		CurrentSpellBar.rectTransform.offsetMin = Vector2.Lerp(	CurrentSpellBar.rectTransform.offsetMin,
		                                                       new Vector2(ManaLeftX, CurrentSpellBar.rectTransform.offsetMin.y),
		                                               			LerpFactor);
		
		CurrentSpellBar.rectTransform.offsetMax = Vector2.Lerp(	CurrentSpellBar.rectTransform.offsetMax,
		                                                       new Vector2(CurPowerRightX, CurrentSpellBar.rectTransform.offsetMax.y),
		                                               			LerpFactor);
		
	}
}

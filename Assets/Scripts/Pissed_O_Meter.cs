using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Pissed_O_Meter : MonoBehaviour
{
	public Collusus AttachedCollusus;
	public List<GameObject> TownList;
	public float CurrentAnger;
	
	private float CollususStartHealth;
	private float OriginalTownNumber;
	
	private Scrollbar AttachedScrollBar;
	
	private bool isInitialised = false;
	
	public float currentModifier = 0.0f;
	
	public Vector3 ActivePosition;
	public Vector3 InActivePosition;
	public float TimeActive = 1.0f;
	private Vector3 CurrentPosition;
	private float currentTimeActive = 0;
	
	public void Initialise( List<GameObject> a_TownList)
	{
		TownList = a_TownList;
		OriginalTownNumber = a_TownList.Count;
		isInitialised = true;
		
		CollususStartHealth = AttachedCollusus.MaxHealth;
		AttachedScrollBar = GetComponent<Scrollbar>();
		UpdateMeter();
		
		GetComponent<RectTransform>().localPosition = InActivePosition;
		CurrentPosition = InActivePosition;
	}
	
	public void AddAngerModifier(float amount)
	{
		currentModifier += amount;
	}
	
	void Start ()
	{
	
	}

	private void UpdateMeter()
	{
		if (isInitialised)
		{
			for (int i = 0; i < TownList.Count; i++)
			{
				if (TownList[i] == null)
				{
					TownList.RemoveAt(i);
					currentTimeActive = TimeActive;
					CurrentPosition = ActivePosition;
				}
			}
		
			float CollususRatio = AttachedCollusus.Health / CollususStartHealth;
			float TownRatio = (float)TownList.Count / OriginalTownNumber;
			
			float FinalValue = ((TownRatio - CollususRatio + currentModifier) + 1) * 0.5f;
			float tempVelocity = 0.0f;
			float newValue = Mathf.SmoothDamp( AttachedScrollBar.value, FinalValue, ref tempVelocity, 0.1f);
			
			//not within a small range
			if (!(AttachedScrollBar.value < newValue + 0.0001f && AttachedScrollBar.value > newValue - 0.0001f))
			{
				CurrentPosition = ActivePosition;
				currentTimeActive = TimeActive;
			}
			AttachedScrollBar.value = newValue;
			CurrentAnger = newValue;
			
		}
		
		Vector3 Velocity = Vector3.zero;
		Vector3 PosToBe = Vector3.SmoothDamp(GetComponent<RectTransform>().localPosition, CurrentPosition ,ref Velocity, 0.1f );
		
		if (currentTimeActive > 0)
		{
			currentTimeActive -= Time.deltaTime;
			Vector2 jitter =  Random.insideUnitCircle * (currentTimeActive / TimeActive) * 0.01f;
			PosToBe += new Vector3(jitter.x, jitter.y, 0);
		}
		else
			CurrentPosition = InActivePosition;
		
		GetComponent<RectTransform>().localPosition = PosToBe;
	}

	// Update is called once per frame
	void Update ()
	{
		UpdateMeter();
	}
}


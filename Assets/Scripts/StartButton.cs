﻿using UnityEngine;
using System.Collections;

public class StartButton : MonoBehaviour {

	public void LoadMainScene()
	{
		Application.LoadLevelAdditive("mainScene");
	}
}
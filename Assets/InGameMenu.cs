using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMenu : MonoBehaviour {

	// Use this for initialization
	public void Exit() {
		Application.Quit();
	}
	public void GoBack() {
		GameController.LeaveMenu();

		
	}
}

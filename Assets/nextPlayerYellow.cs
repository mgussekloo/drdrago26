using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nextPlayerYellow : MonoBehaviour {

	// Use this for initialization
	public GameObject rollDiceOject;
	public GameObject objectPlayer;
	public GameObject gameManager;
	
	// Use this for initialization
		
	public void onClickEnter(){
		objectPlayer.transform.GetComponent<playerMove>().loadPlayerCards();
		gameObject.SetActive(false);
		gameManager.GetComponent<gameManager>().activateNexPlayer();
		//rollDiceOject.SetActive(true);
	}
}

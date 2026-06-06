using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Security.Cryptography;


public class purpleEndPoint : MonoBehaviour {
	private GameObject endturnScreen;
	
	public void confirmEndPoint(GameObject playerObject){
		endturnScreen = GameObject.Find("Canvas");
		endturnScreen = endturnScreen.transform.Find("drawCards").gameObject ;
		endturnScreen.SetActive(true);
        endturnScreen.GetComponent<interActCards>().makeInteractable(playerObject);

		//Debug.Log("you can buy a feature card");
		// you can buy a feature card - provided you have enough money in your account
	}
}
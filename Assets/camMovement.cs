using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camMovement : MonoBehaviour {

	// Use this for initialization
	public GameObject activePlayer;

	// Update is called once per frame
	void Update () {
		
		if(activePlayer != null){
			transform.position = new Vector3 (activePlayer.transform.position.x, activePlayer.transform.position.y, -9.32f) ;
		}
	}
}

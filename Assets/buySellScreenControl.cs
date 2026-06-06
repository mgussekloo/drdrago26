using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buySellScreenControl : MonoBehaviour {

	public GameObject gameLookObject;
	// Use this for initialization
	
	public void destroyChildEstates(){
	Transform[] tmpRealEstate = transform.GetComponentsInChildren<Transform>();
		//	Debug.Log(tmpRealEstate[0].name+tmpRealEstate[1].name);
			for(int i=0; i<tmpRealEstate.Length; i++){
				if(tmpRealEstate[i].tag == "realEstateObject"){
					Destroy(tmpRealEstate[i].gameObject);
				}
			}
	}

 
}



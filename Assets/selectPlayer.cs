using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Security.Cryptography;

public class selectPlayer : MonoBehaviour {

	// Use this for initialization
	public List<Sprite> characterSprites = new List<Sprite>();
	public GameObject gameManagerObject;
	public bool AIPlayer = true;
	private int spriteID = 2;
	public GameObject characterSubmitObject;

	public void enterPlayerSelected(){
		
		if(spriteID == 0 ){
			spriteID = 1;
			AIPlayer=true;
			this.GetComponent<Image>().sprite  = characterSprites[spriteID];
			gameManagerObject.GetComponent<gameManager>().submitCharacter(this.gameObject, characterSubmitObject);
		}
		else if ( spriteID == 1){
			spriteID = 2;
			this.GetComponent<Image>().sprite  = null;
			this.GetComponent<Image>().color = new Color (1f,1f,1f,0f);
			gameManagerObject.GetComponent<gameManager>().removeCharacter(this.gameObject, characterSubmitObject);
		}
		else if(spriteID == 2){
			spriteID = 0;

			this.GetComponent<Image>().sprite  = characterSprites[spriteID];
			this.GetComponent<Image>().color = new Color (1f,1f,1f,1f);
            AIPlayer=false;
			gameManagerObject.GetComponent<gameManager>().submitCharacter(this.gameObject, characterSubmitObject);
		}		
	}
}
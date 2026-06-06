using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Security.Cryptography;

public class rollDice : MonoBehaviour {

	public Sprite[] diceSprit;
	private int timeClicked = 0;
	public GameObject loadGameLook;
	
	// Use this for initialization
	void Start () {
        transform.GetChild(0).transform.GetComponent<Animator>().enabled = false;
    }
	
	

    public void onClickEnter(){
		
		if (timeClicked == 0){
			gameObject.GetComponent<AudioSource>().Play();
		transform.GetChild(0).transform.GetComponent<Animator>().enabled = true;
		transform.GetComponent<Button>().interactable = false; 
		 StartCoroutine(rollDiceCoroutine());
		 timeClicked = timeClicked + 1;
		}
		else{
			//Debug.Log("disable dice");
			timeClicked = 0;
			this.gameObject.SetActive(false);
		}
	}

    public void enterAIRoll()
    {
        gameObject.GetComponent<AudioSource>().Play();
        transform.GetChild(0).transform.GetComponent<Animator>().enabled = true;
        StartCoroutine(rollDiceCoroutine());
    }

    IEnumerator rollDiceCoroutine() {
			
		yield return new WaitForSeconds(2.0f);
		gameObject.GetComponent<AudioSource>().Stop();
		transform.GetChild(0).gameObject.GetComponent<AudioSource>().Play();
        int diceValue = Random.Range(1, 6);
        int tmpRollesDiceLeft = loadGameLook.GetComponent<gameLookFunc>().playerObject.GetComponent<playerMove>().rolledDiceStepsLeft + diceValue;


        transform.GetChild(0).transform.GetComponent<Animator>().enabled = false;
		transform.GetChild(0).transform.GetComponent<Image>().sprite = diceSprit[diceValue-1];
		loadGameLook.GetComponent<gameLookFunc>().setThrownDices(tmpRollesDiceLeft);

        loadGameLook.GetComponent<gameLookFunc>().playerObject.GetComponent<playerMove>().rolledDiceStepsLeft = tmpRollesDiceLeft;
        if (loadGameLook.GetComponent<gameLookFunc>().playerObject.GetComponent<playerMove>().playerIsAI == 0) { 
            transform.GetComponent<Button>().interactable = true; 
        }
        else
        {
            yield return new WaitForSeconds(1.0f);
            this.gameObject.SetActive(false);
            loadGameLook.GetComponent<gameLookFunc>().playerObject.GetComponent<playerMove>().moveTheAIPlayerNow();

        }
    }
   
}

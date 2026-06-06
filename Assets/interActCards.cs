using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Security.Cryptography;

using System.Data;
using Mono.Data.SqliteClient;

public class interActCards : MonoBehaviour {

	private IDbConnection _dbc;
	private IDbCommand _dbcm;
	private IDataReader _dbr;

	private int playerMoney;
	private List<Transform> tmpAIBuyCards;
	private int buyRandomCard;
	public void makeInteractable(GameObject playerObject){

        string _constr = "";
        if (Application.isEditor){
            string dbPath = System.IO.Path.Combine(Application.streamingAssetsPath, "drdragoDB.db");
            _constr = "URI=file:" + dbPath;
        }
        else{
            string dbPath = System.IO.Path.Combine(Application.persistentDataPath, "drdragoDB.db");
            _constr = "URI=file:" + dbPath;

        }
        //string _constr="URI=file:"+ Application.persistentDataPath +"/drdragoDB.db";
		_dbc=new SqliteConnection(_constr);
		_dbc.Open();
		_dbcm=_dbc.CreateCommand();
		_dbcm.CommandText="SELECT playerMoney FROM `playerInfo`  WHERE `id`='"+playerObject.GetComponent<playerMove>().playerID+"'";
		_dbr=_dbcm.ExecuteReader();
		while( _dbr.Read()){
			playerMoney = _dbr.GetInt32(0);
		}
		
		_dbc.Close();
		
		Transform[] tmpLoadedCards = this.transform.GetComponentsInChildren<Transform>();
		
		if(playerObject.GetComponent<playerMove>().playerIsAI == 0)
        {
			for(int i=0; i<tmpLoadedCards.Length; i++){
				if(tmpLoadedCards[i].tag == "playerCards"){
					//Debug.Log("this is card");
					if (playerMoney < tmpLoadedCards[i].GetComponent<cardProperties>().price){
						//Debug.Log("I do not get enough money");
						tmpLoadedCards[i].GetComponent<Button>().interactable = false;
					}
					else{
						tmpLoadedCards[i].GetComponent<cardProperties>().playerObject = playerObject;
					}
				}
			}
		}
		else{
			tmpAIBuyCards = new List<Transform>();
			for(int i=0; i<tmpLoadedCards.Length; i++){
				if(tmpLoadedCards[i].tag == "playerCards"){
					if (playerMoney > tmpLoadedCards[i].GetComponent<cardProperties>().price){
						tmpAIBuyCards.Add(tmpLoadedCards[i]);
					}
				}
			}
			if(tmpAIBuyCards != null){
				int amountOfCardsToBuy = tmpAIBuyCards.Count;
				buyRandomCard = Random.Range(1,amountOfCardsToBuy);
				buyRandomCard = buyRandomCard -1;
				Debug.Log("AI bought random card " + tmpAIBuyCards[buyRandomCard].name);
				tmpAIBuyCards[buyRandomCard].GetComponent<cardProperties>().playerObject = playerObject;
				StartCoroutine(showAIActionsCoroutine());
				
			
			}
		}
	}
	
	 IEnumerator showAIActionsCoroutine(){
			yield return new WaitForSeconds(1.0f);
			tmpAIBuyCards[buyRandomCard].GetComponent<cardProperties>().enterBuyCard();
			
    }
	
}
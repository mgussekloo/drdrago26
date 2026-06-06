using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Security.Cryptography;

using System.Data;
using Mono.Data.SqliteClient;

public class confirmBuyCard : MonoBehaviour {

	public GameObject gameManager;
	private int tmpPriceMoney;
	private string tmpCardName;
	
	private GameObject tmpPlayerObject;
	private IDbConnection _dbc;
	private IDbCommand _dbcm;
	private IDataReader _dbr;
	private int playerMoney;
	
	public void enterConfirmBuyCard(){
	
		int idPlayer = tmpPlayerObject.transform.GetComponent<playerMove>().playerID;
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
		_dbcm.CommandText="SELECT playerMoney FROM `playerInfo`  WHERE `id`='"+idPlayer+"'";
		_dbr=_dbcm.ExecuteReader();
		while( _dbr.Read()){
			playerMoney = _dbr.GetInt32(0);
		}
		
	
		int tmpMoneyPlayer = playerMoney;
		if(tmpMoneyPlayer >= tmpPriceMoney){
			
			int tmpMoney = tmpMoneyPlayer - tmpPriceMoney;
			_dbcm=_dbc.CreateCommand();
			_dbcm.CommandText="UPDATE playerInfo SET playerMoney = "+tmpMoney+" WHERE `id`='"+idPlayer+"'";
			_dbr=_dbcm.ExecuteReader();
			
			gameManager.GetComponent<gameManager>().showPlayerMoney(idPlayer);
			
			_dbcm=_dbc.CreateCommand();
			_dbcm.CommandText="INSERT INTO playerCards (playerID, cardName) VALUES ('"+idPlayer+"', '"+tmpCardName+"')";
			_dbr=_dbcm.ExecuteReader();
			_dbc.Close();
			
			tmpMoneyPlayer = tmpMoneyPlayer - tmpPriceMoney;
			ZPlayerPrefs.SetInt("playerMoney",tmpMoneyPlayer);
			returnBack();
			tmpPlayerObject.transform.GetComponent<playerMove>().loadPlayerCards();
			
		}
	
	}
	
	public void denyBuyCard(){
		returnBack();
	}
	
	private void returnBack(){
		gameObject.SetActive(false);
		
	}
	
	public void buyCard(int price, string cardName, GameObject playerObject){
		this.transform.GetChild(2).GetComponent<Text>().text = "Are you sure to buy this card for "+price+ "?!?!?";
		tmpPriceMoney = price;
		tmpCardName = cardName;
		tmpPlayerObject = playerObject;
	}
}
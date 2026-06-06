using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Security.Cryptography;

using System.Data;
using Mono.Data.SqliteClient;

public class buySellRealEstate : MonoBehaviour {

	public int priceProperty;
	public int profitEstate;
	public int instanceNumber;
	public int goldBoolean;
	public int? ownerRealEstate;
	private GameObject confirmScreen;
	public GameObject activeWayPoint;
	
	private IDbConnection _dbc;
	private IDbCommand _dbcm;
	private IDataReader _dbr;
	
	private int playerMoney ;
	private string tmpName;

	private GameObject gameLook;
	// Use this for initialization
	
	public void onClickBuySell(){
		
		gameLook = this.transform.parent.transform.parent.Find("gameLook").gameObject ;
		int playerID = gameLook.GetComponent<gameLookFunc>().playerObject.GetComponent<playerMove>().playerID;
		
        string _constr = "";
        if (Application.isEditor){
            string dbPath = System.IO.Path.Combine(Application.streamingAssetsPath, "drdragoDB.db");
            _constr = "URI=file:" + dbPath;
        }
        else{
            string dbPath = System.IO.Path.Combine(Application.persistentDataPath, "drdragoDB.db");
            _constr = "URI=file:" + dbPath;

        }
       
		_dbc=new SqliteConnection(_constr);
		_dbc.Open();
		
		if(ownerRealEstate == 0){
			//Debug.Log("is from nobody");
			
			_dbcm=_dbc.CreateCommand();
			_dbcm.CommandText="SELECT playerMoney FROM `playerInfo`  WHERE `id`='"+playerID+"'";
			_dbr=_dbcm.ExecuteReader();
			
			while( _dbr.Read()){
				playerMoney = _dbr.GetInt32(0);
			}

			if(playerMoney >= priceProperty){
				_dbcm=_dbc.CreateCommand();
			_dbcm.CommandText="SELECT name FROM `playerInfo`  WHERE `id`='"+playerID+"'";
			_dbr=_dbcm.ExecuteReader();
			
			while( _dbr.Read()){
				 tmpName = _dbr.GetString(0);
			}
			_dbc.Close();

                //Debug.Log(tmpName + "you can buy real estate");
                confirmScreen = this.transform.parent.Find("confirmBuySell").gameObject ;
				//string tmpName = ZPlayerPrefs.GetString("playerCharName");
				string tmpText = "to buy";
               
				confirmScreen.GetComponent<confirmBuySellScript>().buyEstate(priceProperty, tmpName,activeWayPoint,instanceNumber,goldBoolean, ownerRealEstate,profitEstate,tmpText);
               
                confirmScreen.SetActive(true);
               
                confirmScreen.transform.SetAsLastSibling();
                if (gameLook.GetComponent<gameLookFunc>().playerObject.GetComponent<playerMove>().playerIsAI==1) { 
                StartCoroutine(showBuyAIActionsCoroutine());
                }
            }
		}
		else if(playerID == ownerRealEstate){
			
			_dbcm=_dbc.CreateCommand();
			_dbcm.CommandText="SELECT name FROM `playerInfo`  WHERE `id`='"+playerID+"'";
			_dbr=_dbcm.ExecuteReader();
			
			while( _dbr.Read()){
				tmpName = _dbr.GetString(0);
			}
			_dbc.Close();
			
			confirmScreen = this.transform.parent.Find("confirmBuySell").gameObject ;
			//string tmpName = ZPlayerPrefs.GetString("playerCharName");
			string tmpText = "to sell";
			confirmScreen.GetComponent<confirmBuySellScript>().buyEstate(priceProperty, tmpName,activeWayPoint,instanceNumber,goldBoolean, ownerRealEstate,profitEstate, tmpText);
				confirmScreen.SetActive(true);
				confirmScreen.transform.SetAsLastSibling();
		}
	}
    IEnumerator showBuyAIActionsCoroutine()
    {
       yield return new WaitForSeconds(1.0f);
       confirmScreen.GetComponent<confirmBuySellScript>().confirmBuySell();
    }

    public void updatePriceProperty()
    {
        transform.GetChild(3).GetComponent<Text>().text = string.Format("{0:c0}", priceProperty);
    }
}
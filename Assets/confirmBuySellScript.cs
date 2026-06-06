using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Security.Cryptography;

using System.Data;
using Mono.Data.SqliteClient;

public class confirmBuySellScript : MonoBehaviour {

	private int tmpPriceMoney;
	private string tmpCharName;
	private int tmpSelectedInstance;
	private GameObject tmpChangingWayPoint;
	private string ownerFaceDefine;
	private IDbConnection _dbc;
	private IDbCommand _dbcm;
	private IDataReader _dbr;
	private int tmpGoldBool;
	private int? tmpOwnerEstate;
	private int tmpProfitEstate;
	private int tmpplayerProfit;
	public GameObject gameManager;
	private GameObject tmpPlayerObject;
	private int playerMoney;
    private int playerRevenue;
    private int playerEstateValue;



    public void buyEstate(int priceMoney, string charName, GameObject changingWayPoint,int selectedInstance, int goldBool, int? ownerEstate, int profitEstate, string textBuySell){
       
        this.transform.GetChild(2).GetComponent<Text>().text = "Are you sure "+textBuySell+ "?!?!?";  
        tmpPriceMoney = priceMoney;
		tmpCharName = charName;
		tmpSelectedInstance = selectedInstance;
		tmpChangingWayPoint=changingWayPoint;
		tmpGoldBool = goldBool;
		tmpOwnerEstate = ownerEstate;
		tmpProfitEstate = profitEstate;
    }

	public void confirmBuySell(){
        string _constr = "";
        if (Application.isEditor){
            string dbPath = System.IO.Path.Combine(Application.streamingAssetsPath, "drdragoDB.db");
            _constr = "URI=file:" + dbPath;
        }
        else{
            string dbPath = System.IO.Path.Combine(Application.persistentDataPath, "drdragoDB.db");
            _constr = "URI=file:" + dbPath;

        }

        _dbc =new SqliteConnection(_constr);
        _dbc.Open();

        GameObject tmpObject = transform.parent.GetComponent<buySellScreenControl>().gameLookObject;

		tmpPlayerObject = tmpObject.transform.GetComponent<gameLookFunc>().playerObject;
		
		if(tmpOwnerEstate==0){
			//if(ZPlayerPrefs.GetInt("playerMoney") >= tmpPriceMoney){
				tmpOwnerEstate = tmpPlayerObject.transform.GetComponent<playerMove>().playerID;
				
				_dbcm=_dbc.CreateCommand();
			_dbcm.CommandText="SELECT playerMoney, revenue, estateValue FROM `playerInfo`  WHERE `id`='"+tmpOwnerEstate+"'";
			_dbr=_dbcm.ExecuteReader();
			
			while( _dbr.Read()){
			playerMoney = _dbr.GetInt32(0);
                playerRevenue = _dbr.GetInt32(1);
                playerEstateValue = _dbr.GetInt32(2);
		}
		
				int tmpMoney = playerMoney - tmpPriceMoney;
                int tmpRevenue = playerRevenue - tmpPriceMoney;
            int tmpEstateValue = playerEstateValue + tmpPriceMoney;
				_dbcm=_dbc.CreateCommand();
				_dbcm.CommandText="UPDATE playerInfo SET playerMoney = "+tmpMoney+", revenue = "+tmpRevenue+", estateValue = "+tmpEstateValue+" WHERE `id`='"+tmpOwnerEstate+"'";
				_dbr=_dbcm.ExecuteReader();
		
				gameManager.GetComponent<gameManager>().showPlayerMoney(tmpOwnerEstate);
				defineCharFace();
				_dbcm=_dbc.CreateCommand();
				_dbcm.CommandText="UPDATE cities SET playerID='" + tmpOwnerEstate + "', ownerFace='"+ownerFaceDefine+"' WHERE id='"+tmpSelectedInstance+"'";
				_dbr=_dbcm.ExecuteReader();
				getPlayerProfit();
				tmpplayerProfit = tmpplayerProfit + tmpProfitEstate;
				setPlayerProfit();
				returnBack();
		}
		else{
			
			//update ownerName=tmpCharName into cities where id = selectedInstance 
			if(tmpGoldBool==1){
				ownerFaceDefine = "0M";
			}
			else{
				ownerFaceDefine="0N";
			}
			
			_dbcm=_dbc.CreateCommand();
			_dbcm.CommandText="SELECT playerMoney, revenue FROM `playerInfo`  WHERE `id`='"+tmpOwnerEstate+"'";
			_dbr=_dbcm.ExecuteReader();
			
			while( _dbr.Read()){
			    playerMoney = _dbr.GetInt32(0);
                playerRevenue = _dbr.GetInt32(1);
		    }
		
				int tmpMoney = playerMoney + tmpPriceMoney;
                int tmpRevenue = playerRevenue + tmpPriceMoney;
            int tmpEstateValue = playerEstateValue - tmpPriceMoney;
            _dbcm =_dbc.CreateCommand();
				_dbcm.CommandText="UPDATE playerInfo SET playerMoney = "+tmpMoney+", revenue = "+tmpRevenue+ ", estateValue = " + tmpEstateValue + " WHERE `id`='" + tmpOwnerEstate+"'";
				_dbr=_dbcm.ExecuteReader();
			gameManager.GetComponent<gameManager>().showPlayerMoney(tmpOwnerEstate);
			
			tmpOwnerEstate = null;
			
			_dbcm=_dbc.CreateCommand();
			_dbcm.CommandText="UPDATE cities SET playerID=NULL,ownerFace='"+ownerFaceDefine+"' WHERE id='"+tmpSelectedInstance+"'";
			_dbr=_dbcm.ExecuteReader();
			
			getPlayerProfit();
			tmpplayerProfit = tmpplayerProfit - tmpProfitEstate;
			setPlayerProfit();
			//_dbc.Close();
			returnBack();			
		}
		_dbc.Close();
	}

    public void denyBuySell()
    {
        returnBack();
    }
    private void setPlayerProfit(){
		
		_dbcm=_dbc.CreateCommand();
		_dbcm.CommandText="UPDATE playerInfo SET profit='" + tmpplayerProfit + "' WHERE id='"+tmpPlayerObject.transform.GetComponent<playerMove>().playerID+"'";
		_dbr=_dbcm.ExecuteReader();
	}
	
	private void getPlayerProfit(){
		
		_dbcm=_dbc.CreateCommand();
				_dbcm.CommandText="SELECT profit FROM `playerInfo` WHERE id='"+tmpPlayerObject.transform.GetComponent<playerMove>().playerID+"'";
				_dbr=_dbcm.ExecuteReader();
				
				while( _dbr.Read()){
					tmpplayerProfit = _dbr.GetInt32(0);
				}
	}
	
	private void returnBack(){

		transform.parent.GetComponent<buySellScreenControl>().destroyChildEstates();
		gameObject.SetActive(false);
		transform.parent.gameObject.SetActive(false);
		tmpChangingWayPoint.GetComponent<grayEndPoint>().confirmEndPoint();
	}
	
	private void defineCharFace(){
				
		if(tmpCharName == "Helmut"){
			if(tmpGoldBool == 1){
				ownerFaceDefine = "D_M";
			}
			else{
				ownerFaceDefine = "D_N";
			}
		}
		else if(tmpCharName == "James"){
			if(tmpGoldBool == 1){
				ownerFaceDefine = "E_M";
			}
			else{
				ownerFaceDefine = "E_N";
			}
		}
		else if(tmpCharName == "Luigi"){
			if(tmpGoldBool == 1){
				ownerFaceDefine = "I_M";
			}
			else{
				ownerFaceDefine = "I_N";
			}
		}
		else if(tmpCharName == "Jane"){
			if(tmpGoldBool == 1){
				ownerFaceDefine = "F_M";
			}
			else{
				ownerFaceDefine = "F_N";
			}
		}
		else if(tmpCharName == "Vera"){
			if(tmpGoldBool == 1){
				ownerFaceDefine = "U_M";
			}
			else{
				ownerFaceDefine = "U_N";
			}
		}
		else if(tmpCharName == "Mikro"){
			if(tmpGoldBool == 1){
				ownerFaceDefine = "J_M";
			}
			else{
				ownerFaceDefine = "J_N";
			}
		}
		else if(tmpCharName == "Zora"){
			if(tmpGoldBool == 1){
				ownerFaceDefine = "M_M";
			}
			else{
				ownerFaceDefine = "M_N";
			}
		}
		else if(tmpCharName == "Armino"){
			if(tmpGoldBool == 1){
				ownerFaceDefine = "V_M";
			}
			else{
				ownerFaceDefine = "V_N";
			}
		}
	}
}
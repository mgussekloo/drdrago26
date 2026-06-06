using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Security.Cryptography;

using System.Data;
using Mono.Data.SqliteClient;

public class grayEndPoint : MonoBehaviour {

	private GameObject endturnBuyScreen;
	public bool capitalWayPoint;
	
	private IDbConnection _dbc;
	private IDbCommand _dbcm;
	private IDataReader _dbr;
	
	private int idCity;
	private string cityName;
	private string ownerFace;
	private string typeEstate;
	private string itemName;
	private int price;
	private int profit;
	private string rente;
	private int? ownerID;
	private int goldBool;
	int playerMoney;
	public GameObject realEstateObject;
	private GameObject secondEstate;
    private List<GameObject> tmpAIEstateBuy = new List<GameObject>();


    public void confirmEndPoint(){
		endturnBuyScreen = GameObject.Find("Canvas");
		GameObject gameLook =  endturnBuyScreen.transform.Find("gameLook").gameObject ;
		endturnBuyScreen = endturnBuyScreen.transform.Find("endTurnBuyScreen").gameObject;
		endturnBuyScreen.SetActive(true);
		float tmpY = 300.0f;
		
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
			_dbcm.CommandText="SELECT playerMoney FROM `playerInfo`  WHERE `id`='"+gameLook.GetComponent<gameLookFunc>().playerObject.GetComponent<playerMove>().playerID+"'";
			_dbr=_dbcm.ExecuteReader();

		while( _dbr.Read()){
			playerMoney = _dbr.GetInt32(0);
		}
		
		
		_dbcm=_dbc.CreateCommand();
		_dbcm.CommandText="SELECT * FROM `cities` WHERE `cityName`='"+this.gameObject.name+"'";
		_dbr=_dbcm.ExecuteReader();
           
		while( _dbr.Read()){
			idCity = _dbr.GetInt32(0);
			cityName=_dbr.GetString(1);
			ownerFace=_dbr.GetString(2);
			typeEstate=_dbr.GetString(3);
			itemName=_dbr.GetString(4);
			price=_dbr.GetInt32(5);
			profit=_dbr.GetInt32(6);
			rente=_dbr.GetString(7);
			ownerID=_dbr.GetInt32(8);
			goldBool = _dbr.GetInt32(9);
			
			secondEstate = Instantiate (realEstateObject) as GameObject;
			secondEstate.name = secondEstate.name.Replace("(Clone)","").Trim();
			secondEstate.transform.parent = endturnBuyScreen.transform;
			secondEstate.GetComponent<Button>().interactable = true;
			tmpY = tmpY - 125.0f;
			secondEstate.transform.localPosition = new Vector3 (0, tmpY , 0);
			//endturnBuyScreen.transform.GetChild(i).gameObject.SetActive(true);
			secondEstate.GetComponent<buySellRealEstate>().ownerRealEstate = ownerID;
			secondEstate.GetComponent<buySellRealEstate>().instanceNumber = idCity;
			secondEstate.GetComponent<buySellRealEstate>().goldBoolean = goldBool;
			secondEstate.GetComponent<buySellRealEstate>().activeWayPoint = this.gameObject;
			
			//Debug.Log(ownerID + " ownerID " + gameLook.GetComponent<gameLookFunc>().playerObject.GetComponent<playerMove>().playerID);
			
			if(ownerID != 0 && gameLook.GetComponent<gameLookFunc>().playerObject.GetComponent<playerMove>().playerID != ownerID){
				//Debug.Log(ownerID + "make falssse");
				secondEstate.GetComponent<Button>().interactable = false;
			}
			else if(playerMoney <price && gameLook.GetComponent<gameLookFunc>().playerObject.GetComponent<playerMove>().playerID != ownerID){
				secondEstate.GetComponent<Button>().interactable = false;
			}
            else if (gameLook.GetComponent<gameLookFunc>().playerObject.GetComponent<playerMove>().playerIsAI == 1)
            {
                secondEstate.GetComponent<Button>().interactable = false;
                if(playerMoney >= price && ownerID == 0)
                {
                    secondEstate.GetComponent<buySellRealEstate>().priceProperty = price;
                    secondEstate.GetComponent<buySellRealEstate>().profitEstate = profit;
                    tmpAIEstateBuy.Add(secondEstate);
                }
            }

            for (int j=0; j<secondEstate.transform.childCount; j++){
				if("playerFace" == secondEstate.transform.GetChild(j).transform.name){
					int tmpLength = secondEstate.transform.GetChild(j).GetComponent<realEstateShowFace>().spriteFace.Length;
					for(int k=0; k<tmpLength; k++){
						if(ownerFace == secondEstate.transform.GetChild(j).transform.GetComponent<realEstateShowFace>().spriteFace[k].name){
							Sprite tmpSprite = secondEstate.transform.GetChild(j).transform.GetComponent<realEstateShowFace>().spriteFace[k];
							secondEstate.transform.GetChild(j).transform.GetComponent<Image>().sprite = tmpSprite;
							break;
						}
					}	
				}
				else if("typeEstate" == secondEstate.transform.GetChild(j).transform.name){
					int tmpLength = secondEstate.transform.GetChild(j).GetComponent<realEstateShowType>().spriteType.Length;
					for(int k=0; k<tmpLength; k++){
						if(typeEstate == secondEstate.transform.GetChild(j).transform.GetComponent<realEstateShowType>().spriteType[k].name){
							Sprite tmpSprite = secondEstate.transform.GetChild(j).transform.GetComponent<realEstateShowType>().spriteType[k];
							secondEstate.transform.GetChild(j).transform.GetComponent<Image>().sprite = tmpSprite;
							break;
						}
					}
				}
				else if ("itemName" == secondEstate.transform.GetChild(j).transform.name){
					secondEstate.transform.GetChild(j).GetComponent<Text>().text = itemName;
				}
				else if ("price" == secondEstate.transform.GetChild(j).transform.name){
					secondEstate.GetComponent<buySellRealEstate>().priceProperty = price;
					secondEstate.transform.GetChild(j).GetComponent<Text>().text = string.Format("{0:c0}", price);
					
				}
				else if("profit" == secondEstate.transform.GetChild(j).transform.name){
					secondEstate.GetComponent<buySellRealEstate>().profitEstate = profit;
					secondEstate.transform.GetChild(j).GetComponent<Text>().text = string.Format("{0:c0}", profit);
				}
				else if("rente" == secondEstate.transform.GetChild(j).transform.name){
					secondEstate.transform.GetChild(j).GetComponent<Text>().text = rente;
				}
			}
			//Debug.Log("city id" + idCity+" city " +cityName+ " ownerFace "+ownerFace+" typeEstate " + typeEstate+" itemName " + itemName + " price "+ price +" profit " + profit+ " rente " + rente + " ownerName " + ownerName);
		}
	

        if(gameLook.GetComponent<gameLookFunc>().playerObject.GetComponent<playerMove>().playerIsAI == 1)
        {
            StartCoroutine(showAIActionsCoroutine(gameLook.GetComponent<gameLookFunc>().playerObject.GetComponent<playerMove>().playerID));
        }
        _dbc.Close();

    }
    IEnumerator showAIActionsCoroutine(int playerID)
    {
        yield return new WaitForSeconds(1.0f);
        if (tmpAIEstateBuy.Count > 0)
        {
           
            int countEstates = tmpAIEstateBuy.Count;

            string _constr = "";
            if (Application.isEditor)
            {
                string dbPath = System.IO.Path.Combine(Application.streamingAssetsPath, "drdragoDB.db");
                _constr = "URI=file:" + dbPath;
            }
            else
            {
                string dbPath = System.IO.Path.Combine(Application.persistentDataPath, "drdragoDB.db");
                _constr = "URI=file:" + dbPath;
            }

            _dbc = new SqliteConnection(_constr);
            _dbc.Open();

            int randomBuyEstateValue = Random.Range(0, countEstates);
                _dbcm = _dbc.CreateCommand();
                _dbcm.CommandText = "SELECT playerMoney FROM `playerInfo`  WHERE `id`='" + playerID + "'";
                _dbr = _dbcm.ExecuteReader();

                while (_dbr.Read())
                {
                    playerMoney = _dbr.GetInt32(0);
                }
            
                if (playerMoney >= tmpAIEstateBuy[randomBuyEstateValue].GetComponent<buySellRealEstate>().priceProperty)
                {
                    tmpAIEstateBuy[randomBuyEstateValue].GetComponent<buySellRealEstate>().onClickBuySell();
                  tmpAIEstateBuy.Clear();
                }
  
            _dbc.Close();
        }
        else
        {
            GameObject returnAI = endturnBuyScreen.transform.Find("return").gameObject;
            returnAI.GetComponent<nextPlayer>().onClickDisableParent();
        }
    }
}
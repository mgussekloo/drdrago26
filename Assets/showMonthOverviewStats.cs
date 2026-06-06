using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Security.Cryptography;

using System.Data;
using Mono.Data.SqliteClient;

public class showMonthOverviewStats : MonoBehaviour {

	public GameObject playerStatsObject;
	private GameObject playerStats;
	
	private IDbConnection _dbc;
	private IDbCommand _dbcm;
    private IDbCommand _dbcm2;
    private IDataReader _dbr;
    private IDataReader _dbr2;

    private int playerIDD;
	private int playerMoney;
	private int playerProfit;
	private int charIDPlayer;
    private int playerRevenue;
    private int playerEstateValue;


    public void loadMonthOverview(List<GameObject> listActivePlayers){
		float tmpY = 335f;
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
			_dbcm.CommandText="SELECT id, playerMoney, profit, charID, revenue, estateValue FROM `playerInfo` ORDER BY playerMoney desc";
			_dbr=_dbcm.ExecuteReader();

		while( _dbr.Read()){
            playerIDD = _dbr.GetInt32(0);
            playerMoney = _dbr.GetInt32(1);
			playerProfit = _dbr.GetInt32(2);
			charIDPlayer = _dbr.GetInt32(3);
            playerRevenue = _dbr.GetInt32(4);
            playerEstateValue = _dbr.GetInt32(5);
			playerStats = Instantiate (playerStatsObject) as GameObject;
			playerStats.name = playerStats.name.Replace("(Clone)","").Trim();
			playerStats.transform.parent = this.transform;
			//pos might be wrong test to verify
			tmpY = tmpY - 110.0f;
			playerStats.transform.localPosition = new Vector3 (0, tmpY , 0);
			
			for(int j=0; j<playerStats.transform.childCount; j++){
			
				if("face" == playerStats.transform.GetChild(j).transform.name){
					playerStats.transform.GetChild(j).transform.GetComponent<Image>().sprite = listActivePlayers[0].GetComponent<playerMove>().loadGameLook.GetComponent<gameLookFunc>().charFaces[charIDPlayer];
				}
				else if ("rent" == playerStats.transform.GetChild(j).transform.name){
                    playerMoney = playerMoney + playerProfit;

                    _dbcm2 = _dbc.CreateCommand();
                   // _dbcm.CommandText = "SELECT playerMoney, profit, charID FROM `playerInfo` ORDER BY playerMoney desc";
                    _dbcm2.CommandText = "UPDATE playerInfo SET playerMoney = " + playerMoney + " WHERE `id`='" + playerIDD + "'";
                    _dbr2 = _dbcm2.ExecuteReader();

                    string tmpMoneyString = string.Format("{0:+#;-#;+0}", playerProfit);
                    playerStats.transform.GetChild(j).GetComponent<Text>().text = string.Format("${0}", tmpMoneyString);

                    //playerStats.transform.GetChild(j).GetComponent<Text>().text = string.Format("{0:c0}", playerProfit);
				}
				else if ("account" == playerStats.transform.GetChild(j).transform.name){
                    string tmpMoneyString = string.Format("{0:+#;-#;+0}", playerMoney);
                    playerStats.transform.GetChild(j).GetComponent<Text>().text = string.Format("${0}", tmpMoneyString);

                    // playerStats.transform.GetChild(j).GetComponent<Text>().text = string.Format("{0:c0}", playerMoney);
                }
                else if ("revenue" == playerStats.transform.GetChild(j).transform.name)
                {
                    playerRevenue = playerRevenue + playerProfit;
                    string tmpMoneyString = string.Format("{0:+#;-#;+0}", playerRevenue);
                    playerStats.transform.GetChild(j).GetComponent<Text>().text = string.Format("${0}", tmpMoneyString);
                    //playerStats.transform.GetChild(j).GetComponent<Text>().text = string.Format("{0:c0}", playerRevenue);
                }
                else if ("value" == playerStats.transform.GetChild(j).transform.name)
                {
                    int totalValuePlayer = playerMoney + playerEstateValue + playerProfit;
                    string tmpMoneyString = string.Format("{0:+#;-#;+0}", totalValuePlayer);
                    playerStats.transform.GetChild(j).GetComponent<Text>().text = string.Format("${0}", tmpMoneyString);
                    //playerStats.transform.GetChild(j).GetComponent<Text>().text = string.Format("{0:c0}", playerRevenue);
                }
            }
		}
        _dbc.Close();
    }
	
	public void destroyChildPlayerStats(){
		Transform[] tmpPlayerStats = transform.GetComponentsInChildren<Transform>();
		//	Debug.Log(tmpRealEstate[0].name+tmpRealEstate[1].name);
		for(int i=0; i<tmpPlayerStats.Length; i++){
			if(tmpPlayerStats[i].name == "playerStats"){
				Destroy(tmpPlayerStats[i].gameObject);
			}
		}
	}
}
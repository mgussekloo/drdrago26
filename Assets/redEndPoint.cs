using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Security.Cryptography;

using System.Data;
using Mono.Data.SqliteClient;


public class redEndPoint : MonoBehaviour {
	private GameObject endturnScreen;
	private GameObject gameLook;
	private GameObject gameManager;
	
//	public int minLose = 1000;
	//public int maxLose = 2000;
//	public Sprite faceSpriteShow;
//	public AudioClip otherClip;
	
	
	private IDbConnection _dbc;
	private IDbCommand _dbcm;
	private IDataReader _dbr;
	private int moneyToShow;

    public void confirmEndPoint()
    {

        endturnScreen = GameObject.Find("Canvas");
        gameManager = GameObject.Find("gameManager");
        gameLook = endturnScreen.transform.Find("gameLook").gameObject;
        gameManager.GetComponent<gameManager>().playerLooseMoney(false);

        /* gameLook =  endturnScreen.transform.Find("gameLook").gameObject ;
         endturnScreen = endturnScreen.transform.Find("endTurnActionScreen").gameObject ;

         endturnScreen.SetActive(true);
         endturnScreen.GetComponent<AudioSource>().clip = otherClip;
         endturnScreen.GetComponent<AudioSource>().Play();
         int loseMoney = Random.Range(minLose,maxLose);

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
              moneyToShow = _dbr.GetInt32(0);
         }

         int tmpMoneyPlayer  = moneyToShow - loseMoney;

         _dbcm=_dbc.CreateCommand();
         _dbcm.CommandText="UPDATE playerInfo SET playerMoney = "+tmpMoneyPlayer+", revenue = -"+ loseMoney + " WHERE `id`='"+gameLook.GetComponent<gameLookFunc>().playerObject.GetComponent<playerMove>().playerID+"'";
         _dbr=_dbcm.ExecuteReader();
         _dbc.Close();

         gameManager.GetComponent<gameManager>().showPlayerMoney(gameLook.GetComponent<gameLookFunc>().playerObject.GetComponent<playerMove>().playerID);
         endturnScreen.transform.GetChild(0).GetComponent<Text>().text = "Whoehahahahahaha " + loseMoney + " I found this amount in your appartment";
         endturnScreen.transform.GetChild(1).GetComponent<Image>().sprite = faceSpriteShow;

         if (gameLook.GetComponent<gameLookFunc>().playerObject.GetComponent<playerMove>().playerIsAI == 1)
         {
             StartCoroutine(showAIActionsCoroutine());
         }
     }
     IEnumerator showAIActionsCoroutine()
     {
         yield return new WaitForSeconds(1.0f);
         endturnScreen.SetActive(false);
         //rollDiceOject.GetComponent<rollDice>().startAIPlayers();
         endturnScreen.GetComponent<nextPlayer>().onClickEnter();
     }*/
    }
}
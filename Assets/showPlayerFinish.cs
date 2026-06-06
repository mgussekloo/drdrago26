using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Security.Cryptography;

using System.Data;
using Mono.Data.SqliteClient;

public class showPlayerFinish : MonoBehaviour
{

	private IDbConnection _dbc;
	private IDbCommand _dbcm;
	private IDataReader _dbr;
	private Animation anim;
    public Sprite faceSpriteShow;
    private int waitSeconds = 4;
    private GameObject endturnScreen;
    private GameObject playerFinishedObject;
    private int moneyToShow;
    public GameObject gameManager;


    public void loadPlayerWin(GameObject finishedPlayer){
        playerFinishedObject = finishedPlayer;
        int tmpCharID = playerFinishedObject.GetComponent<playerMove>().characterUI.GetComponent<characterUIInfo>().charID;
        endturnScreen = playerFinishedObject.GetComponent<playerMove>().loadGameLook.GetComponent<gameLookFunc>().transform.parent.Find("endTurnActionScreen").gameObject;
   //     Debug.Log("name of endturnscreen" + endturnScreen.name);
        string tmpCharName = playerFinishedObject.GetComponent<playerMove>().loadGameLook.GetComponent<gameLookFunc>().charFaces[tmpCharID].name;
        if (tmpCharName == "D")
        {
            loadFinishedChar("D");
        }
        else if (tmpCharName == "U")
        {
            loadFinishedChar("U");
        }
        else if (tmpCharName == "V")
        {
            loadFinishedChar("V");
        }
        else if (tmpCharName == "E")
        {
            loadFinishedChar("E");
        }
        else if (tmpCharName == "F")
        {
            loadFinishedChar("F");
        }
        else if (tmpCharName == "I")
        {
            loadFinishedChar("I");
        }
        else if (tmpCharName == "J")
        {
            loadFinishedChar("J");
        }
        else if (tmpCharName == "M")
        {
            loadFinishedChar("M");
        }
	}

    private void loadFinishedChar(string letter)
    {
        transform.GetChild(0).transform.GetComponent<Animator>().SetBool("driving" + letter, true);

        StartCoroutine(waitTimeShowFinished());
    }

    IEnumerator waitTimeShowFinished()
    {
       
        yield return new WaitForSeconds(waitSeconds);
     //   Debug.Log("print and do something");
        gameManager.GetComponent<gameManager>().missionFinishedStatus = true;
        gameManager.GetComponent<gameManager>().finishedScreen.SetActive(false);
        endturnScreen.SetActive(true);

        int wonMoney = 100000;

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

        int tmpPlayerID = playerFinishedObject.GetComponent<playerMove>().playerID;
        //string _constr="URI=file:"+ Application.persistentDataPath +"/drdragoDB.db";
        _dbc = new SqliteConnection(_constr);
        _dbc.Open();
        _dbcm = _dbc.CreateCommand();
        _dbcm.CommandText = "SELECT playerMoney FROM `playerInfo`  WHERE `id`='" + tmpPlayerID + "'";
        _dbr = _dbcm.ExecuteReader();

        while (_dbr.Read())
        {
            moneyToShow = _dbr.GetInt32(0);
        }
        int tmpMoneyPlayer = moneyToShow + wonMoney;

        _dbcm = _dbc.CreateCommand();
        _dbcm.CommandText = "UPDATE playerInfo SET playerMoney = " + tmpMoneyPlayer + " WHERE `id`='" + tmpPlayerID + "'";
        _dbr = _dbcm.ExecuteReader();
        _dbc.Close();
        gameManager.GetComponent<gameManager>().showPlayerMoney(tmpPlayerID);
        endturnScreen.transform.GetChild(0).GetComponent<Text>().text = "Yes you finshed this mission the fastest and won " + wonMoney + " money. I think this can be of use during this race and win the game! Now hurry and go to the next mission";
        endturnScreen.transform.GetChild(1).GetComponent<Image>().sprite = faceSpriteShow;



       // Debug.Log("Should push AI disable screen");
        if (playerFinishedObject.GetComponent<playerMove>().playerIsAI == 1) {
            //  Debug.Log("Should push AI disable screen after 2 sec");
            endturnScreen.GetComponent<Button>().interactable = false;
            endturnScreen.GetComponent<nextPlayer>().onClickEnter();
           // Debug.Log("Should push AI disable screen false");
        }

    }
}

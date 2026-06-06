using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Security.Cryptography;

using System.Data;
using Mono.Data.SqliteClient;

public class yellowEndPoint : MonoBehaviour {

	//public GameObject[] cards;
	private GameObject endturnYellowScreen;
	private GameObject tmpAvailableCards;
	
	private IDbConnection _dbc;
	private IDbCommand _dbcm;
    private IDataReader _dbr;
    private int countCards = 0;
    private GameObject activePlayerObject;

    public void confirmEndPoint(GameObject playerObject)
    {
        activePlayerObject = playerObject;
        GameObject tmpPlayerobject = activePlayerObject.GetComponent<playerMove>().loadGameLook;

        endturnYellowScreen = GameObject.Find("Canvas");
        tmpAvailableCards = endturnYellowScreen.transform.Find("drawCards").gameObject;
        endturnYellowScreen = endturnYellowScreen.transform.Find("endTurnYellowScreen").gameObject;
        endturnYellowScreen.SetActive(true);
        int countCardsPlayer = tmpPlayerobject.transform.GetChild(5).GetComponent<cardDeckInteraction>().transform.childCount;
        countCards = tmpAvailableCards.transform.childCount - 2;
        Debug.Log("countCards" + countCardsPlayer);
        if (countCardsPlayer >= 6)
        {

            showCardsToDelete();
            pickRandomCard();
        }
        else
        {
            pickRandomCard();
            //	Debug.Log(countCards + "countCards");

        }
    }

        private void showCardsToDelete()
        {

        GameObject tmpPlayerobject = activePlayerObject.GetComponent<playerMove>().loadGameLook;
        Debug.Log("name of cardsdeck" + tmpPlayerobject.transform.GetChild(5).gameObject.name);
        tmpPlayerobject.transform.GetChild(5).GetComponent<cardDeckInteraction>().activatePlayerCards("yellowEndPoint");

        }


        private void pickRandomCard()
        {
            int randomCard = Random.Range(0, countCards);
            //  Debug.Log(countCards + "countCards" + " random " + randomCard);
            string tmpCardToPlayer = tmpAvailableCards.transform.GetChild(randomCard).name;
            //  Debug.Log("tmpCardToPlayer " + tmpCardToPlayer);

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
            _dbcm = _dbc.CreateCommand();
            _dbcm.CommandText = "INSERT INTO playerCards (playerID, cardName) VALUES ('" + activePlayerObject.GetComponent<playerMove>().playerID + "', '" + tmpCardToPlayer + "')";
            _dbr = _dbcm.ExecuteReader();
            _dbc.Close();

            endturnYellowScreen.transform.GetChild(1).GetComponent<Image>().sprite = tmpAvailableCards.transform.GetChild(randomCard).transform.GetComponent<Image>().sprite;
            endturnYellowScreen.transform.GetChild(0).GetComponent<Text>().text = "Hi! The cards are over there. Help yourself!";
            endturnYellowScreen.transform.GetComponent<nextPlayerYellow>().objectPlayer = activePlayerObject;


            if (activePlayerObject.GetComponent<playerMove>().playerIsAI == 1)
            {
                StartCoroutine(showAIActionsCoroutine());
            }

        
		//Debug.Log("do yellow things you take a feature card from the pack");
		// you take a feature card from the pack. Since a player
//may not hold more than eight feature cards at any one time, you may have to throw one away before
//picking up a new card
	}
	
	 IEnumerator showAIActionsCoroutine(){
			yield return new WaitForSeconds(1.0f);
			endturnYellowScreen.SetActive(false);
			//rollDiceOject.GetComponent<rollDice>().startAIPlayers();
			endturnYellowScreen.GetComponent<nextPlayerYellow>().onClickEnter();
    }
}

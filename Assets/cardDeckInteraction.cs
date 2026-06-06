using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Security.Cryptography;

using System.Data;
using Mono.Data.SqliteClient;

public class cardDeckInteraction : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject activateCardsScreen;
    public GameObject playerCardsDeck;
    private int defineCardID;
    // Update is called once per frame
    public void activatePlayerCards(string typeCardEnable)
    {

        playerCardsDeck.transform.GetComponent<Button>().interactable = false;
        activateCardsScreen.SetActive(true);
        int countOffChilds = transform.childCount;

        float tmpX = -1500.0f;
        float tmpY = 200.0f;    
        int nextRowCard = 0;

        for (int i = 0; i < countOffChilds; i++)
        {
            playerCardsDeck.transform.GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();
            if (nextRowCard > 4)
            {
                tmpX = -500.0f;
                tmpY = tmpY - 250;
                nextRowCard = 0;
            }
            playerCardsDeck.transform.GetChild(i).localPosition = new Vector3(tmpX, tmpY, 0);
            tmpX = tmpX + 200;
            nextRowCard = nextRowCard + 1;
            defineCardID = i;
            GameObject go = playerCardsDeck.transform.GetChild(i).gameObject;
            Debug.Log("name  "+go.name);
            go.GetComponent<Button>().onClick.RemoveAllListeners();

            playerCardsDeck.transform.GetChild(i).parent = activateCardsScreen.transform;
            if (typeCardEnable == "discountEstate" & go.GetComponent<cardProperties>().typeCard == "discountEstate")
            {
                go.GetComponent<Button>().onClick.AddListener(go.GetComponent<cardProperties>().executeCardAction);
                Debug.Log("discount screen activate");
                go.GetComponent<Image>().raycastTarget = true;
                if(transform.parent.name == "endTurnBuyScreen")
                {
                    Debug.Log("parent is endTurnGreyDiscount");
                    int countedEstates = transform.parent.childCount;
                    for(int j = 0; j < countedEstates; j++)
                    {
                        if(transform.parent.GetChild(j).tag == "realEstateObject")
                        {
                            go.GetComponent<cardProperties>().estateLists.Add(transform.parent.GetChild(j).gameObject);
                        }
                    }
                } 
                return;
            }
            else if(typeCardEnable == "fromMainScreen" & go.GetComponent<cardProperties>().typeCard !="discountEstate")
            {
                go.GetComponent<Button>().onClick.AddListener(go.GetComponent<cardProperties>().executeCardAction);
                Debug.Log("main screen activate");
                go.GetComponent<Image>().raycastTarget = true;
                return;
            }
            else if(typeCardEnable == "yellowEndPoint")
            {
                Debug.Log("ready to delete this cards");
                //call the delete function to remove from DB and destroy this object
                go.GetComponent<Button>().onClick.AddListener(go.GetComponent<cardProperties>().updateDBAndDestroy);
                go.GetComponent<Image>().raycastTarget = true;
                return;
            }
            else
            {
                Debug.Log("raycast false");
                go.GetComponent<Image>().raycastTarget = false;
            }


        }

    }

}

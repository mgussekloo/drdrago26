using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Security.Cryptography;

using System.Data;
using Mono.Data.SqliteClient;

public class drdrago : MonoBehaviour
{

    public List<Sprite> drdragoSprite = new List<Sprite>();
    // Start is called before the first frame update

    public void endOfTurn()

    {
        StartCoroutine(showDRDragoActionsCoroutine());

    }
    IEnumerator showDRDragoActionsCoroutine()
    {
       transform.parent.GetComponent<playerMove>().endTurnButtonObject.GetComponent<endTurnButton>().gameManagerObject.GetComponent<gameManager>().playerLooseMoney(true);

        yield return new WaitForSeconds(2.0f);
        transform.parent.GetComponent<playerMove>().showEndpointPopup();
    }

    public void moveWithPlayer(string playerDirection)
    {
        if(playerDirection == "left")
        {
            transform.GetComponent<SpriteRenderer>().sprite = drdragoSprite[0];
            transform.localPosition = new Vector2(0.5f, 0.2f);
        }
        else if(playerDirection == "right")
        {
            transform.GetComponent<SpriteRenderer>().sprite = drdragoSprite[1];
            transform.localPosition = new Vector2(-0.5f, 0.2f);
        }
        else if(playerDirection == "down")
        {
            transform.GetComponent<SpriteRenderer>().sprite = drdragoSprite[2];
            transform.localPosition = new Vector2(0.0f, 0.5f);
        }
        else if(playerDirection == "up")
        {
            transform.GetComponent<SpriteRenderer>().sprite = drdragoSprite[3];
            transform.localPosition = new Vector2(0.0f, -0.5f);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DecisionManager : MonoBehaviour
{
    public static DecisionManager Instance;
    public ThingManager ownerThingManager;
    public DecisionAsk[] activeDecisions;


    public List<GameObject> buttonList;
    public DecisionAsk selectedDecisionAsk;
    public GameObject selectedDecisionObj;

    public GameObject DecisionTalkBox;
    public GameObject DecisionObjPrefab;
    public Transform DecisionLayout;

    public GameObject DecUICursor;

    public int selectionIndex = 0;


    private void Awake()
    {
        Instance = this;
    }


    public void AskDesicion(params DecisionAsk[] decisionsToAsk)
    {
        StartCoroutine(AskDesicionCoroutine(decisionsToAsk));
    }


    public IEnumerator AskDesicionCoroutine(params DecisionAsk[] desicionsToAsk)
    {
        if (desicionsToAsk.Length == 0) { Debug.LogError("BOS SORU KUMESI OLAMAZ"); }

        PlayerStats.Instance.SetOpenWorldAction(false);
        activeDecisions = desicionsToAsk;
        DecisionTalkBox.SetActive(true);



        foreach(GameObject oldChoice in OguzLib.SubObject.GetAllSubGameObjects(DecisionLayout.gameObject))
        {
            Destroy(oldChoice);
        }
        buttonList = new List<GameObject>();

        foreach(DecisionAsk decisionAsk in desicionsToAsk)
        {
            GameObject dec = Instantiate(DecisionObjPrefab,DecisionLayout);
            dec.GetComponent<TextMeshProUGUI>().text = decisionAsk.DecisionDisplayName;

            buttonList.Add(dec);

        }


        yield return DesicionControls();

        print("desicion controlleri gecildi");
        DecisionTalkBox.SetActive(false);
        PlayerStats.Instance.SetOpenWorldAction(true);

        if(ownerThingManager != null) { ownerThingManager.DoNextThing(); }
        
        yield return null;
    }

    private IEnumerator DesicionControls()
    {
        while (1 == 1)
        {
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                SetDecCursor(selectionIndex + 1);
            }
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                SetDecCursor(selectionIndex - 1);
            }


            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
            {
                PlayerDecisions.Instance.SetDecision(activeDecisions[selectionIndex].decisionName, activeDecisions[selectionIndex].desicionStatus);
                yield break;
            }

            yield return null;
        }
    }

    private void SetDecCursor(int x)
    {
        int no = Mathf.Clamp(x, 0, activeDecisions.Length - 1);
        if (no < 0) { no = 0; }

        selectionIndex = no;
        DecUICursor.GetComponent<RectTransform>().position = new Vector2(960, 540);
        DecUICursor.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, 0);


        selectedDecisionObj = buttonList[selectionIndex];
        selectedDecisionAsk = activeDecisions[selectionIndex];

    }




}



[System.Serializable]
public class DecisionAsk
{
    public string DecisionDisplayName;
    public Color BoxColor = default(Color);

    public string decisionName;
    public int desicionStatus;

}

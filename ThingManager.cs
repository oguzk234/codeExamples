using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class ThingManager : MonoBehaviour, IInteractable, IInteractableAuto
{
    public List<ThingListSO> thingListList;



    public List<ThingSO> ThingsToDo;

    public int indexNo = 0;
    public int indexNoTop = 0;

    public bool isAutoInteract;

    public bool isOneOnly = true;
    public bool isDone = false;
    public bool isInteracted = false;
    public bool DestroyAfter;

    public Fight1DodgeManager ActiveF1DManager;

    private PlayableDirector playableDirector;
    public bool debugg;


    private void Awake()
    {
        playableDirector = GetComponent<PlayableDirector>();

        if (isAutoInteract)
        {
            //this.gameObject.layer = PlayerMove.Instance.gameObject.GetComponent<PlayerCollision>().CollisionLayer;
            this.gameObject.layer = LayerMask.NameToLayer("CollisionLayer");
        }
        else
        {
            //this.gameObject.layer = PlayerMove.Instance.gameObject.GetComponent<PlayerInteract>().interactableLayer;
            this.gameObject.layer = LayerMask.NameToLayer("InteractLayer");
        }
    }

    public void Interact()
    {
        if(!isAutoInteract && !isDone && !isInteracted)
        {
            isInteracted = true;
            DoThing();
        }
    }

    public void InteractAuto()
    {
        if (isAutoInteract && !isDone && !isInteracted)
        {
            isInteracted = true;
            DoThing();
        }

        print("ETKILESIMMM WITH THING MANAGER");
    }






    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace) && debugg)
        {
            //DoThing();
            DoNextThing();
        }
    }





    private void FinishThingList()
    {
        DialogManager.Instance.ownerThingManager = null;
        CutsceneManager.Instance.ownerThingManager = null;
        DecisionManager.Instance.ownerThingManager = null;
        if (ActiveF1DManager != null)
        {

            //var invocationList = OnEnd.GetInvocationList();
            //print($"Action içinde {invocationList.Length} fonksiyon var.");


            print(ActiveF1DManager);
            //ActiveF1DManager.OnEnd -= DoThing;
            ActiveF1DManager.ownerThingManager = null;
            //return;
        }



        print("top liste bittigi icin bitiriliyor");

        if (isOneOnly)
        {
            isDone = true;
        }
        else
        {
            indexNo = 0;
        }


        if (DestroyAfter) { Destroy(this.gameObject); }

        return;
    }
    public void DoNextThing()     //FIGHTTAN SONRAKI SEY BUGLU OLUYOR DIREKT GECIYO    IKI TANE CAGRILIYO SANIRIM FIGHTTAN SONRA     ////FIGHT1MANAGER DA SORUN VAR KAPAYINCA DUZGUN CALISIYOOO
    {
        //DialogManager.Instance.OnEnd -= DoNextThing;
        //DIGERI ZATEN YOK OLDUGU ICIN GEREK YOK GIBIMSI SANIRIM (FIGHT1MANAGER)

        indexNo++;
        print(indexNo);


        if (indexNo+1 > ThingsToDo.Count)
        {
            /*
            //print("THING LISTESI BITTI O YUZDEN DURDURULUYOR");

            DialogManager.Instance.ownerThingManager = null;
            CutsceneManager.Instance.ownerThingManager = null;
            if (ActiveF1DManager != null)
            {

                //var invocationList = OnEnd.GetInvocationList();
                //print($"Action içinde {invocationList.Length} fonksiyon var.");


                print(ActiveF1DManager);
                //ActiveF1DManager.OnEnd -= DoThing;
                ActiveF1DManager.ownerThingManager = null;
                //return;
            }
            */



            indexNoTop++;

            if(indexNoTop +1 > thingListList.Count)
            {
                FinishThingList();
            }
            else
            {
                print("top liste bitmedigi icin devam ediliyor");
                indexNo = 0;


                //thingListList[indexNoTop].conditionName



                //indexNoTop++;
            }


        }


        //print(indexNo);
        DoThing();
    }
    public void DoThing()
    {


        if (thingListList[indexNoTop].thingSoList[indexNo] is ThingF1D thingF1D)
        {
            Fight1Dodge f1d = thingF1D.fight1Dodge.Clone();
            Fight1DodgeManager f1dManager = FightManager.Instance.StartFight1Dodge(f1d);
            ActiveF1DManager = f1dManager;

            ActiveF1DManager.ownerThingManager = this;

        }
        else if (thingListList[indexNoTop].thingSoList[indexNo] is ThingTalkSet thingTalkSet)
        {
            DialogManager.Instance.ReadTalkSet(thingTalkSet.talkSet);

            DialogManager.Instance.ownerThingManager = this;
        }
        else if (thingListList[indexNoTop].thingSoList[indexNo] is ThingAnimation thingAnimation)
        {
            //StartCoroutine(StartAnimation(thingAnimation));

            TimelineAsset animToPlay = thingAnimation.timelineAsset;        //BU IKISI ALTA ALINABILIR SORUN OLURSA
            CutsceneManager.Instance.PlayTimelineFunc(animToPlay);


            CutsceneManager.Instance.ownerThingManager = this;


            //CutsceneManager.playableDirector.stopped -= DoNextThing;
            //CutsceneManager.playableDirector.stopped += DoNextThing;

        }
        else if (thingListList[indexNoTop].thingSoList[indexNo] is ThingDecision thingDecision)
        {
            DecisionManager.Instance.AskDesicion(thingDecision.decisions);
            DecisionManager.Instance.ownerThingManager = this;
        }


        print(indexNo + "  NUMARALI ISLEM YAPILDI");
    }


    private IEnumerator StartAnimation(ThingAnimation anim)
    {
        yield return null;
    }


}


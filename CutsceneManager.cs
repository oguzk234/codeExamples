using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using UnityEngine.Animations;

public class CutsceneManager : MonoBehaviour
{
    public static CutsceneManager Instance;

    public static TimelineAsset timelineAssetToPlay;
    public static PlayableDirector playableDirector;
    [SerializeField] private TimelineAsset TimelineAssetPublic;

    private Animator MCanim;
    [SerializeField] private RuntimeAnimatorController DefaultAnimController;
    [SerializeField] private RuntimeAnimatorController CutsceneAnimController;
    //private ForcePP mcForcePP;


    public List<ForcePPAnim> animationCharactersToForcePP = new List<ForcePPAnim>();
    public ThingManager ownerThingManager;


    private void Awake()
    {
        Instance = this;
        //print("1");

        playableDirector = GetComponent<PlayableDirector>();

        if(TimelineAssetPublic != null) { timelineAssetToPlay = TimelineAssetPublic; }

        playableDirector.stopped += OnDirectorStopped;
        playableDirector.played += OnDirectorStarted;

        MCanim = PlayerMove.Instance.GetComponent<Animator>();
        //mcForcePP = PlayerMove.Instance.GetComponent<ForcePP>();
    }



    private void Update()
    {
        CutsceneDebug();
    }

    private void CutsceneDebug()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            StartCoroutine(PlayTimeline());
        }
    }






    /*
    private IEnumerator ForceCharsToPP()
    {
        while(1 == 1)
        {
            yield return null;

            foreach(AnimationCharacter animChar in animationCharactersToForcePP)
            {
                BlockData.MakePixelPerfectStatic(animChar.transform);
            }
        }
    }
    */


    private void OnDirectorStarted(PlayableDirector obj)
    {
        //MCanim.runtimeAnimatorController = CutsceneAnimController;
        //MCanim.applyRootMotion = false;
        //StartCoroutine(ForceCharsToPP());

        foreach(ForcePPAnim forcePPAnim in animationCharactersToForcePP)
        {
            forcePPAnim.enabled = true;
        }
    }
    private void OnDirectorStopped(PlayableDirector obj)
    {
        //MCanim.runtimeAnimatorController = DefaultAnimController;
        //MCanim.applyRootMotion = true;
        //ResetPlayerSpriteLoc();
        //StopCoroutine(ForceCharsToPP());

        foreach (ForcePPAnim forcePPAnim in animationCharactersToForcePP)
        {
            forcePPAnim.enabled = false;
        }


        print("ANIMASYON BITTI");
        if (ownerThingManager != null)
        {
            MCanim.runtimeAnimatorController = DefaultAnimController;
            PlayerStats.Instance.SetOpenWorldAction(true);
            ownerThingManager.DoNextThing();
        }
    }
    private void ResetPlayerSpriteLoc()
    {
        GameObject playerSpriteObj = PlayerMove.Instance.gameObject.transform.GetChild(0).gameObject;
        playerSpriteObj.transform.localPosition = new Vector3(0, 0, playerSpriteObj.transform.localPosition.z);
    }




    public IEnumerator PlayTimeline()
    {
        PlayerStats.Instance.SetOpenWorldAction(false);
        MCanim.runtimeAnimatorController = CutsceneAnimController;
        //mcForcePP.enabled = true;
        playableDirector.Play();



        yield return new WaitForSecondsRealtime((float)playableDirector.duration + 0.1f);



        //MCanim.runtimeAnimatorController = DefaultAnimController;
        //PlayerStats.Instance.SetOpenWorldAction(true);
    }

    public void PlayTimelineFunc(TimelineAsset timelineAsset)
    {
        timelineAssetToPlay = timelineAsset;
        playableDirector.playableAsset = timelineAssetToPlay;
        StartCoroutine(PlayTimeline());
    }




}

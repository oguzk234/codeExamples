using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Linq;

public class FightManager : MonoBehaviour
{
    public static FightManager Instance;
    //[SerializeField] private Vector2 FightCamLoc;

    public Fight1Dodge fg1;

    public GameObject FightScene;
    public GameObject Fight1DodgeManagerPrefab;

    /*
    [Header("Fight1ReferencesForManager")]
    public GameObject MCFight1Dodge;
    */

    [Header("GeneralFight1DodgeSettings")]
    public float F1DStartingTime;
    public float F1DDamageTakeCDMax;
    public float F1DTimeBeforeTakeDamageAfterAttackSpawned;  //ESKI
    public float F1DTimePercentBeforeTakeDamageAfterAttackSpawned;
    public float F1DEnemyAttackHitPercent;
    public Vector2 AttackAnimUp;
    public Vector2 AttackAnimDown;
    public Vector2 AttackAnimRight;
    public Vector2 AttackAnimLeft;
    public AudioSource audioSourceTakeDamage;
    public GameObject DamageTextPrefab;
    public AnimationCurve DamageTextCurve;
    public GameObject DieFxPrefab;
    //public AnimationCurve DamageTextFadeOutCurve;






    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        //DebugZZZ();
    }
    private void DebugZZZ()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartFight1Dodge(fg1);
        }
    }


    public void Die()
    {

    }



#nullable enable
    public Fight1DodgeManager? StartFight1Dodge(Fight1Dodge fight)
    {
        PlayerStats.Instance.SetOpenWorldAction(false);

        CameraFollow.FollowingPlayer = false;
        CameraFollow.CamObj.transform.position = new Vector3(FightScene.transform.position.x, FightScene.transform.position.y, CameraFollow.CamObj.transform.position.z);

        Fight1DodgeManager fight1DodgeManager = Instantiate(Fight1DodgeManagerPrefab,FightScene.transform).GetComponent<Fight1DodgeManager>();
        fight1DodgeManager.Initialize(fight);

        return fight1DodgeManager;
    }
#nullable disable


    public void FinishFight1Dodge(Fight1DodgeManager fightManager)
    {
        CameraFollow.FollowingPlayer = true;
        PlayerStats.Instance.SetOpenWorldAction(true);

        CursorManager.Instance.UpdateCursorToDefault();


        List<TalkLine> talksAfterFight = fightManager.fight1Dodge.toDoAfterFight.TalksAfterFight;

        //Destroy(fightManager.gameObject); //ALINABILIR

        //DialogManager.Instance.ReadTalkSet(talksAfterFight);    ///// SORUNLU OLABILIRDI    DO NEXT THING CAGIRDIGI ICIN DEVRE DISI BIRAKILDI
    }


}


[System.Serializable]
public class Fight
{
    public Enemy enemy;

}

[System.Serializable]
public class Fight1Dodge : Fight
{
    public Sprite BgSprite;

    public int enemyAttackMaxComboBase = 6;
    public int enemyAttackMaxComboRange = 2;
    public int enemyAttackCombo
    {
        get
        {
            return OguzLib.Others.GetRandomIntWithOffset(enemyAttackMaxComboBase, enemyAttackMaxComboRange);
        }
    }
    public float enemyAttackCDBase = 0.9f;
    public float enemyAttackCDRange = 0.2f;
    public float enemyAttackCD
    {
        get { return OguzLib.Others.GetRandomFloatWithOffset(enemyAttackCDBase, enemyAttackCDRange); }
    }

    public float enemyAttackTime = 0.8f;

    public int MaxHP = 100;
    public int HP = 100;
    public float HpPercentage
    {
        get
        {
            return (float)HP / MaxHP * 100f;
        }
    }

    public int DamageRandomExtra = 5;
    public int DamageBase = 25;
    public int Damage
    {
        get { return Random.Range(DamageBase - DamageRandomExtra, DamageBase + DamageRandomExtra); }
    }

    public int MaxAttackCount = 3;

    public float PlayerAttackTime = 3f;


    [Header("SpaceSpam Settings")]
    public float ActiveSpaceSpamPowa;
    public float ActiveSpaceSpamPowaPercentage;
    public float SpaceSpamBasePowa = 0.1f;
    public float SpaceSpamIncrementalPowa = 0.3f;
    public float SpaceSpamMinVal = -4.5f;  //ZORLUGU ETKILIYOR ELLEME COK
    public float SpaceSpamMaxVal = 5;
    public float SpaceSpamDecrease = 9;
    public float SpaceSpamYOffset = 0;


    public Fight1Dodge()
    {

    }

    [Header("Other")]
    public Vector2 DieFxOffset;
    public Vector2 enemySpritePositionOffset;
    public Vector2 PlayerAttackPos = new Vector2(8,1);
    public Vector2 DamageTextOffset;

    //public TalksWithHpRatios talksWithHpRatios;

    public ToDoInFight toDoInFight;
    public ToDoAfterFight toDoAfterFight;


    
    public Fight1Dodge Clone()
    {
        Fight1Dodge F1D = new Fight1Dodge();
        F1D.BgSprite = BgSprite;
        F1D.enemy = enemy;
        F1D.enemyAttackMaxComboBase = enemyAttackMaxComboBase;
        F1D.enemyAttackMaxComboRange = enemyAttackMaxComboRange;
        F1D.enemyAttackCDBase = enemyAttackCDBase;
        F1D.enemyAttackCDRange = enemyAttackCDRange;
        F1D.enemyAttackTime = enemyAttackTime;
        F1D.HP = HP;
        F1D.MaxHP = MaxHP;
        F1D.DamageRandomExtra = DamageRandomExtra;
        F1D.DamageBase = DamageBase;
        F1D.MaxAttackCount = MaxAttackCount;
        F1D.PlayerAttackTime = PlayerAttackTime;
        F1D.SpaceSpamBasePowa = SpaceSpamBasePowa;
        F1D.SpaceSpamIncrementalPowa = SpaceSpamIncrementalPowa;
        F1D.SpaceSpamMinVal = SpaceSpamMinVal;
        F1D.SpaceSpamMaxVal = SpaceSpamMaxVal;
        F1D.SpaceSpamDecrease = SpaceSpamDecrease;
        F1D.SpaceSpamYOffset = SpaceSpamYOffset;
        F1D.PlayerAttackPos = PlayerAttackPos;
        F1D.enemy.sprite = enemy.sprite;
        F1D.enemySpritePositionOffset = enemySpritePositionOffset;
        F1D.DieFxOffset = DieFxOffset;
        F1D.DamageTextOffset = DamageTextOffset;

        F1D.toDoAfterFight = new ToDoAfterFight();
        F1D.toDoAfterFight.TalksAfterFight = toDoAfterFight.TalksAfterFight.ToList();

        F1D.toDoInFight = new ToDoInFight();
        F1D.toDoInFight.HpIntList = toDoInFight.HpIntList.ToList();
        F1D.toDoInFight.HpTalkList = toDoInFight.HpTalkList.ToList();
        F1D.toDoInFight.BeforeFightTalkLines = toDoInFight.BeforeFightTalkLines.ToList();

        return F1D;
    }
    

}


[System.Serializable]
public class Enemy
{
    public Sprite sprite;

}

[System.Serializable]
public struct ToDoInFight
{
    public List<F1DTalkLine> BeforeFightTalkLines;
    public List<F1DTalkLine> HpTalkList;
    public List<int> HpIntList;  //0 ile 100 arasi girilecek


    public List<F1DTalkLine> GetHpTalks(float enemyHpPercentage)
    {
        //Debug.Log(enemyHpPercentage);

        List<F1DTalkLine> talksToReturn = new List<F1DTalkLine>();
        if (HpTalkList.Count < 1) { Debug.LogWarning("Cekilecek Veri Yok"); return null; }

        for (int i = 0; i < HpTalkList.Count; i++)
        {
            if (enemyHpPercentage < HpIntList[i])
            {
                talksToReturn.Add(HpTalkList[i]);

                HpTalkList.RemoveAt(i);
                HpIntList.RemoveAt(i);
                i -= 1;
            }


        }

        return talksToReturn;

    }

}


[System.Serializable]
public struct ToDoAfterFight
{
    public List<TalkLine> TalksAfterFight;
}


/*
[System.Serializable]
public class TalksWithHpRatios
{
    public List<TalkLine> TalksBeforFight; //AYRI YERDE CALISIYOR ACIK
    public List<TalkLine> TalksAfterFight; //BU DA AYNI

    public List<TalkLine> talks1;
    public List<TalkLine> talks5;
    public List<TalkLine> talks10;
    public List<TalkLine> talks15;
    public List<TalkLine> talks20;
    public List<TalkLine> talks25;
    public List<TalkLine> talks30;
    public List<TalkLine> talks35;
    public List<TalkLine> talks40;
    public List<TalkLine> talks45;
    public List<TalkLine> talks50;
    public List<TalkLine> talks55;
    public List<TalkLine> talks60;
    public List<TalkLine> talks65;
    public List<TalkLine> talks70;
    public List<TalkLine> talks75;
    public List<TalkLine> talks80;
    public List<TalkLine> talks85;
    public List<TalkLine> talks90;
    public List<TalkLine> talks95;
    public List<TalkLine> talks99;

    public List<List<TalkLine>> GetAllHpTalks()
    {
        List<List<TalkLine>> talkListList = new List<List<TalkLine>> { talks1, talks5,talks10,talks15,talks20,talks25,talks30,talks35,talks40,talks45,talks50,talks55,talks60,talks65,talks70,talks75,talks80,talks85,talks90,talks95,talks99 };
        return talkListList;

    }

}
*/

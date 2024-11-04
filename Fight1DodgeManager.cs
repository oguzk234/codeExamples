using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fight1DodgeManager : MonoBehaviour
{
    [Header("REFER")]
    public GameObject MCFight;
    public GameObject EnemyFight;
    public SpriteRenderer EnemyRenderer;
    public SpriteRenderer MCRenderer;
    public Fight1Dodge fight1Dodge;
    public GameObject AttackAnimPrefab;
    public GameObject SpaceSpamObj;
    public SpriteRenderer BgRenderer;


    [Header("Dodge Settings")]
    public float dodgeTime;
    public float dodgePercent; //ELLEME EDITORDE
    public float neededPercentToDodgeOverride = 0.7f;
    public Vector2 ActiveDodgeDir;
    public AnimationCurve DodgeAnimCurve;
    public float DodgeRange;
    public bool isDodging;
    public Coroutine oldDodgeCoroutine;
    public bool isGettingDodgeInput;


    [Header("General Settings")]
    public FightManager fightManager;
    public bool isFightStarted;
    public float DamageTakeCD;
    public Color AttackColor1 = Color.white;
    public Color AttackColor2;
    public bool ifNormalHpTalksDuringBattle = false;

    [Header("EnemySettings")]
    public List<Vector2> ActiveEnemyAttacks = new List<Vector2>();
    public Animator BaseAnimator;


    public ThingManager ownerThingManager;



    private void Start()
    {
        //MCFight = FightManager.Instance.MCFight1Dodge;
        fightManager = FightManager.Instance;
        //StartFight();

    }

    public void Initialize(Fight1Dodge f1d)
    {
        fight1Dodge = f1d;

        EnemyRenderer.sprite = fight1Dodge.enemy.sprite;
        BgRenderer.sprite = fight1Dodge.BgSprite;
        EnemyFight.transform.localPosition = f1d.enemySpritePositionOffset;
        CursorManager.Instance.UpdateCursorToHp();

        isFightStarted = true;

        StartCoroutine(BeforeStart(2f));
    }
    
    private IEnumerator BeforeStart(float waitTime)
    {
        BaseAnimator.Play("F1DodgeEntryAnim");

        yield return new WaitForSecondsRealtime(waitTime);

        isGettingDodgeInput = true;
        isFightStarted = true;
        StartFight();
    }

    private void Update()
    {
        if (isFightStarted)
        {
            DamageTakeCD -= Time.deltaTime;

            DodgeInput();

            if ((ActiveEnemyAttacks.Count > 0 && ActiveDodgeDir == Vector2.zero) || (ActiveEnemyAttacks.Contains(ActiveDodgeDir)))   //BURDA NET BI BOK VAR  (COK DA YOKMUS MEKANIK OYLE)
            {
                if (DamageTakeCD < 0)
                {
                    PlayerStats.Instance.Health -= fight1Dodge.Damage;
                    DamageTakeCD = fightManager.F1DDamageTakeCDMax;
                    //print("HASAR ALINDI  ==  ActýveDodgeDir = " + ActiveDodgeDir + "  Saldiriliar : " + ActiveEnemyAttackDebug());
                    fightManager.audioSourceTakeDamage.Play();
                    CursorManager.Instance.UpdateCursorToHp();
                    StopCoroutine(PlayerDamageAnimation());
                    StartCoroutine(PlayerDamageAnimation());
                }
                else
                {
                    //print("HASAR ALINDI AMA CD BITMEDI");
                }

            }


            /*
            if (Input.GetKeyDown(KeyCode.E))
            {
                //EnemySingleAttack();
                fightManager.FinishFight1Dodge(this);
                print(OnEnd.GetInvocationList().Length);
                OnEnd?.Invoke();
                Destroy(this.gameObject);
            }
            */

        }

    }

    private string ActiveEnemyAttackDebug()
    {
        string x = null;

        foreach(Vector2 vec2 in ActiveEnemyAttacks)
        {
            x = x + " X = " + vec2.x + ", Y = " + vec2.y + " /// ";
        }

        return x;
    }



    private void StartFight()
    {
        StartCoroutine(StartFightCoroutine());
    }
    private IEnumerator StartFightCoroutine()
    {
        yield return new WaitForSecondsRealtime(fightManager.F1DStartingTime);
        
        if(fight1Dodge.toDoInFight.BeforeFightTalkLines != null && fight1Dodge.toDoInFight.BeforeFightTalkLines.Count > 0)
            { yield return F1DialogManager.Instance.ReadF1dTalkSetCoroutine(fight1Dodge.toDoInFight.BeforeFightTalkLines); yield return new WaitForSecondsRealtime(2.0f); }

        isFightStarted = true;

        while (isFightStarted)
        {
            //EnemySingleAttack();
            for (int i = 0; i < fight1Dodge.enemyAttackCombo; i++)
            {
                EnemyMultiAttack(Random.Range(1, fight1Dodge.MaxAttackCount));
                yield return new WaitForSecondsRealtime(fight1Dodge.enemyAttackCD);
            }

            //yield return new WaitUntil(() => isDodging)
            isGettingDodgeInput = false;

            yield return PlayerAttackWait();
            //yield return F1DialogManager.Instance.ReadF1dTalkSetCoroutine(F1DialogManager.Instance.DebugTalks);

            List<F1DTalkLine> HpTalksToTalk = fight1Dodge.toDoInFight.GetHpTalks(fight1Dodge.HpPercentage);

            //if(HpTalksToTalk != null) { print("KONUSMA SAYISI = " + HpTalksToTalk.Count); }

            if(HpTalksToTalk != null && HpTalksToTalk.Count > 0)
            {
                yield return F1DialogManager.Instance.ReadF1dTalkSetCoroutine(HpTalksToTalk);
                //F1DialogManager.Instance.ReadF1dTalkSet(HpTalksToTalk);
                //yield return new WaitForSecondsRealtime(0.01f);
                //yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
                //yield return new WaitForSecondsRealtime(0.4f);
            }

            isGettingDodgeInput = true;

            yield return new WaitForSecondsRealtime(0.4f);


            if (fight1Dodge.HP <= 0)
            {
                GameObject DieFx = Instantiate(FightManager.Instance.DieFxPrefab, EnemyFight.transform);
                DieFx.transform.localPosition = fight1Dodge.DieFxOffset;

                yield return new WaitForSecondsRealtime(1.2f);
                StartCoroutine(Fade(2.4f, 0, EnemyRenderer));
                StartCoroutine(Fade(2.4f, 0, DieFx.GetComponent<SpriteRenderer>()));

                yield return new WaitForSecondsRealtime(3.6f);

                //Destroy(DieFx);


                /*
                var invocationList = OnEnd.GetInvocationList();
                print($"Action içinde {invocationList.Length} fonksiyon var.");
                */

                fightManager.FinishFight1Dodge(this);

                if(ownerThingManager != null)
                {
                    ownerThingManager.DoNextThing();    /////////////////       BU   KAPALIYKEN    NASIL   CALISIYO     AAAAAAMMMMMKKKK
                }

                //OnEnd = null;



                Destroy(this.gameObject);
            }

        }

    }

    private IEnumerator PlayerDamageAnimation(float fadeTime = 0.1f,int fadeCount = 4)
    {
        for (int i = 0; i < fadeCount; i++)
        {
            yield return Fade(fadeTime, 0f, MCRenderer);
            yield return Fade(fadeTime, 1f, MCRenderer);
        }
    }
    private IEnumerator Fade(float fadeTime,float fadeValue,SpriteRenderer spriteRenderer)
    {
        float elapsedTime = 0f;
        float startAlpha = spriteRenderer.color.a;

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, Mathf.Lerp(startAlpha, fadeValue, elapsedTime / fadeTime));
            yield return null;
        }

        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, fadeValue);
    }



    private IEnumerator PlayerAttackWait()
    {
        float elapseTime = 0f;
        float SpaceSpamY = 0f;

        SpaceSpamObj.SetActive(true);
        //isGettingDodgeInput = false;

        while(elapseTime < fight1Dodge.PlayerAttackTime)
        {
            elapseTime += Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                SpaceSpamY += -((Mathf.Sqrt(Mathf.Abs((SpaceSpamY - fight1Dodge.SpaceSpamMinVal) * fight1Dodge.SpaceSpamIncrementalPowa))) + fight1Dodge.SpaceSpamBasePowa);
                //print(SpaceSpamY);
            }
            else
            {
                SpaceSpamY += Time.deltaTime * fight1Dodge.SpaceSpamDecrease;
                SpaceSpamY = Mathf.Clamp(SpaceSpamY, fight1Dodge.SpaceSpamMinVal, fight1Dodge.SpaceSpamMaxVal);
            }

            SpaceSpamObj.transform.localPosition = new Vector3(0, SpaceSpamY+ fight1Dodge.SpaceSpamYOffset, 0);
            BlockData.MakePixelPerfectStatic(SpaceSpamObj.transform);

            fight1Dodge.ActiveSpaceSpamPowa = SpaceSpamY;
            //ActiveSpaceSpamPowaPercentage = OguzLib.Others.CalculatePercentage(SpaceSpamMinVal, SpaceSpamMaxVal, SpaceSpamY);
            fight1Dodge.ActiveSpaceSpamPowaPercentage = Mathf.Abs(100-OguzLib.Others.CalculatePercentage(fight1Dodge.SpaceSpamMinVal,fight1Dodge.SpaceSpamMaxVal, SpaceSpamY));
            yield return null;
        }

        yield return new WaitForSecondsRealtime(0.31f);
        SpaceSpamObj.SetActive(false);
        fightManager.audioSourceTakeDamage.PlayDelayed(0.45f);



        //int damageToHit = (int)(PlayerStats.Instance.Damage * fight1Dodge.ActiveSpaceSpamPowaPercentage) / 10;
        int damageToHit = (int)(PlayerStats.Instance.Damage * (fight1Dodge.ActiveSpaceSpamPowaPercentage/100));
        fight1Dodge.HP -= damageToHit;



        //print(damageToHit + "  KADAR HASAR DUSMANA VURULDU");

        Animator attackAnimFX = Instantiate(AttackAnimPrefab, this.transform).GetComponent<Animator>();
        attackAnimFX.gameObject.transform.localPosition = fight1Dodge.PlayerAttackPos;


        yield return new WaitForSecondsRealtime(0.4f);
        BaseAnimator.Play("F1DodgeEnemyDamageAnim",0,0);
        StartCoroutine(SpawnDamageText(damageToHit));

        /*
        List<List<TalkLine>> TalksToTalk = CheckEnemyHpForTalks();
        if(TalksToTalk != null)
        {
            foreach (List<TalkLine> TalkWithHp in TalksToTalk)
            {
                yield return DialogManager.Instance.ReadTalkSetCoroutine(TalkWithHp);
            }
        }
        */







        yield return new WaitForSecondsRealtime(1.2f);
        Destroy(attackAnimFX.gameObject);
        //isGettingDodgeInput = true;
        //ANIMASYON EKLICEM DUSMANA HASAR YEMESI ICIN

        yield return new WaitForSecondsRealtime(0.5f);

    }

    private IEnumerator SpawnDamageText(int damag)
    {
        TMPro.TextMeshPro text = Instantiate(fightManager.DamageTextPrefab,this.transform).GetComponent<TMPro.TextMeshPro>();
        text.sortingOrder = 950;
        text.gameObject.GetComponent<RectTransform>().localPosition = fight1Dodge.PlayerAttackPos + fight1Dodge.DamageTextOffset;
        text.text = "-" + damag.ToString();

        float textMoveTime = 3.2f;
        float textAlpha = 1f;
        float elapsedTime = 0f;
        RectTransform rectTransform = text.GetComponent<RectTransform>();
        while(elapsedTime < textMoveTime)
        {
            elapsedTime += Time.deltaTime;
            rectTransform.localPosition = new Vector2(rectTransform.localPosition.x,fightManager.DamageTextCurve.Evaluate(elapsedTime / textMoveTime)*4);

            //textAlpha = Mathf.Lerp(1f, 0f, elapsedTime / textMoveTime);
            textAlpha = Mathf.Lerp(1f, 0f, fightManager.DamageTextCurve.Evaluate(elapsedTime / textMoveTime));
            text.color = new Color(text.color.r, text.color.g, text.color.b, textAlpha);
            yield return null;
        }

        Destroy(text.gameObject);

    }

    /*
    private List<List<TalkLine>> CheckEnemyHpForTalks()
    {
        float enemyHpPercent = ((float)fight1Dodge.HP / fight1Dodge.MaxHP) * 100;
        print(enemyHpPercent + " = ENEMY HP PERCENT " +  fight1Dodge.HP +" + "+ fight1Dodge.MaxHP + " + "+ fight1Dodge.HP/fight1Dodge.MaxHP);

        List<List<TalkLine>> talksToTalk = new List<List<TalkLine>>();

        foreach(List<TalkLine> talkSet in fight1Dodge.talksWithHpRatios.GetAllHpTalks())
        {
            if(talkSet!= null)
            { 

                talksToTalk.Add(talkSet);
            }
        }



        return talksToTalk;



    }
    */




    #region SingleAttack
    private void EnemySingleAttack()
    {
        StartCoroutine(EnemySingleAttackCoroutine());
    }
    private IEnumerator EnemySingleAttackCoroutine()
    {
        Animator attackAnimFX = Instantiate(AttackAnimPrefab, this.transform).GetComponent<Animator>();

        Vector2 attackDir = OguzLib.Vectors.ReturnRandomDirection4();
        Vector2 attackAnimDir = Vector2.zero;
        //attackAnimFX.gameObject.transform.position = new Vector3(attackAnimFX.gameObject.transform.position.x + (attackDir.x * 5),attackAnimFX.gameObject.transform.position.y + (attackDir.y *5),attackAnimFX.gameObject.transform.position.z);

        if(attackDir == Vector2.up)
        {
            attackAnimDir = fightManager.AttackAnimUp;
        }
        else if (attackDir == Vector2.down)
        {
            attackAnimDir = fightManager.AttackAnimDown;
        }
        else if(attackDir == Vector2.right)
        {
            attackAnimDir = fightManager.AttackAnimRight;
        }
        else if (attackDir == Vector2.left)
        {
            attackAnimDir = fightManager.AttackAnimLeft;
        }


        attackAnimFX.gameObject.transform.localPosition = attackAnimDir;

        //float elapsedTime = 0f;   // ALTTAKI IKINCI COROUTINE I  KALKDIRINCA AC BUNU


        //yield return new WaitForSecondsRealtime(fightManager.F1DTimeBeforeTakeDamageAfterAttackSpawned);  //BASKA COROUTINE E ALINDI ALTTA
        //ActiveEnemyAttacks.Add(attackDir);
        StartCoroutine(EnemySingleAttackAddList(attackDir,attackAnimFX.gameObject.GetComponent<SpriteRenderer>()));
        StartCoroutine(EnemySingleAttackRemoveList(attackDir, attackAnimFX.gameObject.GetComponent<SpriteRenderer>(), attackAnimFX));
        yield return null;

        /*
        while (elapsedTime < fight1Dodge.enemyAttackTime)
        {
            elapsedTime += Time.deltaTime;
            float animPercent = elapsedTime/fight1Dodge.enemyAttackTime;


            attackAnimFX.Play("AttackFXanim",0,animPercent);
            yield return null;
        }
        




        Destroy(attackAnimFX.gameObject);
        ActiveEnemyAttacks.Remove(attackDir);
        */
    }
    private IEnumerator EnemySingleAttackAddList(Vector2 vector2,SpriteRenderer spriteRenderer)
    {
        //yield return new WaitForSecondsRealtime(fightManager.F1DTimeBeforeTakeDamageAfterAttackSpawned);
        float elapsedTime = 0f;
        float attackPercent = 0f;
        spriteRenderer.color = AttackColor1;

        while(elapsedTime < fight1Dodge.enemyAttackTime)
        {
            elapsedTime += Time.deltaTime;
            attackPercent = elapsedTime / fight1Dodge.enemyAttackTime;

            if (attackPercent > fightManager.F1DTimePercentBeforeTakeDamageAfterAttackSpawned)
            {
                ActiveEnemyAttacks.Add(vector2);
                spriteRenderer.color = AttackColor2;
                yield break;
            }


            yield return null;
        }

        //ActiveEnemyAttacks.Add(vector2);
    }
    private IEnumerator EnemySingleAttackRemoveList(Vector2 vector2,SpriteRenderer spriteRenderer,Animator attackAnimFX)
    {
        float elapsedTime = 0f;

        while(elapsedTime/fight1Dodge.enemyAttackTime < fightManager.F1DEnemyAttackHitPercent)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        spriteRenderer.color = AttackColor1;
        ActiveEnemyAttacks.Remove(vector2);

        while (elapsedTime < fight1Dodge.enemyAttackTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }


        Destroy(attackAnimFX.gameObject);
        //ActiveEnemyAttacks.Remove(vector2);


    }

    #endregion

    #region DoubleAttack

    private void EnemyMultiAttack(int AttackNumber)
    {
        StartCoroutine(EnemyMultiAttackCoroutine(AttackNumber));
    }

    private IEnumerator EnemyMultiAttackCoroutine(int AttackNumber)
    {
        Vector2[] attacks = OguzLib.Vectors.ReturnRandomDirections4(AttackNumber);

        foreach(Vector2 attack in attacks)
        {
            Animator attackAnimFX = Instantiate(AttackAnimPrefab, this.transform).GetComponent<Animator>();
            Vector2 attackDir = attack;
            Vector2 attackAnimDir = Vector2.zero;


            if (attackDir == Vector2.up)
            {
                attackAnimDir = fightManager.AttackAnimUp;
            }
            else if (attackDir == Vector2.down)
            {
                attackAnimDir = fightManager.AttackAnimDown;
            }
            else if (attackDir == Vector2.right)
            {
                attackAnimDir = fightManager.AttackAnimRight;
            }
            else if (attackDir == Vector2.left)
            {
                attackAnimDir = fightManager.AttackAnimLeft;
            }


            attackAnimFX.gameObject.transform.localPosition = attackAnimDir;

            StartCoroutine(EnemySingleAttackAddList(attackDir, attackAnimFX.gameObject.GetComponent<SpriteRenderer>()));
            StartCoroutine(EnemySingleAttackRemoveList(attackDir, attackAnimFX.gameObject.GetComponent<SpriteRenderer>(), attackAnimFX));
            yield return null;


        }





    }


    #endregion



    private void DodgeInput()
    {
        if(!isGettingDodgeInput) { return; }

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Dodge(Vector2.left);
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            Dodge(Vector2.right);
        }
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            Dodge(Vector2.down);
        }
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            Dodge(Vector2.up);
        }
    }
    private void Dodge(Vector2 dodgeDir)
    {
        if (isDodging == true) { return; }
        //StopAllCoroutines();
        if(oldDodgeCoroutine != null) { StopCoroutine(oldDodgeCoroutine); }

        oldDodgeCoroutine = StartCoroutine(DodgeCoroutine(dodgeDir));
    }
    private IEnumerator DodgeCoroutine(Vector2 dodgeDir)
    {
        isDodging = true;
        
        float elapsedTime = 0f;

        ActiveDodgeDir = dodgeDir;

        while (elapsedTime < dodgeTime)
        {
            elapsedTime += Time.deltaTime;

            dodgePercent = DodgeAnimCurve.Evaluate(elapsedTime / dodgeTime);   //0dan 1 sonra yine 0 oluyo
            float dodgePercentRaw = elapsedTime / dodgeTime;

            //dodgePercent = (elapsedTime / dodgeTime);
            //float animationPercent = DodgeAnimCurve.Evaluate(dodgeTime);

            MCFight.transform.localPosition = new Vector3(ActiveDodgeDir.x*dodgePercent * DodgeRange, ActiveDodgeDir.y * dodgePercent * DodgeRange, MCFight.transform.position.z);
            BlockData.MakePixelPerfectStatic(MCFight.transform);
            

            if(dodgePercentRaw > neededPercentToDodgeOverride)
            {
                isDodging = false;
            }


            //print(dodgePercent);  //WORKING

            yield return null;
            //yield break; //KOMPLE DURDURUYOR COROUTINE I
        }

        ActiveDodgeDir = Vector2.zero;

        //Debug.Log("Dodgjj tamamlandý!");

        isDodging = false;
    }




}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance;
    public SceneArea ActiveArea;

    public Image BlackenImage;

    void Awake()
    {
        Instance = this;
        ProceedAreaTop(ActiveArea);
    }

    public void ProceedAreaTop(SceneArea area,bool Blacken = false)
    {
        if (Blacken) { ProceedAreaBlacken(area); }
        else if (!Blacken) { ProceedArea(area); }
    }
    public void ProceedArea(SceneArea area)
    {
        OnAreaChanged(ActiveArea, area);
        ActiveArea = area;



        CameraFollow.ActiveMapLimits = area.area.mapLimits;
        PlayerMove.Instance.gameObject.transform.position = area.area.startLocation;
    }



    public void OnAreaChanged(SceneArea oldArea, SceneArea newArea)
    {
        //oldArea.SetAreaComponentsActive(false);
        //newArea.SetAreaComponentsActive(true);

        oldArea.gameObject.SetActive(false);
        newArea.gameObject.SetActive(true);

    }


    public void ProceedAreaBlacken(SceneArea area, float blackenTime = 1f, float whitingTime = 1f)
    {
        StartCoroutine(ProceedAreaBlackenCoroutine(area, blackenTime, whitingTime));
    } 
    public IEnumerator ProceedAreaBlackenCoroutine(SceneArea area,float blackenTime = 1f,float whitingTime = 1f)
    {
        PlayerStats.Instance.SetOpenWorldAction(false);
        float elapsedBlackenTime = 0f;
        BlackenImage.gameObject.SetActive(true);

        while(elapsedBlackenTime < blackenTime)
        {
            elapsedBlackenTime += Time.deltaTime;
            BlackenImage.color = new Color(BlackenImage.color.r, BlackenImage.color.g, BlackenImage.color.b, elapsedBlackenTime / blackenTime);
            yield return null;
        }


        ProceedArea(area);


        float elapsedWhitingTime = 0f;
        while (elapsedWhitingTime < whitingTime)
        {
            elapsedWhitingTime += Time.deltaTime;
            BlackenImage.color = new Color(BlackenImage.color.r, BlackenImage.color.g, BlackenImage.color.b, OguzLib.Others.MirrorNumber(elapsedWhitingTime / whitingTime,0, 1));
            yield return null;
        }

        PlayerStats.Instance.SetOpenWorldAction(true);
        BlackenImage.gameObject.SetActive(false);

    }

}




[System.Serializable]
public class Area
{
    public MapLimits mapLimits;
    public Vector2 startLocation;
    public SpriteRenderer Renderer;
    public Sprite NewSprite;
    public List<Behaviour> AreaComponents;

    public void InitializeSpriteOverride()
    {
        if(Renderer != null && NewSprite != null) { Renderer.sprite = NewSprite; }
    }

    public void SetAreaComponentsActive(bool bol)
    {
        
        foreach(Behaviour behaviour in AreaComponents)
        {
            behaviour.enabled = bol;
        }
    }

    

    /*   GEREKSIZ???
    public Area(MapLimits MapLimitss, Vector2 StartLocationn, Sprite spriteToOverride = null)
    {
        mapLimits = MapLimitss;
        startLocation = StartLocationn;

        //OPTIONAL
        if(OverridedSprite != null && spriteToOverride != null)
        {
            OverridedSprite.sprite = spriteToOverride;
        }
    }
    */
}


[System.Serializable]
public struct MapLimits
{
    public Vector3 Limit1;
    public Vector3 Limit2;
    public MapLimits(Vector2 LeftDownCorner, Vector2 RightTopCorner)
    {
        Limit1 = LeftDownCorner;
        Limit2 = RightTopCorner;
    }
}

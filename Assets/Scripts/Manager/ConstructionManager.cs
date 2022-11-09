using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ConstructionManager : SingletonMonoBehaviour<ConstructionManager>
{
    [SerializeField] GameObject constructionSitePrefab;

    [FormerlySerializedAs("towerConstructionSitePrefab")]
    [SerializeField] GameObject constructionSiteTowerPrefab;
    [SerializeField, Space(10)] Transform constructionSiteParent_toGroup;

    List<ConstructionSiteController> constuctionsiteList_toReuse = new List<ConstructionSiteController>();
    List<ConstructionSiteController> constuctionsiteTowerList_toReuse = new List<ConstructionSiteController>();


    protected override void Awake()
    {
        base.Awake();
        AttatchNullCheck();
    }


    public ConstructionSiteController CreateConstructionSite(Vector3 pos, int needCount)
    {
        foreach (ConstructionSiteController pickedCsc in constuctionsiteList_toReuse)
        {
            if (pickedCsc.isActive_toJudgeReusable == false)
            {
                pickedCsc.Init(pos, needCount);
                return pickedCsc;
            }
        }

        ConstructionSiteController newConstructionSite_toAddList = Instantiate(constructionSitePrefab, parent: constructionSiteParent_toGroup).GetComponent<ConstructionSiteController>();
        newConstructionSite_toAddList.Init(pos, needCount);
        constuctionsiteList_toReuse.Add(newConstructionSite_toAddList);

        return newConstructionSite_toAddList;
    }


    public ConstructionSiteController CreateContructionTowerSite(Vector3 pos, int needCount)
    {
        foreach (ConstructionSiteController pickedCsc in constuctionsiteTowerList_toReuse)
        {
            if (pickedCsc.isActive_toJudgeReusable == false)
            {
                pickedCsc.Init(pos, needCount);
                return pickedCsc;
            }
        }

        ConstructionSiteController newConstructionSite_toAddList = Instantiate(constructionSiteTowerPrefab, parent: constructionSiteParent_toGroup).GetComponent<ConstructionSiteController>();
        newConstructionSite_toAddList.Init(pos, needCount);
        constuctionsiteTowerList_toReuse.Add(newConstructionSite_toAddList);

        return newConstructionSite_toAddList;
    }


    private void AttatchNullCheck()
    {
        Debug.Log("ConstructionManager Null Check.");
        if (!constructionSitePrefab) Debug.LogError("no attached error.");
        if (!constructionSiteTowerPrefab) Debug.LogError("no attached error.");
        if (!constructionSiteParent_toGroup) Debug.LogError("no attached error.");
    }
}

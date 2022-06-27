using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionManager : SingletonMonoBehaviour<ConstructionManager>
{
    [SerializeField] GameObject constructionSitePrefab;
    [SerializeField] GameObject towerConstructionSitePrefab;


    public void CreateConstructionSite(Vector3 pos, BuildCounterController bcc, int needCount, bool isTower = false)
    {
        if (isTower)
        {
            GameObject temp = Instantiate(towerConstructionSitePrefab, pos + new Vector3(0, 0, 0), Quaternion.identity);
            temp.GetComponent<ConstructionSiteController>().SetBcc(bcc, needCount);
        }
        else
        {
            GameObject temp = Instantiate(constructionSitePrefab, pos + new Vector3(0, 0.25f, 0), Quaternion.identity);
            temp.GetComponent<ConstructionSiteController>().SetBcc(bcc, needCount);
        }
    }
}

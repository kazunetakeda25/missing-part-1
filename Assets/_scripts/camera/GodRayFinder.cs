using UnityEngine;
using System.Collections;

public class GodRayFinder : MonoBehaviour 
{
    public int[] useInEpisodes;

    void Awake()
    {
        //SunShafts shafts = gameObject.GetComponent<SunShafts>();
        //if (shafts != null)
        //{
        //    if (QualitySettings.GetQualityLevel() <= 1)
        //    {
        //       // Destroy(shafts);
        //       // return;
        //    }
        //    foreach (int level in useInEpisodes)
        //    {
        //        if (Application.loadedLevel == level)
        //        {
        //            shafts.sunTransform = GameObject.FindWithTag("MainLight").transform;
        //        }
        //    }
        //}
        //else
        //{
        //    shafts.enabled = false;
        //}
    }
}

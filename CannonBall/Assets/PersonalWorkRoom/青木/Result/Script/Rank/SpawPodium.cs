using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawPodium : MonoBehaviour
{
    [SerializeField]
    float m_distance;

    [SerializeField]
    Transform m_spawPoint;

    [SerializeField, CustomLabel("•\²‘ä")]
    GameObject m_podium;

    List<StandGetPlayerPosition> m_spawPodium = new List<StandGetPlayerPosition>();

    public List<StandGetPlayerPosition> GetSpawPodiumList() { return m_spawPodium; }

    private void Start()
    {
        int spawCount = 0;
        int needRank = 1;       // ŸÒ‚¾‚¯•\¦
        int needRankCount = TemporaryData.GetSearch(needRank);

        for (int i = 0; i < needRankCount; i++)
        {
            Vector3 point = m_spawPoint.position;
            point = new Vector3(GetDigitPosition(needRankCount, i + 1) + point.x, point.y, point.z);


            m_spawPodium.Add(Instantiate(m_podium, point, Quaternion.identity).GetComponent<StandGetPlayerPosition>());


            spawCount++;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_digit"> ‚P‚“‚”‚ÌŒÂ” </param>
    /// <param name="_instanceCount"> ‰½ŒÂ–Ú‚Ì¶¬‚© </param>
    /// <returns></returns>
    private float GetDigitPosition(int _digit, int _instanceCount)
    {
        float posx;

        // ‹ô”
        if (_digit % 2 == 0)
        {
            // ‹ô”‚¾‚Á‚½‚ç
            posx = ((_digit * 0.5f - _instanceCount) + 0.5f) * m_distance;
            //Debug.Log("‹ô”" + (maxDigit * 0.5f - targetDigit) + m_distance * 0.5f);
        }
        // Šï”
        else
        {
            // Šï”‚¾‚Á‚½‚ç
            posx = ((int)(_digit * 0.5f) + 1 - _instanceCount) * m_distance;

            //Debug.Log("Šï”" + ((int)(maxDigit * 0.5f) + 1 - targetDigit));
        }


        return posx;
    }
}

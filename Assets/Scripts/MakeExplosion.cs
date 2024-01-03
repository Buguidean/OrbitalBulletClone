using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeExplosion : MonoBehaviour
{

    public Vector3 lastBossPosition;
    public GameObject panelBoss;

    private bool oneTime;
    private float timer;
    private GameObject finalExplosion;

    void Start()
    {
        oneTime = true;
        timer = 3f;
        finalExplosion = Resources.Load("prefabs/ExplosionFire") as GameObject;
        finalExplosion.GetComponent<MakeExplode>().explosionSound = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(gameObject.transform.childCount == 0 && oneTime)
        {
            oneTime = false;
            GameObject aux1 = Instantiate(finalExplosion, lastBossPosition, Quaternion.identity);
            Destroy(aux1, 2.5f);
        }

        if (timer <= 0f)
        {
            panelBoss.SetActive(true);
            Destroy(gameObject);
        }

        if (!oneTime)
            timer -= Time.deltaTime;

    }
}

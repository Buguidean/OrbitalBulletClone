using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_LifeBar_Boss : MonoBehaviour
{
    //public GameObject canvasLifeBar;
    public float actualHealth;
    public float maxHealth;

    private Image background;
    private Image lifeBar;

    // Start is called before the first frame update
    void Start()
    {
        background = gameObject.transform.GetChild(0).GetComponent<Image>();
        lifeBar = gameObject.transform.GetChild(1).GetComponent<Image>();
        //Debug.Log(lifeBar.name);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        lifeBar.fillAmount = actualHealth / maxHealth;
    }
}

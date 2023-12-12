using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_LifeBar : MonoBehaviour
{
    //public GameObject canvasLifeBar;
    public float actualHealth;
    public float maxHealth;
    public Transform camera;
    public Vector3 posEnemy;
    public float orientation;

    private Image background;
    private Image lifeBar;

    private float offset_y = 2f;

    // Start is called before the first frame update
    void Start()
    {
        background = gameObject.transform.GetChild(0).GetComponent<Image>();
        lifeBar = gameObject.transform.GetChild(1).GetComponent<Image>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = new Vector3(posEnemy.x, posEnemy.y + offset_y, posEnemy.z);

        lifeBar.fillAmount = actualHealth / maxHealth;
        
        transform.rotation = camera.rotation;
    }
}

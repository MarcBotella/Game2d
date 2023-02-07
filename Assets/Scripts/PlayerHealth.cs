using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Image lineLife;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameManager.life = Mathf.Clamp(GameManager.life, 0, 100);
        lineLife.fillAmount = GameManager.life / 100;
    }

    public void TakeDamage (int damage){
        GameManager.life = GameManager.life - damage;
    }
}

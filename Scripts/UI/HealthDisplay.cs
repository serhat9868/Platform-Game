using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthDisplay : MonoBehaviour
{
    GameObject player;
    bool onDamageMode = false;
    float damageAmount;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (onDamageMode) return;
        GetComponent<TextMeshProUGUI>().text = 
            String.Format("Health : {0:0}",player.GetComponent<Health>().GetCurrentHealthPoints());
    }

    public void DamageEffect()
    {
        StartCoroutine(DamageEffectCoroutine());
    }

    private IEnumerator DamageEffectCoroutine()
    {
        onDamageMode = true;
        GetComponent<TextMeshProUGUI>().text =
           String.Format("Health : -{0:0}", damageAmount);

        //Fast Effect
        for (int i = 0; i < 10; i++)
        {         
            GetComponent<TextMeshProUGUI>().color = Color.white;
            yield return new WaitForSeconds(0.1f);
            GetComponent<TextMeshProUGUI>().color = Color.red;
            yield return new WaitForSeconds(0.1f);
        }

         GetComponent<TextMeshProUGUI>().text =
         String.Format("Health : {0:0}", player.GetComponent<Health>().GetCurrentHealthPoints());

        // Effect is slowing Down here and speed of effect is medium
        for (int i = 0; i < 5; i++)
        {
            GetComponent<TextMeshProUGUI>().color = Color.white;
            yield return new WaitForSeconds(0.2f);
            GetComponent<TextMeshProUGUI>().color = Color.red;
            yield return new WaitForSeconds(0.2f);
        }

        //Slow wink
        for (int i = 0; i < 3; i++)
        {
            GetComponent<TextMeshProUGUI>().color = Color.white;
            yield return new WaitForSeconds(0.4f);
            GetComponent<TextMeshProUGUI>().color = Color.red;
            yield return new WaitForSeconds(0.4f);
        }
        yield return new WaitForSeconds(0.6f);

        GetComponent<TextMeshProUGUI>().color = Color.white;
        onDamageMode = false;
    }

    public void SetDamageAmount(float damage)
    {
        damageAmount = damage;
    }
    
}

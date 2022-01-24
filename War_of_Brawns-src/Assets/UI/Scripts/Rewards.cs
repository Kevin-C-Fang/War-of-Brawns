using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rewards : MonoBehaviour
{
    // Start is called before the first frame update
    public Text eincrease, sincrease, hincrease, dincrease, end, str, heal, def;
    public int end_increase, str_increase, health_increase, def_increase, endurance, strength, health, defense;

    void Start()
    {

        end_increase = PlayerPrefs.GetInt("end_increase");
        str_increase = PlayerPrefs.GetInt("str_increase");
        def_increase = PlayerPrefs.GetInt("def_increase");
        health_increase = PlayerPrefs.GetInt("health_increase");

        endurance = PlayerPrefs.GetInt("endurance");
        strength = PlayerPrefs.GetInt("strength");
        defense = PlayerPrefs.GetInt("defense");
        health = PlayerPrefs.GetInt("heal");

        eincrease.text = end_increase.ToString();
        sincrease.text = str_increase.ToString();
        hincrease.text = health_increase.ToString();
        dincrease.text = def_increase.ToString();

        end.text = endurance.ToString();
        str.text = strength.ToString();
        heal.text = health.ToString();
        def.text = defense.ToString();


    }

}

using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class WeaponData : ScriptableObject
{
    public WeaponType WeaponType;
    public  List<WeaponStats> stats;
    public BaseWeapon BaseWeaponPrefab;


    public int WeaponLevel;
    public bool isChild;
    public string WeaponName;
    public Sprite WeaponLogo;
    public string WeaponDescription;

    private void OnEnable()
    {
        WeaponLevel = 0;
    }
    private void Start()
    {
        WeaponLevel = 0;
    }
    public void InitializeWeapon()
    {
        WeaponLevel = 0;
        BaseWeaponPrefab.SetWeaponData(this);
    }
    public void LevelUp()
    {
        if(WeaponLevel<stats.Count-1)
        {
            WeaponLevel++;
        }
    }
    public bool IsMaxLevel()
    {
        return WeaponLevel >= stats.Count - 1;
    }
}

[System.Serializable]
public class WeaponStats
{
    public float cooldown = 5f;
    public float duration = 1f;
    public float damage = 1f;
    public float range = 0.7f;
    public string Description;

}
public enum WeaponType
{
    Melee,
    Range
}
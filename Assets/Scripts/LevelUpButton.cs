using UnityEngine;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;

public class LevelUpButton : MonoBehaviour
{
    public WeaponData weapon;
    public Button button;
    public TMPro.TextMeshProUGUI WeaponName;
    public TMPro.TextMeshProUGUI WeaponLevel;
    public TMPro.TextMeshProUGUI WeaponDescription;
    public Image WeaponIcon;

    public void ActivateButton()
    {
        WeaponName.text = weapon.WeaponName; // Show name in UI
        if(weapon.WeaponLevel<1)
        {
            WeaponLevel.text = "New Weapon Unlocked !!";

        }
        else
        {
            WeaponLevel.text = weapon.WeaponLevel.ToString();
        }
        WeaponDescription.text = weapon.WeaponDescription;
        WeaponIcon.sprite = weapon.WeaponLogo;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            WeaponUpgradeManager.instance.OnWeaponSelected(weapon);
            gameObject.SetActive(false);
        });
    }
}

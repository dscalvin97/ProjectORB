using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public enum UpgradeChoice { PlayerMaxHealth, PlayerSpeed, CritChance, CritDamage, Armor, ArmorDamage, WeaponDamage, WeaponFireRate, AmmoSpeed }

public class UpgradeMenuBehaviour : MonoBehaviour
{
    [SerializeField]
    private List<Image> _coverImages = new List<Image>();
    [SerializeField]
    private List<TextMeshProUGUI> _descriptions = new List<TextMeshProUGUI>();
    [SerializeField]
    private List<Sprite> _upgradeImages = new List<Sprite>();
    [SerializeField]
    private List<String> _upgradeDescriptions = new List<string>();
    [SerializeField]
    private Slider _HealthBar;


    private List<UpgradeChoice> _choices = new List<UpgradeChoice>();
    private GameManager _gameManager;
    private PlayerController _player;

    private void Start()
    {
        _player = FindObjectOfType<PlayerController>();
        _gameManager = FindObjectOfType<GameManager>();
    }

    public void GivePlayerUpgrade(int choiceIdx)
    {
        switch (_choices[choiceIdx])
        {
            case UpgradeChoice.PlayerMaxHealth:
                {
                    _player._MaxHealth = _player._MaxHealth + 50;
                    _HealthBar.maxValue = _player._MaxHealth;
                    _HealthBar.value = _player._CurrentHealth;
                }
                break;
            case UpgradeChoice.PlayerSpeed:
                {
                    _player._CharacterSpeed += 5 * .1f;
                    _gameManager._MoveSpeedText.text = "Move Speed\n" + _player._CharacterSpeed;
                }
                break;
            case UpgradeChoice.CritChance:
                {
                    _player._CritChance += .1f;
                }
                break;
            case UpgradeChoice.CritDamage:
                {
                    _player._CritDamageMultiplier += .25f;
                }
                break;
            case UpgradeChoice.Armor:
                {
                    _player._Armor += 10 * .1f;
                }
                break;
            case UpgradeChoice.ArmorDamage:
                {
                    _player._ArmorDamageReduction = Mathf.Clamp(_player._ArmorDamageReduction - .05f, .35f, 1f);
                }
                break;
            case UpgradeChoice.WeaponDamage:
                {
                    _player._BaseDamage += 20 * .15f;
                    _gameManager._AttackDamageText.text = "Attack Damage\n" + _player._BaseDamage;
                }
                break;
            case UpgradeChoice.WeaponFireRate:
                {
                    _player._FireRate += 1.5f * .2f;
                    _gameManager._FireSpeedText.text = "Fire Speed\n" + _player._FireRate;
                }
                break;
            case UpgradeChoice.AmmoSpeed:
                {
                    _player._AmmoSpeed += 10 * .1f;
                }
                break;
        }
        _gameManager._CurrentLevelText.text = _player._Level.ToString();

        gameObject.SetActive(false);
        _gameManager.ResumeGame();
    }

    public void SetRandomUpgradeChoices()
    {

        // Populate choices with random choice
        _choices.Clear();
        List<UpgradeChoice> availableChoices = new List<UpgradeChoice>();
        for (int i = 0; i < System.Enum.GetNames(typeof(UpgradeChoice)).Length; i++)
        {
            availableChoices.Add((UpgradeChoice)i);
        }
        for (int i = 0; i < _descriptions.Count; i++)
        {
            UpgradeChoice newChoice = availableChoices[Random.Range(0, availableChoices.Count-1)];
            while (_choices.Contains(newChoice))
            {
                availableChoices.Remove(newChoice);
                newChoice = availableChoices[Random.Range(0, availableChoices.Count - 1)];
            }
            _choices.Add(newChoice);

            // fill image and description wrt to choices
            _coverImages[i].sprite = _upgradeImages[(int)_choices[i]];
            _descriptions[i].text = _upgradeDescriptions[(int)_choices[i]];
        }
        
    }
}

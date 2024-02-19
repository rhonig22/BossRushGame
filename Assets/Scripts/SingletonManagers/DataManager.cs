using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    private PlayerData _playerData = new PlayerData();
    private FrogBossDifficulty[] _frogDifficultyList = new FrogBossDifficulty[] {
        new FrogBossDifficulty(300, 1, .25f, 1.5f, 0, new Dictionary<FrogAttackType, int>()
            {
                { FrogAttackType.Proximity, 40 },
                { FrogAttackType.Bubble, 60 },
                { FrogAttackType.BubbleStorm, 00 },
                { FrogAttackType.Bees, 00 },
                { FrogAttackType.Babies, 00 },
            }),
        new FrogBossDifficulty(350, 2, .4f, 1.25f, 3, new Dictionary<FrogAttackType, int>()
            {
                { FrogAttackType.Proximity, 40 },
                { FrogAttackType.Bubble, 50 },
                { FrogAttackType.BubbleStorm, 5 },
                { FrogAttackType.Bees, 20 },
                { FrogAttackType.Babies, 00 },
            }),
        new FrogBossDifficulty(400, 4, .5f, 1f, 3, new Dictionary<FrogAttackType, int>()
            {
                { FrogAttackType.Proximity, 40 },
                { FrogAttackType.Bubble, 35 },
                { FrogAttackType.BubbleStorm, 15 },
                { FrogAttackType.Bees, 10 },
                { FrogAttackType.Babies, 00 },
            }),
        new FrogBossDifficulty(450, 5, .75f, .75f, 5, new Dictionary<FrogAttackType, int>()
            {
                { FrogAttackType.Proximity, 25 },
                { FrogAttackType.Bubble, 40 },
                { FrogAttackType.BubbleStorm, 20 },
                { FrogAttackType.Bees, 15 },
                { FrogAttackType.Babies, 00 },
            }),
    };
    private Ability[,] _initialRewards = new Ability[,]
    {
        {new Ability("Bubble Wand", AbilityType.Bubble), new Ability("Jump Boots", AbilityType.Jump) }
    };
    public FrogBossDifficulty FrogBossDifficulty { get; private set; }
    public Ability[,] Rewards { get; private set; }
    public int CurrentDifficulty
    {
        get { return _currentDifficulty; }
        private set
        {
            _currentDifficulty = (int)Mathf.Clamp(value, 0, _frogDifficultyList.Length - 1);
            FrogBossDifficulty = _frogDifficultyList[_currentDifficulty];
            if (value > _currentDifficulty)
                FrogBossDifficulty.Health += _healthIncrement;
        }
    }
    public float TimePassed { get; private set; } = 0f;
    public bool IsTimeStarted { get; private set; } = false;
    public bool ShouldPauseAtStart { get; private set; } = true;
    public string[] FrogBossNames { get; private set; } = new string[]
    {
        "",
        "... again",
        "with increased difficulty",
        "in its final form"
    };
    [SerializeField] private AbilityType _initialAbility1 = AbilityType.Dodge;
    [SerializeField] private AbilityType _initialAbility2 = AbilityType.Scratch;
    private readonly int _healthIncrement = 50;
    private int _currentDifficulty = 0;
    private string _userName;
    private string _userId;


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        ResetData();
    }

    private void Update()
    {
        if (IsTimeStarted)
        {
            TimePassed += Time.deltaTime;
        }
    }
    public void StartTimer()
    {
        IsTimeStarted = true;
    }
    public void PauseTimer()
    {
        IsTimeStarted = false;
        SetTime(TimePassed);
    }

    public void ResetData()
    {
        _playerData = new PlayerData(_initialAbility1, _initialAbility2);
        CurrentDifficulty = 0;
        TimePassed = 0f;
        Rewards = (Ability[,])_initialRewards.Clone();
        Rewards[0, 0].IsTaken = false;
        Rewards[0, 1].IsTaken = false;
    }

    public void IncreaseDifficulty()
    {
        CurrentDifficulty = CurrentDifficulty + 1;
    }

    public void SetTime(float time)
    {
        _playerData.TimePassed = time;
    }

    public float GetTimeValue()
    {
        return _playerData.TimePassed;
    }

    public string GetTime()
    {
        return TimeSpan.FromSeconds((double)_playerData.TimePassed).ToString(@"mm\:ss");
    }

    public void SetHealth(int health)
    {
        _playerData.Health = health;
    }

    public int GetHealth() { return _playerData.Health; }

    public int GetMaxHealth() { return _playerData.MaxHealth; }

    public void SetName(string name)
    {
        _userName = name;
    }

    public string GetName() { return _userName; }

    public void SetId(string id)
    {
        _userId = id;
    }

    public string GetId() { return _userId; }

    public void AddDamageDealt(int damage)
    {
        _playerData.DamageDealt += damage;
    }

    public int GetDamageDealt() { return _playerData.DamageDealt; }

    public void AddBossDefeated()
    {
        _playerData.BossesDefeated += 1;
    }

    public int GetBossesDefeated() { return _playerData.BossesDefeated; }

    public int GetDamage(float multiplier)
    {
        var damage = Mathf.FloorToInt(_playerData.BaseDamage * multiplier);
        AddDamageDealt(damage);
        return damage;
    }

    public AbilityType[] GetAbilities() { return _playerData.Abilities; }

    public void SetAbilities(AbilityType ability1, AbilityType ability2)
    {
        _playerData.Abilities[0] = ability1;
        _playerData.Abilities[1] = ability2;
    }

    public void InitialPauseComplete()
    {
        ShouldPauseAtStart = false;
    }
}

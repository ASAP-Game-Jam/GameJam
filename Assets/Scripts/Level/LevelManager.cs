using Assets.Scripts.Base;
using Assets.Scripts.CustomEventArgs;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Interfaces.Base;
using Assets.Scripts.Other;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour, ILevelManager
{
    public LevelHUD levelHUD;
    [SerializeField] uint startEnergy = 5;
    private LevelProgress progress;

    public uint Score
    {
        get => progress.LevelScore;
        set
        {
            progress.LevelScore = Math.Abs(
                Math.Max(progress.LevelScore, value) - Math.Min(progress.LevelScore, value)) > 100
                ? 0 : value;
            levelHUD?.AddProgressCommand(progress.LevelScore);
        }
    }

    private void Awake()
    {
        progress = new LevelProgress();
        foreach (IBase i in FindObjectsOfType<Base>())
        {
            i.OnDestroy += this.EndLevel;
        }
    }
    private void Start()
    {
        levelHUD = FindObjectOfType<LevelHUD>();
        Score = startEnergy;
    }
    private void EndLevel(object sender, EventArgs eventArgs)
    {
        if (eventArgs is EventBaseArgs args)
        {
            if (args.HP == 0)
            {
                levelHUD.AddEndOfTheGameCommand(args.BaseType);
                if (args.BaseType == Assets.Scripts.Other.BaseType.EnemyBase)
                    foreach (var i in FindObjectsOfType<EnemyController>())
                    {
                        i.Direction = Direction.Right;
                    }
            }
        }
    }
    public void HoldOnBase(Base baseObject)
    {
        if (baseObject != null)
        {
            switch (baseObject.BaseType)
            {
                case BaseType.EnemyBase:
                    baseObject.OnTakeDamage += (object sender, EventArgs args) =>
                    {
                        if (args is EventBaseArgs baseArgs)
                            levelHUD.AddBaseHPCommand(baseArgs.MaxHP, baseArgs.HP);
                    };
                    baseObject.TakeDamage(0);
                    break;
            }
        }
    }
    public void ReloadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SaveGame()
    {
        throw new NotImplementedException();
    }

    public void LoadGame()
    {
        throw new NotImplementedException();
    }

    public void ExitGame()
    {
        throw new NotImplementedException();
    }

    public void OpenMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
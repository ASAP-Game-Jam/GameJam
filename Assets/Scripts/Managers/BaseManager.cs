using Assets.Scripts.GameObjects.Entities;
using Assets.Scripts.GameObjects.Fractions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public struct HP
    {
        public int MaxHP { get; set; }
        public int Hp { get; set; }
    }
    public class BaseManager : MonoBehaviour, IManager
    {
        public event Action<Func<FractionType,HP>> OnUpdateHP;
        public event Action<FractionType> OnEndingGame;
        private Dictionary<FractionType, List<IBase>> countBases = new Dictionary<FractionType, List<IBase>>()
        {
            {FractionType.None,new List<IBase>()},
            {FractionType.Ally,new List<IBase>()},
            {FractionType.Enemy,new List<IBase>()}
        };
        public EStatusManager Status { get; private set; }

        public void Shutdown()
        {
            Status = EStatusManager.Shutdown;
        }

        public void Startup()
        {
            Status = EStatusManager.Started;
        }
        public void AddBase(IBase ibase)
        {
            if (ibase is MonoBehaviour mono && mono?.GetComponent<IFraction>() is IFraction fraction)
            {
                switch (fraction.Fraction)
                {
                    case FractionType.Ally:
                        ibase.OnDestroyed += AllyBaseDestroyed;
                        break;
                    case FractionType.Enemy:
                        ibase.OnDestroyed += EnemyBaseDestroyed;
                        break;
                }
                countBases[fraction.Fraction].Add(ibase);
                ibase.OnTakenDamage += (e, d) => OnUpdateHP?.Invoke(GetBaseHPInfo);
            }
        }
        private HP GetBaseHPInfo(FractionType type)
        {
            int currentHp = 0, maxHp = 0;
            if (countBases.ContainsKey(type))
                (currentHp, maxHp) = (
                    countBases[type].Select(e => e.HP).Sum(),
                    countBases[type].Select(e => e.MaxHP).Sum());
            return new HP { Hp = currentHp, MaxHP = maxHp };
        }
        private void EndGameCheck()
        {
            bool allyDef = false;
            bool enemyDef = false;
            if (countBases[FractionType.Ally].Count == 0)
                allyDef = true;
            if (countBases[FractionType.Enemy].Count == 0)
                enemyDef = true;

            if (allyDef && enemyDef)
                OnEndingGame?.Invoke(FractionType.None);
            else if (enemyDef)
                OnEndingGame?.Invoke(FractionType.Ally);
            else if (allyDef)
                OnEndingGame?.Invoke(FractionType.Enemy);
        }
        private void AllyBaseDestroyed(IBasicEntity entity)
        {
            if (entity is IBase eBase)
            {
                countBases[FractionType.Ally].Remove(eBase);
                Debug.Log("Ally base destroyed");
                EndGameCheck();
            }
        }
        private void EnemyBaseDestroyed(IBasicEntity entity)
        {
            if (entity is IBase eBase)
            {
                countBases[FractionType.Enemy].Remove(eBase);
                Debug.Log("Enemy base destroyed");
                EndGameCheck();
            }
        }
    }
}

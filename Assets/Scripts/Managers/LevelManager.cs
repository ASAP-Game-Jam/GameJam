using UnityEngine;

namespace Assets.Scripts.Managers
{
    [RequireComponent(typeof(StateManager))]
    [RequireComponent(typeof(UIManager))]
    [RequireComponent(typeof(AllyManager))]
    [RequireComponent(typeof(BaseManager))]
    [RequireComponent(typeof(EnemyManager))]
    [RequireComponent(typeof(GameManager))]
    [RequireComponent(typeof(WaveManager))]
    [RequireComponent(typeof(TimerManager))]
    [RequireComponent(typeof(SellManager))]
    public class LevelManager : BasicManager
    {
        public static StateManager StateManager { get; private set; }
        public static UIManager UIManager { get; private set; }
        public static AllyManager AllyManager { get; private set; }
        public static BaseManager BaseManager { get; private set; }
        public static EnemyManager EnemyManager { get; private set; }
        public static WaveManager WaveManager { get; private set; }
        public static GameManager GameManager { get; private set; }
        public static TimerManager TimerManager { get; private set; }
        public static SellManager SellManager { get; private set; }
        private void Awake()
        {
            StateManager = GetComponent<StateManager>();
            UIManager = GetComponent<UIManager>();
            AllyManager = GetComponent<AllyManager>();
            BaseManager = GetComponent<BaseManager>();
            EnemyManager = GetComponent<EnemyManager>();
            GameManager = GetComponent<GameManager>();
            WaveManager = GetComponent<WaveManager>();
            TimerManager = GetComponent<TimerManager>();
            SellManager = GetComponent<SellManager>();

            managers.Add(StateManager);
            managers.Add(UIManager);
            managers.Add(AllyManager);
            managers.Add(BaseManager);
            managers.Add(EnemyManager);
            managers.Add(GameManager);
            managers.Add(WaveManager);
            managers.Add(TimerManager);
            managers.Add(SellManager);
        }
    }
}

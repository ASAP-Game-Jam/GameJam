using UnityEngine;

namespace Assets.Scripts.Managers
{
    [RequireComponent(typeof(StateManager))]
    [RequireComponent(typeof(UIManager))]
    [RequireComponent(typeof(AllyManager))]
    [RequireComponent(typeof(BaseManager))]
    [RequireComponent(typeof(EnemyManager))]
    [RequireComponent(typeof(GameManager))]
    public class LevelManager : BasicManager
    {
        public static StateManager StateManager { get; private set; }
        public static UIManager UIManager { get; private set; }
        public static AllyManager AllyManager { get; private set; }
        public static BaseManager BaseManager { get; private set; }
        public static EnemyManager EnemyManager { get; private set; }
        public static GameManager GameManager { get; private set; }

        private void Awake()
        {
            StateManager = GetComponent<StateManager>();
            UIManager = GetComponent<UIManager>();
            AllyManager = GetComponent<AllyManager>();
            BaseManager = GetComponent<BaseManager>();
            EnemyManager = GetComponent<EnemyManager>();
            GameManager = GetComponent<GameManager>();

            managers.Add(StateManager);
            managers.Add(UIManager);
            managers.Add(AllyManager);
            managers.Add(BaseManager);
            managers.Add(EnemyManager);
            managers.Add(GameManager);
        }
    }
}

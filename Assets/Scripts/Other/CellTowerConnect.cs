using Assets.Scripts.Interfaces;

namespace Assets.Scripts.Other
{
    public class CellTowerConnect : ICellandITower
    {
        public IUICard Card { get; set; }
        public ICell Cell { get; set; }
        public TowerType Type {  get; set; }
    }
}

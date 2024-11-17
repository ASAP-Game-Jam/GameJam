using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Interfaces
{
    public interface ICellandITower
    {
        TowerType Type { get; set; }
        IUICard Card { get; set; }
        ICell Cell { get; set; }
    }
}

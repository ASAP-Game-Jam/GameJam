using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyCommand
{
    public uint Energy { get; private set; }

    public EnergyCommand(uint energy)
    {
        Energy = energy;
    }

    // ����� ����� �������� �������������� ������ � �������� ��� EnergyCommand
}

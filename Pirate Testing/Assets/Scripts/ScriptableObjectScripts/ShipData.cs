using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Ship Data", menuName = "Data/Ships/Data")]
public class ShipData : ScriptableObject
{
    public ShipType ShipType;

    public ShipStationsData ShipStationsData
    {
        get => shipStationsData;
        set => shipStationsData = value;
    }

    public ShipDurabilityData ShipDurabilityData
    {
        get => shipDurabilityData;
        set => shipDurabilityData = value;
    }

    public int maxCrew;
    private ShipStationsData shipStationsData;
    private ShipDurabilityData shipDurabilityData;
}
public enum ShipType {Sloop, Frigate, Brigantine, RedShip}

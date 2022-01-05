using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ShipStations Data", menuName = "Data/Ships/Stations Data")]
public class ShipStationsData : ScriptableObject
{
    public int maxNavigationStations;
    public int maxSailingStations;
    public int maxRepairStations;
    public int maxBilgingStations;
    public int maxGunningStations;
}

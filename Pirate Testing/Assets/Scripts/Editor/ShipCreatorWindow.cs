using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine.WSA;

public class ShipCreatorWindow : EditorWindow
{
    private ShipType shipType;
    private ShipData currentShipData;
    
    SerializedObject soData;
    SerializedObject soStationsData;
    SerializedObject soDurabilityData;

    [MenuItem("Game/Ship Creator")]
    public static void CreateShip() => GetWindow<ShipCreatorWindow>("Ship Creator");

    public string[] array = new string[3];
    
    private void OnEnable()
    {
        FindShipData(shipType);
    }

    private void OnGUI()
    {
        GUILayout.Label("Use this window to modify the ships value.");
        GUILayout.Label("Check for a ships data here instead of going through the scriptable objects");

        GUILayout.Space(10);

        using (new GUILayout.HorizontalScope(EditorStyles.helpBox))
        {
            GUILayout.Label("Ship Type: ", EditorStyles.boldLabel, GUILayout.Width(80));

            using (var check = new EditorGUI.ChangeCheckScope())
            {
                shipType = (ShipType) EditorGUILayout.EnumPopup(shipType);
                
                if (check.changed)
                {
                    if (!FindShipData(shipType))
                    {
                        CreateShipAsset(typeof(ShipData),
                            $"Assets/Scripts/Data/ShipData/{shipType.ToString()}ShipData.asset");
                        
                        AssetDatabase.CreateFolder("Assets/Scripts/Data/ShipData", $"{shipType.ToString()}Data");
                        
                        CreateShipAsset(typeof(ShipStationsData),
                            $"Assets/Scripts/Data/ShipData/{shipType.ToString()}Data/{shipType.ToString()}ShipStations.asset");
                        CreateShipAsset(typeof(ShipDurabilityData), $"Assets/Scripts/Data/ShipData/{shipType.ToString()}Data/{shipType.ToString()}ShipDurability.asset");
                        
                        ShipData currentShip = (ShipData)AssetDatabase.LoadAssetAtPath($"Assets/Scripts/Data/ShipData/{shipType.ToString()}ShipData.asset", typeof(ShipData));
                        Debug.Log(currentShip);
                        
                        currentShip.ShipStationsData = (ShipStationsData)AssetDatabase.LoadAssetAtPath(
                            $"Assets/Scripts/Data/ShipData/{shipType.ToString()}Data/{shipType.ToString()}ShipStations.asset", typeof(ShipStationsData));
                        Debug.Log(currentShip.ShipStationsData);
                        currentShip.ShipDurabilityData = (ShipDurabilityData)AssetDatabase.LoadAssetAtPath(
                            $"Assets/Scripts/Data/ShipData/{shipType.ToString()}Data/{shipType.ToString()}ShipDurability.asset", typeof(ShipDurabilityData));
                        Debug.Log(currentShip.ShipDurabilityData);
                        
                        FindShipData(shipType);
                    }
                }
            }
        }
        PrintShipData(currentShipData);
    }

    void CreateShipAsset(Type T, string path)
    {
        ScriptableObject so = CreateInstance(T);
        AssetDatabase.CreateAsset(so, path);
    }

    bool FindShipData(ShipType shipType)
    {
        string path = $"Assets/Scripts/Data/ShipData/{shipType}ShipData.asset";
        currentShipData = (ShipData)AssetDatabase.LoadAssetAtPath(path, typeof(ShipData));

        if (currentShipData == null)
            return false;
        
        soData = new SerializedObject(currentShipData);
        soStationsData = new SerializedObject(currentShipData.ShipStationsData);
        soDurabilityData = new SerializedObject(currentShipData.ShipDurabilityData);
        return true;
    }

    void PrintShipData(ShipData shipData)
    {
        using (new GUILayout.VerticalScope(EditorStyles.helpBox))
        {
            ValueScope("Maximum Crew", 100, soData, soData.FindProperty("maxCrew"), true);
            ValueScope("Durability", 100, soDurabilityData, soDurabilityData.FindProperty("maxDurability"), true);
            
            using (new GUILayout.VerticalScope(EditorStyles.helpBox))
            {
                GUILayout.Label("Stations", EditorStyles.boldLabel);
                GUILayout.Space(10);
                ValueScope("Navigation", 80, soStationsData, soStationsData.FindProperty("maxNavigationStations"), false);
                GUILayout.Space(10);
                using (new GUILayout.HorizontalScope())
                {
                    ValueScope("Sailing", 80, soStationsData, soStationsData.FindProperty("maxSailingStations"), false);
                    ValueScope("Gunning", 80, soStationsData, soStationsData.FindProperty("maxGunningStations"), false);
                }
                using (new GUILayout.HorizontalScope())
                {
                    ValueScope("Repair", 80, soStationsData, soStationsData.FindProperty("maxRepairStations"), false);
                    ValueScope("Bilging", 80, soStationsData, soStationsData.FindProperty("maxBilgingStations"), false);
                }
            }
        }
    }

    void ValueScope(string title, int space, SerializedObject so, SerializedProperty sP, bool helpBox)
    {
        if (!helpBox)
        {
            using (new GUILayout.HorizontalScope())
            {
                so.Update();
                EditorGUIUtility.labelWidth = space; 
                EditorGUILayout.PropertyField(sP, new GUIContent(title));
                so.ApplyModifiedProperties();
            }
        }
        else
        {
            using (new GUILayout.HorizontalScope(EditorStyles.helpBox))
            {
                so.Update();
                EditorGUIUtility.labelWidth = space; 
                EditorGUILayout.PropertyField(sP, new GUIContent(title));
                so.ApplyModifiedProperties();
            }
        }
    }
}

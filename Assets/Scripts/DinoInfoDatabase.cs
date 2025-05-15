using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dino/DinoDatabase")]
public class DinoDatabase : ScriptableObject
{
    public List<DinoInfo> dinos;

    public DinoInfo GetDinoInfo(string dinoName)
    {
        return dinos.Find(d => d.dinoName == dinoName);
    }
}

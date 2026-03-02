using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "LevelData", menuName = "Game/Level Data")]
public class LevelData : ScriptableObject
{
    public int levelIndex;
    public string judulLevel;

    [Header("PROLOG")]
    public List<NarrativeSlide> prologSlides;
    
    [Header("EPILOG")]
    public List<NarrativeSlide> epilogSlides;
}

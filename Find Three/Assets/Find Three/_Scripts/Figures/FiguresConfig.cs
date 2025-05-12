using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

[Serializable]
public struct FigureShape
{
    public FigureType FigureType;
    public Figure FigurePrefab;
}

[Serializable]
public struct FigureColor
{
    public ColorType ColorType;
    public Color Color;
}

[Serializable]
public struct FigureCreature
{
    public CreatureType CreatureType;
    public Sprite CreatureSprite;
}


[CreateAssetMenu(fileName = "Figures Config", menuName = "Configs/FiguresConfig")]
public class FiguresConfig : ScriptableObject
{
    [SerializeField] private List<FigureShape> _figureShapes = new List<FigureShape>();
    [SerializeField] private List<FigureColor> _figureColors = new List<FigureColor>();
    [SerializeField] private List<FigureCreature> _figureCreatures = new List<FigureCreature>();

    private IReadOnlyDictionary<FigureType, Figure> _readOnlyFigures;
    private IReadOnlyDictionary<ColorType, Color> _readOnlyColors;
    private IReadOnlyDictionary<CreatureType, Sprite> _readOnlyCreatures;

    public IReadOnlyDictionary<FigureType, Figure> FigurePrefabs => _readOnlyFigures;
    public IReadOnlyDictionary<ColorType, Color> FigureColors => _readOnlyColors;
    public IReadOnlyDictionary<CreatureType, Sprite> CreatureSprites => _readOnlyCreatures;

    private void OnEnable()
    {
        InitializeDictionaries();
    }

    private void InitializeDictionaries()
    {
        var figuresDict = new Dictionary<FigureType, Figure>();
        foreach (var item in _figureShapes)
        {
            if (figuresDict.ContainsKey(item.FigureType))
            {
                Debug.LogWarning($"[{name}] Duplicate FigureType key '{item.FigureType}' in _figureShapesList. Overwriting with new value.");
            }
            figuresDict[item.FigureType] = item.FigurePrefab;
        }
        _readOnlyFigures = new ReadOnlyDictionary<FigureType, Figure>(figuresDict);

        var colorsDict = new Dictionary<ColorType, Color>();
        foreach (var item in _figureColors)
        {
            if (colorsDict.ContainsKey(item.ColorType))
            {
                Debug.LogWarning($"[{name}] Duplicate ColorType key '{item.ColorType}' in _figureColorsList. Overwriting with new value.");
            }
            colorsDict[item.ColorType] = item.Color;
        }
        _readOnlyColors = new ReadOnlyDictionary<ColorType, Color>(colorsDict);

        var creaturesDict = new Dictionary<CreatureType, Sprite>();
        foreach (var item in _figureCreatures)
        {
            if (creaturesDict.ContainsKey(item.CreatureType))
            {
                Debug.LogWarning($"[{name}] Duplicate CreatureType key '{item.CreatureType}' in _figureCreaturesList. Overwriting with new value.");
            }
            creaturesDict[item.CreatureType] = item.CreatureSprite;
        }
        _readOnlyCreatures = new ReadOnlyDictionary<CreatureType, Sprite>(creaturesDict);
    }
}

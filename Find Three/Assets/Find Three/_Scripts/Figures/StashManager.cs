using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class StashManager : IInitializable, IDisposable
{
    public event Action OnStashCompleted;

    private List<FigureDefinition> _figures = new List<FigureDefinition>();

    [Inject] private readonly GameManager _gameManager;
    [Inject] private readonly FiguresManager _figuresManager;
    private readonly StashImage[] _stashImages;
    private readonly FiguresConfig _figuresConfig;

    public StashManager(StashImage[] stashImages, FiguresConfig figuresConfig)
    {
        _figuresConfig = figuresConfig;
        _stashImages = stashImages;
    }

    public int CurrentStashedFiguresCount => _figures.Count;

    public void Initialize()
    {
        Figure.OnInteracted += FigureInteractedHandler;
        _gameManager.OnNewGameStarted += ClearStash;
        _figuresManager.OnNewFiguresSpawned += ClearStash;
    }

    public void Dispose()
    {
        Figure.OnInteracted -= FigureInteractedHandler;
        _gameManager.OnNewGameStarted -= ClearStash;
        _figuresManager.OnNewFiguresSpawned -= ClearStash;
    }

    private void ClearStash()
    {
        _figures.Clear();

        UpdateStashImages();
    }

    private void FigureInteractedHandler(Figure figure)
    {
        var addedDefinition = figure.FigureDefinition;
        _figures.Add(addedDefinition);

        if (_figures.Count(f => f.Equals(addedDefinition)) >= 3)
        {
            _figures.RemoveAll(f => f.Equals(addedDefinition));
        }

        if (_figures.Count == _stashImages.Length)
        {
            OnStashCompleted?.Invoke();
        }

        UpdateStashImages();
    }

    private void UpdateStashImages()
    {
        for (int i = 0; i < _stashImages.Length; i++)
        {
            if (i < _figures.Count)
            {
                _stashImages[i].Show(
                    _figuresConfig.FigurePrefabs[_figures[i].ShapeType].GetComponent<SpriteRenderer>().sprite,
                    _figuresConfig.FigureColors[_figures[i].ColorType],
                    _figuresConfig.CreatureSprites[_figures[i].CreatureType]
                );
            }
            else
            {
                _stashImages[i].Hide();
            }
        }
    }
}

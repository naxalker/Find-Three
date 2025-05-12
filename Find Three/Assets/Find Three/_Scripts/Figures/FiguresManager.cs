using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class FiguresManager : IInitializable, IDisposable
{
    public event Action OnCompleted;
    public event Action OnNewFiguresSpawned;

    private const int BASE_TRIPLETS_COUNT = 20;
    private const float SPAWN_DELAY = 0.15f;

    private List<Figure> _figures;

    [Inject] private readonly GameManager _gameManager;
    [Inject] private readonly StashManager _stashManager;
    private readonly FigureFactory _figureFactory;
    private readonly Transform _figuresHolder;
    private readonly Button _spawnNewFiguresButton;

    public FiguresManager(FigureFactory figureFactory, Transform figuresHolder, Button spawnNewFiguresButton)
    {
        _figureFactory = figureFactory;
        _figuresHolder = figuresHolder;
        _spawnNewFiguresButton = spawnNewFiguresButton;
    }

    public async void Initialize()
    {
        _figures = await PopulateFigures(BASE_TRIPLETS_COUNT);

        ThrowFigures();

        Figure.OnInteracted += FigureInteractedHandler;
        _gameManager.OnNewGameStarted += NewGameStartedHandler;

        _spawnNewFiguresButton.onClick.AddListener(SpawnNewFiguresButtonPressedHandler);
    }

    public void Dispose()
    {
        Figure.OnInteracted -= FigureInteractedHandler;
        _gameManager.OnNewGameStarted -= NewGameStartedHandler;
    }

    private void FigureInteractedHandler(Figure figure)
    {
        _figures.Remove(figure);

        if (_figures.Count == 0)
        {
            Debug.Log("All figures interacted with.");
            OnCompleted?.Invoke();
        }
    }

    private async void NewGameStartedHandler()
    {
        _figures = await PopulateFigures(BASE_TRIPLETS_COUNT);
        ThrowFigures();
    }

    private async void SpawnNewFiguresButtonPressedHandler()
    {
        if (_gameManager.IsInputActive == false) { return; }

        _figures = await PopulateFigures((_figures.Count + _stashManager.CurrentStashedFiguresCount) / 3);
        ThrowFigures();
    }

    private async UniTask<List<Figure>> PopulateFigures(int tripletsCount)
    {
        if (_figures != null && _figures.Count > 0)
        {
            List<UniTask> deactivationTasks = new List<UniTask>();
            foreach (Figure figure in _figures)
            {
                deactivationTasks.Add(figure.Deactivate());
            }
            await UniTask.WhenAll(deactivationTasks);
        }

        FigureDefinition[] figureDefinitions = CreateDefinitions(tripletsCount);

        List<Figure> figures = new List<Figure>(tripletsCount * 3);

        for (int i = 0; i < tripletsCount * 3; i++)
        {
            Figure figure = _figureFactory.CreateFigure(figureDefinitions[i]);
            figure.transform.SetParent(_figuresHolder, false);
            figures.Add(figure);
        }

        _figures = new List<Figure>(figures);

        OnNewFiguresSpawned?.Invoke();

        return figures;
    }

    private FigureDefinition[] CreateDefinitions(int tripletsCount)
    {
        FigureDefinition[] figureDefinitions = new FigureDefinition[tripletsCount * 3];
        HashSet<string> usedCombinations = new HashSet<string>();

        for (int i = 0; i < tripletsCount * 3; i += 3)
        {
            FigureDefinition figureDefinition;
            string combination;

            do
            {
                figureDefinition = new FigureDefinition(
                    (FigureType)UnityEngine.Random.Range(0, Enum.GetValues(typeof(FigureType)).Length),
                    (ColorType)UnityEngine.Random.Range(0, Enum.GetValues(typeof(ColorType)).Length),
                    (CreatureType)UnityEngine.Random.Range(0, Enum.GetValues(typeof(CreatureType)).Length)
                );

                combination = $"{(int)figureDefinition.ShapeType}_{(int)figureDefinition.ColorType}_{(int)figureDefinition.CreatureType}";
            }
            while (usedCombinations.Contains(combination));

            usedCombinations.Add(combination);

            figureDefinitions[i] = figureDefinition;
            figureDefinitions[i + 1] = figureDefinition;
            figureDefinitions[i + 2] = figureDefinition;
        }

        var random = new System.Random();
        figureDefinitions = figureDefinitions.OrderBy(x => random.Next()).ToArray();

        return figureDefinitions;
    }

    private async void ThrowFigures()
    {
        _gameManager.IsInputActive = false;

        // List<UniTask> deactivationTasks = new List<UniTask>();
        // foreach (Figure figure in _figures)
        // {
        //     deactivationTasks.Add(figure.Deactivate());
        // }
        // await UniTask.WhenAll(deactivationTasks);

        foreach (Figure figure in _figures)
        {
            figure.transform.localPosition = new Vector3(UnityEngine.Random.Range(-0.1f, 0.1f), 0f, 0f);
            figure.Activate().Forget();
            await UniTask.Delay(TimeSpan.FromSeconds(SPAWN_DELAY));
        }

        _gameManager.IsInputActive = true;
    }
}

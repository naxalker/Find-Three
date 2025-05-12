using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameplayInstaller : MonoInstaller
{
    [SerializeField] private FiguresConfig _figuresConfig;

    [SerializeField] private Transform _figuresHolder;
    [SerializeField] private Button _spawnNewFiguresButton;

    [SerializeField] private StashImage[] _stashImages;

    [SerializeField] private GameOverPanel _gameOverPanel;

    public override void InstallBindings()
    {
        Container.BindInstance(_figuresConfig).AsSingle();
        Container.Bind<FigureFactory>().AsSingle();
        Container.BindInterfacesAndSelfTo<FiguresManager>().AsSingle().WithArguments(_figuresHolder, _spawnNewFiguresButton);

        Container.BindInterfacesAndSelfTo<InputManager>().AsSingle();

        Container.BindInterfacesAndSelfTo<StashManager>().AsSingle().WithArguments(_stashImages);

        Container.BindInterfacesAndSelfTo<GameManager>().AsSingle().WithArguments(_gameOverPanel);
    }
}
using System;
using Zenject;

public class GameManager : IInitializable, IDisposable
{
    public event Action OnNewGameStarted;

    public bool IsInputActive = true;

    [Inject] private readonly StashManager _stashManager;
    [Inject] private readonly FiguresManager _figuresManager;

    private GameOverPanel _gameOverPanel;

    public GameManager(GameOverPanel gameOverPanel)
    {
        _gameOverPanel = gameOverPanel;
    }

    public void Initialize()
    {
        _figuresManager.OnCompleted += FiguresCompletedHandler;
        _stashManager.OnStashCompleted += StashCompletedHandler;

        _gameOverPanel.RestartButton.onClick.AddListener(() => RestartGameButtonPressedHandler());
    }

    public void Dispose()
    {
        _figuresManager.OnCompleted -= FiguresCompletedHandler;
        _stashManager.OnStashCompleted -= StashCompletedHandler;
    }

    private void RestartGameButtonPressedHandler()
    {
        _gameOverPanel.Hide();

        OnNewGameStarted?.Invoke();
    }

    private void FiguresCompletedHandler()
    {
        IsInputActive = false;

        _gameOverPanel.Show(true);
    }

    private void StashCompletedHandler()
    {
        IsInputActive = false;

        _gameOverPanel.Show(false);
    }
}

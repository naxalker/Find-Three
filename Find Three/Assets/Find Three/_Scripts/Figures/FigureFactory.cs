using Zenject;

public class FigureFactory
{
    private readonly FiguresConfig _figuresConfig;
    private readonly DiContainer _diContainer;

    public FigureFactory(FiguresConfig figuresConfig, DiContainer diContainer)
    {
        _figuresConfig = figuresConfig;
        _diContainer = diContainer;
    }

    public Figure CreateFigure(FigureDefinition figureDefinition)
    {
        Figure figure =
            _diContainer.InstantiatePrefabForComponent<Figure>(_figuresConfig.FigurePrefabs[figureDefinition.ShapeType]);

        figure.Setup(figureDefinition,
            _figuresConfig.FigureColors[figureDefinition.ColorType],
            _figuresConfig.CreatureSprites[figureDefinition.CreatureType]);

        return figure;
    }
}

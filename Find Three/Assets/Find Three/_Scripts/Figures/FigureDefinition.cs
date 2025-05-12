using System;

public struct FigureDefinition
{
    public FigureType ShapeType;
    public ColorType ColorType;
    public CreatureType CreatureType;

    public FigureDefinition(FigureType shape, ColorType color, CreatureType creature)
    {
        ShapeType = shape;
        ColorType = color;
        CreatureType = creature;
    }

    public override bool Equals(object obj)
    {
        return obj is FigureDefinition other &&
               ShapeType == other.ShapeType &&
               ColorType == other.ColorType &&
               CreatureType == other.CreatureType;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(ShapeType, ColorType, CreatureType);
    }

    public static bool operator ==(FigureDefinition a, FigureDefinition b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(FigureDefinition a, FigureDefinition b)
    {
        return !(a == b);
    }
}
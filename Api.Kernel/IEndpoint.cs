

public interface IGetEndpoint
{
    static abstract string Route { get; }
    static abstract Delegate Handler { get; }
}

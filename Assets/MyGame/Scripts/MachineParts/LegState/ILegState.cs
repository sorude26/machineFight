using System;
public partial class LegStateContext
{
    public interface ILegState
    {
        void ExecuteEnter(LegStateContext context);
        void ExecuteUpdate(LegStateContext context);
        void ExecuteExit(LegStateContext context);
    }
}
[Serializable]
public struct LegAnimation 
{
    public string Idle;
    public string Walk;
    public string Back;
    public string TurnLeft;
    public string TurnRight;
    public string Jump;
    public string Landing;
}
[Serializable]
public struct LegActionParam
{
    public float WalkSpeed;
    public float WalkTurnSpeed;
    public float JumpPower;
    public float DotSub;
    public float AnimeChangeSpeed;
}
public interface MovementInterface
{
    bool ableToMove { get; set; }
    void ChangeSpeed(float coef, float time);
}

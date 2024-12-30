public class BreakableWall : Door
{
    override protected void PlayOpeningAnimation()
    {
        gameObject.SetActive(false);
    }

    override protected void PlayOpeningSFX()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.doorSound);
    }

    override protected void ResetDoor()
    {
        gameObject.SetActive(true);

        _transform.SetPositionAndRotation(_initialPosition, _initialRotation);
    }
}
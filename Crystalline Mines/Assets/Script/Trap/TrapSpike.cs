using UnityEngine;

public class TrapSpike : MonoBehaviour
{
    Animation animationClass;

    void Start()
    {
        animationClass = Animation.Instance;
    }
    
    void OnTriggerEnter2D(Collider2D p_collision2D)
    {
        if (p_collision2D.TryGetComponent(out ICarriable iCarriable))
        {
            iCarriable.Reinitialize();
            return;
        }

        if (p_collision2D.gameObject.CompareTag("Player"))
        {
            // Security
            if (Player.PlayerTransform.GetComponent<Player>().isDead)
                return;

            if (!p_collision2D.TryGetComponent(out Player player))
            {
                Debug.LogError($"ERROR ! The collided GameObject '{p_collision2D.name}' has the 'Player' tag but has not the 'Player' Component");
                return;
            }

            HandlePlayerDeath(player);
            return;
        }

        Debug.LogError(
            $"ERROR ! The GameObject '{p_collision2D.name}' has collided with '{name}' {nameof(TrapSpike)}, " +
            $"but there is nothing planned to handle it."
        );
    }

    void HandlePlayerDeath(Player p_player)
    {
        // NOTE : We can't use a switch here because of the NameToLayer method's bahaviour

        p_player.isDead = true;

        if (gameObject.layer == LayerMask.NameToLayer("SpikeBottom"))
        {
            animationClass.DeadSpikeDownAnimation();
            TimerManager.StartTimer(0.4f, () => p_player.Respawn());
            return;
        }
        else if (gameObject.layer == LayerMask.NameToLayer("SpikeTop"))
        {
            animationClass.DeadSpikeUpAnimation();
            TimerManager.StartTimer(0.4f, () => p_player.Respawn());
            return;
        }
        else if (gameObject.layer == LayerMask.NameToLayer("TrapCrystal"))
        {
            animationClass.DeadTrapCrystalAnimation();
            TimerManager.StartTimer(0.4f, () => p_player.Respawn());
            return;
        }
        else
        {
            Debug.LogError(
                $"ERROR ! The GameObject '{p_player.name}' has collided with '{name}' {nameof(TrapSpike)}, " +
                $"but there is nothing planned to handle it."
            );
        }
    }
}
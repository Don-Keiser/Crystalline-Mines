using System.Linq;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] private Player _player;

    [Header("Player Interaction Range")]
    [SerializeField] private float _rangeRadius;
    [SerializeField] private LayerMask _interactibleMask;

    private void Update()
    {
        Vector2 _moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _player.SetMoveInput(_moveInput);


        if (Input.GetKeyDown(KeyCode.Space))
        {
            _player.Jump();
        }
        if(Input.GetKeyDown(KeyCode.LeftShift) | Input.GetKeyDown(KeyCode.S))
        {
            _player.DropThroughPlatform(-1);
        }
        if (Input.GetKeyDown(KeyCode.E)) 
        {
            GameObject nearestObject = GetNearestInteractableObject();
            if (nearestObject != null)
            {
                Interactible interactible = nearestObject.GetComponent<Interactible>();
                if (interactible != null)
                {
                    interactible.PlayerInteract();
                }
            }
        }
    }

    private GameObject GetNearestInteractableObject()
    {
        RaycastHit2D[] allHits = Physics2D.CircleCastAll(_player.transform.position, _rangeRadius, Vector2.zero, 0f, _interactibleMask);
        if (allHits.Length == 0)
            return null;

        return allHits.OrderBy(hit => (hit.transform.position - _player.transform.position).sqrMagnitude).FirstOrDefault().collider.gameObject;
    }
}

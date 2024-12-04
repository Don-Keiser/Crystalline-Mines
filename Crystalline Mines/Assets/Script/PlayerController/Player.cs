using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Coyotte Time")]
    [SerializeField] private float _coyoteTimeDuration = 0.2f; // Temps où on permet encore de sauter après avoir quitté le sol
    private float _coyoteTimeCounter;

    [Header("Player Size and Physics")]
    [SerializeField] private Vector2 _size;
    [SerializeField] private float _skinWidth = 0.01f;
    [SerializeField] private float _gravity = 10f;

    [Header("Raycast Settings")]
    [SerializeField] private float _verticalRaySpacing; //distance entre les raycasts vertical
    [SerializeField] private float _horizontalRaySpacing; // distance horizontal

    [Header("Movement Settings")]
    [SerializeField] private float _moveSpeed = 8f;
    [SerializeField] private float _jumpForce = 10f;

    [Header("Collision and Slope Settings")]
    [SerializeField] private LayerMask _solidMask;  // layers détecter pour les collisions
    [SerializeField] private LayerMask _passThroughMask; //layer pour les platformes que le joueur peut traverser
    private int _platformLayerIndex;
    private GameObject _passThroughPlatform;
    [SerializeField] private float _maxSlopeAngle = 50f; // Angle maximal que le joueur peut monter ou descendre

    [Header("Internal States")]
    private Bounds _bounds;
    private Vector2 _velocity;

    private int _verticalRaysCount;
    private int _horizontalRaysCount;
    private Vector2 _bottomLeft;
    private Vector2 _bottomRight;
    private Vector2 _topLeft;

    private bool _climbingSlope;
    private float _currentSlopeAngle;
    private float _oldSlopeAngle;

    [Header("debug")]
    [SerializeField] private bool _hasFloor;
    [SerializeField] private bool _coyotteJump;
    [SerializeField] private bool _doubleJump;

    [Header("TEST")]
    [SerializeField] private LayerMask _interactibleMask;
    [SerializeField] private SimonGame _simon;


    private void Start()
    {
        _velocity = Vector2.zero;
        InitializeData();
    }
    public void DetectCristal()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, 1.5f, Vector2.right, 0, _interactibleMask);
        if (hit && Input.GetMouseButtonDown(0))
        {
            _simon.PlayerInteractCristal(hit.collider.gameObject);
        }
    }

    private void Update()
    {
        UpdateColliderInfos();
        ApplyGravity();

        DetectCristal();

        Vector2 deltaMovement = _velocity * Time.deltaTime;
        //print($" Y velocity {deltaMovement.y}");

        if (deltaMovement.x != 0)
        {
            DetectWallAndSlopes(ref deltaMovement);
            DownSlope();
        }
        if (deltaMovement.y != 0)
        {
            DetectFloorAndCeil(ref deltaMovement);
            if (deltaMovement.y > 0)
            {
                DropThroughPlatform(1);
            }
        }
        HandleCoyoteTime();

        transform.Translate(deltaMovement);
    }

    private void HandleCoyoteTime()
    {
        if (!_hasFloor)
        {
            _coyoteTimeCounter -= Time.deltaTime;
            _coyotteJump = _coyoteTimeCounter > 0;
        }
        else
        {
            _coyoteTimeCounter = _coyoteTimeDuration;
            _coyotteJump = true;
        }
    }

    public void DropThroughPlatform(int raySign)
    {
        float raycastLenth = 10f;
        float rayDirection = Mathf.Sign(raySign);

        //parametre de la box
        Vector2 boxDirection = (rayDirection > 0) ? Vector2.up : Vector2.down;
        Vector2 rayOrigin = (rayDirection > 0) ? _topLeft + ((Vector2.right * _size.x) / 2) : (_bottomLeft + _bottomRight) / 2;
        Vector2 boxSize = Vector2.one;

        RaycastHit2D hitInfo = Physics2D.BoxCast(rayOrigin, boxSize, 0f, boxDirection, 1f, _passThroughMask);

        if (hitInfo.collider != null)
        {

            _coyoteTimeCounter = 0; //desactive le jump et coyotte time
            _coyotteJump = false;

            _passThroughPlatform = hitInfo.collider.gameObject;

            _platformLayerIndex = _passThroughPlatform.layer; //prend le layer d'origine de la platform
            _passThroughPlatform.layer = 0 /*LayerMask.NameToLayer("IgnorePlayer")*/; //met le layer a default ou ignorePlayer le temps que le joueur passe

            StartCoroutine(ReenableCollision(hitInfo.collider));
        }
    }
    private IEnumerator ReenableCollision(Collider2D platformCollider)
    {
        yield return new WaitForSeconds(0.5f);
        _passThroughPlatform.layer = _platformLayerIndex;
        _passThroughPlatform = null;
    }
    private void UpdateColliderInfos()
    {
        _bounds.center = transform.position;
        _bottomLeft = _bounds.min;
        _bottomRight = _bottomLeft + Vector2.right * _bounds.size.x;
        _topLeft = _bottomLeft + Vector2.up * _bounds.size.y;

        _hasFloor = false;
        _climbingSlope = false;
        _oldSlopeAngle = _currentSlopeAngle;
        _currentSlopeAngle = 0;

    }
    private void ApplyGravity()
    {
        _velocity.y -= _gravity * Time.deltaTime;
    }

    private void DetectWallAndSlopes(ref Vector2 deltaMovement) //detection des obstacles 
    {
        float horizontalDirection = Mathf.Sign(deltaMovement.x);
        float rayDistance = Mathf.Abs(deltaMovement.x) + _skinWidth;

        for (int i = 0; i < _horizontalRaysCount; i++)
        {
            Vector2 rayOrigin = horizontalDirection > 0 ? _bottomRight : _bottomLeft;

            rayOrigin += Vector2.up * (_horizontalRaySpacing * i);
            Vector2 rayEnd = rayOrigin + Vector2.right * (rayDistance * horizontalDirection);

            RaycastHit2D hit = Physics2D.Linecast(rayOrigin, rayEnd, _solidMask | _passThroughMask);
            if (hit)
            {
                if (i == 0)
                {
                    float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

                    if (slopeAngle <= _maxSlopeAngle)
                    {
                        float movementToSlope = 0f;

                        if (Mathf.Approximately(slopeAngle, _oldSlopeAngle))
                        {
                            movementToSlope = (hit.distance - _skinWidth) * horizontalDirection;
                        }

                        deltaMovement.x -= movementToSlope;
                        ClimbSlope(ref deltaMovement, slopeAngle);
                        deltaMovement.x += movementToSlope;
                    }
                }

                if (!_climbingSlope)
                {
                    deltaMovement.x = (Mathf.Max(hit.distance - _skinWidth, 0) * horizontalDirection);
                    _velocity.x = 0;
                    rayDistance = hit.distance;
                }
            }
        }
    }

    private void ClimbSlope(ref Vector2 deltaMovement, float slopeAngle) //permet de se déplace sur les surfaces diagonale praticables
    {
        float magnitude = Mathf.Abs(deltaMovement.x);
        float slopeMouvementY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * magnitude;

        if (slopeMouvementY >= deltaMovement.y)
        {
            deltaMovement.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * magnitude * Mathf.Sign(deltaMovement.x);
            deltaMovement.y = slopeMouvementY;

            _climbingSlope = true;
            _hasFloor = true;
            _doubleJump = true;
            _currentSlopeAngle = slopeAngle;
        }
    }

    private void DetectFloorAndCeil(ref Vector2 deltaMovement)
    {
        float verticalDirection = Mathf.Sign(deltaMovement.y);
        float rayDistance = Mathf.Abs(deltaMovement.y) + _skinWidth;

        for (int i = 0; i < _verticalRaysCount; i++)
        {
            Vector2 rayOrigin = verticalDirection > 0 ? _topLeft : _bottomLeft;

            rayOrigin += Vector2.right * (_verticalRaySpacing * i + deltaMovement.x);
            Vector2 rayEnd = rayOrigin + Vector2.up * (rayDistance * verticalDirection);

            RaycastHit2D hit = Physics2D.Linecast(rayOrigin, rayEnd, _solidMask | _passThroughMask);
            if (hit)
            {
                // Ignore la plateforme en cours de traversée
                if (_passThroughPlatform != null && hit.collider.gameObject == _passThroughPlatform)
                {
                    continue;
                }

                deltaMovement.y = (Mathf.Max(hit.distance - _skinWidth, 0) * verticalDirection);
                _velocity.y = deltaMovement.y;
                rayDistance = hit.distance;
                if (verticalDirection < 0)
                {
                    _hasFloor = true; // Le joueur touche le sol
                    _doubleJump = true;
                    _coyoteTimeCounter = _coyoteTimeDuration; // Réinitialiser le coyote time dès qu'on touche le sol

                    //if (_passThroughPlatform != null) // si le joueur a traverser un platform remettre le layer a l'etat d'origine
                    //{
                    //    _passThroughPlatform.layer = _platformLayerIndex;
                    //    _passThroughPlatform = null;
                    //}
                }
            }
        }
    }


    private void InitializeData()
    {
        _bounds = new Bounds(transform.position, _size);
        _bounds.Expand(-2 * _skinWidth);
        _verticalRaysCount = Mathf.Max((int)(_bounds.size.x / _verticalRaySpacing), 2);
        _verticalRaySpacing = _bounds.size.x / (_verticalRaysCount - 1);

        _horizontalRaysCount = Mathf.Max((int)(_bounds.size.y / _horizontalRaySpacing), 2);
        _horizontalRaySpacing = _bounds.size.y / (_horizontalRaysCount - 1);
    }

    public void SetMoveInput(Vector2 moveInput)
    {
        _velocity.x = _moveSpeed * moveInput.x;
    }

    public void Jump()
    {
        if (_hasFloor || _coyotteJump /*|| _doubleJump*/)
        {
            //if (!_hasFloor && !_coyotteJump && _doubleJump)
            //{
            //    Debug.Log("Double Jump executed");
            //    _doubleJump = false;
            //}


            //Debug.Log("Jump executed");
            _velocity.y = _jumpForce;
            _hasFloor = false; // Définir à false immédiatement après le saut
            _coyoteTimeCounter = 0; // Réinitialiser le coyote time dès le saut

        }
    }

    private void OnDrawGizmosSelected() // debug raycast
    {
        _bounds = new Bounds(transform.position, _size);

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(_bounds.center, _bounds.size);

        _bounds.Expand(-2 * _skinWidth);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_bounds.center, _bounds.size);

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, 1.5f);
    }
    private void DownSlope() //permet de descendre une rampe 
    {
        if (!_hasFloor && !_coyotteJump && !_doubleJump) return;

        Vector2 rayOrigin = _velocity.x < 0 ? _bottomRight : _bottomLeft;
        Vector2 rayEnd = rayOrigin + Vector2.up * (_velocity.y * Time.deltaTime + Mathf.Sign(_velocity.y) * _skinWidth);
        RaycastHit2D hitInfo = Physics2D.Linecast(rayOrigin, rayEnd);
        Debug.DrawRay(rayOrigin, (rayEnd - rayOrigin) * 10, Color.magenta);

        if (hitInfo.collider is null)
        {
            return;
        }
       //Debug.Log("down slope");

        float angle = Vector2.Angle(hitInfo.normal, Vector2.up);
        if (angle > _maxSlopeAngle)
        {
            return;
        }

        _velocity = ProjectOnLine(_velocity, hitInfo.normal);  //adapte la velocity en fonction de l'inclinaison de la rampe 
        _hasFloor = true;
        _doubleJump = true;
        Debug.DrawRay(transform.position, _velocity * 50, Color.green);
    }

    public static Vector2 ProjectOnLine(Vector2 vector, Vector2 normal) //retourne un vecteur qui varies en fonction de la rampe
    {
        Vector2 direction = new(-normal.y, normal.x);

        direction.Normalize();

        float dotProduct = Vector2.Dot(vector, direction);
        Vector2 projectedVector = dotProduct * direction;

        return projectedVector;
    }
}
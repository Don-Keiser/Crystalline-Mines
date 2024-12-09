using UnityEngine;

public class PuzzleKeySystem : MonoBehaviour
{
    public GameObject correctForm;

    private bool _isMoving;
    private bool _isFinish;

    private float _startPosX;
    private float _startPosY;
    
    private Vector3 _resetPosition;
    // Start is called before the first frame update
    void Start()
    {
        _resetPosition = this.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isFinish == true)
        {
            if (_isMoving)
            {
                Vector3 mousePos;
                mousePos = Input.mousePosition;
                mousePos = Camera.main.ScreenToWorldPoint(mousePos);
                this.gameObject.transform.localPosition = new Vector3(mousePos.x - _startPosX, mousePos.y - _startPosY, this.gameObject.transform.localPosition.z);
            }
        }
    }

    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos;
            mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            _startPosX = mousePos.x - this.gameObject.transform.localPosition.x;
            _startPosY = mousePos.y - this.gameObject.transform.localPosition.y;
            _isMoving = true;
        }
    }

    private void OnMouseUp()
    {
        _isMoving = false;

        if (Mathf.Abs(this.transform.localPosition.x - correctForm.transform.localPosition.x) < 0.3f &&
            Mathf.Abs(this.transform.localPosition.y - correctForm.transform.localPosition.y) < 0.3f)
        {
            this.transform.position = new Vector3(correctForm.transform.position.x, correctForm.transform.position.y, this.transform.position.z);
            _isFinish = true;
        }
        else
        {
            this.transform.localPosition = new Vector3(_resetPosition.x, _resetPosition.y, _resetPosition.z);
        }
    }
}

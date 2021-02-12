using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Cursor : MonoBehaviour
{
    #region StateMachine
    public CursorStateMachine StateMachine { get; private set; }
    public CursosNormalMovementState CursorNormalMovement { get; private set; }
    public CursorSelectedObjectState CursorSelectedMovement { get; private set; }
    public CursorPlayPreviewState CursorPlayPreview { get; private set; }
    #endregion
    #region Components
    public Animator Anim { get; private set; }
    public PlayerInputHandler InputHandler;
    public SpriteRenderer SpriteR { get; private set; }
    // Start is called before the first frame update
    #endregion
    #region variables
    public float speed = 1;
    public GameObject upLeft;
    public GameObject upRight;
    public GameObject bottomRight;
    public GameObject bottomLeft;
    public GameObject upSelectedLeft;
    public GameObject upSelectedRight;
    public GameObject bottomSelectedRight;
    public GameObject bottomSelectedLeft;
    public GameObject middlePoint;
    public GameObject SelectedPlatform;
    public float width = 0.25f;
    public float height = 0.25f;
    public bool inBounds = false;
    public Vector3 bounds;
    public Vector3 position;
    public Terminal terminal;
    public GameManager gameManager;
    /*
     * 0 ----> Button B
     * 1 ----> Button A
     * 2 ----> Button X
     */
    public InputButton[] inputsButtons;
    public GameObject[] buttonsObjects;
    #endregion
    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        StateMachine = new CursorStateMachine();
        CursorNormalMovement = new CursosNormalMovementState(this, StateMachine, "");
        CursorSelectedMovement = new CursorSelectedObjectState(this, StateMachine, "");
        CursorPlayPreview = new CursorPlayPreviewState(this, StateMachine, "");
        StateMachine.Initialize(CursorNormalMovement);
    }
    public void Activate()
    {
        Color c = SpriteR.color;
        c.a = 1;
        SpriteR.color = c; 
    }
    public void Deactivate()
    {
        for(int i = 0; i < buttonsObjects.Length; i++)
        {
            buttonsObjects[i].SetActive(false);
        }
        this.gameObject.SetActive(false);
        
    }
    void Start()
    {
        Anim = GetComponent<Animator>();
        
        SpriteR = GetComponent<SpriteRenderer>();
        
    }
    public void ActivateCursor()
    {
        upLeft.SetActive(true);
        upRight.SetActive(true);
        bottomRight.SetActive(true);
        bottomLeft.SetActive(true);
    }
    public void DeactivateCursor()
    {
        upLeft.SetActive(false);
        upRight.SetActive(false);
        bottomRight.SetActive(false);
        bottomLeft.SetActive(false);
    }
    public void ActivateCursorSelected()
    {
        upSelectedLeft.SetActive(true);
        upSelectedRight.SetActive(true);
        bottomSelectedRight.SetActive(true);
        bottomSelectedLeft.SetActive(true);
    }
    public void DeactivateCursorSelected()
    {
        upSelectedLeft.SetActive(false);
        upSelectedRight.SetActive(false);
        bottomSelectedRight.SetActive(false);
        bottomSelectedLeft.SetActive(false);
    }
    public void SetSize(float w, float h)
    {
        upLeft.transform.position = new Vector2(middlePoint.transform.position.x - (w / 2.0f) - (width / 2.0f), middlePoint.transform.position.y + (h / 2.0f) + (height / 2.0f));
        upRight.transform.position = new Vector2(middlePoint.transform.position.x + (w / 2.0f) + (width / 2.0f), middlePoint.transform.position.y + (h / 2.0f) + (height / 2.0f));
        bottomRight.transform.position = new Vector2(middlePoint.transform.position.x + (w / 2.0f) + (width / 2.0f), middlePoint.transform.position.y - (h / 2.0f) - (height / 2.0f));
        bottomLeft.transform.position = new Vector2(middlePoint.transform.position.x - (w / 2.0f) - (width / 2.0f), middlePoint.transform.position.y - (h / 2.0f) - (height / 2.0f));

        upSelectedLeft.transform.position = new Vector2(middlePoint.transform.position.x - (w / 2.0f) - (width / 2.0f), middlePoint.transform.position.y + (h / 2.0f) + (height / 2.0f));
        upSelectedRight.transform.position = new Vector2(middlePoint.transform.position.x + (w / 2.0f) + (width / 2.0f), middlePoint.transform.position.y + (h / 2.0f) + (height / 2.0f));
        bottomSelectedRight.transform.position = new Vector2(middlePoint.transform.position.x + (w / 2.0f) + (width / 2.0f), middlePoint.transform.position.y - (h / 2.0f) - (height / 2.0f));
        bottomSelectedLeft.transform.position = new Vector2(middlePoint.transform.position.x - (w / 2.0f) - (width / 2.0f), middlePoint.transform.position.y - (h / 2.0f) - (height / 2.0f));
    }
    public bool IsColliding()
    {
        Collider2D[] colliders = Physics2D.OverlapAreaAll(upLeft.transform.position, bottomRight.transform.position);
        foreach (Collider2D collider in colliders)
        {
            //Debug.Log(collider.tag);
            if (collider.gameObject.CompareTag("MovingPlatform"))
            {
                position = collider.bounds.center;
                bounds = collider.bounds.size;
                SelectedPlatform = collider.gameObject;

                return true;

            }

        }
        position = new Vector3();
        bounds = new Vector3();
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(InputHandler.CancelInput);
        if (GameObject.FindObjectOfType<GameManager>().currentState == GameManager.GameState.CursorMode)
        {
           
            StateMachine.CurrentState.LogicUpdate();

            if(GameObject.FindObjectOfType<GameManager>().currentState == GameManager.GameState.CursorMode)
            {
                if (!buttonsObjects[0].activeInHierarchy)
                {
                    buttonsObjects[0].GetComponentInChildren<Image>().sprite = inputsButtons[0].image;
                    buttonsObjects[0].GetComponentInChildren<TMP_Text>().text = inputsButtons[0].name;
                    buttonsObjects[0].SetActive(true);
                }
                if (gameManager.HasPlatformWithCommand())
                {
                    if (StateMachine.CurrentState == CursorSelectedMovement)
                    {
                        if (!buttonsObjects[2].activeInHierarchy)
                        {
                            buttonsObjects[2].GetComponentInChildren<Image>().sprite = inputsButtons[2].image;
                            buttonsObjects[2].GetComponentInChildren<TMP_Text>().text = inputsButtons[2].name;
                            buttonsObjects[2].SetActive(true);
                        }
                    }
                    
                    else
                    {
                        if (!buttonsObjects[1].activeInHierarchy)
                        {
                            buttonsObjects[1].GetComponentInChildren<Image>().sprite = inputsButtons[2].image;
                            buttonsObjects[1].GetComponentInChildren<TMP_Text>().text = inputsButtons[2].name;
                            buttonsObjects[1].SetActive(true);
                        }
                        if (buttonsObjects[2].activeInHierarchy)
                        {
                            buttonsObjects[2].SetActive(false);
                        }
                    }
                    if (StateMachine.CurrentState == CursorPlayPreview)
                    {
                        if (buttonsObjects[1].activeInHierarchy)
                        {   
                            buttonsObjects[1].SetActive(false);
                        }
                       
                        
                        DeactivateCursorSelected();
                    }
                }
            }


        }
        else
        {
            Deactivate();
        }
    }
}

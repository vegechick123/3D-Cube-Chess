using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
enum InputState
{
    None,
    ReadyToSelect,
    Selected,
    Skill,
}
/// <summary>
/// 用以
/// </summary>
public class PlayerControlManager : SingletonMonoBehaviour<PlayerControlManager>
{
    [NonSerialized]
    public GChess selectedChess;
    protected CyclicTask selectTask;
    protected RangeTask curTask;
    protected PlayerSkill selectedSkill;
    private InputState inputState = InputState.None;
    [NonSerialized]
    public UnityEvent<GChess> eClickChess = new EventWrapper<GChess>();
    [NonSerialized]
    public UnityEvent<GFloor> eClickFloor = new EventWrapper<GFloor>();
    [NonSerialized]
    public UnityEvent<GActor> eClickActor = new EventWrapper<GActor>();
    [NonSerialized]
    public UnityEvent<Vector2Int> eOverTile = new EventWrapper<Vector2Int>();
    [NonSerialized]
    public UnityEvent<GPlayerChess> eSelectChess = new EventWrapper<GPlayerChess>();
    [NonSerialized]
    public UnityEvent eDeselect = new UnityEvent();
    Vector2Int curTile = Vector2Int.down;
    public UnityEvent eRightMouseClick = new UnityEvent();
    public Button turnEndButton;
    public Stack<MoveInfo> moveInfoSta = new Stack<MoveInfo>();
    public Button undoMoveButton;
    public EventSystem eventSystem;
    public GraphicRaycaster raycaster;
    [NonSerialized]
    public bool bProcessing=false;
    private PlayerTurn currentPlayerTurn;


    //尝试选中target
    public bool TrySelect(GChess target)
    {
        if (!selectedChess)
        {
            Select(target);
            return true;
        }
        else
        {
            return false;
        }
    }
    protected void Select(GChess target)
    {
        if (target == selectedChess)
            return;
        if (selectedChess != null)
            DeSelect();
        selectedChess = target;
        selectedChess.GetComponent<CAgentComponent>().eSelect.Invoke();
        eSelectChess.Invoke(selectedChess as GPlayerChess);

    }
    public void PlayerTurnEnter(PlayerTurn playerTurn)
    {
        currentPlayerTurn = playerTurn;
        turnEndButton.interactable = true;
        StartInputTask();
    }
    public void PlayerTurnExit()
    {
        currentPlayerTurn.EndTurn();
        currentPlayerTurn = null;
        ClearMoveInfo();
        turnEndButton.interactable = false;
        TerminateInputTask();
    }
    //取消选中
    protected void DeSelect()
    {
        if (!selectedChess)
            return;
        var temp = selectedChess;
        selectedChess = null;
        temp.GetComponent<CAgentComponent>().eDeselect.Invoke();
        eDeselect.Invoke();
    }
    protected override void Awake()
    {
        base.Awake();
        eRightMouseClick.AddListener(CancelCurrentCommand);
        selectTask = CyclicTask.CreateSelectTask();
        selectTask.Begin();
        selectTask.bPaused = true;
    }
    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        // Casts the ray and get the first game object hit
        bool bHit = Physics.Raycast(ray, out hit);
        PointerEventData pointerEventData = new PointerEventData(eventSystem);
        //Set the Pointer Event Position to that of the mouse position
        pointerEventData.position = Input.mousePosition;

        //Create a list of Raycast Results
        List<RaycastResult> results = new List<RaycastResult>();

        //Raycast using the Graphics Raycaster and mouse click position
        raycaster.Raycast(pointerEventData, results);

        bool isInteractingWithUI = results.Count > 0;
        GActor hitResult = null;
        if (bHit && !isInteractingWithUI)
        {
            hitResult = hit.transform.gameObject.GetComponent<GActor>();
        }
        Vector2Int hitTile = new Vector2Int(0, -1);
        if (hitResult)
        {
            hitTile = hitResult.location;
        }
        if (hitTile != curTile)
        {
            curTile = hitTile;
            eOverTile.Invoke(hitTile);
        }

        if (Input.GetMouseButtonDown(0) && bHit)
        {
            switch (hitResult)
            {
                case GChess chess:
                    eClickChess.Invoke(chess);
                    eClickActor.Invoke(chess);
                    break;
                case GFloor floor:
                    eClickFloor.Invoke(floor);
                    eClickActor.Invoke(floor);
                    break;

            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            eRightMouseClick.Invoke();
        }
    }





    public void SwitchToNone()
    {
        switch (inputState)
        {
            case InputState.Skill:
                selectedSkill = null;                
                goto case InputState.Selected;
            case InputState.Selected:
                selectTask.bPaused = true;
                curTask.Abort();
                curTask = null;
                DeSelect();
                break;
            case InputState.None:
                return;
            case InputState.ReadyToSelect:
                selectTask.bPaused = true;
                break;
            default:
                Debug.LogError("ErrorState");
                break;
        };
        inputState = InputState.None;
    }
    public void SwitchToReadySelect()
    {
        switch (inputState)
        {
            case InputState.Skill:
                selectedSkill = null;
                selectTask.bPaused = false;
                goto case InputState.Selected;
            case InputState.Selected:
                curTask.Abort();
                curTask = null;
                DeSelect();
                break;
            case InputState.None:
                selectTask.bPaused = false;
                break;
            case InputState.ReadyToSelect:
                return;
            default:
                Debug.LogError("ErrorState");
                break;
        };
        inputState = InputState.ReadyToSelect;
    }
    public void SwitchToSelected(GPlayerChess chess)
    {
        switch (inputState)
        {
            case InputState.Skill:
                selectedSkill = null;
                selectTask.bPaused = false;
                curTask.Abort();
                curTask = null;
                break;
            case InputState.Selected:
                if (chess == selectedChess)
                    return;
                else
                {
                    curTask.Abort();
                    curTask = null;
                    Select(chess);
                    break;
                }
            case InputState.ReadyToSelect:
                Select(chess);
                break;
            default:
                Debug.LogError("ErrorState");
                break;
        };
        curTask = RangeTask.CreateMoveCommand(chess);
        curTask.CreateFloorHUD(new Color(0, 1, 0, 0.8f));
        curTask.Begin();
        inputState = InputState.Selected;
        
    }
    public void SwitchToSkill(PlayerSkill skill)
    {
        switch(inputState)
        {
            case InputState.Selected:
                curTask.Abort();
                curTask = null;
                break;
            case InputState.Skill:
                if (skill == selectedSkill)
                    return;
                else
                {
                    curTask.Abort();
                    curTask = null;
                }
                break;
            default:
                Debug.LogError("ErrorState");
                break;
        };
        selectTask.bPaused = true;
        curTask = skill.CallGetPlayerInput();
        curTask.CreateFloorHUD(new Color(0, 1, 1, 0.8f));
        curTask.Begin();
        inputState = InputState.Skill;
    }


    public void CancelCurrentCommand()
    {
        switch (inputState)
        {
            case InputState.Selected:
                SwitchToReadySelect();
                break;
            case InputState.Skill:
                SwitchToSelected(selectedChess as GPlayerChess);                
                break;
        }
    }
    protected void TerminateInputTask()
    {
        SwitchToNone();
    }
    protected void StartInputTask()
    {
        SwitchToReadySelect();
    }



    public void AddMoveInfo(MoveInfo info)
    {
        moveInfoSta.Push(info);
        undoMoveButton.interactable = true;
    }
    public void ClearMoveInfo()
    {
        moveInfoSta.Clear();
        undoMoveButton.interactable = false;
    }
    public void CancelMove()
    {
        MoveInfo t = moveInfoSta.Pop();
        t.owner.Teleport(t.origin);
        t.owner.curAP = t.owner.maxAP;
        t.owner.render.transform.rotation = t.originRotation;
        t.owner.AbortMove();
        if (moveInfoSta.Count == 0)
            undoMoveButton.interactable = false;
    }
    public void AddPlayerSkillCaller(PlayerSkill skill,GActor[] inputParams)
    {

    }
    public void BeginProcess()
    {
        bProcessing = true;
        TerminateInputTask();
    }
    public void EndProcess()
    {
        bProcessing = false;
        StartInputTask();
    }
}
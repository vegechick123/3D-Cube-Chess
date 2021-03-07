using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
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
    public GPlayerChess selectedChess;
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
    public UnityEvent<GActor> eSelectMessage = new EventWrapper<GActor>();
    [NonSerialized]
    public UnityEvent<Vector2Int> eOverTile = new EventWrapper<Vector2Int>();
    [NonSerialized]
    public UnityEvent<GPlayerChess> eSelectChess = new EventWrapper<GPlayerChess>();

    [NonSerialized]
    public UnityEvent eDeselect = new UnityEvent();
    Vector2Int curTile = Vector2Int.down;
    [NonSerialized]
    public UnityEvent eRightMouseClick = new UnityEvent();
    [NonSerialized]
    public UnityEvent eSkillPerformEnd = new UnityEvent();

    public Button turnEndButton;
    public Stack<MoveInfo> moveInfoSta = new Stack<MoveInfo>();
    public Button undoMoveButton;
    public EventSystem eventSystem;
    public GraphicRaycaster raycaster;

    [NonSerialized]
    public bool m_bProcessing = false;
    public bool bProcessing
    {
        get { return m_bProcessing; }
        set
        {
            m_bProcessing = value;
            if (m_bProcessing)
                eProcessStart.Invoke();
            else
                eProcessEnd.Invoke();
        }
    }

    [HideInInspector]
    public UnityEvent eProcessStart;
    [HideInInspector]
    public UnityEvent eProcessEnd;

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
        selectedChess = target as GPlayerChess;
        selectedChess.GetComponent<CAgentComponent>().eSelect.Invoke();
        eSelectChess.Invoke(selectedChess);

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
            OnClickActor(hitResult);
        }
        if (Input.GetMouseButtonDown(1))
        {
            eRightMouseClick.Invoke();
            eSelectMessage.Invoke(null);
        }
    }


    void OnClickActor(GActor target)
    {
        switch (target)
        {
            case GChess chess:
                eClickChess.Invoke(chess);
                eClickActor.Invoke(chess);
                eSelectMessage.Invoke(chess);
                break;
            case GFloor floor:
                eClickFloor.Invoke(floor);
                eClickActor.Invoke(floor);
                eSelectMessage.Invoke(floor);
                break;

        }
    }


    public void SwitchToNone()
    {
        switch (inputState)
        {
            case InputState.Skill:
                selectedSkill = null;
                selectedChess.CancelSkill();
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
    public void SwitchToReadyToSelect()
    {
        switch (inputState)
        {
            case InputState.Skill:
                selectedSkill = null;
                selectedChess.CancelSkill();
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
                selectedChess.CancelSkill();
                selectTask.bPaused = false;
                curTask.Abort();
                curTask = null;
                break;
            case InputState.Selected:
                curTask.Abort();
                curTask = null;
                Select(chess);
                break;

            case InputState.ReadyToSelect:
                Select(chess);
                break;
            default:
                Debug.LogError("ErrorState");
                break;
        };
        curTask = CreateMoveCommand(chess);
        curTask.CreateFloorHUD(new Color(0, 1, 0, 0.8f));
        curTask.Begin();
        inputState = InputState.Selected;

    }
    public void SwitchToSkill(PlayerSkill skill)
    {
        switch (inputState)
        {
            case InputState.Selected:
            case InputState.Skill:
                curTask.Abort();
                curTask = null;
                break;
            default:
                Debug.LogError("ErrorState");
                break;
        };
        selectTask.bPaused = true;
        curTask = skill.CallGetPlayerInput();
        selectedChess.PrepareSkill(skill);
        curTask.CreateFloorHUD(new Color(0, 1, 1, 0.8f));
        curTask.Begin();
        inputState = InputState.Skill;
    }


    public void CancelCurrentCommand()
    {
        switch (inputState)
        {
            case InputState.Selected:
                SwitchToReadyToSelect();
                break;
            case InputState.Skill:
                SwitchToSelected(selectedChess);
                break;
        }
    }
    protected void TerminateInputTask()
    {
        SwitchToNone();
    }
    protected void StartInputTask()
    {
        SwitchToReadyToSelect();
    }
    public async void CallChessToMoveAsync(GPlayerChess chess, Vector2Int location)
    {
        bProcessing = true;
        await currentPlayerTurn.BeforeMoveReaction(chess, location);
        await chess.MoveToAsync(location);
        bProcessing = false;
        SwitchToSelected(chess);
    }

    public async void PerformChessSkill(GPlayerChess chess, PlayerSkill skill, GActor[] inputParams)
    {
        bProcessing = true;
        await chess.PerformSkill(skill, inputParams);
        bProcessing = false;
        eSkillPerformEnd.Invoke();
        SwitchToSelected(chess);
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

    public async UniTask<T> GetNextClickChessAsync<T>() where T : GChess
    {
        GActor temp;
        do
        {
            temp = await MyUniTaskExtensions.WaitUntilEvent(eSelectMessage) as T;
        } while (temp != null && !(temp is T));
        return temp as T;
    }
    public async UniTask<PlayerSkill> GetSkillAsync(List<PlayerSkill> candidateSkills)
    {
        UIManager.instance.skillDisplayView.SwitchSkillButton(candidateSkills);
        PlayerSkill result = await MyUniTaskExtensions.WaitUntilEvent(UIManager.instance.skillDisplayView.eClickSkill);
        UIManager.instance.skillDisplayView.CleanSkillButton();
        return result; 
    }
    public async UniTask OpenChestAsync(GChest chest)
    {
        chest.Open();
        int stage = 0;
        PlayerSkill addSkill=null;
        GPlayerChess target = null;
        PlayerSkill replaceSkill = null;
        while (0<=stage&&stage<=2)
        {
            switch(stage)
            {
                case 0:
                    addSkill = await GetSkillAsync(chest.storeSkills);
                    if (addSkill == null)
                        stage--;
                    else
                        stage++;
                    break;
                case 1:
                    target = await GetNextClickChessAsync<GPlayerChess>();
                    if (target == null)
                        stage--;
                    else
                        stage++;
                    break;
                case 2:
                    replaceSkill = await GetSkillAsync(target.skills);
                    if (target == null)
                        stage--;
                    else
                        stage++;
                    break;

            }
        }
        if(stage==-1)
        {
            chest.Close();
        }
        else if(stage == 3)
        {
            await chest.AssignSkill(addSkill,target,replaceSkill);
            chest.Close();
        }
    }
    public static RangeTask CreateMoveCommand(GPlayerChess chess)
    {
        Action<GActor[]> t = (o) =>
        {
            instance.CallChessToMoveAsync(chess, o[0].location);
        };
        Func<int, GActor, bool> checker = (index, target) =>
        {
            return !GridManager.instance.GetChess(target.location);
        };
        return new RangeTask(chess.navComponent.GetMoveRange, t, 1, checker);
    }
}
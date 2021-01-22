using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 用以
/// </summary>
public class PlayerControlManager : SingletonMonoBehaviour<PlayerControlManager>
{
	[NonSerialized]
	public GChess selectedChess;
	public AudioClip selectAudio;
	protected SelectCommand selectCommand;
	protected RangeCommand moveCommand;
	protected CommandTask skillCommand;
	[NonSerialized]
	public UnityEvent<GChess> eClickChess = new EventWrapper<GChess>();
	[NonSerialized]
	public UnityEvent<GFloor> eClickFloor = new EventWrapper<GFloor>();
	[NonSerialized]
	public UnityEvent<GActor> eClickActor = new EventWrapper<GActor>();
	public UnityEvent<Vector2Int> eOverTile = new EventWrapper<Vector2Int>();
	Vector2Int curTile= Vector2Int.down;
	public UnityEvent eRightMouseClick = new UnityEvent();
	public Button turnEndButton;
	public Stack<MoveInfo> moveInfoSta=new Stack<MoveInfo>();
	public Button undoMoveButton;
	public EventSystem eventSystem;
	public GraphicRaycaster raycaster;
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
	public void Select(GChess target)
	{
		GetComponent<AudioSource>().PlayOneShot(selectAudio, 0.3f);
		if (selectedChess != null)
			DeSelect();
		selectedChess = target;
		selectedChess.GetComponent<CAgentComponent>().eSelect.Invoke();
	}
	public void PlayerTurnEnter(PlayerTurn playerTurn)
    {
		currentPlayerTurn = playerTurn;
		selectCommand.bPaused = false;
		turnEndButton.interactable = true;
	}
	public void PlayerTurnExit()
	{
		currentPlayerTurn.EndTurn();
		currentPlayerTurn = null;
		ClearMoveInfo();
		turnEndButton.interactable = false;
		DeSelect();
		selectCommand.bPaused = true;
	}
	//取消选中
	public void DeSelect()
	{
		if (!selectedChess)
			return;
		var temp = selectedChess;
		selectedChess = null;
		temp.GetComponent<CAgentComponent>().eDeselect.Invoke();
		TerminateMoveCommand();
		
	}
    protected override void Awake()
    {
        base.Awake();
		eRightMouseClick.AddListener(CancelCurrentCommand);
		selectCommand = new SelectCommand(null, Select);
		selectCommand.bPaused = true;
	}
    private void Update()
    {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		// Casts the ray and get the first game object hit
		bool bHit=Physics.Raycast(ray, out hit);
		PointerEventData pointerEventData = new PointerEventData(eventSystem);
		//Set the Pointer Event Position to that of the mouse position
		pointerEventData.position = Input.mousePosition;

		//Create a list of Raycast Results
		List<RaycastResult> results = new List<RaycastResult>();

		//Raycast using the Graphics Raycaster and mouse click position
		raycaster.Raycast(pointerEventData, results);

		bool isInteractingWithUI = results.Count > 0;
		GActor hitResult = null;
		if(bHit&&!isInteractingWithUI)
        {
			hitResult = hit.transform.gameObject.GetComponent<GActor>();
		}
		Vector2Int hitTile=new Vector2Int(0,-1);
		if(hitResult)
        {
			hitTile = hitResult.location;
        }
		if(hitTile!=curTile)
        {
			curTile = hitTile;
			eOverTile.Invoke(hitTile);
		}
		
		if (Input.GetMouseButtonDown(0)&&bHit)
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
		if(Input.GetMouseButtonDown(1))
        {
			eRightMouseClick.Invoke(); 
        }
	}

	public void GenMoveCommand(MoveCommand _moveCommand)
    {
		if (moveCommand != null)
			moveCommand.Abort();
		moveCommand = _moveCommand;

		moveCommand.eTaskComplete.AddListener(() =>
		{
			moveCommand = null;
			UIManager.instance.eRefreshFloorHUD.Invoke();
		});
	}
	protected void TerminateMoveCommand()
    {
		if (moveCommand != null)
			moveCommand.Abort();
		moveCommand = null;
	}


	public void PreemptSkillCommand(PlayerSkill skill)
    {
		selectCommand.bPaused = true;
		if (moveCommand != null)
		{
			moveCommand.bPaused = true;
			moveCommand.HideFloorHUD();
		}
		if(skillCommand!=null)
			skillCommand.Abort();

		skillCommand = new SkillCommand(skill);

		skillCommand.eTaskComplete.AddListener(() =>
		{
			ClearMoveInfo();
			skillCommand = null;
			selectCommand.bPaused = false;
			if (moveCommand != null)
			{
				moveCommand.bPaused = false;
				moveCommand.ShowFloorHUD();
			}
			UIManager.instance.eRefreshFloorHUD.Invoke();
			selectedChess.hasActed = true;
			DeSelect();
		});
    }
	protected void TerminateSkillCommand()
	{
		selectCommand.bPaused = false;
		if (moveCommand != null)
		{
			moveCommand.bPaused = false;
			moveCommand.ShowFloorHUD();
		}


		if(skillCommand!=null)
			skillCommand.Abort();
		skillCommand = null;
	}
	public void CancelCurrentCommand()
    {
		if (skillCommand != null)
			TerminateSkillCommand();
		else if (selectedChess != null)
			DeSelect();
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
		t.owner.curMovement = t.owner.movement;
		t.owner.render.transform.rotation = t.originRotation;
		t.owner.AbortMove();
		if (moveInfoSta.Count==0)
			undoMoveButton.interactable = false;
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 用以
/// </summary>
public class PlayerControlManager : Manager<PlayerControlManager>
{
	public GChess selectedChess;

	protected SelectCommand selectCommand;
	protected RangeCommand moveCommand;
	protected CommandTask skillCommand;

	public UnityEvent<GChess> eClickChess = new EventWrapper<GChess>();
	public UnityEvent<GFloor> eClickFloor = new EventWrapper<GFloor>();
	public UnityEvent<Vector2Int> eOverTile = new EventWrapper<Vector2Int>();
	Vector2Int curTile= Vector2Int.down;
	public UnityEvent eRightMouseClick = new UnityEvent();
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
		selectedChess = target;
		selectedChess.GetComponent<CAgentComponent>().eSelect.Invoke();
	}
	public void PlayerTurnEnter()
    {
		selectCommand.bPaused = false;
	}
	public void PlayerTurnExit()
	{
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

		GActor hitResult = null;
		if(bHit)
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
					break;
				case GFloor floor:
					eClickFloor.Invoke(floor);
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


	public void PreemptSkillCommand(CommandTask commandTask)
    {
		selectCommand.bPaused = true;
		if (moveCommand != null)
		{
			moveCommand.bPaused = true;
			moveCommand.HideFloorHUD();
		}
		if(skillCommand!=null)
			skillCommand.Abort();

		skillCommand = commandTask;

		skillCommand.eTaskComplete.AddListener(() =>
		{
			skillCommand = null;
			selectCommand.bPaused = false;
			if (moveCommand != null)
			{
				moveCommand.bPaused = false;
				moveCommand.ShowFloorHUD();
			}
			UIManager.instance.eRefreshFloorHUD.Invoke();
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
}

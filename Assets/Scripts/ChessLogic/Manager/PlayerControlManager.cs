using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class PlayerControlManager : Manager<PlayerControlManager>
{
	public GChess selectedChess;

	protected SelectCommand selectCommand;
	protected RangeCommand moveCommand;
	protected CommandTask skillCommand;

	public UnityEvent<GChess> eClickChess = new EventWrapper<GChess>();
	public UnityEvent<GFloor> eClickFloor = new EventWrapper<GFloor>();
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
    }
    private void Start()
    {
		selectCommand = new SelectCommand(null, Select);
	}
    private void Update()
    {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		// Casts the ray and get the first game object hit
		bool bHit=Physics.Raycast(ray, out hit);
		if (Input.GetMouseButtonDown(0)&&bHit)
		{
			switch (hit.transform.gameObject.GetComponent<GActor>())
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

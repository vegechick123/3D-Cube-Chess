using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class PlayerControlManager : Manager<PlayerControlManager>
{
	public GChess selectedChess;

	protected SelectCommand selectCommand;
	protected RangeCommand<Action<GFloor>> moveCommand;
	protected CommandTaskBase skillCommand;

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
		if (Input.GetMouseButton(0)&&bHit)
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
		if(Input.GetMouseButton(1))
        {
			eRightMouseClick.Invoke(); 
        }
	}

	public void GenMoveCommand(RangeCommand<Action<GFloor>> _moveCommand)
    {
		if (moveCommand != null)
			moveCommand.Abort();
		moveCommand = _moveCommand;

		moveCommand.eTaskComplete.AddListener(() => moveCommand = null);
	}
	protected void TerminateMoveCommand()
    {
		if (moveCommand != null)
			moveCommand.Abort();
		moveCommand = null;
	}


	public void PreemptSkillCommand(CommandTaskBase commandTask)
    {
		selectCommand.bPaused = true;
		if (moveCommand != null)
			moveCommand.bPaused = true;

		if(skillCommand!=null)
			skillCommand.Abort();
		skillCommand = commandTask;

        skillCommand.eTaskComplete.AddListener(() => skillCommand = null);
    }
	protected void TerminateSkillCommand()
	{
		selectCommand.bPaused = false;
		if (moveCommand != null)
			moveCommand.bPaused = false;

		if(skillCommand!=null)
			skillCommand.Abort();
		skillCommand = null;
	}

}

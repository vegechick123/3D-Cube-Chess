using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPlayerChess : GChess
{
    public List<PlayerSkill> skills;
    protected CAgentComponent agentComponent;
    protected override void Awake()
    {
        base.Awake();
        agentComponent = GetComponent<CAgentComponent>();
        if (agentComponent)
        {
            agentComponent.eSelect.AddListener(OnSelect);
            agentComponent.eDeselect.AddListener(OnDeselect);
        }
        for( int i=0;i<skills.Count; i++)
        {
            skills[i] = Instantiate(skills[i]) as PlayerSkill;
            skills[i].owner = this;
        }
    }
    protected virtual void OnSelect()
    {
        if (curMovement > 0&&!unableAct&&!hasActed&&!freezeFoot)
        {
            navComponent.GenNavInfo();
            MoveCommand moveCommand = new MoveCommand(navComponent.GetMoveRange, this, MoveTo);
            moveCommand.CreateFloorHUD(new Color(0, 1, 0, 0.8f));
            PlayerControlManager.instance.GenMoveCommand(moveCommand);
            
        }
        outline.AddReference();
        ShowUI();
        if (unableAct||hasActed)
            UIManager.instance.DisableSkillButton();
    }
    protected virtual void OnDeselect()
    {
        outline.RemoveReference();
        HideUI();
    }
    protected void ShowUI()
    {
        
        UIManager.instance.SwitchSkillButton(skills.ToArray());
    }
    protected void HideUI()
    {
        UIManager.instance.CleanSkillButton();
    }
    protected override void OnDestroy()
    {
        for (int i = 0; i < skills.Count; i++)
        {
            Destroy(skills[i]);
            skills[i] = null;
        }
        base.OnDestroy();
    }
    public override void MoveTo(Vector2Int destination)
    {
        MoveInfo t = new MoveInfo();
        t.owner = this;
        t.origin = location;
        t.originRotation = render.transform.rotation;
        t.destination = destination;
        PlayerControlManager.instance.AddMoveInfo(t);
        base.MoveTo(destination);
        t.destinationRotation = render.transform.rotation;
    }
    public virtual void OnPerformSkill(Skill skill)
    {
        PlayerControlManager.instance.ClearMoveInfo();
    }
}

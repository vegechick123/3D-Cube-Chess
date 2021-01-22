using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPlayerChess : GChess
{
    public List<PlayerSkill> skills;
    protected CAgentComponent agentComponent;
    public override void GAwake()
    {
        base.GAwake();
        agentComponent = GetComponent<CAgentComponent>();
        if (agentComponent)
        {
            agentComponent.eSelect.AddListener(OnSelect);
            agentComponent.eDeselect.AddListener(OnDeselect);
        }
        for( int i=0;i<skills.Count; i++)
        {
            skills[i] = Instantiate(skills[i]);
            skills[i].owner = this;
        }
    }
    protected virtual void OnSelect()
    {
        if (curMovement > 0&&!unableAct&&!freezeFoot)
        {
            navComponent.GenNavInfo();
            MoveCommand moveCommand = new MoveCommand(navComponent.GetMoveRange, this, MoveToAsync);
            moveCommand.CreateFloorHUD(new Color(0, 1, 0, 0.8f));
            PlayerControlManager.instance.GenMoveCommand(moveCommand);

        }
        outline.AddReference();
    }
    protected virtual void OnDeselect()
    {
        outline.RemoveReference();
    }
    async public override UniTask MoveToAsync(Vector2Int destination)
    {
        MoveInfo t = new MoveInfo();
        t.owner = this;
        t.origin = location;
        t.originRotation = render.transform.rotation;
        t.destination = destination;
        PlayerControlManager.instance.AddMoveInfo(t);
        await base.MoveToAsync(destination);
        t.destinationRotation = render.transform.rotation;
    }
    public virtual void OnPerformSkill(Skill skill)
    {
        PlayerControlManager.instance.ClearMoveInfo();
    }
    public override string GetTitle()
    {
        string res= "<color=yellow>";
        res+=base.GetTitle();
        res += "</color>";
        return res;
    }
}


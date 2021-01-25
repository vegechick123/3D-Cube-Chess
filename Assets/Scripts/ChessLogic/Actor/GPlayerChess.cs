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
            PlayerControlManager.instance.PreemptMoveTask(this);
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
    public async UniTask PerformSkill(PlayerSkill skill)
    {
        PlayerControlManager.instance.bProcessing = true;
        await skill.ProcessAsync();
        PlayerControlManager.instance.bProcessing = false;
    }
}


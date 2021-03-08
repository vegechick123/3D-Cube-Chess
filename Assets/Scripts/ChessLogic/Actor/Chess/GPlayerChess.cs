using System;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GPlayerChess : GChess
{
    public bool persistentSave = true;
    [NonSerialized]
    public GameObject prefabPrototype = null;
    public List<PlayerSkill> skills;
    [NonSerialized]
    public UnityEvent eSkillChange = new UnityEvent();
    protected CAgentComponent agentComponent;
    bool useSaveData = false;
    public int maxSkillsCount;
    public bool skillContainFull { get { return maxSkillsCount <= skills.Count; }}
    public void InitWithSaveData(PlayerChessData data)
    {
        Debug.Assert(data.prototype == prefabPrototype);
        useSaveData = true;
        skills.Clear();
        foreach (PlayerSkillData skillData in data.skills)
        {
            skills.Add(PlayerSkill.CreateInstanceFromSaveData(skillData));
        }

    }
    public override void GAwake()
    {
        base.GAwake();
        agentComponent = GetComponent<CAgentComponent>();
        if (agentComponent)
        {
            agentComponent.eSelect.AddListener(OnSelect);
            agentComponent.eDeselect.AddListener(OnDeselect);
        }
        if (!useSaveData)
        {
            for (int i = 0; i < skills.Count; i++)
            {
                skills[i] = skills[i].CreateCopy();
            }
        }
        for (int i = 0; i < skills.Count; i++)
        {
            skills[i].Init(this);
        }
    }
    protected virtual void OnSelect()
    {
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
        navComponent.GenNavInfo();
        int distance = navComponent.navInfo.GetPath(destination).Length;
        await base.MoveToAsync(destination);
        spendMoveCost(distance);
        t.destinationRotation = render.transform.rotation;

    }
    public virtual void OnPerformSkill(Skill skill)
    {
        PlayerControlManager.instance.ClearMoveInfo();
    }
    public override string GetTitle()
    {
        string res = "<color=yellow>";
        res += base.GetTitle();
        res += "</color>";
        return res;
    }
    public async UniTask PerformSkill(PlayerSkill skill, GActor[] inputParams)
    {

        waitShoot = true;
        animator.SetTrigger("PerformSkill");
        await UniTask.WaitWhile(() => waitShoot);
        animator.SetBool("ReadySkill", false);
        await skill.CallProcessAsync(inputParams);
        if (!skill.infinite)
        {
            skill.useCount--;
            if (skill.useCount <= 0)
            {
                RemoveSkill(skill);
            }

        }
    }
    public void PrepareSkill(PlayerSkill skill)
    {
        render.GetComponent<Animator>().SetBool("ReadySkill", true);
    }
    public void CancelSkill()
    {
        render.GetComponent<Animator>().SetBool("ReadySkill", false);
    }
    public void ReplaceSkill(PlayerSkill addskill, PlayerSkill removeskill)
    {
        int index=skills.IndexOf(removeskill);
        Destroy(skills[index]);
        skills[index]=addskill.CreateCopy();
        skills[index].Init(this);
        eSkillChange.Invoke();
    }
    public void AddSkill(PlayerSkill skill)
    {
        PlayerSkill copy = skill.CreateCopy();
        skills.Add(copy);
        copy.Init(this);
        eSkillChange.Invoke();
    }
    public void RemoveSkill(PlayerSkill skill)
    {
        skills.Remove(skill);
        eSkillChange.Invoke();
    }
    public PlayerChessData GetSaveData()
    {
        PlayerChessData res = new PlayerChessData();
        res.prototype = prefabPrototype;
        foreach (PlayerSkill skill in skills)
        {
            res.skills.Add(skill.GetSaveData());
        }
        return res;
    }
}


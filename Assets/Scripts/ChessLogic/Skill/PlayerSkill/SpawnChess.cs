using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Skill/PlayerSkill/SpawnChess")]
public class SpawnChess : PlayerSkill
{
    public int selectRange;
    [SerializeField]
    public GChess prefabChess;
    public int damage=0;
    public int affectRange=0;
    public Element element=Element.None;
    public override RangeTask GetPlayerInput()
    {
        return GetInputTargets(SkillTarget.EmptyFloor);
    }

    public override Vector2Int[] GetSelectRange()
    {
        return GridManager.instance.GetCircleRange(owner.location,selectRange);
    }                                              

    public override async UniTask ProcessAsync(GActor[] inputParams)
    {
        Vector2Int targetLocation = inputParams[0].location;
        await Shoot(targetLocation);        
        GridManager.instance.InstansiateChessAt(prefabChess.gameObject,targetLocation);
        ElementSystem.ApplyElementAt(targetLocation,element);
    }
}

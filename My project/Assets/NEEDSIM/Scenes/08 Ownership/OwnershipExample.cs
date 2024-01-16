/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

using UnityEngine;
using System.Collections;

public class OwnershipExample : MonoBehaviour {

    public NEEDSIM.NEEDSIMNode BedJohn;
    public NEEDSIM.NEEDSIMNode BedAdam;
    public NEEDSIM.NEEDSIMNode BedMarie;
    public NEEDSIM.NEEDSIMNode BedElena;

    public NEEDSIM.NEEDSIMNode John;
    public NEEDSIM.NEEDSIMNode Adam;
    public NEEDSIM.NEEDSIMNode Marie;
    public NEEDSIM.NEEDSIMNode Elena;

    // Use this for initialization
    void Start ()
    {
        //assignAllOwnerships();

    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            unassignAllOwnerships();
        }
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            assignAllOwnerships();
        }
	}

    private void assignAllOwnerships()
    {
        Simulation.Ownership.Instance.ClaimOwnership(BedJohn.AffordanceTreeNode.Affordance, John.AffordanceTreeNode);
        Simulation.Ownership.Instance.ClaimOwnership(BedAdam.AffordanceTreeNode.Affordance, Adam.AffordanceTreeNode);
        Simulation.Ownership.Instance.ClaimOwnership(BedMarie.AffordanceTreeNode.Affordance, Marie.AffordanceTreeNode);
        Simulation.Ownership.Instance.ClaimOwnership(BedElena.AffordanceTreeNode.Affordance, Elena.AffordanceTreeNode);
    }

    private void unassignAllOwnerships()
    {
        Simulation.Ownership.Instance.RemoveAllOwnerships(BedJohn.AffordanceTreeNode.Affordance);
        Simulation.Ownership.Instance.RemoveAllOwnerships(BedAdam.AffordanceTreeNode.Affordance);
        Simulation.Ownership.Instance.RemoveAllOwnerships(BedMarie.AffordanceTreeNode.Affordance);
        Simulation.Ownership.Instance.RemoveAllOwnerships(BedElena.AffordanceTreeNode.Affordance);
    }
}

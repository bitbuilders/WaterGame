using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] Player m_player;

    public void FootstepLeft()
    {
        m_player.Footstep(true);
    }

    public void FootstepRight()
    {
        m_player.Footstep(false);
    }
}

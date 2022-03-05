using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class AbilityManager : NetworkBehaviour
{
    [Header("SelectCircle")]
    [SerializeField] private  GameObject selectCircle=null;

    [Header("Skills")]
    [SerializeField] private Blink blink;
    [SerializeField] private MeteorShower meteorShower;
    [SerializeField] private FireStun fireStun;




    public GameObject SelectCircle
    {
        get { return selectCircle; }
    }

    public Blink Blink
    {
        get { return blink; }

    }

    public MeteorShower MeteorShower
    {
        get { return meteorShower; }

    }

    public FireStun FireStun
    {
        get { return fireStun; }
    }



}

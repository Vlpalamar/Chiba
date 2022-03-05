using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

//спелл худ вешается в префаб игрока    
public class SpellHUD : NetworkBehaviour
{
    private const string PlayerTag = "Player";

    public List<Image> slotsIcone;
    public List<Image> reloadsIcone;

    private CastSystem _castSystem;

    

    private void Update()
    {
        if (_castSystem)
            return;


        if (GameObject.FindGameObjectWithTag(PlayerTag))
            Init();
        
       
    }

    
    private void Init()
    {
        _castSystem = GameObject.FindGameObjectWithTag(PlayerTag).GetComponent<CastSystem>();
        for (int i = 0; i < _castSystem.Spells.Count; i++)
        {
            slotsIcone[i].sprite = _castSystem.Spells[i].Attribute.Icone;
        }
    }
}

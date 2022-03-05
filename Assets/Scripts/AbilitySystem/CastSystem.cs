using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class CastSystem : NetworkBehaviour
{

    private const string TarrainTag = "Terrain"; //возможно будет другой тег


    [Header("Status")]
    public bool Cast = false;

    public int spellId = 0;

    [Header("Reference")]
    public List<Spell> Spells;
    [SerializeField] private GameObject spellPanel=null;
    //[Tooltip("1-HeroTarget\n2-Target\n3-Area")]
    //[SerializeField] private List<GameObject> Cursors;
    [SerializeField] private AbilityManager abilityManager = null;
   private GameObject SelectedCircle= null;


    private Ray mainRay;
    private RaycastHit MainRayhit;
    private Camera MainCamera = null;

    private Hero MainHero = null;

    private Spell.SpellType type = Spell.SpellType.Target;

    #region AddSpell

    public void AddSpell(Spell spell)
    {
        Spells.Add(spell);
        if (isServer)
        {
            Server_AddSpell();
        }
        else
        {
            CmdAddSpell();
        }

    }

    [Command(requiresAuthority = false)]
    private void CmdAddSpell( )
    {
        Server_AddSpell();
    }

    [Server]
    private void Server_AddSpell( )
    {
        RpcAddSpell();

    }

    #endregion

    [TargetRpc]
    private void RpcAddSpell( )
    {
        //Spells.Add(spell);
        spellPanel.transform.GetChild(Spells.Count - 1).gameObject.SetActive(true);

    }


    public void RemoveSpell(Spell spell)
    {
        Spells.Remove(spell);
        spellPanel.transform.GetChild(Spells.Count - 1).gameObject.SetActive(false);

    }

 
    public override void OnStartLocalPlayer()
    {
        MainCamera = Camera.main;
        MainHero = GetComponent<Hero>();
        spellPanel = GameObject.Find("SpellPanel");

        SelectedCircle = Instantiate(abilityManager.SelectCircle);
        SelectedCircle.SetActive(false);
        //foreach (GameObject obj in CursorsForInst)
        //{
        //    GameObject n=  Instantiate(obj);
        //    n.SetActive(false);
        //    Cursors.Add(n);
        //}

        //для тестов
        Spell d = new FireStun();
          d = abilityManager.FireStun;

        d.User = this.gameObject;
        AddSpell(d);
    }


    private void Update()
    {
        if (!isLocalPlayer) return;
        
        if (Input.GetKey(KeyCode.Q))
        {
            if (Spells.Count < 1) return;
            spellId = 0;
            Cast = true;
        }
        if (Input.GetKey(KeyCode.W))
        {
            if (Spells.Count < 2) return;
            spellId = 1;
            Cast = true;
        }

        if (Input.GetKey(KeyCode.E))
        {
            if (Spells.Count < 3) return;
            spellId = 2;
            Cast = true;
        }
        if (Input.GetKey(KeyCode.R))
        {
            if (Spells.Count < 4) return;
            spellId = 3;
            Cast = true;
        }
        if (Input.GetKey(KeyCode.D))
        {
            if (Spells.Count < 5) return;
            spellId = 4;
            Cast = true;
        }
        if (Input.GetKey(KeyCode.F))
        {
            if (Spells.Count < 6) return;
            spellId = 5;
            Cast = true;
        }

        


        if (Input.GetKey(KeyCode.Escape))
        {
            Cast = false;
        }
        if (!Cast)
        {
            SelectedCircle.SetActive(false);
            return;
        }

        if (Cast)
        {
            mainRay = MainCamera.ScreenPointToRay(Input.mousePosition);
            type = Spells[spellId].type;
            switch (type)
            {
                case Spell.SpellType.Area:
                    if (RayMousePosition(TarrainTag))
                    {
                        SelectedCircle.transform.position = new Vector3(MainRayhit.point.x, MainRayhit.point.y + 0.5f, MainRayhit.point.z);
                        SelectedCircle.SetActive(true);
                    }
                    break;
                case Spell.SpellType.HeroTarget:
                    if (RayMouseOnEnemy(TagManager.TagEnemy))
                    {
                        //изменение курсора

                    }
                    break;
                case Spell.SpellType.Target:
                    if (RayMousePosition(TarrainTag))
                    {
                        //изменение курсора
                    }
                    break;

                default:
                    return;
                    break;
            }
           
            if (Input.GetMouseButton(1))
            {
                Cast = false;
                SelectedCircle.SetActive(false);
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (type==Spell.SpellType.HeroTarget)
                {
                    if (!RayMouseOnEnemy(TagManager.TagEnemy))
                        return;
                    Spells[spellId].UseSpell(MainRayhit.collider.GetComponent<Hero>());
                    //курсор 
                }
                else
                {
                    Spells[spellId].UseSpell(MainRayhit.point);
                    SelectedCircle.SetActive(false);
                }
                
                Cast = false;
            }

        }

    }

    //private void UseSpell()
    //{
    //    if (isClient)
    //    {
    //        CmdUseSpell();
    //    }
    //    else
    //    {
    //        Server_UseSpell();
    //    }
    //}

    //[Server]
    //private void Server_UseSpell()
    //{
    //    RpcUseSpell();
    //}

    //[Command]
    //private void CmdUseSpell()
    //{
    //    RpcUseSpell();
    //}

    //[ClientRpc]
    //private void RpcUseSpell()
    //{
    //    //Spells[spellId].UseSpell(MainRayhit.point);
    //}





    private bool RayMousePosition(string _tag)
    {
        if (Physics.Raycast(mainRay, out MainRayhit) && MainRayhit.collider.CompareTag(_tag))
            return true;
        return false;

    }

    private bool RayMouseOnEnemy(string _tag)
    {
        if (Physics.Raycast(mainRay, out MainRayhit) && MainRayhit.collider.CompareTag(_tag))
            return true;
        return false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellButton : MonoBehaviour
{
    public GameObject Spell;
    Button button;
    GameObject caster;

    void Start()
    {
        caster = GameObject.Find("Player");
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(Cast);
    }

    void Update()
    {

    }

    void Cast()
    {
        var obj = Instantiate(Spell, caster.transform.position, caster.transform.rotation);
        var proj = obj.GetComponent<Projectile>();
        proj.Target = proj.TargetSelf ? caster : caster.GetComponent<PlayerInput>().Target;
    }
}

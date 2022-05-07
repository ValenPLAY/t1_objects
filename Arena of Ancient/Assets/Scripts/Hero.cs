using System.Collections.Generic;
using UnityEngine;

public class Hero : Unit
{
    [Header("Hero Abilities")]
    public List<Ability> abilities = new List<Ability>();

    [Header("Hero Body Parts")]
    [SerializeField] protected GameObject unitModel;
    [SerializeField] protected GameObject upperBody;
    protected Quaternion defaultUpperBodyRotation;
    [SerializeField] protected GameObject lowerBody;
    [SerializeField] protected float rotationSpeed = 500.0f;
    protected CharacterController characterController;

    [Header("Hero Upgrades")]
    public int upgradePoints;

    protected override void Awake()
    {
        base.Awake();
        GameController.Instance.waveFinishedEvent += GainSkillPoint;
        defaultUpperBodyRotation = transform.rotation;
        characterController = GetComponent<CharacterController>();

    }

    protected void GainSkillPoint()
    {
        Debug.Log(name + " gained a Skill Point!");
        upgradePoints++;
    }

    protected override void Update()
    {
        if (unitState == state.normal) MoveHero(GameController.Instance.inputVector);
        base.Update();
    }

    protected void MoveHero(Vector3 movementVector)
    {
        movementVector.y = Physics.gravity.y;
        //movementSpeedActual = movementSpeed;
        characterController.Move(movementVector * Time.deltaTime * movementSpeedActual);

        if (lowerBody != null && movementVector != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movementVector, Vector3.up);
            lowerBody.transform.rotation = Quaternion.RotateTowards(lowerBody.transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }

        if (upperBody != null)
        {
            upperBody.transform.LookAt(GameController.Instance.playerWorldMousePos);
            //upperBody.transform.rotation = Quaternion.Euler(Vector3.zero);
        }

        if (unitModel != null && lowerBody == null && upperBody == null)
        {
            unitModel.transform.LookAt(GameController.Instance.playerWorldMousePos);

        }
    }
}

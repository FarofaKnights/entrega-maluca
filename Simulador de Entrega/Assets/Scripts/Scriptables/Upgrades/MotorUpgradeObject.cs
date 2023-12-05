using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Motor tal", menuName = "Entrega Maluca/Upgrade/Motor"), System.Serializable]
public class MotorUpgradeObject : UpgradeObject {
    public int maxSpeed;
    public float acelleration;

    protected override void _Ativar() {
        OficinaController.instance.SetMotor(maxSpeed, acelleration);
    }

    protected override void _Desativar() {
        OficinaController.instance.SetMotor(-1, -1);
    }

    public override Dictionary<string, (string, float)> GetInfo() {
        Dictionary<string, (string, float)> info = new Dictionary<string, (string, float)>();

        string maxSpeedTxt = maxSpeed.ToString() + " km/h";

        info.Add("Velocidade Max.", (maxSpeedTxt, maxSpeed));
        info.Add("Aceleração", (acelleration.ToString(), acelleration));

        return info;
    }
}

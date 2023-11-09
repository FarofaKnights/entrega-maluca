using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhellControler : MonoBehaviour
{
    public float velocidade;
    public int maxVelocidade;
    public float aceleracao;  
    public int maxRe;
    public int freio;
    public float forca;
    public float maxAngulo;
   
    public Transform rodaFrenteDir;
    public Transform rodaFrenteEsq;
    public Transform rodaTraseiraDir;
    public Transform rodaTraseiraEsq;

    public WheelCollider frenteDirCollider;
    public WheelCollider frenteEsqCollider;
    public WheelCollider traseiraDirCollider;
    public WheelCollider traseiraEsqCollider;

    public Rigidbody CarRb;
    public Transform centroDeMassa;

    public static WhellControler instance;

     void Start()
    {
        CarRb = GetComponent<Rigidbody>();
        CarRb.centerOfMass = centroDeMassa.localPosition;
        instance = this;
    }

    public void Execute(float deltaTime) // Chamado pelo dirigindoState, ou seja, não irá executar quando não estiver dirigindo.
     {
        velocidade = CarRb.velocity.magnitude * 3.6f;   
        if(velocidade < maxVelocidade)
        {
            traseiraDirCollider.motorTorque = aceleracao*  forca * Input.GetAxis("Vertical");
            traseiraEsqCollider.motorTorque = aceleracao* forca * Input.GetAxis("Vertical");
        }
        else
        {
            traseiraDirCollider.motorTorque = 0;
            traseiraEsqCollider.motorTorque = 0;
        }

        frenteDirCollider.steerAngle = maxAngulo * Input.GetAxis("Horizontal");
        frenteEsqCollider.steerAngle = maxAngulo * Input.GetAxis("Horizontal");

        if (Input.GetKey(KeyCode.Space))
        {
            traseiraDirCollider.brakeTorque = freio;
            traseiraEsqCollider.brakeTorque = freio;
        }
        else
        {
            traseiraDirCollider.brakeTorque = 0;
            traseiraEsqCollider.brakeTorque = 0;
        }

        RotacaoRoda(traseiraEsqCollider, rodaTraseiraEsq);
        RotacaoRoda(traseiraDirCollider, rodaTraseiraDir);
        RotacaoRoda(frenteEsqCollider,   rodaFrenteEsq);
        RotacaoRoda(frenteDirCollider,   rodaFrenteDir);
         

    }
    
    private void RotacaoRoda(WheelCollider coll, Transform transform)
    {
        Vector3 position;
        Quaternion rotation;

        coll.GetWorldPose(out position, out rotation);
        transform.rotation = rotation;
        transform.position = position;
    }
}

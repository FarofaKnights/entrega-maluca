using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RodaRotacao : MonoBehaviour
{
    public Transform roda_FD;
    public Transform roda_FE;
    public Transform roda_TD;
    public Transform roda_TE;
    public Transform centroDeMassa;
    public Rigidbody rbCarroceria;

    public float raioGeometria; //Aumentar faz com que o angulo da roda seja menor
    public float entreEixos;
    public float distanciaRodasTras;
    float anguloAckermanFrenteEsq;
    float anguloAckermanFrenteDir;

    public float coefArrasto;
    public float areaCarro;
    public float DensidadeAr;
    public float fArrasto;
    public Vector3 velocidadeLocalCarroceria;

    // Update is called once per frame
    void Update()
    {
        rbCarroceria.centerOfMass = centroDeMassa.localPosition;

        if (Input.GetAxis("Horizontal") < 0)
        {
            anguloAckermanFrenteDir = Mathf.Rad2Deg * Mathf.Atan2(entreEixos, raioGeometria - (distanciaRodasTras / 2));
            anguloAckermanFrenteEsq = Mathf.Rad2Deg * Mathf.Atan2(entreEixos, raioGeometria + (distanciaRodasTras / 2));

        }
        else if (Input.GetAxis("Horizontal") > 0)
        {
            anguloAckermanFrenteDir = Mathf.Rad2Deg * Mathf.Atan2(entreEixos, raioGeometria - (distanciaRodasTras / 2));
            anguloAckermanFrenteEsq = Mathf.Rad2Deg * Mathf.Atan2(entreEixos, raioGeometria + (distanciaRodasTras / 2));

        }
        else if (Input.GetAxis("Horizontal") == 0)
        {
            anguloAckermanFrenteDir = 0;
            anguloAckermanFrenteEsq = 0;

        }
        roda_FD.transform.localRotation = Quaternion.Euler(anguloAckermanFrenteDir * Input.GetAxis("Horizontal") * transform.up);
        roda_FE.transform.localRotation = Quaternion.Euler(anguloAckermanFrenteEsq * Input.GetAxis("Horizontal") * transform.up);

    }

     void FixedUpdate()
    {
        velocidadeLocalCarroceria = transform.InverseTransformDirection(rbCarroceria.velocity);
        fArrasto = 0.5f * coefArrasto * areaCarro * DensidadeAr * -Mathf.Sign(velocidadeLocalCarroceria.z);
        rbCarroceria.AddForce((fArrasto * Mathf.Pow( rbCarroceria.velocity.magnitude,2)) * transform.forward);
    }
}

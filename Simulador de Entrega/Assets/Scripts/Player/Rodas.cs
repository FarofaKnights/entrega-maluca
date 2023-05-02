using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rodas : MonoBehaviour
{
    public RodaRotacao carroceria;
    public Rigidbody rb;
    public Transform vizRoda; //Vizualisacao da roda
    public Transform rotRoda; //Posição da roda visual

    public float distanciaSusp;
    public float raioRoda = 0.515f; //raio = diamentro/2, o modelo do gabriel possue uma roda com 1.03 de diametro
    public float forcamola; // constante
    public float rigidezAmortecedor;
    float tamanhoInicialSusp;
    float tamanhoAtualSusp;

    public float fE; // Força elástica  lei de hooke
    public float fA; // Força Amortecedor = velocidade de deslocamento * força

    public float forcasus;
    float forcaAtritoLateral;
    float forcaAtritoLongitudinal;
    public float resRolamento;

    public AnimationCurve coefAtrito;

    public AnimationCurve fLongNorm; //força longitudinal normalizada
    public float slipRatio;
    public float torqueNaRoda;
    public float torqueResultante;
    public float veloAngularRoda;
    public float acelAngular;
    public float massaRoda;
    public float inerciaRoda;

    Vector3 coleta;
    Vector3 velocidadeLocal = new Vector3(0.0001f, 0.0001f, 0.0001f)    ;

     void Update()
    {
        rotRoda.transform.Rotate(Mathf.Rad2Deg * veloAngularRoda * Time.deltaTime, 0,0);
    }
     void Start()
    {
        rotRoda.localPosition = new Vector3(rotRoda.localPosition.x, coleta.y + raioRoda, rotRoda.localPosition.z);
    }
    void FixedUpdate()
    {
        if(Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, distanciaSusp + raioRoda))
        {
            inerciaRoda = massaRoda * Mathf.Pow(raioRoda, 2) / 2;

            torqueResultante = (Input.GetAxis("Vertical") * torqueNaRoda) + (-forcaAtritoLongitudinal * raioRoda);
            acelAngular = torqueResultante / inerciaRoda;
            veloAngularRoda += acelAngular * Time.fixedDeltaTime;
            veloAngularRoda = Mathf.Clamp(veloAngularRoda, -100, 450);

            slipRatio = Mathf.Clamp(((veloAngularRoda * raioRoda) - velocidadeLocal.z) / velocidadeLocal.magnitude,-1,1);

            coleta = hit.point;
            //Transformando velocidade global em velocidade local
            velocidadeLocal = transform.InverseTransformDirection(rb.GetPointVelocity(hit.point));

            Debug.DrawLine(transform.position, hit.point);
            tamanhoInicialSusp = tamanhoAtualSusp; 
            tamanhoAtualSusp = hit.distance - raioRoda;

            //Resultado da força elástica da malo do veículo
            fE = forcamola * (distanciaSusp - tamanhoAtualSusp);

            //resultado da força gerada pelo amortecedor
            fA = rigidezAmortecedor * (tamanhoInicialSusp - tamanhoAtualSusp) / Time.fixedDeltaTime;
           
           //não deixar a roda passar a lataria do carro
           if(hit.distance > raioRoda)
            {
                forcasus = fE + fA;
            }
           else if(hit.distance < (raioRoda + 0.08f))
            {
                forcasus = fE + fA;
            }
           //forca de resistencia ao rolamento do pneu
            resRolamento = carroceria.fArrasto * 30 * Mathf.Abs(velocidadeLocal.z);

            //forca de atrito lateral
            forcaAtritoLateral = (coefAtrito.Evaluate(forcasus) * forcasus) * Mathf.Sign(velocidadeLocal.x);

            //forca de atrito longitudinal
            forcaAtritoLongitudinal = coefAtrito.Evaluate(forcasus) * forcasus * fLongNorm.Evaluate(slipRatio);

            //forca sendo aplicada
            rb.AddForceAtPosition((forcasus * transform.up) + (forcaAtritoLateral * -transform.right) + ((forcaAtritoLongitudinal + resRolamento) * transform.forward), hit.point);

            //visualização do movimento que a suspensão faz na roda
            vizRoda.transform.position = hit.point;
        }
       
    }
}

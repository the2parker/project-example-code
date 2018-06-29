using System.Collections;
using UnityEngine;

public abstract class BulletParent : MonoBehaviour
{

	#region Variables
	protected bool initialized;

	[HideInInspector]
	public BulletInfo bulletInfo;

    [HideInInspector]
    public Rigidbody myRigidbody;

	public ParticleSystem smokeTrail;

	[HideInInspector]
	public BulletSliderManager bulletSliderManager;

	[HideInInspector]
	public bool destroyOnDisable;


	#endregion

	#region Methods

	public virtual void Initialize()
    {
        myRigidbody = GetComponent<Rigidbody>();

		initialized = true;
    }

	public abstract void FixedUpdate(); //used for physics updates

	public abstract void OnTriggerEnter(Collider collider); //for when you hit something

	public abstract void OnEnable(); 

	public abstract void OnDisable();

	public abstract void DamageObject(IEntity entity, float amount); //for damaging any objects you hit

	public abstract void SetToDestroy(); //for when resetting the bullets

	public virtual IEnumerator LifetimeCountdown()
    {
		yield return new WaitForSeconds(bulletInfo.lifetime);

		gameObject.SetActive(false);
    }

	#endregion

}

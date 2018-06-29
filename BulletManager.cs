using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class BulletManager : ScriptableObject //inherit from this to create a new ammo type manager. //Still need to implement PlayerModifiers
{
	protected int ammoCount; //current ammunition count //change back to protected

	protected bool isAmmoRegen; //is ammo currently regenerating

	protected Transform bulletParent; //reference to where we should spawn the bullets and fire them
	protected Transform barrelEnd;

	protected BulletSliderManager bulletSliderManager; //reference to BulletUI to use

	protected IBulletContainer container; //reference to the class using the managers (if it uses the interface)

	protected List<Transform> ammoList = new List<Transform>(); //List to contain a reference to all the bullets

	protected BulletInfo bulletInfo; //make sure to override this with custom bulletInfo

	protected EntityTeamManager teamManager;

	protected PlayerModifiers playerModifiers;

	#region Public Methods

	public void Delete() //call if the manager is going to be replaced with a new one
	{
		CleanUp();
		bulletInfo.RemoveManager(this);
		Destroy(this);
	}

	public void Reset() //reset the manager if needed
	{
		CleanUp();
		CreateAmmo();
	}

	public abstract void ShootButtonDown(float timePressed); //this is for firing, timePressed can be used to change the way its used depending on how long its been held

	public abstract void ShootButtonUp(); //this is for letting the manager known when the button has stopped being pressed

	public abstract void IncreaseAmmo(); //these are for potential power ups, allowing for increasing the ammo ammount on the go

	public abstract void DecreaseAmmo(); //same for this

	#endregion

	protected void Initialize() //initialize the manager, called when one is created from the create method
	{
		CreateAmmo();
	}

	protected virtual void CreateAmmo() //this is allowed to be overwritten, but has a default method that should be used in most cases
	{
		Transform bulletPrefab = bulletInfo.ammoPrefab;

		//create a bullet for each spot in the array
		for (int i = 0; i < bulletInfo.maxAmmoCount; i++)
		{
			Transform newBullet = Instantiate(bulletPrefab, new Vector3(0, -10, 0), barrelEnd.rotation, bulletParent);
			ammoList.Add(newBullet);
			newBullet.gameObject.GetComponentInChildren<Rigidbody>().gameObject.SetActive(false); //create a bullet from the prefab

			//if we have a TeamManager then add it to the team list
			teamManager?.AddTeamObject(newBullet.GetComponentInChildren<Rigidbody>(true).transform);

			if (teamManager == null) //if not, set it to Team Blue
				newBullet.GetComponentInChildren<Rigidbody>(true).transform.tag = TeamManager.Team_Blue;

			BulletParent bulletScript = newBullet.gameObject.GetComponentInChildren<BulletParent>(true); //get the BulletParent script from the prefab, pass it the UI and initialize it
			bulletScript.bulletSliderManager = bulletSliderManager;
			bulletScript.Initialize();
		}

		//reset the currentBullets
		ammoCount = bulletInfo.maxAmmoCount;
	}

	protected virtual void CleanUp() //this is for if the bullets need to be removed
	{
		if (ammoList.Count != 0)
		{
			foreach (Transform bullet in ammoList)
			{
				teamManager?.RemoveTeamObject(bullet.GetComponentInChildren<Rigidbody>(true).transform);

				if (!bullet.gameObject.activeSelf)
					Destroy(bullet.gameObject);
				else
					bullet.GetComponent<BulletParent>().destroyOnDisable = true;
			}

			ammoList.Clear();
		}
	}

	protected abstract IEnumerator AmmoRegen(); //this is for the CoRoutine to regenerate ammo

	#region Initializers/Create Methods

	private void Initialize(Transform bulletParent, Transform barrelEnd, BulletInfo bulletInfo) //these are all the different variations of the initialize code
	{
		this.bulletParent = bulletParent;
		this.barrelEnd = barrelEnd;
		this.bulletInfo = bulletInfo;
		Initialize();
	}

	private void Initialize(Transform bulletParent, Transform barrelEnd, BulletInfo bulletInfo, IBulletContainer bulletContainer, EntityTeamManager teamManager)
	{
		this.teamManager = teamManager;
		container = bulletContainer;
		Initialize(bulletParent, barrelEnd, bulletInfo);
	}

	private void Initialize(Transform bulletParent, Transform barrelEnd, BulletInfo bulletInfo, IBulletContainer bulletContainer, EntityTeamManager teamManager, BulletSliderManager bulletUI)
	{
		this.bulletSliderManager = bulletUI;
		Initialize(bulletParent, barrelEnd, bulletInfo, bulletContainer, teamManager);
	}

	private void Initialize(Transform bulletParent, Transform barrelEnd, BulletInfo bulletInfo, IBulletContainer bulletContianer, EntityTeamManager teamManager, BulletSliderManager bulletUI, PlayerModifiers playerModifiers)
	{
		this.playerModifiers = playerModifiers;
		Initialize(bulletParent, barrelEnd, bulletInfo, bulletContianer, teamManager, bulletUI);
	}

	public virtual BulletManager Create(Transform bulletParent, Transform barrelEnd, BulletInfo bulletInfo) //these are all used to create a bulletManager from the bulletInfo
	{
		BulletManager bulletManager = (BulletManager)CreateInstance(GetType()); //I use GetType() here so that it will create a manager of the current class, not the base class
		bulletManager.Initialize(bulletParent, barrelEnd, bulletInfo);
		return bulletManager;
	}

	public virtual BulletManager Create(Transform bulletParent, Transform barrelEnd, BulletInfo bulletInfo, IBulletContainer bulletContainer, EntityTeamManager teamManager)
	{
		BulletManager bulletManager = (BulletManager)CreateInstance(GetType());
		bulletManager.Initialize(bulletParent, barrelEnd, bulletInfo, bulletContainer, teamManager);
		return bulletManager;
	}

	public virtual BulletManager Create(Transform bulletParent, Transform barrelEnd, BulletInfo bulletInfo, IBulletContainer bulletContainer, EntityTeamManager teamManager, BulletSliderManager bulletSliderManager)
	{
		BulletManager bulletManager = (BulletManager)CreateInstance(GetType());
		bulletManager.Initialize(bulletParent, barrelEnd, bulletInfo, bulletContainer, teamManager, bulletSliderManager);
		return bulletManager;
	}

	public virtual BulletManager Create(Transform bulletParent, Transform barrelEnd, BulletInfo bulletInfo, IBulletContainer bulletContainer, EntityTeamManager teamManager, BulletSliderManager bulletSliderManager, PlayerModifiers playerModifiers)
	{
		BulletManager bulletManager = (BulletManager)CreateInstance(GetType());
		bulletManager.Initialize(bulletParent, barrelEnd, bulletInfo, bulletContainer, teamManager, bulletSliderManager, playerModifiers);
		return bulletManager;
	}

	#endregion
}

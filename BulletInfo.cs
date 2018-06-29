using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

public class BulletInfo : ScriptableObject
{
	//reference to all the different stats the bullets would use
	[DisableInPlayMode]
	public int maxAmmoCount;
	public float regenRate, lifetime, damageAmount, switchTime;
	[Required]
	public BulletManager bulletManager; //reference to the bulletManager for this bullet
	public WeaponType weaponType; //if this weapon is a primary or secondary weapon
	public BulletUI.SliderType sliderType; //what type of UI should it use
	[Required]
	public Transform ammoPrefab; //reference to the actual bullet

	[HideInInspector]
	public enum WeaponType { Primary, Secondary};

	private List<BulletManager> managers = new List<BulletManager>(); //a reference to all managers created so changes can be made on the fly

	public void OnEnable()
	{
		managers.Clear();
	}

	public void RemoveManager(BulletManager managerToRemove)
	{
		managers.Remove(managerToRemove);
	}

	[DisableInEditorMode]
	[ButtonGroup("Ammo", 0)]
	[PropertyOrder(-1)]
	public void IncreaseAmmo() //this show up as buttons in the Editor's inspector, allowing me to change the maximum ammo count in game
	{
		if (maxAmmoCount < 10)
		{
			maxAmmoCount++;
			foreach (BulletManager manager in managers)
				manager?.IncreaseAmmo();
		}
	}

	[DisableInEditorMode]
	[ButtonGroup("Ammo", 0)]
	[PropertyOrder(-1)]
	public void DecreaseAmmo() //this as well
	{
		maxAmmoCount--;

		foreach (BulletManager manager in managers)
			manager?.DecreaseAmmo();
	}

	public BulletManager CreateManager(Transform bulletParent, Transform barrelEnd) //these are called to create a new manager
	{
		BulletManager manager = bulletManager.Create(bulletParent, barrelEnd, this);
		managers.Add(manager);
		return manager;
	}

	public BulletManager CreateManager(Transform bulletParent, Transform barrelEnd, IBulletContainer bulletContainer, EntityTeamManager teamManager)
	{
		BulletManager manager = bulletManager.Create(bulletParent, barrelEnd, this, bulletContainer, teamManager);
		managers.Add(manager);
		return manager;
	}

	public BulletManager CreateManager(Transform bulletParent, Transform barrelEnd, IBulletContainer bulletContainer, EntityTeamManager teamManager, BulletSliderManager bulletSliderManager)
	{
		BulletManager manager = bulletManager.Create(bulletParent, barrelEnd, this, bulletContainer, teamManager, bulletSliderManager);
		managers.Add(manager);
		return manager;
	}

	public BulletManager CreateManager(Transform bulletParent, Transform barrelEnd, IBulletContainer bulletContainer, EntityTeamManager teamManager, BulletSliderManager bulletSliderManager, PlayerModifiers playerModifiers)
	{
		BulletManager manager = bulletManager.Create(bulletParent, barrelEnd, this, bulletContainer, teamManager, bulletSliderManager, playerModifiers);
		managers.Add(manager);
		return manager;
	}
}

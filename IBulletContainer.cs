using UnityEngine;
using System.Collections;

public interface IBulletContainer //this is here so that BulletManager's can affect their container, as well as so they can play and stop Coroutines
{
	void CoroutinePlay(IEnumerator coroutineToPlay);

	void CoroutineStop(IEnumerator coroutineToStop);

	void PhysicsReaction(Vector3 force);
}
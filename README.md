
## This doesn't work with the Scene System at the moment!!
### It's very close to working without even needing Facepunch to update it due to the Entity System -> Scene System change. The TypeLibrary is failing to get the type even though the package seems to mount correctly. 🤷


## Example addon:
```cs
using Sandbox;
using System;

namespace MyAddon;

public class MyAddonComponent : Sandbox.Component
{
	private RealTimeSince _lastJump;
	private Rigidbody _rb;
	private ModelRenderer _mr;
	private ModelCollider _mc;

	protected override void OnAwake()
	{
		_rb = Components.GetOrCreate<Rigidbody>();
		_mr = Components.GetOrCreate<ModelRenderer>();
		_mc = Components.GetOrCreate<ModelCollider>();

		var model = Sandbox.Cloud.Model( "facepunch.soda_can" );
		_mr.Model = model;
		_mc.Model = model;

		Log.Info( "Hi stranger :)" );
	}

	protected override void OnFixedUpdate()
	{
		if ( _lastJump >= 3 )
		{
			_rb.ApplyForce( Vector3.Up * Random.Shared.Next( 80, 120 ) + Transform.Rotation.Forward * 60 );
			_lastJump = 0;
		}
	}
}
```

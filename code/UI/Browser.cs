using Sandbox.UI;

namespace Phystest;
public partial class Browser : Panel
{
	async void SpawnPackage( Package p )
	{
		Log.Info( p );
		Log.Info( $"Is local? : {p.IsRemote}" );
		var package = await Package.FetchAsync( p.FullIdent, false );
		Log.Info( package );
		var primaryType = package.GetMeta( "PrimaryAsset", "" );
		Log.Info( primaryType );

		var isMounted = package.IsMounted( true, true );
		Log.Info( $"Is mounted: {isMounted}" );

		var type = TypeLibrary.GetType( primaryType );
		Log.Info( $"Type: {type}" );

		if ( type is null )
			return;

		var go = NetworkManager.Instance.Scene.CreateObject();
		go.Name = primaryType;
		go.Components.Create( type, true );
	}
}

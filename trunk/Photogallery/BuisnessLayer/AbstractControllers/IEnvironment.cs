using Photogallery;

namespace BuisnessLayer.AbstractControllers
{
	public  interface IEnvironment
	{
		IGalleryUser CurrentClient { get; }
	}
}

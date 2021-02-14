using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
namespace VoxelImporter.grendgine_collada
{
	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
	public partial class Grendgine_Collada_Polylist : Grendgine_Collada_Geometry_Common_Fields
	{
		

		[XmlElement(ElementName = "vcount")]
		public Grendgine_Collada_Int_Array_String VCount;			
		
		
	}
}

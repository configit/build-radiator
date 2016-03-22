using System.Xml;

namespace BuildRadiator.Helpers {
  public static class XmlNodeExtensions {
    public static string GetInnerText( this XmlNode node, string selector, string defaultValue = null) {
      if ( node == null ) {
        return defaultValue;
      }

      var selectedNode = node.SelectSingleNode( selector );
      if ( selectedNode == null ) {
        return defaultValue;
      }

      return selectedNode.InnerText;
    }
  }
}
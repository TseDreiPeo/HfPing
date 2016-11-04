using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;

namespace APureUwpApp {
    public class MyTileUpdater {

        private static Windows.UI.Notifications.TileUpdater updater = null;
        private static XmlDocument tileXml = null;

        public void UpdateTile(List<string> lines) {
            // create the instance of Tile Updater, which enables you to change the appearance of the calling app's tile
            if(updater == null) {
                updater = Windows.UI.Notifications.TileUpdateManager.CreateTileUpdaterForApplication();
                updater.EnableNotificationQueue(false);
                updater.Clear();
                // get the XML content of one of the predefined tile templates, so that, you can customize it
                //tileXmlWide = Windows.UI.Notifications.TileUpdateManager.GetTemplateContent(Windows.UI.Notifications.TileTemplateType.TileWide310x150Text04);
                tileXml = GetTileXML();
            }
            //tileXml.GetElementsByTagName("text")[1].InnerText = $"T: {ticks.ToString()}";

            var wideTile = tileXml.SelectSingleNode("/tile/visual/binding[@template=\"TileWide\"]");
            var nodes = tileXml.SelectNodes("/tile/visual/binding[@template=\"TileWide\"]/text").Reverse();
            foreach(var n in nodes) {
                wideTile.RemoveChild(n);
            }
            foreach(var t in lines) {
                XmlElement txt = tileXml.CreateElement("text");
                txt.InnerText = t;
                wideTile.AppendChild(txt);
            }


            // Create a new tile notification.
            updater.Update(new Windows.UI.Notifications.TileNotification(tileXml));
        }

        private XmlDocument GetTileXML() {
            XmlDocument tile = new XmlDocument();

            try {
                var assembly = typeof(MyTileUpdater).GetTypeInfo().Assembly;
                Stream stream = assembly.GetManifestResourceStream("APureUwpApp.MyTileTemplate.xml");
                string text = "";
                using(var reader = new System.IO.StreamReader(stream)) {
                    text = reader.ReadToEnd();
                }
                tile.LoadXml(text);
            } catch (Exception ex) {
#if DEBUG
                if(System.Diagnostics.Debugger.IsAttached) {
                    var message = ex.Message;
                    System.Diagnostics.Debugger.Break();
                }
#endif
                throw ex;
            }
            return tile;
        }

    }
}


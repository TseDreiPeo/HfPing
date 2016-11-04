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
                tileXml = GetTileXML();
            }

            UpdateSingleTile("TileWide", lines);
            UpdateSingleTile("TileMedium", lines);
            UpdateSingleTile("TileSmall", lines);

            updater.Update(new Windows.UI.Notifications.TileNotification(tileXml));
        }

        private void UpdateSingleTile(string tileType, List<string> lines) {
            int linesToUse = Math.Min(lines.Count, 6);
            int lengthToUse = 100;
            switch(tileType) {  case "TileSmall": { linesToUse = 2; lengthToUse = 6; break; };
                                case "TileMedium": {lengthToUse = 14; break; };
                                default: break; }            
       
            var aTile = tileXml.SelectSingleNode($"/tile/visual/binding[@template=\"{tileType}\"]");
            if(aTile != null) {
                var nodes = tileXml.SelectNodes($"/tile/visual/binding[@template=\"{tileType}\"]/text").Reverse();
                foreach(var n in nodes) {
                    aTile.RemoveChild(n);
                }
                for(int i = 0; i < linesToUse; i++) {
                    XmlElement txt = tileXml.CreateElement("text");
                    txt.InnerText = lines[i].Length > lengthToUse?lines[i].Substring(0, lengthToUse):lines[i];
                    aTile.AppendChild(txt);
                }
            }
        }

        private XmlDocument GetTileXML() {
            XmlDocument tile = new XmlDocument();

            try {
                var assembly = typeof(MyTileUpdater).GetTypeInfo().Assembly;
                Stream stream = assembly.GetManifestResourceStream("APureUwpApp.TileUpdater.MyTileTemplate.xml");
                string text = "";
                if(stream != null) {
                    using(var reader = new System.IO.StreamReader(stream)) {
                        text = reader.ReadToEnd();
                    }
                }
                tile.LoadXml(text);
            } catch (Exception ex) {
#if DEBUG
                if(System.Diagnostics.Debugger.IsAttached) {
                    var message = ex.Message;
                    System.Diagnostics.Debugger.Break();
                }
#else 
                throw ex;
#endif
            }
            return tile;
        }

    }
}


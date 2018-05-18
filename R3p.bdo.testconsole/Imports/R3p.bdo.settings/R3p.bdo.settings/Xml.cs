using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using R3p.bdo.GameExternals.Enums;
using R3p.bdo.GameExternals.Structs.AutoItemBuy;
using R3p.bdo.GameExternals.Structs.AutoItemRegister;
using R3p.bdo.GameExternals.Structs.AutoProcessing;
using R3p.bdo.GameExternals.Structs.Overlay;
using R3p.bdo.GameInternals.Structs.SystemVariables;

namespace R3p.bdo.settings
{
    public class Xml
    {
        public class Writer
        {
            public static void SaveSettings()
            {
                XmlDocument xmlDoc = new XmlDocument();

                XmlNode Root = xmlDoc.CreateElement("Settings");
                xmlDoc.AppendChild(Root);

                XmlComment hotkeysComment = xmlDoc.CreateComment(String.Join("\t", Enum.GetNames(typeof(VirtualKeyCode))));

                Root.AppendChild(hotkeysComment);

                XmlNode child = xmlDoc.CreateElement("HotKeys");

                foreach (var entry in Settings.HotKeys.List)
                {
                    XmlAttribute attribute = xmlDoc.CreateAttribute(entry.Key);
                    attribute.Value = entry.Value;

                    child.Attributes.Append(attribute);
                }

                Root.AppendChild(child);

                child = xmlDoc.CreateElement("AutoFish");

                foreach (var entry in Settings.AutoFish.List)
                {
                    XmlAttribute attribute = xmlDoc.CreateAttribute(entry.Key);
                    attribute.Value = entry.Value;

                    child.Attributes.Append(attribute);
                }

                Root.AppendChild(child);

                child = xmlDoc.CreateElement("AutoRestore");

                foreach (var entry in Settings.AutoRestore.List)
                {
                    XmlAttribute attribute = xmlDoc.CreateAttribute(entry.Key);
                    attribute.Value = entry.Value;

                    child.Attributes.Append(attribute);
                }

                Root.AppendChild(child);

                child = xmlDoc.CreateElement("UIHack");

                foreach (var entry in Settings.UIHack.List)
                {
                    XmlAttribute attribute = xmlDoc.CreateAttribute(entry.Key);
                    attribute.Value = entry.Value;

                    child.Attributes.Append(attribute);
                }

                Root.AppendChild(child);
                
                child = xmlDoc.CreateElement("SpeedHack");

                foreach (var entry in Settings.SpeedHack.List)
                {
                    XmlAttribute attribute = xmlDoc.CreateAttribute(entry.Key);
                    attribute.Value = entry.Value;

                    child.Attributes.Append(attribute);
                }

                XmlNode horse = xmlDoc.CreateElement("Horse");

                foreach (var entry in Settings.SpeedHack.Horse.List)
                {
                    XmlAttribute attribute = xmlDoc.CreateAttribute(entry.Key);
                    attribute.Value = entry.Value;

                    horse.Attributes.Append(attribute);
                }

                XmlNode ship = xmlDoc.CreateElement("Ship");

                foreach (var entry in Settings.SpeedHack.Ship.List)
                {
                    XmlAttribute attribute = xmlDoc.CreateAttribute(entry.Key);
                    attribute.Value = entry.Value;

                    ship.Attributes.Append(attribute);
                }

                XmlNode player = xmlDoc.CreateElement("Player");

                foreach (var entry in Settings.SpeedHack.Player.List)
                {
                    XmlAttribute attribute = xmlDoc.CreateAttribute(entry.Key);
                    attribute.Value = entry.Value;

                    player.Attributes.Append(attribute);
                }

                child.AppendChild(horse);
                child.AppendChild(ship);
                child.AppendChild(player);
                Root.AppendChild(child);

                child = xmlDoc.CreateElement("AutoPotion");

                foreach (var entry in Settings.AutoPotion.List)
                {
                    XmlAttribute attribute = xmlDoc.CreateAttribute(entry.Key);
                    attribute.Value = entry.Value;

                    child.Attributes.Append(attribute);
                }

                Root.AppendChild(child);

                child = xmlDoc.CreateElement("AutoItemRegister");

                foreach (var entry in Settings.AutoItemRegister.List)
                {
                    XmlAttribute attribute = xmlDoc.CreateAttribute(entry.Key);
                    attribute.Value = entry.Value;

                    child.Attributes.Append(attribute);
                }

                XmlNode items = xmlDoc.CreateElement("Items");
                
                foreach (var itemEntry in Settings.AutoItemRegister.Items)
                {
                    XmlNode item = xmlDoc.CreateElement("Item");

                    XmlAttribute itemAtr = xmlDoc.CreateAttribute("Enabled");
                    itemAtr.Value = Convert.ToInt32(itemEntry.Enabled).ToString();
                    item.Attributes.Append(itemAtr);

                    itemAtr = xmlDoc.CreateAttribute("ItemId");
                    itemAtr.Value = itemEntry.ItemId.ToString();
                    item.Attributes.Append(itemAtr);

                    itemAtr = xmlDoc.CreateAttribute("SellValue");
                    itemAtr.Value = itemEntry.SellValue.ToString();
                    item.Attributes.Append(itemAtr);

                    items.AppendChild(item);
                }

                child.AppendChild(items);
                Root.AppendChild(child);

                child = xmlDoc.CreateElement("AutoItemBuy");

                foreach (var entry in Settings.AutoItemBuy.List)
                {
                    XmlAttribute attribute = xmlDoc.CreateAttribute(entry.Key);
                    attribute.Value = entry.Value;

                    child.Attributes.Append(attribute);
                }

                items = xmlDoc.CreateElement("Items");

                foreach (var itemEntry in Settings.AutoItemBuy.Items)
                {
                    XmlNode item = xmlDoc.CreateElement("Item");

                    XmlAttribute itemAtr = xmlDoc.CreateAttribute("Enabled");
                    itemAtr.Value = Convert.ToInt32(itemEntry.Enabled).ToString();
                    item.Attributes.Append(itemAtr);

                    itemAtr = xmlDoc.CreateAttribute("ItemId");
                    itemAtr.Value = itemEntry.ItemId.ToString();
                    item.Attributes.Append(itemAtr);

                    itemAtr = xmlDoc.CreateAttribute("EnchantLevel");
                    itemAtr.Value = itemEntry.EnchantLevel.ToString();
                    item.Attributes.Append(itemAtr);

                    itemAtr = xmlDoc.CreateAttribute("SessionMax");
                    itemAtr.Value = itemEntry.SessionMax.ToString();
                    item.Attributes.Append(itemAtr);

                    itemAtr = xmlDoc.CreateAttribute("MaxPrice");
                    itemAtr.Value = itemEntry.MaxPrice.ToString();
                    item.Attributes.Append(itemAtr);

                    itemAtr = xmlDoc.CreateAttribute("IsStack");
                    itemAtr.Value = Convert.ToInt32(itemEntry.IsStack).ToString();
                    item.Attributes.Append(itemAtr);

                    items.AppendChild(item);
                }

                child.AppendChild(items);
                Root.AppendChild(child);

                child = xmlDoc.CreateElement("Overlay");

                foreach (var entry in Settings.Overlay.List)
                {
                    XmlAttribute attribute = xmlDoc.CreateAttribute(entry.Key);
                    attribute.Value = entry.Value;

                    child.Attributes.Append(attribute);
                }

                XmlNode actors = xmlDoc.CreateElement("Actors");
                XmlComment comment = xmlDoc.CreateComment("Types: ActorType_All, ActorType_Player, ActorType_Monster, ActorType_Npc, ActorType_Vehicle, ActorType_Gate, ActorType_Alterego, ActorType_Collect, ActorType_Household, ActorType_Installation, ActorType_Deadbody");

                actors.AppendChild(comment);

                foreach (var actorEntry in Settings.Overlay.Actors)
                {
                    XmlNode actor = xmlDoc.CreateElement("Actor");

                    XmlAttribute actorAtr = xmlDoc.CreateAttribute("Enabled");
                    actorAtr.Value = Convert.ToInt32(actorEntry.Enabled).ToString();
                    actor.Attributes.Append(actorAtr);

                    actorAtr = xmlDoc.CreateAttribute("Type");
                    actorAtr.Value = actorEntry.ActorType.ToString();
                    actor.Attributes.Append(actorAtr);

                    actorAtr = xmlDoc.CreateAttribute("IDs");
                    actorAtr.Value = String.Join(",", actorEntry.ActorIds);
                    actor.Attributes.Append(actorAtr);

                    actorAtr = xmlDoc.CreateAttribute("Color");
                    actorAtr.Value = String.Join(",", actorEntry.ColorCode);
                    actor.Attributes.Append(actorAtr);

                    actorAtr = xmlDoc.CreateAttribute("DrawCircle");
                    actorAtr.Value = Convert.ToInt32(actorEntry.DrawCircle).ToString();
                    actor.Attributes.Append(actorAtr);

                    actorAtr = xmlDoc.CreateAttribute("DrawLine");
                    actorAtr.Value = Convert.ToInt32(actorEntry.DrawLine).ToString();
                    actor.Attributes.Append(actorAtr);

                    actorAtr = xmlDoc.CreateAttribute("Thickness");
                    actorAtr.Value = actorEntry.Thickness.ToString();
                    actor.Attributes.Append(actorAtr);

                    actorAtr = xmlDoc.CreateAttribute("ShowName");
                    actorAtr.Value = Convert.ToInt32(actorEntry.ShowName).ToString();
                    actor.Attributes.Append(actorAtr);

                    actorAtr = xmlDoc.CreateAttribute("ShowActorId");
                    actorAtr.Value = Convert.ToInt32(actorEntry.ShowActorId).ToString();
                    actor.Attributes.Append(actorAtr);

                    actorAtr = xmlDoc.CreateAttribute("ShowDistance");
                    actorAtr.Value = Convert.ToInt32(actorEntry.ShowDistance).ToString();
                    actor.Attributes.Append(actorAtr);

                    actorAtr = xmlDoc.CreateAttribute("ShowOnWorldMap");
                    actorAtr.Value = Convert.ToInt32(actorEntry.ShowOnWorldMap).ToString();
                    actor.Attributes.Append(actorAtr);

                    actorAtr = xmlDoc.CreateAttribute("ShowLevel");
                    actorAtr.Value = Convert.ToInt32(actorEntry.ShowLevel).ToString();
                    actor.Attributes.Append(actorAtr);

                    actorAtr = xmlDoc.CreateAttribute("ShowHp");
                    actorAtr.Value = Convert.ToInt32(actorEntry.ShowHp).ToString();
                    actor.Attributes.Append(actorAtr);

                    actorAtr = xmlDoc.CreateAttribute("MinDistance");
                    actorAtr.Value = actorEntry.MinDistance.ToString();
                    actor.Attributes.Append(actorAtr);

                    actorAtr = xmlDoc.CreateAttribute("MaxDistance");
                    actorAtr.Value = actorEntry.MaxDistance.ToString();
                    actor.Attributes.Append(actorAtr);

                    actors.AppendChild(actor);
                }

                XmlNode waypoints = xmlDoc.CreateElement("Waypoints");
                
                foreach (var waypointEntry in Settings.Overlay.Waypoints)
                {
                    XmlNode waypoint = xmlDoc.CreateElement("Waypoint");

                    XmlAttribute waypointAtr = xmlDoc.CreateAttribute("Enabled");
                    waypointAtr.Value = Convert.ToInt32(waypointEntry.Enabled).ToString();
                    waypoint.Attributes.Append(waypointAtr);

                    waypointAtr = xmlDoc.CreateAttribute("Color");
                    waypointAtr.Value = String.Join(",", waypointEntry.ColorCode);
                    waypoint.Attributes.Append(waypointAtr);

                    waypointAtr = xmlDoc.CreateAttribute("Thickness");
                    waypointAtr.Value = waypointEntry.Thickness.ToString();
                    waypoint.Attributes.Append(waypointAtr);

                    waypointAtr = xmlDoc.CreateAttribute("Position");
                    waypointAtr.Value = String.Join(",", waypointEntry.Position);
                    waypoint.Attributes.Append(waypointAtr);

                    waypointAtr = xmlDoc.CreateAttribute("Name");
                    waypointAtr.Value = waypointEntry.Name;
                    waypoint.Attributes.Append(waypointAtr);

                    waypoints.AppendChild(waypoint);
                }

                child.AppendChild(waypoints);
                child.AppendChild(actors);
                Root.AppendChild(child);

                child = xmlDoc.CreateElement("AutoProcessing");

                foreach (var entry in Settings.AutoProcessing.List)
                {
                    XmlAttribute attribute = xmlDoc.CreateAttribute(entry.Key);
                    attribute.Value = entry.Value;

                    child.Attributes.Append(attribute);
                }

                XmlNode pItems = xmlDoc.CreateElement("Items");
                XmlComment pComment = xmlDoc.CreateComment("Types: Shaking, Grinding, Chopping, Drying, Thinning, Heating");

                pItems.AppendChild(pComment);

                foreach (var itemEntry in Settings.AutoProcessing.Items)
                {
                    XmlNode item = xmlDoc.CreateElement("Item");

                    XmlAttribute itemAtr = xmlDoc.CreateAttribute("Enabled");
                    itemAtr.Value = Convert.ToInt32(itemEntry.Enabled).ToString();
                    item.Attributes.Append(itemAtr);

                    itemAtr = xmlDoc.CreateAttribute("ItemId");
                    itemAtr.Value = itemEntry.ItemId.ToString();
                    item.Attributes.Append(itemAtr);

                    itemAtr = xmlDoc.CreateAttribute("ProcessingType");
                    itemAtr.Value = itemEntry.ProcessingType.ToString();
                    item.Attributes.Append(itemAtr);

                    itemAtr = xmlDoc.CreateAttribute("MinCount");
                    itemAtr.Value = itemEntry.MinCount.ToString();
                    item.Attributes.Append(itemAtr);

                    itemAtr = xmlDoc.CreateAttribute("ResultItemIds");
                    itemAtr.Value = String.Join(",", itemEntry.ResultItemIds);
                    item.Attributes.Append(itemAtr);

                    itemAtr = xmlDoc.CreateAttribute("Comment");
                    itemAtr.Value = itemEntry.Comment;
                    item.Attributes.Append(itemAtr);

                    pItems.AppendChild(item);
                }

                child.AppendChild(pItems);
                Root.AppendChild(child);

                xmlDoc.Save(Files.fileSettings);
            }

            public static void DumpSystemVariables()
            {
                XmlDocument xmlDoc = new XmlDocument();

                XmlNode Root = xmlDoc.CreateElement("SystemVariables");
                xmlDoc.AppendChild(Root);

                foreach (var sysvar in Collection.SystemVariables.Base.origList.List)
                {
                    XmlNode child = xmlDoc.CreateElement(sysvar.Name);

                    XmlAttribute attrib = xmlDoc.CreateAttribute("Enabled");
                    attrib.Value = Convert.ToInt32(sysvar.Enabled).ToString();
                    child.Attributes.Append(attrib);

                    attrib = xmlDoc.CreateAttribute("ValueInt");
                    attrib.Value = sysvar.ValueInt.ToString();
                    child.Attributes.Append(attrib);

                    attrib = xmlDoc.CreateAttribute("ValueFloat");
                    attrib.Value = sysvar.ValueFloat.ToString();
                    child.Attributes.Append(attrib);

                    Root.AppendChild(child);
                }
                
                xmlDoc.Save(Files.fileSystemVariables);
            }
        }

        public class Reader
        {
            public static void LoadSettings()
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(Files.fileSettings);

                XmlNode root = xmlDoc.FirstChild;

                XmlNode hotkeys = root["HotKeys"];

                if (hotkeys != null)
                {

                    foreach (XmlAttribute atr in hotkeys.Attributes)
                    {
                        Settings.HotKeys.List[atr.Name] = atr.Value;
                    }
                }

                XmlNode autoFish = root["AutoFish"];

                if (autoFish != null)
                {

                    foreach (XmlAttribute atr in autoFish.Attributes)
                    {
                        Settings.AutoFish.List[atr.Name] = atr.Value;
                    }
                }

                XmlNode autoRestore = root["AutoRestore"];

                if (autoRestore != null)
                {

                    foreach (XmlAttribute atr in autoRestore.Attributes)
                    {
                        Settings.AutoRestore.List[atr.Name] = atr.Value;
                    }
                }

                XmlNode uiHack = root["UIHack"];

                if (uiHack != null)
                {

                    foreach (XmlAttribute atr in uiHack.Attributes)
                    {
                        Settings.UIHack.List[atr.Name] = atr.Value;
                    }
                }
                
                XmlNode autoPot = root["AutoPotion"];

                if (autoPot != null)
                {

                    foreach (XmlAttribute atr in autoPot.Attributes)
                    {
                        Settings.AutoPotion.List[atr.Name] = atr.Value;
                    }
                }

                XmlNode autoItemRegister = root["AutoItemRegister"];

                if (autoItemRegister != null)
                {

                    foreach (XmlAttribute atr in autoItemRegister.Attributes)
                    {
                        Settings.AutoItemRegister.List[atr.Name] = atr.Value;
                    }


                    XmlNode items = autoItemRegister["Items"];

                    if (items != null)
                    {

                        List<ItemRegisterObject> lItems = new List<ItemRegisterObject>();

                        foreach (XmlNode item in items.ChildNodes)
                        {
                            bool enabled = false;
                            if (item.Attributes.GetNamedItem("Enabled") != null)
                                enabled = Convert.ToBoolean(Convert.ToInt32(item.Attributes["Enabled"].Value));

                            int itemId = 0;
                            if (item.Attributes.GetNamedItem("ItemId") != null)
                                itemId = Convert.ToInt32(item.Attributes["ItemId"].Value);

                            int sellValue = 0;
                            if (item.Attributes.GetNamedItem("SellValue") != null)
                                sellValue = Convert.ToInt32(item.Attributes["SellValue"].Value);

                            lItems.Add(new ItemRegisterObject(enabled, itemId, sellValue));
                        }

                        Settings.AutoItemRegister.Items = lItems;
                    }
                }

                XmlNode autoItemBuy = root["AutoItemBuy"];

                if (autoItemBuy != null)
                {

                    foreach (XmlAttribute atr in autoItemBuy.Attributes)
                    {
                        Settings.AutoItemBuy.List[atr.Name] = atr.Value;
                    }


                    XmlNode items = autoItemBuy["Items"];

                    if (items != null)
                    {

                        List<ItemBuyObject> lItems = new List<ItemBuyObject>();

                        foreach (XmlNode item in items.ChildNodes)
                        {
                            bool enabled = false;
                            if (item.Attributes.GetNamedItem("Enabled") != null)
                                enabled = Convert.ToBoolean(Convert.ToInt32(item.Attributes["Enabled"].Value));

                            int itemId = 0;
                            if (item.Attributes.GetNamedItem("ItemId") != null)
                                itemId = Convert.ToInt32(item.Attributes["ItemId"].Value);

                            int enchantLevel = 0;
                            if (item.Attributes.GetNamedItem("EnchantLevel") != null)
                                enchantLevel = Convert.ToInt32(item.Attributes["EnchantLevel"].Value);

                            int sessionMax = 0;
                            if (item.Attributes.GetNamedItem("SessionMax") != null)
                                sessionMax = Convert.ToInt32(item.Attributes["SessionMax"].Value);

                            long maxPrice = 0;
                            if (item.Attributes.GetNamedItem("MaxPrice") != null)
                                maxPrice = Convert.ToInt32(item.Attributes["MaxPrice"].Value);

                            bool isStack = false;
                            if (item.Attributes.GetNamedItem("IsStack") != null)
                                isStack = Convert.ToBoolean(Convert.ToInt32(item.Attributes["IsStack"].Value));

                            lItems.Add(new ItemBuyObject(enabled, itemId, enchantLevel, sessionMax, maxPrice, isStack));
                        }

                        Settings.AutoItemBuy.Items = lItems;
                    }
                }

                XmlNode overlay = root["Overlay"];

                if (overlay != null)
                {

                    foreach (XmlAttribute atr in overlay.Attributes)
                    {
                        Settings.Overlay.List[atr.Name] = atr.Value;
                    }
                }

                XmlNode actors = overlay["Actors"];

                if (actors != null)
                {

                    List<ActorObject> lActors = new List<ActorObject>();

                    foreach (XmlNode actor in actors.ChildNodes)
                    {
                        if (actor.NodeType == XmlNodeType.Comment)
                            continue;

                        bool enabled = false;
                        if (actor.Attributes.GetNamedItem("Enabled") != null)
                            enabled = Convert.ToBoolean(Convert.ToInt32(actor.Attributes["Enabled"].Value));

                        ActorType actorType = ActorType.ActorType_All;
                        if (actor.Attributes.GetNamedItem("Type") != null)
                            actorType = (ActorType) Enum.Parse(typeof (ActorType), actor.Attributes["Type"].Value);



                        int[] actorIDs = new int[] {0};
                        if (actor.Attributes.GetNamedItem("IDs") != null)
                        {
                            List<int> lActorIDs = new List<int>();
                            var idSplit = actor.Attributes["IDs"].Value.Split(',');
                            foreach (var id in idSplit)
                            {
                                lActorIDs.Add(Convert.ToInt32(id));
                            }
                            actorIDs = lActorIDs.ToArray();
                        }

                        int[] color = new int[] {255, 0, 0, 255};
                        if (actor.Attributes.GetNamedItem("Color") != null)
                        {
                            List<int> lColor = new List<int>();
                            var colorSplit = actor.Attributes["Color"].Value.Split(',');
                            foreach (var cc in colorSplit)
                            {
                                lColor.Add(Convert.ToInt32(cc));
                            }
                            color = lColor.ToArray();
                        }

                        bool drawCircle = true;
                        if (actor.Attributes.GetNamedItem("DrawCircle") != null)
                            Convert.ToBoolean(Convert.ToInt32(actor.Attributes["DrawCircle"].Value));

                        bool drawLine = false;
                        if (actor.Attributes.GetNamedItem("DrawLine") != null)
                            drawLine = Convert.ToBoolean(Convert.ToInt32(actor.Attributes["DrawLine"].Value));

                        bool showName = false;
                        if (actor.Attributes.GetNamedItem("ShowName") != null)
                            showName = Convert.ToBoolean(Convert.ToInt32(actor.Attributes["ShowName"].Value));

                        bool showActorId = false;
                        if (actor.Attributes.GetNamedItem("ShowActorId") != null)
                            showActorId = Convert.ToBoolean(Convert.ToInt32(actor.Attributes["ShowActorId"].Value));

                        int thickness = 1;
                        if (actor.Attributes.GetNamedItem("Thickness") != null)
                            thickness = Convert.ToInt32(actor.Attributes["Thickness"].Value);

                        bool showDistance = false;
                        if (actor.Attributes.GetNamedItem("ShowDistance") != null)
                            showDistance = Convert.ToBoolean(Convert.ToInt32(actor.Attributes["ShowDistance"].Value));

                        bool showOnWorldMap = false;
                        if (actor.Attributes.GetNamedItem("ShowOnWorldMap") != null)
                            showOnWorldMap = Convert.ToBoolean(Convert.ToInt32(actor.Attributes["ShowOnWorldMap"].Value));

                        bool showLevel = false;
                        if (actor.Attributes.GetNamedItem("ShowLevel") != null)
                            showLevel = Convert.ToBoolean(Convert.ToInt32(actor.Attributes["ShowLevel"].Value));

                        bool showHp = false;
                        if (actor.Attributes.GetNamedItem("ShowHp") != null)
                            showHp = Convert.ToBoolean(Convert.ToInt32(actor.Attributes["ShowHp"].Value));

                        int minDistance = 0;
                        if (actor.Attributes.GetNamedItem("MinDistance") != null)
                            minDistance = Convert.ToInt32(actor.Attributes["MinDistance"].Value);

                        int maxDistance = 0;
                        if (actor.Attributes.GetNamedItem("MaxDistance") != null)
                            maxDistance = Convert.ToInt32(actor.Attributes["MaxDistance"].Value);

                        lActors.Add(new ActorObject(enabled, actorType, actorIDs, color, drawCircle, drawLine, thickness,
                            showName, showActorId, showDistance, showOnWorldMap, minDistance, maxDistance, showLevel, showHp));
                    }

                    Settings.Overlay.Actors = lActors;
                }

                XmlNode waypoints = overlay["Waypoints"];

                if (waypoints != null)
                {

                    List<WaypointObject> lWaypoints = new List<WaypointObject>();

                    foreach (XmlNode waypoint in waypoints.ChildNodes)
                    {
                        if (waypoint.NodeType == XmlNodeType.Comment)
                            continue;

                        bool enabled = false;
                        if (waypoint.Attributes.GetNamedItem("Enabled") != null)
                            enabled = Convert.ToBoolean(Convert.ToInt32(waypoint.Attributes["Enabled"].Value));
                        
                        int[] color = new int[] { 255, 0, 0, 255 };
                        if (waypoint.Attributes.GetNamedItem("Color") != null)
                        {
                            List<int> lColor = new List<int>();
                            var colorSplit = waypoint.Attributes["Color"].Value.Split(',');
                            foreach (var cc in colorSplit)
                            {
                                lColor.Add(Convert.ToInt32(cc));
                            }
                            color = lColor.ToArray();
                        }
                        
                        int thickness = 1;
                        if (waypoint.Attributes.GetNamedItem("Thickness") != null)
                            thickness = Convert.ToInt32(waypoint.Attributes["Thickness"].Value);

                        float[] position = new float[] { 0f, 0f, 0f};
                        if (waypoint.Attributes.GetNamedItem("Position") != null)
                        {
                            List<float> lColor = new List<float>();
                            var colorSplit = waypoint.Attributes["Position"].Value.Split(',');
                            foreach (var cc in colorSplit)
                            {
                                lColor.Add(Convert.ToSingle(cc));
                            }
                            position = lColor.ToArray();
                        }

                        string name = "";
                        if (waypoint.Attributes.GetNamedItem("Name") != null)
                            name = waypoint.Attributes["Name"].Value;

                        lWaypoints.Add(new WaypointObject(enabled, color, thickness, position, name));
                    }

                    Settings.Overlay.Waypoints = lWaypoints;
                }

                XmlNode speedHack = root["SpeedHack"];

                if (speedHack != null)
                {

                    foreach (XmlAttribute atr in speedHack.Attributes)
                    {
                        Settings.SpeedHack.List[atr.Name] = atr.Value;
                    }

                    XmlNode horse = speedHack["Horse"];
                    XmlNode ship = speedHack["Ship"];
                    XmlNode player = speedHack["Player"];

                    if (player != null)
                    {

                        foreach (XmlAttribute atr in horse.Attributes)
                        {
                            Settings.SpeedHack.Horse.List[atr.Name] = atr.Value;
                        }

                        foreach (XmlAttribute atr in ship.Attributes)
                        {
                            Settings.SpeedHack.Ship.List[atr.Name] = atr.Value;
                        }

                        foreach (XmlAttribute atr in player.Attributes)
                        {
                            Settings.SpeedHack.Player.List[atr.Name] = atr.Value;
                        }
                    }
                }

                XmlNode autoprocessing = root["AutoProcessing"];

                if (autoprocessing != null)
                {

                    foreach (XmlAttribute atr in autoprocessing.Attributes)
                    {
                        Settings.AutoProcessing.List[atr.Name] = atr.Value;
                    }
                }

                XmlNode pItems = autoprocessing["Items"];

                if (pItems != null)
                {

                    List<ProcessingObject> lpItems = new List<ProcessingObject>();

                    foreach (XmlNode item in pItems.ChildNodes)
                    {
                        if (item.NodeType == XmlNodeType.Comment)
                            continue;

                        bool enabled = false;
                        if (item.Attributes.GetNamedItem("Enabled") != null)
                            enabled = Convert.ToBoolean(Convert.ToInt32(item.Attributes["Enabled"].Value));

                        int itemId = 0;
                        if (item.Attributes.GetNamedItem("ItemId") != null)
                            itemId = Convert.ToInt32(item.Attributes["ItemId"].Value);

                        ProcessingType pType = ProcessingType.Chopping;
                        if (item.Attributes.GetNamedItem("ProcessingType") != null)
                            pType =
                                (ProcessingType)
                                    Enum.Parse(typeof (ProcessingType), item.Attributes["ProcessingType"].Value);

                        int minCount = 10;
                        if (item.Attributes.GetNamedItem("MinCount") != null)
                            minCount = Convert.ToInt32(item.Attributes["MinCount"].Value);

                        int[] resultItemIds = new int[] {0};
                        if (item.Attributes.GetNamedItem("ResultItemIds") != null)
                        {
                            List<int> lb = new List<int>();
                            var idSplit = item.Attributes["ResultItemIds"].Value.Split(',');
                            foreach (var id in idSplit)
                            {
                                lb.Add(Convert.ToInt32(id));
                            }
                            resultItemIds = lb.ToArray();
                        }

                        string comment = "";
                        if (item.Attributes.GetNamedItem("Comment") != null)
                            comment = item.Attributes["Comment"].Value;

                        lpItems.Add(new ProcessingObject(enabled, itemId, pType, minCount, resultItemIds, comment));
                    }

                    Settings.AutoProcessing.Items = lpItems;
                }

                Xml.Writer.SaveSettings();
            }

            public static void LoadSystemVariables()
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(Files.fileSystemVariables);

                XmlNode root = xmlDoc.FirstChild;

                int index = 0;

                foreach (XmlNode child in root.ChildNodes)
                {
                    string name = child.Name;
                    bool enabled = Convert.ToBoolean(Convert.ToInt32(child.Attributes["Enabled"].Value));
                    int valueInt = Convert.ToInt32(child.Attributes["ValueInt"].Value);
                    float valueFloat = Convert.ToSingle(child.Attributes["ValueFloat"].Value);

                    SystemVariable origSysVar = Collection.SystemVariables.Base.origList.List.FirstOrDefault(x => x.Name == name);

                    if (origSysVar != null)
                    {
                        if (origSysVar.Enabled != enabled)
                            origSysVar.SetEnabled((byte) Convert.ToInt32(enabled));

                        if (origSysVar.ValueInt != valueInt)
                            origSysVar.SetValue(valueInt);

                        if (!origSysVar.ValueFloat.Equals(valueFloat))
                            origSysVar.SetValue(valueFloat);
                    }

                    index++;
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3p.bdo.Pipe
{
    public enum FromWhereType
    {
        Inventory = 0,
        Warehouse = 2
    }

    public class Call
    {
        public static void SetVehicleSpeeds(int actorId, int accel, int speed, int turn, int brake)
        {
            //ManualResetEvent SyncClientServer = (ManualResetEvent)obj;
            using (NamedPipeClientStream pipeStream = new NamedPipeClientStream("PipesOfPiece"))
            {
                pipeStream.Connect();

                //MessageBox.Show(“[Client] Pipe connection established”);
                using (StreamWriter sw = new StreamWriter(pipeStream))
                {
                    sw.AutoFlush = true;
                    sw.WriteLine("SetVehicleSpeeds" + " " + Engine.Instance.mThread.Id + " " + actorId + " " + accel + " " + speed + " " + turn + " " + brake);
                }
            }
        }

        public static void SetPlayerSpeeds(int mvmt, int atk, int cst)
        {
            //ManualResetEvent SyncClientServer = (ManualResetEvent)obj;
            using (NamedPipeClientStream pipeStream = new NamedPipeClientStream("PipesOfPiece"))
            {
                pipeStream.Connect();

                //MessageBox.Show(“[Client] Pipe connection established”);
                using (StreamWriter sw = new StreamWriter(pipeStream))
                {
                    sw.AutoFlush = true;
                    sw.WriteLine("SetPlayerSpeeds" + " " + Engine.Instance.mThread.Id + " " + mvmt + " " + atk + " " + cst);
                }
            }
        }

        public static void GetDroppedItemList(int actorKey)
        {
            //ManualResetEvent SyncClientServer = (ManualResetEvent)obj;
            using (NamedPipeClientStream pipeStream = new NamedPipeClientStream("PipesOfPiece"))
            {
                pipeStream.Connect();

                //MessageBox.Show(“[Client] Pipe connection established”);
                using (StreamWriter sw = new StreamWriter(pipeStream))
                {
                    sw.AutoFlush = true;
                    sw.WriteLine("GetDroppedItemList" + " " + Engine.Instance.mThread.Id + " " + Collection.Actors.Local.PlayerData.ActorKey + " " + actorKey);
                }
            }
        }

        public static void PickupDropedItem(int actorKey, int slot, int count)
        {
            //ManualResetEvent SyncClientServer = (ManualResetEvent)obj;
            using (NamedPipeClientStream pipeStream = new NamedPipeClientStream("PipesOfPiece"))
            {
                pipeStream.Connect();

                //MessageBox.Show(“[Client] Pipe connection established”);
                using (StreamWriter sw = new StreamWriter(pipeStream))
                {
                    sw.AutoFlush = true;
                    sw.WriteLine("PickupDropedItem" + " " + Engine.Instance.mThread.Id + " " + Collection.Actors.Local.PlayerData.ActorKey + " " + actorKey + " " + slot  + " " + count);
                }
            }
        }

        public static void WorldMapNaviStart(float[] pos, byte isAuto, byte someValue)
        {
            //ManualResetEvent SyncClientServer = (ManualResetEvent)obj;
            using (NamedPipeClientStream pipeStream = new NamedPipeClientStream("PipesOfPiece"))
            {
                pipeStream.Connect();

                //MessageBox.Show(“[Client] Pipe connection established”);
                using (StreamWriter sw = new StreamWriter(pipeStream))
                {
                    sw.AutoFlush = true;
                    sw.WriteLine("WorldMapNaviStart" + " " + Engine.Instance.mThread.Id + " " + pos[0] + " " + pos[1] + " " + pos[2] + " " + isAuto + " " + someValue);
                }
            }
        }

        public static void ProcessInteraction(int value)
        {
            using (NamedPipeClientStream pipeStream = new NamedPipeClientStream("PipesOfPiece"))
            {
                pipeStream.Connect();

                //MessageBox.Show(“[Client] Pipe connection established”);
                using (StreamWriter sw = new StreamWriter(pipeStream))
                {
                    sw.AutoFlush = true;
                    sw.WriteLine("ProcessInteraction" + " " + Engine.Instance.mThread.Id + " " + value);
                }
            }
        }

        public static void DoString(string code)
        {
            code = code.Replace("\t", "").Replace("\n", "").Replace("\r", "").Replace(@"""", "'");

            //ManualResetEvent SyncClientServer = (ManualResetEvent)obj;
            using (NamedPipeClientStream pipeStream = new NamedPipeClientStream("PipesOfPiece"))
            {
                pipeStream.Connect();

                //MessageBox.Show(“[Client] Pipe connection established”);
                using (StreamWriter sw = new StreamWriter(pipeStream))
                {
                    sw.AutoFlush = true;
                    sw.WriteLine("LuaEvent" + " " + Engine.Instance.mThread.Id + " " + Collection.UI.Base.UIBase.LuaState + " " + code + " " + code.Length);
                }
            }
        }

        public static void Chatting_SendMessage(long someValue, string message, ChatType chatType, ChatSystemType chatSystemType)
        {
            //ManualResetEvent SyncClientServer = (ManualResetEvent)obj;
            using (NamedPipeClientStream pipeStream = new NamedPipeClientStream("PipesOfPiece"))
            {
                pipeStream.Connect();

                //MessageBox.Show(“[Client] Pipe connection established”);
                using (StreamWriter sw = new StreamWriter(pipeStream))
                {
                    sw.AutoFlush = true;
                    sw.WriteLine("ChattingSendMessage" + " " + Engine.Instance.mThread.Id + " " + someValue + " " + message + " " + (int)chatType + " " + (int)chatSystemType);
                }
            }
        }

        public static void SellItemAtItemMarket(FromWhereType fromWhereType, int slotNo, int itemCount, long pricePerOne)
        {
            //ManualResetEvent SyncClientServer = (ManualResetEvent)obj;
            using (NamedPipeClientStream pipeStream = new NamedPipeClientStream("PipesOfPiece"))
            {
                pipeStream.Connect();

                //MessageBox.Show(“[Client] Pipe connection established”);
                using (StreamWriter sw = new StreamWriter(pipeStream))
                {
                    sw.AutoFlush = true;
                    sw.WriteLine("SellItemAtItemMarket" + " " + Engine.Instance.mThread.Id + " " + Offsets._marketBase + " " + (int)fromWhereType + " " + slotNo + " " + itemCount + " " + pricePerOne);
                }
            }
        }

        public static void GetItemCooldown(byte ItemWhereType, byte Slot)
        {
            //ManualResetEvent SyncClientServer = (ManualResetEvent)obj;
            using (NamedPipeClientStream pipeStream = new NamedPipeClientStream("PipesOfPiece"))
            {
                pipeStream.Connect();

                StreamReader reader = new StreamReader(pipeStream);
                StreamWriter writer = new StreamWriter(pipeStream);
                
                writer.WriteLine("GetItemCooldown" + " " + Engine.Instance.mThread.Id + " " + ItemWhereType + " " + Slot);
                writer.Flush();
                var ret = reader.ReadLine();
                float a = float.Parse(ret);
                
                Console.WriteLine(a);
            }
        }   //broken

        public static void useInventoryItem(FromWhereType fromWhereType, int inventorySlotNo)
        {
            //ManualResetEvent SyncClientServer = (ManualResetEvent)obj;
            using (NamedPipeClientStream pipeStream = new NamedPipeClientStream("PipesOfPiece"))
            {
                pipeStream.Connect();

                //MessageBox.Show(“[Client] Pipe connection established”);
                using (StreamWriter sw = new StreamWriter(pipeStream))
                {
                    sw.AutoFlush = true;
                    sw.WriteLine("UseInventoryItem" + " " + Engine.Instance.mThread.Id + " " + Collection.Actors.Local.PlayerData.Address + " " + (int)fromWhereType + " " + inventorySlotNo + " " + 31 + " " + 1 + " " + 1);
                }
            }
        }

        public static void buyItemFromItemMarketByMaid(FromWhereType moneyType, int itemId, int marketSlot, int itemCount)
        {
            int type = (int) moneyType;

            //ManualResetEvent SyncClientServer = (ManualResetEvent)obj;
            using (NamedPipeClientStream pipeStream = new NamedPipeClientStream("PipesOfPiece"))
            {
                pipeStream.Connect();

                //MessageBox.Show(“[Client] Pipe connection established”);
                using (StreamWriter sw = new StreamWriter(pipeStream))
                {
                    sw.AutoFlush = true;
                    sw.WriteLine("BuyItemFromMarketByMaid" + " " + Engine.Instance.mThread.Id + " " + Collection.ItemMarket.Base.ItemMarketList.Address + " " + moneyType + " " + itemId + " " + marketSlot + " " + itemCount);
                }
            }
        }   //broken

        public static void requestOpenItemmarket(int someType, uint someValue, int anotherType)
        {
            //ManualResetEvent SyncClientServer = (ManualResetEvent)obj;
            using (NamedPipeClientStream pipeStream = new NamedPipeClientStream("PipesOfPiece"))
            {
                pipeStream.Connect();

                //MessageBox.Show(“[Client] Pipe connection established”);
                using (StreamWriter sw = new StreamWriter(pipeStream))
                {
                    sw.AutoFlush = true;
                    sw.WriteLine("OpenItemMarket" + " " + Engine.Instance.mThread.Id + " " + someType + " " + someValue + " " + anotherType);
                }
            }
        }   //broken

        public static void lootingSlotClick(int slotNo, int count)
        {
            //ManualResetEvent SyncClientServer = (ManualResetEvent)obj;
            using (NamedPipeClientStream pipeStream = new NamedPipeClientStream("PipesOfPiece"))
            {
                pipeStream.Connect();

                //MessageBox.Show(“[Client] Pipe connection established”);
                using (StreamWriter sw = new StreamWriter(pipeStream))
                {
                    sw.AutoFlush = true;
                    sw.WriteLine("LootingSlotClick" + " " + Engine.Instance.mThread.Id + " " + slotNo + " " + count);
                }
            }
        }

        public static void lootingPickItems(int lootingType)
        {
            //ManualResetEvent SyncClientServer = (ManualResetEvent)obj;
            using (NamedPipeClientStream pipeStream = new NamedPipeClientStream("PipesOfPiece"))
            {
                pipeStream.Connect();

                //MessageBox.Show(“[Client] Pipe connection established”);
                using (StreamWriter sw = new StreamWriter(pipeStream))
                {
                    sw.AutoFlush = true;
                    sw.WriteLine("LootingPickItems" + " " + Engine.Instance.mThread.Id + " " + lootingType);
                }
            }
        }

        public static void popWarehouseToInventory(int warehouseItemSlot, int itemCount)
        {
            //ManualResetEvent SyncClientServer = (ManualResetEvent)obj;
            using (NamedPipeClientStream pipeStream = new NamedPipeClientStream("PipesOfPiece"))
            {
                pipeStream.Connect();

                //MessageBox.Show(“[Client] Pipe connection established”);
                using (StreamWriter sw = new StreamWriter(pipeStream))
                {
                    sw.AutoFlush = true;
                    sw.WriteLine("PopWarehouseToInventory" + " " + Engine.Instance.mThread.Id + " " + warehouseItemSlot + " " + itemCount + " " + Collection.Base.Values._Values.ItemMove_WarehouseToInventory);
                }
            }
        }

        public static void moveItemActorToActor(int actorKeyFrom, int actorKeyTo, int type, int slotNo, int itemCount)
        {
            //ManualResetEvent SyncClientServer = (ManualResetEvent)obj;
            using (NamedPipeClientStream pipeStream = new NamedPipeClientStream("PipesOfPiece"))
            {
                pipeStream.Connect();

                //MessageBox.Show(“[Client] Pipe connection established”);
                using (StreamWriter sw = new StreamWriter(pipeStream))
                {
                    sw.AutoFlush = true;
                    sw.WriteLine("MoveActorToActor" + " " + Engine.Instance.mThread.Id + " " + actorKeyFrom + " " + actorKeyTo + " " + type + " " + slotNo + " " + itemCount);
                }
            }
        }

        public static void TestFunction()
        {
            //ManualResetEvent SyncClientServer = (ManualResetEvent)obj;
            using (NamedPipeClientStream pipeStream = new NamedPipeClientStream("PipesOfPiece"))
            {
                pipeStream.Connect();

                //MessageBox.Show(“[Client] Pipe connection established”);
                using (StreamWriter sw = new StreamWriter(pipeStream))
                {
                    sw.AutoFlush = true;
                    sw.WriteLine("TestFunc");
                }
            }
        }

        public class NavigationGuideSetup
        {
            public static void setFGColor(float[] color)
            {
                //ManualResetEvent SyncClientServer = (ManualResetEvent)obj;
                using (NamedPipeClientStream pipeStream = new NamedPipeClientStream("PipesOfPiece"))
                {
                    pipeStream.Connect();

                    //MessageBox.Show(“[Client] Pipe connection established”);
                    using (StreamWriter sw = new StreamWriter(pipeStream))
                    {
                        sw.AutoFlush = true;
                        sw.WriteLine("setFGColor" + " " + Engine.Instance.mThread.Id + " " + color[0] + " " + color[1] + " " + color[2] + " " + color[3]);
                    }
                }
            }

            public static void setBGColor(float[] color)
            {
                //ManualResetEvent SyncClientServer = (ManualResetEvent)obj;
                using (NamedPipeClientStream pipeStream = new NamedPipeClientStream("PipesOfPiece"))
                {
                    pipeStream.Connect();

                    //MessageBox.Show(“[Client] Pipe connection established”);
                    using (StreamWriter sw = new StreamWriter(pipeStream))
                    {
                        sw.AutoFlush = true;
                        sw.WriteLine("setBGColor" + " " + Engine.Instance.mThread.Id + " " + color[0] + " " + color[1] + " " + color[2] + " " + color[3]);
                    }
                }
            }

            public static void setBeamColor(float[] color)
            {
                //ManualResetEvent SyncClientServer = (ManualResetEvent)obj;
                using (NamedPipeClientStream pipeStream = new NamedPipeClientStream("PipesOfPiece"))
                {
                    pipeStream.Connect();

                    //MessageBox.Show(“[Client] Pipe connection established”);
                    using (StreamWriter sw = new StreamWriter(pipeStream))
                    {
                        sw.AutoFlush = true;
                        sw.WriteLine("setBeamColor" + " " + Engine.Instance.mThread.Id + " " + color[0] + " " + color[1] + " " + color[2] + " " + color[3]);
                    }
                }
            }

            public static void setWidth(float width, float bgWidth)
            {
                //ManualResetEvent SyncClientServer = (ManualResetEvent)obj;
                using (NamedPipeClientStream pipeStream = new NamedPipeClientStream("PipesOfPiece"))
                {
                    pipeStream.Connect();

                    //MessageBox.Show(“[Client] Pipe connection established”);
                    using (StreamWriter sw = new StreamWriter(pipeStream))
                    {
                        sw.AutoFlush = true;
                        sw.WriteLine("setWidth" + " " + Engine.Instance.mThread.Id + " " + width + " " + bgWidth);
                    }
                }
            }

            public static void setBeamTime(float time)
            {
                //ManualResetEvent SyncClientServer = (ManualResetEvent)obj;
                using (NamedPipeClientStream pipeStream = new NamedPipeClientStream("PipesOfPiece"))
                {
                    pipeStream.Connect();

                    //MessageBox.Show(“[Client] Pipe connection established”);
                    using (StreamWriter sw = new StreamWriter(pipeStream))
                    {
                        sw.AutoFlush = true;
                        sw.WriteLine("setBeamTime" + " " + Engine.Instance.mThread.Id + " " + time);
                    }
                }
            }

            public static void setWorldMapColor(float[] color)
            {
                //ManualResetEvent SyncClientServer = (ManualResetEvent)obj;
                using (NamedPipeClientStream pipeStream = new NamedPipeClientStream("PipesOfPiece"))
                {
                    pipeStream.Connect();

                    //MessageBox.Show(“[Client] Pipe connection established”);
                    using (StreamWriter sw = new StreamWriter(pipeStream))
                    {
                        sw.AutoFlush = true;
                        sw.WriteLine("setWorldMapColor" + " " + Engine.Instance.mThread.Id + " " + color[0] + " " + color[1] + " " + color[2] + " " + color[3]);
                    }
                }
            }

            public static void setWorldMapBGColor(float[] color)
            {
                //ManualResetEvent SyncClientServer = (ManualResetEvent)obj;
                using (NamedPipeClientStream pipeStream = new NamedPipeClientStream("PipesOfPiece"))
                {
                    pipeStream.Connect();

                    //MessageBox.Show(“[Client] Pipe connection established”);
                    using (StreamWriter sw = new StreamWriter(pipeStream))
                    {
                        sw.AutoFlush = true;
                        sw.WriteLine("setWorldMapBGColor" + " " + Engine.Instance.mThread.Id + " " + color[0] + " " + color[1] + " " + color[2] + " " + color[3]);
                    }
                }
            }

            public static void setOtherParams(byte _isSetRenderPath, byte _isEternalBeam, int _questGroupNo, int _questNo, byte _isAutoErase, byte _isSafePath)
            {
                //ManualResetEvent SyncClientServer = (ManualResetEvent)obj;
                using (NamedPipeClientStream pipeStream = new NamedPipeClientStream("PipesOfPiece"))
                {
                    pipeStream.Connect();

                    //MessageBox.Show(“[Client] Pipe connection established”);
                    using (StreamWriter sw = new StreamWriter(pipeStream))
                    {
                        sw.AutoFlush = true;
                        sw.WriteLine("setOtherParams" + " " + Engine.Instance.mThread.Id + " " + _isSetRenderPath + " " + _isEternalBeam + " " + _questGroupNo + " " + _questNo + " " + _isAutoErase + " " + _isSafePath);
                    }
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpDX.Direct2D1;
using Factory = SharpDX.Direct2D1.Factory;
using FontFactory = SharpDX.DirectWrite.Factory;
using Format = SharpDX.DXGI.Format;
using SharpDX;
using SharpDX.DirectWrite;
using System.Threading;
using System.Runtime.InteropServices;
//using R3p.bdo.grinder;
using R3p.bdo.GameExternals.Structs.Overlay;
using R3p.bdo.GameInternals.Structs.Actors;
//using R3p.bdo.navigator;
using R3p.bdo.settings;
using SharpDX.Mathematics.Interop;
using Vector3 = R3p.bdo.Collection.NavMesh.Vector3;

namespace R3p.bdo.ui
{
    public partial class MainWindow : Form
    {
        private WindowRenderTarget device;
        private HwndRenderTargetProperties renderProperties;
        private SolidColorBrush solidColorBrush;
        private Factory factory;

        //text fonts
        private TextFormat font, fontSmall;
        private FontFactory fontFactory;
        private const string fontFamily = "Arial";//you can edit this of course
        private const float fontSize = 12.0f;
        private const float fontSizeSmall = 10.0f;
        private TextLayout textLayout;

        private IntPtr handle;
        private Thread sDX = null;
        //DllImports
        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        static extern bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("dwmapi.dll")]
        public static extern void DwmExtendFrameIntoClientArea(IntPtr hWnd, ref int[] pMargins);
        //Styles
        public const UInt32 SWP_NOSIZE = 0x0001;
        public const UInt32 SWP_NOMOVE = 0x0002;
        public const UInt32 TOPMOST_FLAGS = SWP_NOMOVE | SWP_NOSIZE;
        public static IntPtr HWND_TOPMOST = new IntPtr(-1);
        
        public MainWindow()
        {
            this.handle = Handle;
            int initialStyle = GetWindowLong(this.Handle, -20);
            SetWindowLong(this.Handle, -20, initialStyle | 0x80000 | 0x20);
            SetWindowPos(this.Handle, HWND_TOPMOST, 0, 0, 0, 0, TOPMOST_FLAGS);
            OnResize(null);

            this.Load += MainWindow_Load;

            InitializeComponent();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            this.DoubleBuffered = true;
            this.Width = 1920;// set your own size
            this.Height = 1080;
            this.Left = 0;
            this.Top = 0;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer |// this reduce the flicker
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.DoubleBuffer |
                ControlStyles.UserPaint |
                ControlStyles.Opaque |
                ControlStyles.ResizeRedraw |
                ControlStyles.SupportsTransparentBackColor, true);
            this.TopMost = true;
            this.Visible = true;

            factory = new Factory();
            fontFactory = new FontFactory();
            renderProperties = new HwndRenderTargetProperties()
            {
                Hwnd = this.Handle,
                PixelSize = new Size2(1920, 1080),
                PresentOptions = PresentOptions.None
            };

            //SetLayeredWindowAttributes(this.Handle, 0, 255, Managed.LWA_ALPHA);// caution directx error

            //Init DirectX
            device = new WindowRenderTarget(factory, new RenderTargetProperties(new PixelFormat(Format.B8G8R8A8_UNorm, AlphaMode.Premultiplied)), renderProperties);

            solidColorBrush = new SolidColorBrush(device, Color.White);
            // Init font's
            font = new TextFormat(fontFactory, fontFamily, FontWeight.Bold, FontStyle.Normal,  FontStretch.Normal, fontSize);
            fontSmall = new TextFormat(fontFactory, fontFamily, fontSizeSmall);
            textLayout = new TextLayout(fontFactory, "placeholder", font, 400f,200f);
            //line = new device.DrawLine;

            sDX = new Thread(new ParameterizedThreadStart(Draw));

            sDX.Priority = ThreadPriority.AboveNormal;
            sDX.IsBackground = true;
            sDX.Start();
        }

        protected override void OnPaint(PaintEventArgs e)// create the whole form
        {
            int[] marg = new int[] { 0, 0, Width, Height };
            DwmExtendFrameIntoClientArea(this.handle, ref marg);
        }

        public bool _draw = false;
        public bool _dispose = false;
        public bool Enabled;
        public bool GoldenChests;
        
        public List<ActorObject> settingsActors;

        private static PathGeometry pathGeometry;
        private static GeometrySink geometrySink;
        
        private void Draw(object sender)
        {
            while (Enabled)
            {
                
                while(!_draw)
                    Thread.Sleep(1);

                
                device.BeginDraw();
                device.Clear(Color.Transparent);
                device.TextAntialiasMode = SharpDX.Direct2D1.TextAntialiasMode.Aliased;

                if(isActiveWindow())
                    doDraws();

                device.EndDraw();

                _draw = false;
            }
        }

        private bool isActiveWindow()
        {
            return Win32.GetForegroundWindow() == Engine.Instance.Process.MainWindowHandle;
        }

        private void doDraws()
        {
            drawActors();
            drawStaticSpawns();
#if DEBUG
            //drawNavMesh();
            //drawDeadBodies();
            //drawLootableActors();
            //drawColorTestLines();
#endif

            this.Invoke(new MethodInvoker(delegate { this.TopMost = true; }));
        }

        private void drawColorTestLines()
        {
            for (int i = 0; i < 256; i++)
            {
                solidColorBrush.Color = new RawColor4((float)i/255, 0.5f, 0.0f, 1.0f);
                RenderingDrawLine(new int[]{0, i}, new int[]{1920, i}, new int[]{0, 0, 0, 0}, 1, false);
            }
        }

        private void drawDeadBodies()
        {
            foreach (var actor in Collection.Actors.Global.ActorList.Where(x => x.ActorType == ActorType.ActorType_Deadbody))
            {
                RenderingDrawSquare(actor.WorldPosition, 100, new int[] { 255, 255, 255, 255 }, 1,
                        Collection.World.Base.WorldMap.x0688_isOpened);
            }
        }

        private void drawLootableActors()
        {
            foreach (var actor in Collection.Actors.Global.ActorList)
            {
                if(actor.hasLoot)
                    RenderingDrawSquare(actor.WorldPosition, 100, new int[] { 255, 0, 0, 255 }, 1,
                        Collection.World.Base.WorldMap.x0688_isOpened);
            }
        }

        private void drawNavMesh()
        {
            //if (CheckpointManager.Instance.Checkpoints.Count > 0)
            //{
            //    foreach (var node in CheckpointManager.Instance.Checkpoints)
            //    {
            //        RenderingDrawSquare(node, 100, new int[] {255, 255, 255, 255}, 1,
            //            Collection.World.Base.WorldMap.x0688_isOpened);
            //    }

            //    RenderingDrawMultiLine(CheckpointManager.Instance.Checkpoints, new int[] {255, 255, 255, 255}, 1,
            //        Collection.World.Base.WorldMap.x0688_isOpened);
            //}

            //if (Grinder.Instance.Script != null)
            //{
            //    if (Grinder.Instance.Script.RomaingCheckpoints.Count > 0)
            //    {
            //        foreach (var node in Grinder.Instance.Script.RomaingCheckpoints)
            //        {
            //            RenderingDrawSquare(node, 100, new int[] { 0, 255, 0, 255 }, 1,
            //                Collection.World.Base.WorldMap.x0688_isOpened);
            //        }

            //        //RenderingDrawMultiLine(Grinder.Instance.Script.RomaingCheckpoints, new int[] { 0, 255, 0, 255 }, 1, Collection.World.Base.WorldMap.x0688_isOpened);
            //    }

            //    if (Grinder.Instance.Target != null)
            //    {
            //        foreach (var target in Grinder.Instance.Targets.Where(x => x.ActorKey != Grinder.Instance.Target.ActorKey))
            //        {
            //            RenderingDrawSquare(target.WorldPosition, 100, new int[] { 255, 255, 255, 255 }, 2, Collection.World.Base.WorldMap.x0688_isOpened, false);
            //        }

            //        RenderingDrawSquare(Grinder.Instance.Target.WorldPosition, 100, new int[] { 255, 0, 0, 255 }, 2, Collection.World.Base.WorldMap.x0688_isOpened, true);
            //    }
            //}
        }

        private bool isAvailableFilter(ActorData actor, out ActorObject filter)
        {
            filter = settingsActors.FirstOrDefault(x => x.Enabled && x.ActorType == ActorType.ActorType_All && x.ActorIds.Contains(actor.ActorId));

            if (filter != null)
                return true;

            filter = settingsActors.FirstOrDefault(x => x.Enabled && x.ActorType == ActorType.ActorType_All && x.ActorIds[0] == 0);

            if (filter != null)
                return true;

            filter = settingsActors.FirstOrDefault(x => x.Enabled && x.ActorType == actor.ActorType && x.ActorIds.Contains(actor.ActorId));

            if (filter != null)
                return true;

            filter = settingsActors.FirstOrDefault(x => x.Enabled && x.ActorType == actor.ActorType && x.ActorIds[0] == 0);

            if(filter != null)
                return true;

            return false;
        }

        private void drawStaticSpawns()
        {
            if (GoldenChests)
            {
                for (int i = 0; i < Collection.StaticSpawnData.GoldenChests.List.Count; i++)
                {
                    if(!Collection.World.Base.WorldMap.x0688_isOpened && Helpers.World.DistanceHelper.getDistance(Collection.Actors.Local.PlayerData.WorldPosition, Collection.StaticSpawnData.GoldenChests.List[i]) > 100000)
                        continue;

                    RenderingDrawTriangleWithText(Collection.StaticSpawnData.GoldenChests.List[i], "GoldenChest", 50f, 1.0, new int[]{255,127,0,255}, 1, Collection.World.Base.WorldMap.x0688_isOpened, true);
                }
            }

            if (Settings.Overlay.Waypoints.Count > 0)
            {
                foreach (var waypoint in Settings.Overlay.Waypoints)
                {
                    RenderingDrawTriangleWithText(waypoint.Position, waypoint.Name, 50f, 1.0, waypoint.ColorCode, waypoint.Thickness, Collection.World.Base.WorldMap.x0688_isOpened);
                }
            }
        }

        private void drawActors()
        {
            for (int i = Collection.Actors.Global.ActorList.Length - 1; i >= 0; i--)
            {
                var actor = Collection.Actors.Global.ActorList[i];
                ActorObject filter;

                if (!isAvailableFilter(actor, out filter))
                    continue;

                if(filter.ActorType != ActorType.ActorType_Deadbody && actor.isDead)
                    continue;

                if (filter.MinDistance != 0 && actor.Distance < filter.MinDistance)
                    continue;

                if (filter.MaxDistance != 0 && actor.Distance > filter.MaxDistance)
                    continue;

                if (!filter.ShowOnWorldMap && Collection.World.Base.WorldMap.x0688_isOpened)
                    continue;

                int[] ScreenPosition;
                bool drawWorldMap = Collection.World.Base.WorldMap.x0688_isOpened && filter.ShowOnWorldMap;

                if(!actor.IsOnScreen(drawWorldMap, out ScreenPosition))
                    continue;
                
                if(filter.DrawLine)
                    RenderingDrawLine(ScreenPosition, filter.ColorCode, 1f, drawWorldMap);

                if(filter.DrawCircle)
                    RenderingDrawCircle(actor.WorldPosition, 50, 1, filter.ColorCode, 1, drawWorldMap);

                if(filter.ShowName || filter.ShowActorId || filter.ShowDistance || filter.ShowLevel || filter.ShowHp)
                    RenderingDrawText(ScreenPosition, filter.ColorCode, actor, filter.ShowName, filter.ShowActorId, filter.ShowDistance, filter.ShowLevel, filter.ShowHp, drawWorldMap);
            }
        }

        private void ChangeColor(int[] color)
        {
            solidColorBrush.Color = new RawColor4((float)color[0]/255, (float)color[1]/255, (float)color[2]/255, (float)color[3]/255);
        }

        private void RenderingDrawLine(int[] sDest, int[] color, float thickness, bool drawWorldMap)
        {
            int[] sOri;
            var onScreen = Collection.Actors.Local.PlayerData.IsOnScreen(drawWorldMap, out sOri);
            
            ChangeColor(color);
            
            device.DrawLine(new RawVector2(sOri[0], sOri[1]), new RawVector2(sDest[0], sDest[1]), solidColorBrush, thickness);
        }

        private void RenderingDrawLine(int[] sOri, int[] sDest, int[] color, float thickness, bool drawWorldMap)
        {
            ChangeColor(color);

            device.DrawLine(new RawVector2(sOri[0], sOri[1]), new RawVector2(sDest[0], sDest[1]), solidColorBrush, thickness);
        }

        private void RenderingDrawCircle(float[] center, float radius, double percentage, int[] color, int thickness,bool drawWorldMap, bool fill = false)
        {
            if (drawWorldMap)
                radius = radius*10;

            double segments = 20;

            if (drawWorldMap)
                segments /= 2;
            
            RawVector2[] pts = new RawVector2[(int)segments + 1];

            double angle = 0;
            double segmentSize = (360*percentage) / segments;

            int[] sCenter;
            if (!Helpers.ViewTransform.ToScreen.WorldToScreen(center, out sCenter, drawWorldMap))
                return;

            int count = 0;

            if (percentage < 1)
            {
                pts = new RawVector2[(int)segments + 3];

                pts[0] = new RawVector2(sCenter[0], sCenter[1]);

                count = 1;
            }

            while (angle <= (360*percentage))
            {
                float X = (float)(center[0] + (radius * Math.Cos(angle / (180 / Math.PI))));
                float Z = (float)(center[2] + (radius * Math.Sin(angle / (180 / Math.PI))));

                int[] screen;
                if (Helpers.ViewTransform.ToScreen.WorldToScreen(new float[] { X, center[1], Z }, out screen, drawWorldMap))
                    pts[count] = new RawVector2(screen[0], screen[1]);
                else
                {
                    return;
                }

                angle += segmentSize;
                count++;
            }

            if (percentage < 1)
                pts[pts.Length - 1] = pts[0];
            
            pathGeometry = new PathGeometry(factory);
            geometrySink = pathGeometry.Open();

            geometrySink.BeginFigure(pts[0], new FigureBegin());
            geometrySink.AddLines(pts);
            geometrySink.EndFigure(new FigureEnd());
            geometrySink.Close();

            ChangeColor(color);

            device.DrawGeometry(pathGeometry, solidColorBrush);

            if(fill)
                device.FillGeometry(pathGeometry, solidColorBrush);

            pathGeometry.Dispose();
            geometrySink.Dispose();
        }

        private void RenderingDrawTriangle(float[] center, float radius, double percentage, int[] color, int thickness, bool drawWorldMap, bool fill = false)
        {
            if (drawWorldMap)
                radius = radius * 20;

            double segments = 3;
            
            RawVector2[] pts = new RawVector2[(int)segments + 1];

            double angle = 0;
            double segmentSize = (360 * percentage) / segments;

            int[] sCenter;
            if (!Helpers.ViewTransform.ToScreen.WorldToScreen(center, out sCenter, drawWorldMap))
                return;

            int count = 0;

            if (percentage < 1)
            {
                pts = new RawVector2[(int)segments + 3];

                pts[0] = new RawVector2(sCenter[0], sCenter[1]);

                count = 1;
            }

            while (angle <= (360 * percentage))
            {
                float X = (float)(center[0] + (radius * Math.Cos(angle / (180 / Math.PI))));
                float Z = (float)(center[2] + (radius * Math.Sin(angle / (180 / Math.PI))));

                int[] screen;
                if (Helpers.ViewTransform.ToScreen.WorldToScreen(new float[] { X, center[1], Z }, out screen, drawWorldMap))
                    pts[count] = new RawVector2(screen[0], screen[1]);
                else
                {
                    return;
                }

                angle += segmentSize;
                count++;
            }

            if (percentage < 1)
                pts[pts.Length - 1] = pts[0];

            //if (pts.Any(x => x.X == 0 || x.Y == 0))
            //return;

            pathGeometry = new PathGeometry(factory);
            geometrySink = pathGeometry.Open();

            geometrySink.BeginFigure(pts[0], new FigureBegin());
            geometrySink.AddLines(pts);
            geometrySink.EndFigure(new FigureEnd());
            geometrySink.Close();

            ChangeColor(color);

            device.DrawGeometry(pathGeometry, solidColorBrush);

            if (fill)
                device.FillGeometry(pathGeometry, solidColorBrush);

            pathGeometry.Dispose();
            geometrySink.Dispose();
        }

        private void RenderingDrawTriangleWithText(float[] center, string text, float radius, double percentage, int[] color, int thickness, bool drawWorldMap, bool fill = false)
        {
            if (drawWorldMap)
                radius = radius * 20;

            double segments = 3;

            RawVector2[] pts = new RawVector2[(int)segments + 1];

            double angle = 0;
            double segmentSize = (360 * percentage) / segments;

            int[] sCenter;
            if (!Helpers.ViewTransform.ToScreen.WorldToScreen(center, out sCenter, drawWorldMap))
                return;

            int count = 0;

            if (percentage < 1)
            {
                pts = new RawVector2[(int)segments + 3];

                pts[0] = new RawVector2(sCenter[0], sCenter[1]);

                count = 1;
            }

            int[] screen = null;

            while (angle <= (360 * percentage))
            {
                float X = (float)(center[0] + (radius * Math.Cos(angle / (180 / Math.PI))));
                float Z = (float)(center[2] + (radius * Math.Sin(angle / (180 / Math.PI))));

                
                if (Helpers.ViewTransform.ToScreen.WorldToScreen(new float[] { X, center[1], Z }, out screen, drawWorldMap))
                    pts[count] = new RawVector2(screen[0], screen[1]);
                else
                {
                    return;
                }

                angle += segmentSize;
                count++;
            }

            if (percentage < 1)
                pts[pts.Length - 1] = pts[0];

            //if (pts.Any(x => x.X == 0 || x.Y == 0))
            //return;

            pathGeometry = new PathGeometry(factory);
            geometrySink = pathGeometry.Open();

            geometrySink.BeginFigure(pts[0], new FigureBegin());
            geometrySink.AddLines(pts);
            geometrySink.EndFigure(new FigureEnd());
            geometrySink.Close();

            ChangeColor(color);

            device.DrawGeometry(pathGeometry, solidColorBrush);

            if (fill)
                device.FillGeometry(pathGeometry, solidColorBrush);

            pathGeometry.Dispose();
            geometrySink.Dispose();

            RenderingDrawText(screen, color, text, drawWorldMap);
        }

        private void RenderingDrawSquare(float[] center, float squaresize, int[] color, int thickness, bool drawWorldMap, bool fill = false)
        {
            if (drawWorldMap)
                squaresize = squaresize * 1f;
            
            RawVector2[] pts = new RawVector2[4];

            bool draw = false;

            int[] screen;
            float[] buffer = center.ToArray();
            buffer[0] -= squaresize/2;
            buffer[2] -= squaresize/2;
            if (Helpers.ViewTransform.ToScreen.WorldToScreen(buffer, out screen, drawWorldMap))
            {
                pts[0] = new RawVector2(screen[0], screen[1]);

                buffer = center.ToArray();
                buffer[0] += squaresize / 2;
                buffer[2] -= squaresize / 2;

                if (Helpers.ViewTransform.ToScreen.WorldToScreen(buffer, out screen, drawWorldMap))
                {
                    pts[1] = new RawVector2(screen[0], screen[1]);

                    buffer = center.ToArray();
                    buffer[0] += squaresize / 2;
                    buffer[2] += squaresize / 2;

                    if (Helpers.ViewTransform.ToScreen.WorldToScreen(buffer, out screen, drawWorldMap))
                    {
                        pts[2] = new RawVector2(screen[0], screen[1]);

                        buffer = center.ToArray();
                        buffer[0] -= squaresize / 2;
                        buffer[2] += squaresize / 2;

                        if (Helpers.ViewTransform.ToScreen.WorldToScreen(buffer, out screen, drawWorldMap))
                        {
                            pts[3] = new RawVector2(screen[0], screen[1]);

                            draw = true;
                        }
                    }
                }
            }


            if (!draw)
                return;

            pathGeometry = new PathGeometry(factory);
            geometrySink = pathGeometry.Open();

            geometrySink.BeginFigure(pts[0], new FigureBegin());
            geometrySink.AddLines(new RawVector2[] {pts[1], pts[2], pts[3], pts[0]});
            geometrySink.EndFigure(new FigureEnd());
            geometrySink.Close();

            ChangeColor(color);

            device.DrawGeometry(pathGeometry, solidColorBrush);

            if (fill)
                device.FillGeometry(pathGeometry, solidColorBrush);

            pathGeometry.Dispose();
            geometrySink.Dispose();
        }

        private void RenderingDrawMultiLine(List<float[]> points, int[] color, int thickness, bool drawWorldMap)
        {
            
            RawVector2[] pts = new RawVector2[points.Count + 1];
            
            for (int i = 0; i < points.Count; i++)
            {
                int[] screen;
                Helpers.ViewTransform.ToScreen.WorldToScreen(points[i], out screen, drawWorldMap);

                pts[i] = new RawVector2(screen[0], screen[1]);
            }

            pts[pts.Length - 1] = pts[0];

            pathGeometry = new PathGeometry(factory);
            geometrySink = pathGeometry.Open();

            geometrySink.BeginFigure(pts[0], new FigureBegin());
            geometrySink.AddLines(pts);
            geometrySink.EndFigure(new FigureEnd());
            geometrySink.Close();

            ChangeColor(color);

            device.DrawGeometry(pathGeometry, solidColorBrush);
            
            pathGeometry.Dispose();
            geometrySink.Dispose();
        }

        private void MainWindow_Load_1(object sender, EventArgs e)
        {

        }

        private void RenderingDrawText(int[] pos, int[] color, ActorData actor, bool showName, bool showActorId, bool showDistance, bool showLevel, bool showHp, bool drawWorldMap)
        {
            ChangeColor(color);

            string text = "";
            if (showName)
                text += actor.Name + "\n";
            if (showActorId)
                text += "ActorId: " + actor.ActorId + "\n";
            if (showDistance)
                text += "Distance: " + actor.Distance.ToString("0") + "\n";
            if (showLevel)
                text += "Level: " + actor.Level + "\n";
            if (showHp)
                text += "HP: " + actor.CurHitpoints.ToString("0") + " / " + actor.MaxHitpoints.ToString("0");

            device.DrawText(text, font, new RawRectangleF(pos[0], pos[1], pos[0] + 200, pos[1] + 50), solidColorBrush, DrawTextOptions.None);
        }

        private void RenderingDrawText(int[] pos, int[] color, string text, bool drawWorldMap)
        {
            ChangeColor(color);

            device.DrawText(text, font, new RawRectangleF(pos[0], pos[1], pos[0] + 200, pos[1] + 50), solidColorBrush, DrawTextOptions.None);
        }
    }
}

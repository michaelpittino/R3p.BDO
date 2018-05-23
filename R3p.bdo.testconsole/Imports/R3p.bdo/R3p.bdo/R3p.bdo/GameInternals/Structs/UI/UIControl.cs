using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using R3p.bdo.Memory;

namespace R3p.bdo.GameInternals.Structs.UI
{
    public class UIControl : MemoryObject
    {
        public UIControl(long address)
        {
            Address = address;
        }

        public UIControl x0008_PTR_Parent1 => new UIControl(ReadPointer8b(0x0008));
        public UIControl x0010_PTR_Parent2 => new UIControl(ReadPointer8b(0x0010));
        public string x0018_Name => GetName();
        public List<UIControl> Children => GetChilds(); 
        public int x0038_Key => ReadInt32(0x0038);
        public byte x003C_isNotFront => ReadByte(0x003C);
        public byte x003D_State => ReadByte(0x003D);
        public byte x0041_State => ReadByte(0x0041);
        public byte x0044_InteractState => ReadByte(0x0044);
        public int x0048_Depth => ReadInt32(0x0048);
        public float x004C_SizeX => ReadFloat(0x004C);
        public float x0050_SizeY => ReadFloat(0x0050);
        public float[] x0054_ScreenPos => ReadVec2(0x0054);
        public float x005C_SizeX => ReadFloat(0x005C);
        public float x0060_SizeY => ReadFloat(0x0060);
        public byte x0066_CheckState => ReadByte(0x0066);
        public float x0090_Alpha => ReadFloat(0x0090);
        //public Texture x00A8_PTR_BaseTexture => new Texture(0x00A8);
        public float x00C8_ScaleX => ReadFloat(0x00C8);
        public float x00CC_ScaleY => ReadFloat(0x00CC);
        public string x00D0_IconPath => ReadStringASCII(0x00D0);
        //public Texture x01F8_PTR_OnTexture => new Texture(0x01F8);
        //public Texture x0200_PTR_ClickTexture => new Texture(0x0200);
        //public Texture x0208_PTR_MaskingTexture => new Texture(0x0208);
        public float x0224_ScaleMin => ReadFloat(0x0224);
        public float x022C_EnableStart => ReadFloat(0x022C);
        public float x0234_EnableEnd => ReadFloat(0x0234);
        public float x023C_SpanSize => ReadFloat(0x023C);
        public float x0254_ScaleX => ReadFloat(0x0254);
        public float x0258_ScaleY => ReadFloat(0x0258);
        public float x026C_SizeXOrigin => ReadFloat(0x026C);
        public float x0270_SizeYOrigin => ReadFloat(0x0270);
        public long x0288_ListChildControls => ReadPointer8b(0x1A8);
        public long x0290_ListChildControlsEnd => ReadPointer8b(0x1B0);
        public byte x032B_AlphaIgnore => ReadByte(0x032B);
        public float x0338_WorldPosX => ReadFloat(0x0338);
        public float x033C_WorldPosY => ReadFloat(0x033C);
        public float x0340_WorldPosZ => ReadFloat(0x0340);
        public byte IsNotInteractable => ReadByte(0x0328);
        public float x0344_3DRotationX => ReadFloat(0x0344);
        public float x0348_3DRotationY => ReadFloat(0x0348);
        public int x0348_CoolDown => ReadInt32(0x0348);
        public float x034C_3DRotationZ => ReadFloat(0x034C);
        public float x0350_3DOffsetZ => ReadFloat(0x0350);
        public byte x0354_Render3DType => ReadByte(0x0354);
        public byte x0355_IsVerticalMode => ReadByte(0x0355);
        //public Button x0358_PTR_Button => new Button(0x0358);
        //public Button x0370_PTR_Button => new Button(0x0370);
        public byte x0378_IsDragEnable => ReadByte(0x0378);
        public byte x03A4_IsMaskingChild => ReadByte(0x03A4);
        public byte x03A5_IsFlushable => ReadByte(0x03A5);
        public string x03B0_Text => GetText();
        //public EventFunc x03C8_PTR_ShowEventFunc => new EventFunc(0x03C8);
        //public EventFunc x03E8_PTR_HideEventFunc => new EventFunc(0x03E8);
        public int x0438_TextSizeX => ReadInt32(0x0438);
        public int x043C_TextSizeY => ReadInt32(0x043C);
        public float x0454_FontAlpha => ReadFloat(0x0454);
        public int x0468_OverFontColor => ReadInt32(0x0468);
        public long UpdateFunction => ReadInt64(0x0488);
        public int x048C_TextSpanY => ReadInt32(0x048C);
        public byte x04D8_IsChecked => ReadByte(0x04D8);
        public int x04E0_MAxInput => ReadInt32(0x04E0);
        //public EventFunc x04F0_PTR_ShowPreUpdateFunc => new EventFunc(0x04F0);
        public int x0518_EditTextSize => ReadInt32(0x0518);
        public byte x052A_isIgnoreFlashPanel => ReadByte(0x052A);
        public byte x052B_isApplyGlobalScale => ReadByte(0x052B);
        public int x0530_ConventionalInputMode => ReadInt32(0x0530);
        public int x0534_IsUse => ReadInt32(0x0534);
        public byte x0548_IsSafeMode => ReadByte(0x0548);
        public byte x0549_IsNumberInputMode => ReadByte(0x0549);
        public byte x054D_IsAutoLineFeed => ReadByte(0x054D);
        public int x056C_CursorPosIndex => ReadInt32(0x056C);
        public byte x0580_IsCursorMove => ReadByte(0x0580);
        //public EventFunc x05A8_PTR_HitReturnEvent => new EventFunc(0x05A8);
        public int x05B0_MaxEditLine => ReadInt32(0x05B0);

        public void SetUpdateFunction(long value)
        {
            Write(0x488, BitConverter.GetBytes(value));
        }

        public bool IsInteractable()
        {
            return IsNotInteractable == 0;
        }

        public void SetInteractable()
        {
            Write(0x0328, BitConverter.GetBytes((byte)0));
        }

        //public bool isMouseOver()
        //{
        //    //return x003C_isNotFront == 65 || x003C_isNotFront == 193 || x003C_isNotFront == 1 || isMouseLeftBtnDown();
        //    return Ui._UiBase.x0188_HoveredUIControl.x0038_Key == x0038_Key;
        //}

        public bool isMouseLeftBtnDown()
        {
            return x0044_InteractState == 32;
        }

        public bool isMouseRightBtnDown()
        {
            return x0044_InteractState == 64;
        }

        public bool isChecked()
        {
            if (x0066_CheckState == 57)
                return false;

            return true;
        }

        public bool isVisible()
        {
            if (x003D_State == 12)
                return false;

            if (x003D_State == 8 || x003D_State == 72)
                return true;

            return false;
        }

        public void SetVisible()
        {
            Write(0x3D, BitConverter.GetBytes((byte)8));
        }

        public void SetScreenPos(float[] pos)
        {
            byte[] p = new byte[8];
            Array.Copy(BitConverter.GetBytes(pos[0]), 0, p, 0, 4);
            Array.Copy(BitConverter.GetBytes(pos[1]), 0, p, 4, 4);

            Write(0x54, p);
        }

        private List<UIControl> _bChildren;
        private List<UIControl> GetChilds()
        {
            if(_bChildren != null)
                return _bChildren;

            List<UIControl> childs = new List<UIControl>();

            //int unvalidCount = 0;

            //for (int i = 0; i < 1000; i++)
            //{
            //    if (unvalidCount > 10)
            //        break;

            //    UIControl u = new UIControl(x0288_ListChildControls + (i * 0x08));

            //    if (!u.isValid())
            //    {
            //        unvalidCount++;
            //        continue;
            //    }

            //    if (!u.hasParent())
            //    {
            //        unvalidCount++;
            //        continue;
            //    }

            //    if(u.isValid())
            //        childs.Add(u);
            //}

            long adr = x0288_ListChildControls;
            long count = (x0290_ListChildControlsEnd - x0288_ListChildControls) / 8;

            for (int i = 0; i < count; i++)
            {
                UIControl child = new UIControl(ReadPointer8b(adr + (i * 0x08)));

                //if(child.isValid())
                childs.Add(child);
            }

            if (_bChildren == null)
                _bChildren = childs;

            return childs;
        }

        public bool isValid()
        {
            return x003D_State == 8 || x003D_State == 12;
        }

        public bool isChild()
        {
            return x0038_Key != x0008_PTR_Parent1.x0038_Key;
        }

        public bool hasParent()
        {
            return x0008_PTR_Parent1.x0038_Key != 0 || x0010_PTR_Parent2.x0038_Key != 0;
        }

        public float[] GetScreenPos()
        {
            if (!isChild())
                return x0054_ScreenPos;

            float[] v = new float[] {x0054_ScreenPos[0], x0054_ScreenPos[1]};

            float offsetX = 0;
            float offsetY = 0;

            UIControl t = this;
            UIControl p = this.x0008_PTR_Parent1;

            for (int i = 0; i < 1000; i++)
            {
                if (t.x0038_Key == p.x0038_Key)
                    break;

                offsetX += p.x0054_ScreenPos[0];
                offsetY += p.x0054_ScreenPos[1];

                t = t.x0008_PTR_Parent1;
                p = t.x0008_PTR_Parent1;
            }

            v[0] += offsetX;
            v[1] += offsetY;

            return v;
        }

        private string _bName;
        public string GetName()
        {
            if (_bName == null)
            {
                _bName = ReadStringASCII(ReadPointer8b(0x0018));

                if (_bName.Length == 0)
                    _bName = ReadStringASCII(0x0018);
            }

            return _bName;
        }

        private string _bText;
        public string GetText()
        {
            _bText = ReadStringUnicode(0x0360);

                if (_bText.Length == 0)
                    _bText = ReadStringUnicode(ReadPointer8b(0x0360));
            

            return _bText;
        }

        public void SetText(string text)
        {
            Write(ReadPointer8b(0x378), Encoding.Unicode.GetBytes(text));
        }
    }
}

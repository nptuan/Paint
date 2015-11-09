using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Paint
{
    abstract class IHanhDong
    {
        public virtual void Undo(Canvas canvas) { }
        public virtual void Redo(Canvas canvas) { }
    }
    class HanhDongThem : IHanhDong
    {
        UIElement uiElement;

        public HanhDongThem(UIElement uie)
        {
            uiElement = uie;
        }
        public override void Undo(Canvas canvas)
        {      
            canvas.Children.Remove(uiElement);
        }
        public override void Redo(Canvas canvas)
        {
            canvas.Children.Add(uiElement);
        }
    }

    class HanhDongXoa : IHanhDong
    {
        UIElement uiElement;

        public HanhDongXoa(UIElement uie)
        {
            uiElement = uie;
        }
        public override void Undo(Canvas canvas)
        {
            canvas.Children.Add(uiElement);
        }
        public override void Redo(Canvas canvas)
        {
            canvas.Children.Remove(uiElement);
        }
    }

    class HanhDongChuoi : IHanhDong
    {
        List<IHanhDong> chuoiHanhDong;

        public HanhDongChuoi(List<IHanhDong> chd)
        {
            chuoiHanhDong = chd;
        }
        public override void Undo(Canvas canvas)
        {
            foreach ( IHanhDong hd in chuoiHanhDong)
            {
                hd.Undo( canvas);
            }                
        }
        public override void Redo(Canvas canvas)
        {
            foreach (IHanhDong hd in chuoiHanhDong)
            {
                hd.Redo(canvas);
            }
        }
    }


    class UndoRedoManager
    {
        List<IHanhDong> UndoList = new List<IHanhDong>();
        List<IHanhDong>  RedoList = new List<IHanhDong>();

        public void Add( IHanhDong hanhdong)
        {
            UndoList.Add(hanhdong);
            RedoList.Clear();
        }

        public void Undo( Canvas canvas)
        {
            if (UndoList.Count == 0)
            {
                return;
            }
            IHanhDong temp = UndoList.Last();
            UndoList.Remove(temp);
            RedoList.Add(temp);
            temp.Undo(canvas);
        }

        public void Redo( Canvas canvas)
        {
            if (RedoList.Count == 0)
            {
                return;
            }
            IHanhDong temp = RedoList.Last();
            RedoList.Remove(temp);
            UndoList.Add(temp);
            temp.Redo(canvas);
        }

    }
}

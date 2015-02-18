using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Table : Cell
    {
        Random generator = new Random();
        protected int Theight;
        private int BOMBCOUNT = 10;
        protected int Twidth;
        protected Cell[,] CellArray;
        int CellsLeft;
        public string buf;

        public Table() { InitializeComponent(); }

        public Table(int BombCount, int width, int height, int wd, int ht)
        {
            BOMBCOUNT = BombCount;
            Theight = ht;
            Twidth = wd;
            this.Height = height-38;
            this.Width = width-15;
            int height_buffer = this.Height;
            int width_buffer = this.Width;
            CellArray = new Cell[ht, wd];
            CellsLeft = wd * ht;
            buf = "";

            while ((height_buffer) % Theight != 0)
            {
                --height_buffer;
            }
            while ((width_buffer) % Twidth != 0)
            {
                --width_buffer;
            }

            int vertical = (this.Height - height_buffer)/2;
            int horizontal = (this.Width - width_buffer)/2;

            for (int i = 0; i < Theight; ++i)
            {
                int j = 0;
                for (; j < Twidth; ++j)
                {
                    CellArray[i, j] = new Cell();
                    CellArray[i, j].Height = height_buffer / Theight;
                    CellArray[i, j].Width = width_buffer / Twidth;
                    CellArray[i, j].Location = new Point(horizontal, vertical);
                    //CellArray[i, j].LClick += new LeftClick(bfs);
                    //this.Controls.Add(CellArray[i, j]);
                    horizontal = horizontal + CellArray[i, j].Width;
                }
                --j;
                vertical = vertical + CellArray[i, j].Height;
                horizontal = (this.Width - width_buffer)/2;
            }
            Generator(CellArray, BOMBCOUNT);
        }

        private void Generator(Cell[,] Array, int count)
        {
            this.Controls.Clear();

            int counter = 0;
            int X, Y;
            Cell temp;
            while (counter < count)
            {
                X = generator.Next(Array.GetUpperBound(0) + 1);
                Y = generator.Next(Array.GetUpperBound(1) + 1);

                if (!Array[X, Y].Bomb)
                {
                    temp = Array[X, Y];
                    Array[X, Y] = new Cell(-1, X, Y);
                    Array[X, Y].Height = temp.Height;
                    Array[X, Y].Width = temp.Width;
                    Array[X, Y].Location = temp.Location;
                    Array[X, Y].LClick +=new LeftClick(bfs);
                    Array[X, Y].Explosion += new OnExpo(Table_Explosion);
                    Array[X, Y].TwoButtonsClick += new DoubCli(Table_TwoButtonsClick);
                    Array[X, Y].ButtonsUp += new TwoButtonsUp(Table_ButtonsUp);
                    Array[X, Y].CBC += new CellVisibleChanged(Table_CBC);
                    Array[X, Y].ProgressMade += new ProgressMadeDel(Table_ProgressMade);
                    this.Controls.Add(CellArray[X, Y]);
                    counter++;
                }
            }

            for (int i = 0; i < Array.GetUpperBound(0) + 1; i++)
            {
                for (int j = 0; j < Array.GetUpperBound(1) + 1; j++)
                {
                    counter = 0;
                    temp = Array[i, j];
                    if (!Array[i, j].Bomb)
                    {
                        if ((i - 1 >= 0) && (j - 1 >= 0) && Array[i - 1, j - 1].Bomb)
                        {
                            counter++;
                        }
                        if ((i - 1 >= 0) && Array[i - 1, j].Bomb)
                        {
                            counter++;
                        }
                        if ((i - 1 >= 0) && (j + 1 < Array.GetUpperBound(1) + 1) && Array[i - 1, j + 1].Bomb)
                        {
                            counter++;
                        }
                        if ((j - 1 >= 0) && Array[i, j - 1].Bomb)
                        {
                            counter++;
                        }
                        if ((j + 1 < Array.GetUpperBound(1) + 1) && Array[i, j + 1].Bomb)
                        {
                            counter++;
                        }
                        if ((i + 1 < Array.GetUpperBound(0) + 1) && (j - 1 >= 0) && Array[i + 1, j - 1].Bomb)
                        {
                            counter++;
                        }
                        if ((i + 1 < Array.GetUpperBound(0) + 1) && Array[i + 1, j].Bomb)
                        {
                            counter++;
                        }
                        if ((i + 1 < Array.GetUpperBound(0) + 1) && (j + 1 < Array.GetUpperBound(1) + 1) && Array[i + 1, j + 1].Bomb)
                        {
                            counter++;
                        }
                        Array[i, j] = new Cell(counter, i, j);
                        Array[i, j].Height = temp.Height;
                        Array[i, j].Width = temp.Width;
                        Array[i, j].Location = temp.Location;
                        Array[i, j].LClick += new LeftClick(bfs);
                        Array[i, j].Explosion += new OnExpo(Table_Explosion);
                        Array[i, j].TwoButtonsClick += new DoubCli(Table_TwoButtonsClick);
                        Array[i, j].ButtonsUp += new TwoButtonsUp(Table_ButtonsUp);
                        Array[i, j].CBC += new CellVisibleChanged(Table_CBC);
                        Array[i, j].ProgressMade += new ProgressMadeDel(Table_ProgressMade);
                        this.Controls.Add(CellArray[i, j]);                        
                    }                
                }                
            }
            //нужно в cell добавить показатель количества бомб, иначе генерация не получается.

            //ChangeSize(this.Width, this.Height);
        }

        void Table_ProgressMade(int XVM, int YVM, int Par)
        {
            buf += XVM.ToString() + " " + YVM.ToString() + " " + Par.ToString() + "|";
        }

        void Table_CBC()
        {
            if (StopEnding)
            {
                CellsLeft--;
                if ((CellsLeft == BOMBCOUNT) && (BattleIsEnded != null))
                    BattleIsEnded(1);
            }
        }

        void Table_ButtonsUp(int XVM, int YVM)
        {
            Int32 i = XVM, j = YVM;
            if ((i - 1 >= 0) && (j - 1 >= 0))
                CellArray[i - 1, j - 1].DoubleCLickPB.Visible = false;

            if ((i - 1 >= 0))
                CellArray[i - 1, j].DoubleCLickPB.Visible = false;

            if ((i - 1 >= 0) && (j + 1 < CellArray.GetUpperBound(1) + 1))
                CellArray[i - 1, j + 1].DoubleCLickPB.Visible = false;

            if ((j + 1 < CellArray.GetUpperBound(1) + 1))
                CellArray[i, j + 1].DoubleCLickPB.Visible = false;

            if ((i + 1 <= CellArray.GetUpperBound(0)) && (j + 1 < CellArray.GetUpperBound(1) + 1))
                CellArray[i + 1, j + 1].DoubleCLickPB.Visible = false;

            if ((i + 1 <= CellArray.GetUpperBound(0)))
                CellArray[i + 1, j].DoubleCLickPB.Visible = false;

            if ((i + 1 <= CellArray.GetUpperBound(0)) && (j - 1 >= 0))
                CellArray[i + 1, j - 1].DoubleCLickPB.Visible = false;

            if ((j - 1 >= 0))
                CellArray[i, j - 1].DoubleCLickPB.Visible = false;
        }

        void Table_TwoButtonsClick(int XVM, int YVM)
        {
            Int32 counter = 0;
            Int32 i = XVM, j = YVM;

            if ((i - 1 >= 0) && (j - 1 >= 0) && CellArray[i - 1, j - 1].Flag)
            {
                counter++;
            }
            if ((i - 1 >= 0) && CellArray[i - 1, j].Flag)
            {
                counter++;
            }
            if ((i - 1 >= 0) && (j + 1 < CellArray.GetUpperBound(1) + 1) && CellArray[i - 1, j + 1].Flag)
            {
                counter++;
            }
            if ((j - 1 >= 0) && CellArray[i, j - 1].Flag)
            {
                counter++;
            }
            if ((j + 1 < CellArray.GetUpperBound(1) + 1) && CellArray[i, j + 1].Flag)
            {
                counter++;
            }
            if ((i + 1 < CellArray.GetUpperBound(0) + 1) && (j - 1 >= 0) && CellArray[i + 1, j - 1].Flag)
            {
                counter++;
            }
            if ((i + 1 < CellArray.GetUpperBound(0) + 1) && CellArray[i + 1, j].Flag)
            {
                counter++;
            }
            if ((i + 1 < CellArray.GetUpperBound(0) + 1) && (j + 1 < CellArray.GetUpperBound(1) + 1) && CellArray[i + 1, j + 1].Flag)
            {
                counter++;
            }

            if (counter >= CellArray[i, j].BombsAround)
            {
                if ((i - 1 >= 0) && (j - 1 >= 0))
                    CellArray[i - 1, j - 1].InterButt.PerformClick();

                if ((i - 1 >= 0))
                    CellArray[i - 1, j].InterButt.PerformClick();

                if ((i - 1 >= 0) && (j + 1 < CellArray.GetUpperBound(1) + 1))
                    CellArray[i - 1, j + 1].InterButt.PerformClick();

                if ((j + 1 < CellArray.GetUpperBound(1) + 1))
                    CellArray[i, j + 1].InterButt.PerformClick();

                if ((i + 1 <= CellArray.GetUpperBound(0)) && (j + 1 < CellArray.GetUpperBound(1) + 1))
                    CellArray[i + 1, j + 1].InterButt.PerformClick();

                if ((i + 1 <= CellArray.GetUpperBound(0)))
                    CellArray[i + 1, j].InterButt.PerformClick();

                if ((i + 1 <= CellArray.GetUpperBound(0)) && (j - 1 >= 0))
                    CellArray[i + 1, j - 1].InterButt.PerformClick();

                if ((j - 1 >= 0))
                    CellArray[i, j - 1].InterButt.PerformClick();
            }
            else
            {
                if ((i - 1 >= 0) && (j - 1 >= 0) && CellArray[i - 1, j - 1].InterButt.Visible && !CellArray[i - 1, j - 1].Flag)
                    CellArray[i - 1, j - 1].DoubleCLickPB.Visible = true;

                if ((i - 1 >= 0) && CellArray[i - 1, j].InterButt.Visible && !CellArray[i - 1, j].Flag)
                    CellArray[i - 1, j].DoubleCLickPB.Visible = true;

                if ((i - 1 >= 0) && (j + 1 < CellArray.GetUpperBound(1) + 1) && CellArray[i - 1, j + 1].InterButt.Visible && !CellArray[i - 1, j + 1].Flag)
                    CellArray[i - 1, j + 1].DoubleCLickPB.Visible = true;

                if ((j + 1 < CellArray.GetUpperBound(1) + 1) && CellArray[i, j + 1].InterButt.Visible && !CellArray[i, j + 1].Flag)
                    CellArray[i, j + 1].DoubleCLickPB.Visible = true;

                if ((i + 1 <= CellArray.GetUpperBound(0)) && (j + 1 < CellArray.GetUpperBound(1) + 1) && CellArray[i + 1, j + 1].InterButt.Visible && !CellArray[i + 1, j + 1].Flag)
                    CellArray[i + 1, j + 1].DoubleCLickPB.Visible = true;

                if ((i + 1 <= CellArray.GetUpperBound(0)) && CellArray[i + 1, j].InterButt.Visible && !CellArray[i + 1, j].Flag)
                    CellArray[i + 1, j].DoubleCLickPB.Visible = true;

                if ((i + 1 <= CellArray.GetUpperBound(0)) && (j - 1 >= 0) && CellArray[i + 1, j - 1].InterButt.Visible && !CellArray[i + 1, j - 1].Flag)
                    CellArray[i + 1, j - 1].DoubleCLickPB.Visible = true;

                if ((j - 1 >= 0) && CellArray[i, j - 1].InterButt.Visible && !CellArray[i, j - 1].Flag)
                    CellArray[i, j - 1].DoubleCLickPB.Visible = true;
            }
        }

        public delegate void BattleisEnd(int POE);
        public event BattleisEnd BattleIsEnded;
        bool StopEnding = true;

        void Table_Explosion()
        {            
            StopEnding = false;
            for (Int32 i = 0; i < CellArray.GetUpperBound(0) + 1; i++)
                for (Int32 j = 0; j < CellArray.GetUpperBound(1) + 1; j++)
                    CellArray[i, j].InterButt.Visible = false;
            if (BattleIsEnded != null)
                BattleIsEnded(0);
        }


        public void ChangeSize(int width, int height)
        {
            this.Controls.Clear();

            this.Height = height - 38;
            this.Width = width - 15;
            int height_buffer = this.Height;
            int width_buffer = this.Width;

            while ((height_buffer) % Theight != 0)
            {
                --height_buffer;
            }
            while ((width_buffer) % Twidth != 0)
            {
                --width_buffer;
            }
            int vertical = (this.Height - height_buffer) / 2;
            int horizontal = (this.Width - width_buffer) / 2;
            for (int i = 0; i < Theight; ++i)
            {
                int j = 0;
                for (; j < Twidth; ++j)
                {
                    CellArray[i, j].Height = height_buffer / Theight;
                    CellArray[i, j].Width = width_buffer / Twidth;
                    CellArray[i, j].Location = new Point(horizontal, vertical);
                    this.Controls.Add(CellArray[i, j]);
                    horizontal = horizontal + CellArray[i, j].Width;
                }
                --j;
                vertical = vertical + CellArray[i, j].Height;
                horizontal = (this.Width - width_buffer) / 2;
            }
        }

        private void Table_Load(object sender, EventArgs e)
        {

        }

        public void bfs_tmp(Cell thi)
        {
            if ((thi.XM - 1 >= 0) && (thi.YM - 1 >= 0) && CellArray[thi.XM - 1, thi.YM - 1].InterButt.Visible)
            {
                CellArray[thi.XM - 1, thi.YM - 1].InterButt.Visible = false;
                if (CellArray[thi.XM - 1, thi.YM - 1].bombs == 0)
                    bfs_tmp(CellArray[thi.XM - 1, thi.YM - 1]);
            }
            if ((thi.XM - 1 >= 0) && CellArray[thi.XM - 1, thi.YM].InterButt.Visible)
            {
                CellArray[thi.XM - 1, thi.YM].InterButt.Visible = false;
                if (CellArray[thi.XM - 1, thi.YM].bombs == 0)
                    bfs_tmp(CellArray[thi.XM - 1, thi.YM]);
            }
            if ((thi.XM - 1 >= 0) && (thi.YM + 1 < CellArray.GetUpperBound(1) + 1) && CellArray[thi.XM - 1, thi.YM + 1].InterButt.Visible)
            {
                CellArray[thi.XM - 1, thi.YM + 1].InterButt.Visible = false;
                if (CellArray[thi.XM - 1, thi.YM + 1].bombs == 0)
                    bfs_tmp(CellArray[thi.XM - 1, thi.YM + 1]);
            }
            if ((thi.YM - 1 >= 0) && CellArray[thi.XM, thi.YM - 1].InterButt.Visible && CellArray[thi.XM, thi.YM - 1].InterButt.Visible)
            {
                CellArray[thi.XM, thi.YM - 1].InterButt.Visible = false;
                if (CellArray[thi.XM, thi.YM - 1].bombs == 0)
                    bfs_tmp(CellArray[thi.XM, thi.YM - 1]);
            }
            if ((thi.YM + 1 < CellArray.GetUpperBound(1) + 1) && CellArray[thi.XM, thi.YM + 1].InterButt.Visible)
            {
                CellArray[thi.XM, thi.YM + 1].InterButt.Visible = false;
                if (CellArray[thi.XM, thi.YM + 1].bombs == 0)
                    bfs_tmp(CellArray[thi.XM, thi.YM + 1]);
            }
            if ((thi.XM + 1 < CellArray.GetUpperBound(0) + 1) && (thi.YM - 1 >= 0) && CellArray[thi.XM + 1, thi.YM - 1].InterButt.Visible)
            {
                CellArray[thi.XM + 1, thi.YM - 1].InterButt.Visible = false;
                if (CellArray[thi.XM + 1, thi.YM - 1].bombs == 0)
                    bfs_tmp(CellArray[thi.XM + 1, thi.YM - 1]);
            }
            if ((thi.XM + 1 < CellArray.GetUpperBound(0) + 1) && CellArray[thi.XM + 1, thi.YM].InterButt.Visible)
            {
                CellArray[thi.XM + 1, thi.YM].InterButt.Visible = false;
                if (CellArray[thi.XM + 1, thi.YM].bombs == 0)
                    bfs_tmp(CellArray[thi.XM + 1, thi.YM]);
            }
            if ((thi.XM + 1 < CellArray.GetUpperBound(0) + 1) && (thi.YM + 1 < CellArray.GetUpperBound(1) + 1) && CellArray[thi.XM + 1, thi.YM + 1].InterButt.Visible)
            {
                CellArray[thi.XM + 1, thi.YM + 1].InterButt.Visible = false;
                if (CellArray[thi.XM + 1, thi.YM + 1].bombs == 0)
                    bfs_tmp(CellArray[thi.XM + 1, thi.YM + 1]);
            }
        }
        
        public void bfs (int a, int b) {
            bfs_tmp(CellArray[a,b]);
        }

        private void InterButt_Click(object sender, EventArgs e)
        {

        }
    }
}

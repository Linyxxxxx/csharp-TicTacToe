using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TicTacToe
{
    public partial class Form : System.Windows.Forms.Form
    {
        /*
         * mode记录游戏模式
         * 1 为玩家vsAI 玩家先手
         * 2 为玩家vsAI AI先手
         * 3 为玩家vs玩家 玩家1先手
         * 4 为玩家vs玩家 玩家2先手
         */
        int mode = 0;
        int lastGameMode = 0;

        //记录当前是否为玩家1的回合
        bool isPlayer1Turn = true;

        /*
         * 记录当前按钮状态
         * 0 空按钮
         * 1 玩家1按下
         * 2 玩家2(AI)按下
         */
        int[,] ButtonArray = { { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 } };

        int player1Score = 0; //玩家1(玩家)得分
        int player2Score = 0; //玩家2(AI)得分

        Image crossImg = Image.FromFile("image/cross.png"); // "X" 图片
        Image circleImg= Image.FromFile("image/circle.png"); // "O" 图片

        public Form()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            string clickedButtonName = clickedButton.Name;

            Console.Write("button be clicked");

            //点击确认选择按钮 开始游戏
            if (clickedButtonName == "BtnStart")
            {
                mode = getModeIndex();
                lastGameMode = mode;
                clickedButton.Enabled = false;

                return;
            }

            //点击重新开始按钮 重置棋盘
            if (clickedButtonName == "BtnRestart")
            {
                Restart();
                return;
            }
            // 未选择模式 阻止进入
            if (mode == 0)
            {
                MessageBox.Show("请先选择游戏模式！", "信息提示！", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 分别记录当前按钮的X和Y坐标
            int curBtnX = Convert.ToInt32(clickedButtonName.Substring(3,1));
            int curBtnY = Convert.ToInt32(clickedButtonName.Substring(4,1));

            // 单人模式
            if (mode == 1 || mode == 2)
            {
                Chess(1, curBtnX, curBtnY);
            }
            // 多人模式
            else if (isPlayer1Turn == true)
            {
                Chess(1, curBtnX, curBtnY);
            }
            else if (isPlayer1Turn == false)
            {
                Chess(2, curBtnX, curBtnY);
            }

            // 回合替换
            isPlayer1Turn = !isPlayer1Turn;
        }

        // 获得当前模式代码 若玩家未选择 则为默认值0
        private int getModeIndex()
        {

            //棋盘开放
            foreach (Button item in ChessBoard.Controls)
            {
                item.Text = "";
                item.Enabled = true;
            }

            //单人 玩家先手
            if (radioType1.Checked)
            {
                isPlayer1Turn = true;

                #region 切换模式 得分清零
                if (lastGameMode == 3 || lastGameMode == 4)
                {
                    player1Score = 0;
                    player2Score = 0;
                    ScoreLabel1.Text = player1Score.ToString();
                    ScoreLabel2.Text = player2Score.ToString();
                }
                Player1Name.Text = "玩家";
                Player2Name.Text = "电脑";
                #endregion

                return 1;
            }
            
            //单人 电脑先手
            else if (radioType2.Checked)
            {
                isPlayer1Turn = false;

                #region 切换模式 得分清零
                if (lastGameMode == 3 || lastGameMode == 4)
                {
                    player1Score = 0;
                    player2Score = 0;
                    ScoreLabel1.Text = player1Score.ToString();
                    ScoreLabel2.Text = player2Score.ToString();
                }
                Player1Name.Text = "玩家";
                Player2Name.Text = "电脑";
                #endregion

                //电脑先进行一次回合

                #region 电脑随机走第一步
                Random ro = new Random();
                int x = ro.Next(1, 10);
                switch (x)
                {
                    case 1: Chess(2, 0, 0); break;
                    case 2: Chess(2, 0, 1); break;
                    case 3: Chess(2, 0, 2); break;
                    case 4: Chess(2, 1, 0); break;
                    case 5: Chess(2, 1, 1); break;
                    case 6: Chess(2, 1, 2); break;
                    case 7: Chess(2, 2, 0); break;
                    case 8: Chess(2, 2, 1); break;
                    case 9: Chess(2, 2, 2); break;
                    default: MessageBox.Show("Error!", "信息提示！", MessageBoxButtons.OK, MessageBoxIcon.Error); break;
                }
                #endregion

                return 2;
            }

            //双人 玩家1先手
            else if (radioType3.Checked)
            {
                isPlayer1Turn = true;

                #region 切换模式 得分清零
                if (lastGameMode == 1 || lastGameMode == 2)
                {
                    player1Score = 0;
                    player2Score = 0;
                    ScoreLabel1.Text = player1Score.ToString();
                    ScoreLabel2.Text = player2Score.ToString();
                }
                Player1Name.Text = "玩家1";
                Player2Name.Text = "玩家2";
                #endregion

                return 3;
            }

            //双人 玩家2先手
            else if (radioType4.Checked)
            {
                isPlayer1Turn = false;

                #region 切换模式 得分清零
                if (lastGameMode == 1 || lastGameMode == 2)
                {
                    player1Score = 0;
                    player2Score = 0;
                    ScoreLabel1.Text = player1Score.ToString();
                    ScoreLabel2.Text = player2Score.ToString();
                }
                Player1Name.Text = "玩家1";
                Player2Name.Text = "玩家2";
                #endregion

                return 4;
            }
            else
            {
                return 0;
            }
        }

        private void Chess(int player, int x, int y)
        {
            // 显示在棋盘上的图形
            Image displayImg;
            if (player == 1)
            {
                displayImg = circleImg;
            }
            else
            {
                displayImg = crossImg;
            }

            //更改在数组中的记录
            ButtonArray[x, y] = player;
            ((Button)Controls.Find("Btn" + x + y, true)[0]).BackgroundImage = displayImg;
            ((Button)Controls.Find("Btn" + x + y, true)[0]).Enabled = false;

            //判断获胜
            if (JudgeWin(player) == 0)
            {
                //AI回合
                if (player == 1 && (mode == 1 || mode == 2))
                {
                    AITurn();
                }
            }          
        }

        private int JudgeWin(int player)
        {
            int Result = 0;

            #region 判断胜负
            //判断第一横排
            if (ButtonArray[0, 0] == player && ButtonArray[0, 1] == player && ButtonArray[0, 2] == player) Result = player;

            //判断第二横排
            else if (ButtonArray[1, 0] == player && ButtonArray[1, 1] == player && ButtonArray[1, 2] == player) Result = player;

            //判断第三横排
            else if (ButtonArray[2, 0] == player && ButtonArray[2, 1] == player && ButtonArray[2, 2] == player) Result = player;

            //判断第一纵排
            else if (ButtonArray[0, 0] == player && ButtonArray[1, 0] == player && ButtonArray[2, 0] == player) Result = player;

            //判断第二纵排
            else if (ButtonArray[0, 1] == player && ButtonArray[1, 1] == player && ButtonArray[2, 1] == player) Result = player;

            //判断第三纵排
            else if (ButtonArray[0, 2] == player && ButtonArray[1, 2] == player && ButtonArray[2, 2] == player) Result = player;

            //判断左上到右下
            else if (ButtonArray[0, 0] == player && ButtonArray[1, 1] == player && ButtonArray[2, 2] == player) Result = player;

            //判断右上到左下
            else if (ButtonArray[0, 2] == player && ButtonArray[1, 1] == player && ButtonArray[2, 0] == player) Result = player;
            #endregion

            #region 弹出消息
            if (Result == 1) // 玩家1获胜
            {
                //单人模式
                if (mode == 1 || mode == 2) MessageBox.Show("玩家获胜");
                //双人模式
                else MessageBox.Show("玩家1获胜");
                player1Score++;
                Restart();
                return 1;
            }
            if (Result == 2) // 玩家2获胜
            {
                //单人模式
                if (mode == 1 || mode == 2) MessageBox.Show("电脑获胜");
                //双人模式
                else MessageBox.Show("玩家2获胜");
                player2Score++;
                Restart();
                return 2;
            }
            #endregion


            //遍历棋盘 判断是否有空位
            #region 格子已满，打成平局
            bool full = true;
            foreach (int item in ButtonArray)
            {
                if (item == 0) full = false; // 有空位
            }
            if (full)
            {
                MessageBox.Show("平局");
                Restart();
            }
            #endregion

            // 比赛未结束
            return 0;
        }

        private void Restart()
        {
            //更新计分板
            ScoreLabel1.Text = player1Score.ToString();
            ScoreLabel2.Text = player2Score.ToString();

            //数组还原
            for (int x = 0; x <= 2; x++)
            {
                for (int y = 0; y <= 2; y++)
                {
                    ButtonArray[x, y] = 0;
                }
            }

            //棋盘还原
            foreach(Button item in ChessBoard.Controls)
            {
                //item.Text = "";
                item.BackgroundImage = null;
                item.Enabled = false;
            }

            //模式选择还原
            BtnStart.Enabled = true;
            mode = 0;
        }

        #region 电脑AI

        private void AITurn()
        {
            #region 调用寻找缺口的函数
            //寻找电脑的缺口
            int Result = FindBreach(1);

            //未找到电脑的缺口，寻找玩家的缺口
            if (Result == 0) Result = FindBreach(2);
            #endregion
            //找是否有条件更好的四个角落位置
            if (Result == 0) Result = FindBreach2(1);

            #region 未找到玩家的缺口，随便寻找四角是否被占
            if (Result == 0)
                if (ButtonArray[0, 0] == 0) Chess(2, 0, 0);
                else if (ButtonArray[0, 2] == 0) Chess(2, 0, 2);
                else if (ButtonArray[2, 0] == 0) Chess(2, 2, 0);
                else if (ButtonArray[2, 2] == 0) Chess(2, 2, 2);
                //寻找中心是否被占
                else if (ButtonArray[1, 1] == 0) Chess(2, 1, 1);
                //寻找是否有空位
                else if (ButtonArray[0, 1] == 0) Chess(2, 0, 1);
                else if (ButtonArray[1, 0] == 0) Chess(2, 1, 0);
                else if (ButtonArray[1, 2] == 0) Chess(2, 1, 2);
                else if (ButtonArray[2, 1] == 0) Chess(2, 2, 1);
            #endregion
        }

        //寻找可以直接获胜的空位
        private int FindBreach(int Player)
        {
            int Result = 0, Opponent = 0;

            #region 分配自己和对手的Player值
            if (Player == 1) Opponent = 2;
            else Opponent = 1;
            #endregion

            #region 判断并执行缺口
            //判断第一横排
            if (ButtonArray[0, 0] + ButtonArray[0, 1] + ButtonArray[0, 2] == 2 * Opponent &&
                ButtonArray[0, 0] != Player && ButtonArray[0, 1] != Player && ButtonArray[0, 2] != Player)
            {
                //寻找该排是否有空位
                if (ButtonArray[0, 0] == 0)
                {
                    Chess(2, 0, 0);
                    Result = 1;
                }
                else if (ButtonArray[0, 1] == 0)
                {
                    Chess(2, 0, 1);
                    Result = 1;
                }
                else if (ButtonArray[0, 2] == 0)
                {
                    Chess(2, 0, 2);
                    Result = 1;
                }
            }

            //判断第二横排
            else if (ButtonArray[1, 0] + ButtonArray[1, 1] + ButtonArray[1, 2] == 2 * Opponent &&
                     ButtonArray[1, 0] != Player && ButtonArray[1, 1] != Player && ButtonArray[1, 2] != Player)
            {
                if (ButtonArray[1, 0] == 0)
                {
                    Chess(2, 1, 0);
                    Result = 1;
                }
                else if (ButtonArray[1, 1] == 0)
                {
                    Chess(2, 1, 1);
                    Result = 1;
                }
                else if (ButtonArray[1, 2] == 0)
                {
                    Chess(2, 1, 2);
                    Result = 1;
                }
            }

            //判断第三横排
            else if (ButtonArray[2, 0] + ButtonArray[2, 1] + ButtonArray[2, 2] == 2 * Opponent &&
                     ButtonArray[2, 0] != Player && ButtonArray[2, 1] != Player && ButtonArray[2, 2] != Player)
            {
                if (ButtonArray[2, 0] == 0)
                {
                    Chess(2, 2, 0);
                    Result = 1;
                }
                else if (ButtonArray[2, 1] == 0)
                {
                    Chess(2, 2, 1);
                    Result = 1;
                }
                else if (ButtonArray[2, 2] == 0)
                {
                    Chess(2, 2, 2);
                    Result = 1;
                }
            }

            //判断第一纵排
            else if (ButtonArray[0, 0] + ButtonArray[1, 0] + ButtonArray[2, 0] == 2 * Opponent &&
                     ButtonArray[0, 0] != Player && ButtonArray[1, 0] != Player && ButtonArray[2, 0] != Player)
            {
                if (ButtonArray[0, 0] == 0)
                {
                    Chess(2, 0, 0);
                    Result = 1;
                }
                else if (ButtonArray[1, 0] == 0)
                {
                    Chess(2, 1, 0);
                    Result = 1;
                }
                else if (ButtonArray[2, 0] == 0)
                {
                    Chess(2, 2, 0);
                    Result = 1;
                }
            }

            //判断第二纵排
            else if (ButtonArray[0, 1] + ButtonArray[1, 1] + ButtonArray[2, 1] == 2 * Opponent &&
                     ButtonArray[0, 1] != Player && ButtonArray[1, 1] != Player && ButtonArray[2, 1] != Player)
            {
                if (ButtonArray[0, 1] == 0)
                {
                    Chess(2, 0, 1);
                    Result = 1;
                }
                else if (ButtonArray[1, 1] == 0)
                {
                    Chess(2, 1, 1);
                    Result = 1;
                }
                else if (ButtonArray[2, 1] == 0)
                {
                    Chess(2, 2, 1);
                    Result = 1;
                }
            }

            //判断第三纵排
            else if (ButtonArray[0, 2] + ButtonArray[1, 2] + ButtonArray[2, 2] == 2 * Opponent &&
                     ButtonArray[0, 2] != Player && ButtonArray[1, 2] != Player && ButtonArray[2, 2] != Player)
            {
                if (ButtonArray[0, 2] == 0)
                {
                    Chess(2, 0, 2);
                    Result = 1;
                }
                else if (ButtonArray[1, 2] == 0)
                {
                    Chess(2, 1, 2);
                    Result = 1;
                }
                else if (ButtonArray[2, 2] == 0)
                {
                    Chess(2, 2, 2);
                    Result = 1;
                }
            }

            //判断左上到右下
            else if (ButtonArray[0, 0] + ButtonArray[1, 1] + ButtonArray[2, 2] == 2 * Opponent &&
                     ButtonArray[0, 0] != Player && ButtonArray[1, 1] != Player && ButtonArray[2, 2] != Player)
            {
                if (ButtonArray[0, 0] == 0)
                {
                    Chess(2, 0, 0);
                    Result = 1;
                }
                else if (ButtonArray[1, 1] == 0)
                {
                    Chess(2, 1, 1);
                    Result = 1;
                }
                else if (ButtonArray[2, 2] == 0)
                {
                    Chess(2, 2, 2);
                    Result = 1;
                }
            }

            //判断右上到左下
            else if (ButtonArray[0, 2] + ButtonArray[1, 1] + ButtonArray[2, 0] == 2 * Opponent &&
                     ButtonArray[0, 2] != Player && ButtonArray[1, 1] != Player && ButtonArray[2, 0] != Player)
            {
                if (ButtonArray[0, 2] == 0)
                {
                    Chess(2, 0, 2);
                    Result = 1;
                }
                else if (ButtonArray[1, 1] == 0)
                {
                    Chess(2, 1, 1);
                    Result = 1;
                }
                else if (ButtonArray[2, 0] == 0)
                {
                    Chess(2, 2, 0);
                    Result = 1;
                }
            }
            #endregion

            return Result;
        }

        //判断是否有 创造两个一排的良好机会
        private int FindBreach2(int Player)
        {
            int Result = 0, Opponent = 0;

            #region 分配自己和对手的Player值
            if (Player == 1) Opponent = 2;
            else Opponent = 1;
            #endregion
            //判断第一横排
            if (ButtonArray[0, 0] + ButtonArray[0, 1] + ButtonArray[0, 2] == Opponent &&
                ButtonArray[0, 0] != Player && ButtonArray[0, 1] != Player && ButtonArray[0, 2] != Player)
                if (ButtonArray[0, 0] == Opponent)
                {
                    Chess(2, 0, 2);
                    return Result = 1;
                }
                else if (ButtonArray[0, 2] == Opponent)
                {
                    Chess(2, 0, 0);
                    return Result = 1;
                }
            //判断第三横排
            if (ButtonArray[2, 0] + ButtonArray[2, 1] + ButtonArray[2, 2] == Opponent &&
                ButtonArray[2, 0] != Player && ButtonArray[2, 1] != Player && ButtonArray[2, 2] != Player)
                if (ButtonArray[2, 0] == Opponent)
                {
                    Chess(2, 2, 2);
                    return Result = 1;
                }
                else if (ButtonArray[2, 2] == Opponent)
                {
                    Chess(2, 2, 0);
                    return Result = 1;
                }
            //判断第一纵排
            if (ButtonArray[0, 0] + ButtonArray[1, 0] + ButtonArray[2, 0] == Opponent &&
                ButtonArray[0, 0] != Player && ButtonArray[1, 0] != Player && ButtonArray[2, 0] != Player)
                if (ButtonArray[0, 0] == Opponent)
                {
                    Chess(2, 2, 0);
                    return Result = 1;
                }
                else if (ButtonArray[2, 0] == Opponent)
                {
                    Chess(2, 0, 0);
                    return Result = 1;
                }
            //判断第三纵排
            if (ButtonArray[0, 2] + ButtonArray[1, 2] + ButtonArray[2, 2] == Opponent &&
                ButtonArray[0, 2] != Player && ButtonArray[1, 2] != Player && ButtonArray[2, 2] != Player)
                if (ButtonArray[0, 2] == Opponent)
                {
                    Chess(2, 2, 2);
                    return Result = 1;
                }
                else if (ButtonArray[2, 2] == Opponent)
                {
                    Chess(2, 0, 2);
                    return Result = 1;
                }
            //判断左上到右下
            if (ButtonArray[0, 0] + ButtonArray[1, 1] + ButtonArray[2, 2] == Opponent &&
                ButtonArray[0, 0] != Player && ButtonArray[1, 1] != Player && ButtonArray[2, 2] != Player)
                if (ButtonArray[0, 0] == Opponent)
                {
                    Chess(2, 2, 2);
                    return Result = 1;
                }
                else if (ButtonArray[2, 2] == Opponent)
                {
                    Chess(2, 0, 0);
                    return Result = 1;
                }
            //判断右上到左下
            if (ButtonArray[0, 2] + ButtonArray[1, 1] + ButtonArray[2, 0] == Opponent &&
                ButtonArray[0, 2] != Player && ButtonArray[1, 1] != Player && ButtonArray[2, 0] != Player)
                if (ButtonArray[0, 2] == Opponent)
                {
                    Chess(2, 2, 0);
                    return Result = 1;
                }
                else if (ButtonArray[2, 0] == Opponent)
                {
                    Chess(2, 0, 2);
                    return Result = 1;
                }
            return Result;
        }

        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}

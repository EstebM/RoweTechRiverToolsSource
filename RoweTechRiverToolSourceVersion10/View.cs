using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace RoweTech
{
    public partial class View : Form
    {
        public View(String ViewStr)//, int x, int y)
        {
            InitializeComponent();
            //this.StartPosition = FormStartPosition.Manual;
            //this.Left = 0;
            //this.Top = 0;
            //this.textBox1.Text = ViewStr;
            this.Text = ViewStr;
        }

        private System.Windows.Forms.TextBox textBox1;

        //private TextBox textBox1;
        static System.Windows.Forms.Timer Timer_Update = new System.Windows.Forms.Timer();
        internal static class IniVar
        {
            public static string UpdateStr = "";
            public static string StrToTheTop = "";
            public static bool BadFlag = true;
        }

        private void View_Load(object sender, EventArgs e)
        {
            Timer_Update.Tick += new System.EventHandler(Timer_Update_Tick);
            Timer_Update.Interval = 10;
            Timer_Update.Enabled = true;
            Timer_Update.Start();
        }
        private void Timer_Update_Tick(object sender, EventArgs e)
        {
            if(IniVar.BadFlag)
                textBox1.ForeColor = Color.Red;
            else
                textBox1.ForeColor = Color.Black;

            textBox1.Text = IniVar.UpdateStr;
            
            if (IniVar.StrToTheTop == "Top")
            {   
                this.BringToFront();//this.TopMost = true;
                IniVar.StrToTheTop = "";
            }
        }

        private void InitializeComponent()
        {
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(12, 12);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(290, 60);
            this.textBox1.TabIndex = 0;
            // 
            // View
            // 
            this.ClientSize = new System.Drawing.Size(314, 84);
            this.Controls.Add(this.textBox1);
            this.Name = "View";
            this.Load += new System.EventHandler(this.View_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        
    }
}

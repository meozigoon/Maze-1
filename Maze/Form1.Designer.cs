namespace Maze
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            numericUpDown1 = new NumericUpDown();
            button1 = new Button();
            button2 = new Button();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            numericUpDown2 = new NumericUpDown();
            label5 = new Label();
            label6 = new Label();
            button3 = new Button();
            numericUpDown3 = new NumericUpDown();
            label7 = new Label();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown3).BeginInit();
            SuspendLayout();
            // 
            // numericUpDown1
            // 
            numericUpDown1.Location = new Point(143, 9);
            numericUpDown1.Margin = new Padding(2);
            numericUpDown1.Maximum = new decimal(new int[] { 80, 0, 0, 0 });
            numericUpDown1.Minimum = new decimal(new int[] { 2, 0, 0, 0 });
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new Size(129, 31);
            numericUpDown1.TabIndex = 0;
            numericUpDown1.Value = new decimal(new int[] { 2, 0, 0, 0 });
            numericUpDown1.ValueChanged += numericUpDown1_ValueChanged;
            numericUpDown1.KeyDown += numericUpDown1_KeyDown;
            // 
            // button1
            // 
            button1.Location = new Point(302, 5);
            button1.Margin = new Padding(2);
            button1.Name = "button1";
            button1.Size = new Size(142, 36);
            button1.TabIndex = 1;
            button1.Text = "미로 만들기";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Enabled = false;
            button2.Location = new Point(448, 5);
            button2.Margin = new Padding(2);
            button2.Name = "button2";
            button2.Size = new Size(115, 36);
            button2.TabIndex = 2;
            button2.Text = "미로 실행";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(9, 84);
            label1.Margin = new Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new Size(58, 25);
            label1.TabIndex = 3;
            label1.Text = "BFS : ";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(9, 116);
            label2.Margin = new Padding(2, 0, 2, 0);
            label2.Name = "label2";
            label2.Size = new Size(60, 25);
            label2.TabIndex = 4;
            label2.Text = "DFS : ";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(1, 11);
            label3.Margin = new Padding(2, 0, 2, 0);
            label3.Name = "label3";
            label3.Size = new Size(138, 25);
            label3.TabIndex = 5;
            label3.Text = "미로 한변 셀 수";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(581, 11);
            label4.Margin = new Padding(2, 0, 2, 0);
            label4.Name = "label4";
            label4.Size = new Size(190, 25);
            label4.TabIndex = 6;
            label4.Text = "미로 설정 클럭 수(ms)";
            // 
            // numericUpDown2
            // 
            numericUpDown2.Location = new Point(776, 11);
            numericUpDown2.Margin = new Padding(2);
            numericUpDown2.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            numericUpDown2.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDown2.Name = "numericUpDown2";
            numericUpDown2.Size = new Size(118, 31);
            numericUpDown2.TabIndex = 7;
            numericUpDown2.Value = new decimal(new int[] { 100, 0, 0, 0 });
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(9, 165);
            label5.Margin = new Padding(2, 0, 2, 0);
            label5.Name = "label5";
            label5.Size = new Size(54, 25);
            label5.TabIndex = 9;
            label5.Text = "Loop";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(9, 190);
            label6.Margin = new Padding(2, 0, 2, 0);
            label6.Name = "label6";
            label6.Size = new Size(57, 25);
            label6.TabIndex = 10;
            label6.Text = "n = 2";
            // 
            // button3
            // 
            button3.Location = new Point(9, 258);
            button3.Margin = new Padding(2);
            button3.Name = "button3";
            button3.Size = new Size(90, 36);
            button3.TabIndex = 11;
            button3.Text = "Go";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // numericUpDown3
            // 
            numericUpDown3.Location = new Point(9, 217);
            numericUpDown3.Margin = new Padding(2);
            numericUpDown3.Maximum = new decimal(new int[] { 80, 0, 0, 0 });
            numericUpDown3.Minimum = new decimal(new int[] { 2, 0, 0, 0 });
            numericUpDown3.Name = "numericUpDown3";
            numericUpDown3.Size = new Size(90, 31);
            numericUpDown3.TabIndex = 12;
            numericUpDown3.Value = new decimal(new int[] { 10, 0, 0, 0 });
            numericUpDown3.KeyDown += numericUpDown3_KeyDown;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(9, 296);
            label7.Margin = new Padding(2, 0, 2, 0);
            label7.Name = "label7";
            label7.Size = new Size(83, 25);
            label7.TabIndex = 13;
            label7.Text = "횟수 = 0";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1062, 570);
            Controls.Add(label7);
            Controls.Add(numericUpDown3);
            Controls.Add(button3);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(numericUpDown2);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(numericUpDown1);
            Margin = new Padding(2);
            Name = "Form1";
            Text = "Form1";
            SizeChanged += Form1_SizeChanged;
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown2).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown3).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private NumericUpDown numericUpDown1;
        private Button button1;
        private Button button2;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private NumericUpDown numericUpDown2;
        private Label label5;
        private Label label6;
        private Button button3;
        private NumericUpDown numericUpDown3;
        private Label label7;
    }
}

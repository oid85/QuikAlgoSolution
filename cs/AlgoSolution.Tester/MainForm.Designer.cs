namespace AlgoSolution.Tester
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpCandleChart = new System.Windows.Forms.TabPage();
            this.candlesChart1 = new AlgoSolution.GraphControls.CandlesChart();
            this.tpPerformanceTable = new System.Windows.Forms.TabPage();
            this.performanceTable1 = new AlgoSolution.GraphControls.PerformanceTable();
            this.tpEquityCurve = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.equityChart1 = new AlgoSolution.GraphControls.EquityChart();
            this.drawdownChart1 = new AlgoSolution.GraphControls.DrawdownChart();
            this.tpPositionsChart1 = new System.Windows.Forms.TabPage();
            this.positionsChart1 = new AlgoSolution.GraphControls.PositionsChart();
            this.tabControl1.SuspendLayout();
            this.tpCandleChart.SuspendLayout();
            this.tpPerformanceTable.SuspendLayout();
            this.tpEquityCurve.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tpPositionsChart1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tpCandleChart);
            this.tabControl1.Controls.Add(this.tpPerformanceTable);
            this.tabControl1.Controls.Add(this.tpEquityCurve);
            this.tabControl1.Controls.Add(this.tpPositionsChart1);
            this.tabControl1.Location = new System.Drawing.Point(5, 5);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(748, 503);
            this.tabControl1.TabIndex = 0;
            // 
            // tpCandleChart
            // 
            this.tpCandleChart.Controls.Add(this.candlesChart1);
            this.tpCandleChart.Location = new System.Drawing.Point(4, 22);
            this.tpCandleChart.Name = "tpCandleChart";
            this.tpCandleChart.Padding = new System.Windows.Forms.Padding(3);
            this.tpCandleChart.Size = new System.Drawing.Size(740, 477);
            this.tpCandleChart.TabIndex = 0;
            this.tpCandleChart.Text = "Свечной график";
            this.tpCandleChart.UseVisualStyleBackColor = true;
            // 
            // candlesChart1
            // 
            this.candlesChart1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.candlesChart1.Location = new System.Drawing.Point(6, 6);
            this.candlesChart1.Name = "candlesChart1";
            this.candlesChart1.Size = new System.Drawing.Size(728, 465);
            this.candlesChart1.TabIndex = 0;
            // 
            // tpPerformanceTable
            // 
            this.tpPerformanceTable.Controls.Add(this.performanceTable1);
            this.tpPerformanceTable.Location = new System.Drawing.Point(4, 22);
            this.tpPerformanceTable.Name = "tpPerformanceTable";
            this.tpPerformanceTable.Padding = new System.Windows.Forms.Padding(3);
            this.tpPerformanceTable.Size = new System.Drawing.Size(740, 477);
            this.tpPerformanceTable.TabIndex = 1;
            this.tpPerformanceTable.Text = "Показатели";
            this.tpPerformanceTable.UseVisualStyleBackColor = true;
            // 
            // performanceTable1
            // 
            this.performanceTable1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.performanceTable1.Location = new System.Drawing.Point(6, 6);
            this.performanceTable1.Name = "performanceTable1";
            this.performanceTable1.Size = new System.Drawing.Size(728, 465);
            this.performanceTable1.TabIndex = 0;
            // 
            // tpEquityCurve
            // 
            this.tpEquityCurve.Controls.Add(this.splitContainer1);
            this.tpEquityCurve.Location = new System.Drawing.Point(4, 22);
            this.tpEquityCurve.Name = "tpEquityCurve";
            this.tpEquityCurve.Size = new System.Drawing.Size(740, 477);
            this.tpEquityCurve.TabIndex = 2;
            this.tpEquityCurve.Text = "Эквити";
            this.tpEquityCurve.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.equityChart1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.drawdownChart1);
            this.splitContainer1.Size = new System.Drawing.Size(734, 471);
            this.splitContainer1.SplitterDistance = 332;
            this.splitContainer1.TabIndex = 0;
            // 
            // equityChart1
            // 
            this.equityChart1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.equityChart1.Location = new System.Drawing.Point(3, 3);
            this.equityChart1.Name = "equityChart1";
            this.equityChart1.Size = new System.Drawing.Size(726, 324);
            this.equityChart1.TabIndex = 0;
            // 
            // drawdownChart1
            // 
            this.drawdownChart1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.drawdownChart1.Location = new System.Drawing.Point(3, 3);
            this.drawdownChart1.Name = "drawdownChart1";
            this.drawdownChart1.Size = new System.Drawing.Size(726, 127);
            this.drawdownChart1.TabIndex = 0;
            // 
            // tpPositionsChart1
            // 
            this.tpPositionsChart1.Controls.Add(this.positionsChart1);
            this.tpPositionsChart1.Location = new System.Drawing.Point(4, 22);
            this.tpPositionsChart1.Name = "tpPositionsChart1";
            this.tpPositionsChart1.Size = new System.Drawing.Size(740, 477);
            this.tpPositionsChart1.TabIndex = 3;
            this.tpPositionsChart1.Text = "Позиции";
            this.tpPositionsChart1.UseVisualStyleBackColor = true;
            // 
            // positionsChart1
            // 
            this.positionsChart1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.positionsChart1.Location = new System.Drawing.Point(3, 3);
            this.positionsChart1.Name = "positionsChart1";
            this.positionsChart1.Size = new System.Drawing.Size(734, 471);
            this.positionsChart1.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(758, 513);
            this.Controls.Add(this.tabControl1);
            this.Name = "MainForm";
            this.Text = "Статистика";
            this.tabControl1.ResumeLayout(false);
            this.tpCandleChart.ResumeLayout(false);
            this.tpPerformanceTable.ResumeLayout(false);
            this.tpEquityCurve.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tpPositionsChart1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpCandleChart;
        private System.Windows.Forms.TabPage tpPerformanceTable;
        private GraphControls.CandlesChart candlesChart1;
        private GraphControls.PerformanceTable performanceTable1;
        private System.Windows.Forms.TabPage tpEquityCurve;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private GraphControls.EquityChart equityChart1;
        private GraphControls.DrawdownChart drawdownChart1;
        private System.Windows.Forms.TabPage tpPositionsChart1;
        private GraphControls.PositionsChart positionsChart1;
    }
}


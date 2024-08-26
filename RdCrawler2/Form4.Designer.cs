namespace RdCrawler2
{
    partial class OptionsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.onlyhtmloptioncheck = new System.Windows.Forms.CheckBox();
            this.fullsitecraeloptioncheck = new System.Windows.Forms.CheckBox();
            this.ok_button = new System.Windows.Forms.Button();
            this.cancel_button = new System.Windows.Forms.Button();
            this.apply_button = new System.Windows.Forms.Button();
            this.redirectsoption = new System.Windows.Forms.CheckBox();
            this.getexternalurloption = new System.Windows.Forms.CheckBox();
            this.rendermodeoption = new System.Windows.Forms.CheckBox();
            this.ignoreresourcesoption = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // onlyhtmloptioncheck
            // 
            this.onlyhtmloptioncheck.AutoSize = true;
            this.onlyhtmloptioncheck.Location = new System.Drawing.Point(13, 25);
            this.onlyhtmloptioncheck.Name = "onlyhtmloptioncheck";
            this.onlyhtmloptioncheck.Size = new System.Drawing.Size(232, 17);
            this.onlyhtmloptioncheck.TabIndex = 0;
            this.onlyhtmloptioncheck.Text = "Only return html files in crawler list as default";
            this.onlyhtmloptioncheck.UseVisualStyleBackColor = true;
            this.onlyhtmloptioncheck.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // fullsitecraeloptioncheck
            // 
            this.fullsitecraeloptioncheck.AutoSize = true;
            this.fullsitecraeloptioncheck.Location = new System.Drawing.Point(13, 61);
            this.fullsitecraeloptioncheck.Name = "fullsitecraeloptioncheck";
            this.fullsitecraeloptioncheck.Size = new System.Drawing.Size(138, 17);
            this.fullsitecraeloptioncheck.TabIndex = 1;
            this.fullsitecraeloptioncheck.Text = "Full site crawl as default";
            this.fullsitecraeloptioncheck.UseVisualStyleBackColor = true;
            // 
            // ok_button
            // 
            this.ok_button.Location = new System.Drawing.Point(35, 226);
            this.ok_button.Name = "ok_button";
            this.ok_button.Size = new System.Drawing.Size(75, 23);
            this.ok_button.TabIndex = 2;
            this.ok_button.Text = "Ok";
            this.ok_button.UseVisualStyleBackColor = true;
            this.ok_button.Click += new System.EventHandler(this.ok_button_Click);
            // 
            // cancel_button
            // 
            this.cancel_button.Location = new System.Drawing.Point(116, 226);
            this.cancel_button.Name = "cancel_button";
            this.cancel_button.Size = new System.Drawing.Size(75, 23);
            this.cancel_button.TabIndex = 3;
            this.cancel_button.Text = "Cancel";
            this.cancel_button.UseVisualStyleBackColor = true;
            this.cancel_button.Click += new System.EventHandler(this.cancel_button_Click);
            // 
            // apply_button
            // 
            this.apply_button.Location = new System.Drawing.Point(197, 226);
            this.apply_button.Name = "apply_button";
            this.apply_button.Size = new System.Drawing.Size(75, 23);
            this.apply_button.TabIndex = 4;
            this.apply_button.Text = "Apply";
            this.apply_button.UseVisualStyleBackColor = true;
            this.apply_button.Click += new System.EventHandler(this.apply_button_Click);
            // 
            // redirectsoption
            // 
            this.redirectsoption.AutoSize = true;
            this.redirectsoption.Location = new System.Drawing.Point(13, 94);
            this.redirectsoption.Name = "redirectsoption";
            this.redirectsoption.Size = new System.Drawing.Size(124, 17);
            this.redirectsoption.TabIndex = 5;
            this.redirectsoption.Text = "Consider redirections";
            this.redirectsoption.UseVisualStyleBackColor = true;
            // 
            // getexternalurloption
            // 
            this.getexternalurloption.AutoSize = true;
            this.getexternalurloption.Location = new System.Drawing.Point(13, 126);
            this.getexternalurloption.Name = "getexternalurloption";
            this.getexternalurloption.Size = new System.Drawing.Size(191, 17);
            this.getexternalurloption.TabIndex = 6;
            this.getexternalurloption.Text = "Only get Urls from external domains";
            this.getexternalurloption.UseVisualStyleBackColor = true;
            // 
            // rendermodeoption
            // 
            this.rendermodeoption.AutoSize = true;
            this.rendermodeoption.Location = new System.Drawing.Point(13, 158);
            this.rendermodeoption.Name = "rendermodeoption";
            this.rendermodeoption.Size = new System.Drawing.Size(153, 17);
            this.rendermodeoption.TabIndex = 7;
            this.rendermodeoption.Text = "HTML Render Mode(Slow)";
            this.rendermodeoption.UseVisualStyleBackColor = true;
            // 
            // ignoreresourcesoption
            // 
            this.ignoreresourcesoption.AutoSize = true;
            this.ignoreresourcesoption.Location = new System.Drawing.Point(13, 190);
            this.ignoreresourcesoption.Name = "ignoreresourcesoption";
            this.ignoreresourcesoption.Size = new System.Drawing.Size(185, 17);
            this.ignoreresourcesoption.TabIndex = 8;
            this.ignoreresourcesoption.Text = "Ignore resourses (.zip, .pdf, .exe..)";
            this.ignoreresourcesoption.UseVisualStyleBackColor = true;
            // 
            // OptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.ignoreresourcesoption);
            this.Controls.Add(this.rendermodeoption);
            this.Controls.Add(this.getexternalurloption);
            this.Controls.Add(this.redirectsoption);
            this.Controls.Add(this.apply_button);
            this.Controls.Add(this.cancel_button);
            this.Controls.Add(this.ok_button);
            this.Controls.Add(this.fullsitecraeloptioncheck);
            this.Controls.Add(this.onlyhtmloptioncheck);
            this.Name = "OptionsForm";
            this.Text = "Options";
            this.Load += new System.EventHandler(this.OptionsForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox onlyhtmloptioncheck;
        private System.Windows.Forms.CheckBox fullsitecraeloptioncheck;
        private System.Windows.Forms.Button ok_button;
        private System.Windows.Forms.Button cancel_button;
        private System.Windows.Forms.Button apply_button;
        private System.Windows.Forms.CheckBox redirectsoption;
        private System.Windows.Forms.CheckBox getexternalurloption;
        private System.Windows.Forms.CheckBox rendermodeoption;
        private System.Windows.Forms.CheckBox ignoreresourcesoption;
    }
}
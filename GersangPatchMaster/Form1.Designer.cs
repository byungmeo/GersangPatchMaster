
namespace GersangPatchMaster
{
    partial class Form1
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_checkVersion = new System.Windows.Forms.Button();
            this.tb_version = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_startPatch = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_gersangPath = new System.Windows.Forms.TextBox();
            this.btn_openPathFinder = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.btn_applyPatch = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_checkVersion
            // 
            this.btn_checkVersion.Location = new System.Drawing.Point(167, 12);
            this.btn_checkVersion.Name = "btn_checkVersion";
            this.btn_checkVersion.Size = new System.Drawing.Size(99, 23);
            this.btn_checkVersion.TabIndex = 0;
            this.btn_checkVersion.Text = "패치 버전 확인";
            this.btn_checkVersion.UseVisualStyleBackColor = true;
            this.btn_checkVersion.Click += new System.EventHandler(this.btn_checkVersion_Click);
            // 
            // tb_version
            // 
            this.tb_version.Location = new System.Drawing.Point(37, 12);
            this.tb_version.MaxLength = 5;
            this.tb_version.Name = "tb_version";
            this.tb_version.Size = new System.Drawing.Size(100, 21);
            this.tb_version.TabIndex = 1;
            this.tb_version.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tb_version_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(272, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(149, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "패치 버전을 확인해주세요.";
            // 
            // btn_startPatch
            // 
            this.btn_startPatch.Location = new System.Drawing.Point(191, 204);
            this.btn_startPatch.Name = "btn_startPatch";
            this.btn_startPatch.Size = new System.Drawing.Size(75, 23);
            this.btn_startPatch.TabIndex = 3;
            this.btn_startPatch.Text = "패치 다운";
            this.btn_startPatch.UseVisualStyleBackColor = true;
            this.btn_startPatch.Click += new System.EventHandler(this.btn_startPatch_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(272, 209);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "선택 버전 : ";
            // 
            // tb_gersangPath
            // 
            this.tb_gersangPath.Location = new System.Drawing.Point(28, 104);
            this.tb_gersangPath.Name = "tb_gersangPath";
            this.tb_gersangPath.Size = new System.Drawing.Size(357, 21);
            this.tb_gersangPath.TabIndex = 5;
            // 
            // btn_openPathFinder
            // 
            this.btn_openPathFinder.Location = new System.Drawing.Point(405, 102);
            this.btn_openPathFinder.Name = "btn_openPathFinder";
            this.btn_openPathFinder.Size = new System.Drawing.Size(31, 23);
            this.btn_openPathFinder.TabIndex = 6;
            this.btn_openPathFinder.Text = "...";
            this.btn_openPathFinder.UseVisualStyleBackColor = true;
            this.btn_openPathFinder.Click += new System.EventHandler(this.btn_openPathFinder_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(26, 89);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(193, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "패치를 적용할 거상 폴더 경로 지정";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.label4.Location = new System.Drawing.Point(12, 243);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(109, 12);
            this.label4.TabIndex = 8;
            this.label4.Text = "테스트 버전입니다.";
            // 
            // btn_applyPatch
            // 
            this.btn_applyPatch.Location = new System.Drawing.Point(191, 238);
            this.btn_applyPatch.Name = "btn_applyPatch";
            this.btn_applyPatch.Size = new System.Drawing.Size(75, 23);
            this.btn_applyPatch.TabIndex = 9;
            this.btn_applyPatch.Text = "패치 적용";
            this.btn_applyPatch.UseVisualStyleBackColor = true;
            this.btn_applyPatch.Click += new System.EventHandler(this.btn_applyPatch_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(463, 273);
            this.Controls.Add(this.btn_applyPatch);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btn_openPathFinder);
            this.Controls.Add(this.tb_gersangPath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btn_startPatch);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tb_version);
            this.Controls.Add(this.btn_checkVersion);
            this.Name = "Form1";
            this.Text = "GersangPatchMaster";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_checkVersion;
        private System.Windows.Forms.TextBox tb_version;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_startPatch;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb_gersangPath;
        private System.Windows.Forms.Button btn_openPathFinder;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button btn_applyPatch;
    }
}


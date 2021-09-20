
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.btn_checkVersion = new System.Windows.Forms.Button();
            this.tb_version = new System.Windows.Forms.TextBox();
            this.label_patchDate = new System.Windows.Forms.Label();
            this.btn_startPatch = new System.Windows.Forms.Button();
            this.tb_gersangPath = new System.Windows.Forms.TextBox();
            this.btn_openPathFinder = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.btn_applyPatch = new System.Windows.Forms.Button();
            this.radio_mainServer = new System.Windows.Forms.RadioButton();
            this.radio_testServer = new System.Windows.Forms.RadioButton();
            this.group_selectServer = new System.Windows.Forms.GroupBox();
            this.group_path = new System.Windows.Forms.GroupBox();
            this.group_version = new System.Windows.Forms.GroupBox();
            this.label_frontVersionCount = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.group_multiClient = new System.Windows.Forms.GroupBox();
            this.radio_single = new System.Windows.Forms.RadioButton();
            this.radio_multi = new System.Windows.Forms.RadioButton();
            this.group_multiClientName = new System.Windows.Forms.GroupBox();
            this.label_3client = new System.Windows.Forms.Label();
            this.label_2client = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btn_patch = new System.Windows.Forms.Button();
            this.check_keepFile = new System.Windows.Forms.CheckBox();
            this.check_shortcut = new System.Windows.Forms.CheckBox();
            this.pic_github = new System.Windows.Forms.PictureBox();
            this.linkLabel_github = new System.Windows.Forms.LinkLabel();
            this.pic_naver = new System.Windows.Forms.PictureBox();
            this.linkLabel_blog = new System.Windows.Forms.LinkLabel();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.group_selectServer.SuspendLayout();
            this.group_path.SuspendLayout();
            this.group_version.SuspendLayout();
            this.group_multiClient.SuspendLayout();
            this.group_multiClientName.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pic_github)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_naver)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_checkVersion
            // 
            this.btn_checkVersion.Location = new System.Drawing.Point(95, 24);
            this.btn_checkVersion.Name = "btn_checkVersion";
            this.btn_checkVersion.Size = new System.Drawing.Size(75, 23);
            this.btn_checkVersion.TabIndex = 0;
            this.btn_checkVersion.Text = "버전 확인";
            this.btn_checkVersion.UseVisualStyleBackColor = true;
            this.btn_checkVersion.Click += new System.EventHandler(this.btn_checkVersion_Click);
            // 
            // tb_version
            // 
            this.tb_version.Location = new System.Drawing.Point(17, 25);
            this.tb_version.MaxLength = 5;
            this.tb_version.Name = "tb_version";
            this.tb_version.Size = new System.Drawing.Size(72, 21);
            this.tb_version.TabIndex = 1;
            this.tb_version.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tb_version_KeyPress);
            // 
            // label_patchDate
            // 
            this.label_patchDate.AutoSize = true;
            this.label_patchDate.Font = new System.Drawing.Font("굴림", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_patchDate.Location = new System.Drawing.Point(176, 30);
            this.label_patchDate.Name = "label_patchDate";
            this.label_patchDate.Size = new System.Drawing.Size(107, 11);
            this.label_patchDate.TabIndex = 2;
            this.label_patchDate.Text = "(hide) 패치게시날짜";
            // 
            // btn_startPatch
            // 
            this.btn_startPatch.Location = new System.Drawing.Point(30, 592);
            this.btn_startPatch.Name = "btn_startPatch";
            this.btn_startPatch.Size = new System.Drawing.Size(75, 23);
            this.btn_startPatch.TabIndex = 3;
            this.btn_startPatch.Text = "패치 다운";
            this.btn_startPatch.UseVisualStyleBackColor = true;
            this.btn_startPatch.Click += new System.EventHandler(this.btn_startPatch_Click);
            // 
            // tb_gersangPath
            // 
            this.tb_gersangPath.Location = new System.Drawing.Point(17, 25);
            this.tb_gersangPath.Name = "tb_gersangPath";
            this.tb_gersangPath.ReadOnly = true;
            this.tb_gersangPath.ShortcutsEnabled = false;
            this.tb_gersangPath.Size = new System.Drawing.Size(253, 21);
            this.tb_gersangPath.TabIndex = 5;
            this.tb_gersangPath.TabStop = false;
            // 
            // btn_openPathFinder
            // 
            this.btn_openPathFinder.Location = new System.Drawing.Point(277, 24);
            this.btn_openPathFinder.Name = "btn_openPathFinder";
            this.btn_openPathFinder.Size = new System.Drawing.Size(31, 23);
            this.btn_openPathFinder.TabIndex = 6;
            this.btn_openPathFinder.Text = "...";
            this.btn_openPathFinder.UseVisualStyleBackColor = true;
            this.btn_openPathFinder.Click += new System.EventHandler(this.btn_openPathFinder_Click);
            // 
            // btn_applyPatch
            // 
            this.btn_applyPatch.Location = new System.Drawing.Point(29, 563);
            this.btn_applyPatch.Name = "btn_applyPatch";
            this.btn_applyPatch.Size = new System.Drawing.Size(75, 23);
            this.btn_applyPatch.TabIndex = 9;
            this.btn_applyPatch.Text = "패치 적용";
            this.btn_applyPatch.UseVisualStyleBackColor = true;
            this.btn_applyPatch.Click += new System.EventHandler(this.btn_applyPatch_Click);
            // 
            // radio_mainServer
            // 
            this.radio_mainServer.AutoSize = true;
            this.radio_mainServer.Checked = true;
            this.radio_mainServer.Location = new System.Drawing.Point(18, 20);
            this.radio_mainServer.Name = "radio_mainServer";
            this.radio_mainServer.Size = new System.Drawing.Size(59, 16);
            this.radio_mainServer.TabIndex = 10;
            this.radio_mainServer.TabStop = true;
            this.radio_mainServer.Text = "본서버";
            this.radio_mainServer.UseVisualStyleBackColor = true;
            this.radio_mainServer.Click += new System.EventHandler(this.radio_mainServer_Click);
            // 
            // radio_testServer
            // 
            this.radio_testServer.AutoSize = true;
            this.radio_testServer.Location = new System.Drawing.Point(95, 20);
            this.radio_testServer.Name = "radio_testServer";
            this.radio_testServer.Size = new System.Drawing.Size(83, 16);
            this.radio_testServer.TabIndex = 11;
            this.radio_testServer.Text = "테스트서버";
            this.radio_testServer.UseVisualStyleBackColor = true;
            this.radio_testServer.Click += new System.EventHandler(this.radio_testServer_Click);
            // 
            // group_selectServer
            // 
            this.group_selectServer.Controls.Add(this.radio_mainServer);
            this.group_selectServer.Controls.Add(this.radio_testServer);
            this.group_selectServer.Location = new System.Drawing.Point(12, 11);
            this.group_selectServer.Name = "group_selectServer";
            this.group_selectServer.Size = new System.Drawing.Size(186, 50);
            this.group_selectServer.TabIndex = 12;
            this.group_selectServer.TabStop = false;
            this.group_selectServer.Text = "서버선택";
            this.group_selectServer.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // group_path
            // 
            this.group_path.Controls.Add(this.tb_gersangPath);
            this.group_path.Controls.Add(this.btn_openPathFinder);
            this.group_path.Location = new System.Drawing.Point(12, 67);
            this.group_path.Name = "group_path";
            this.group_path.Size = new System.Drawing.Size(323, 62);
            this.group_path.TabIndex = 13;
            this.group_path.TabStop = false;
            this.group_path.Text = "거상 설치 경로";
            // 
            // group_version
            // 
            this.group_version.Controls.Add(this.label_frontVersionCount);
            this.group_version.Controls.Add(this.label5);
            this.group_version.Controls.Add(this.tb_version);
            this.group_version.Controls.Add(this.btn_checkVersion);
            this.group_version.Controls.Add(this.label_patchDate);
            this.group_version.Location = new System.Drawing.Point(12, 137);
            this.group_version.Name = "group_version";
            this.group_version.Size = new System.Drawing.Size(323, 149);
            this.group_version.TabIndex = 14;
            this.group_version.TabStop = false;
            this.group_version.Text = "패치버전";
            this.group_version.Enter += new System.EventHandler(this.groupBox3_Enter);
            // 
            // label_frontVersionCount
            // 
            this.label_frontVersionCount.AutoSize = true;
            this.label_frontVersionCount.Location = new System.Drawing.Point(16, 120);
            this.label_frontVersionCount.Name = "label_frontVersionCount";
            this.label_frontVersionCount.Size = new System.Drawing.Size(281, 12);
            this.label_frontVersionCount.TabIndex = 4;
            this.label_frontVersionCount.Text = "(hide) 00000->00000 (총 0개의 패치가 존재합니다)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 78);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(93, 12);
            this.label5.TabIndex = 3;
            this.label5.Text = "현재 거상 버전 :";
            // 
            // group_multiClient
            // 
            this.group_multiClient.Controls.Add(this.radio_single);
            this.group_multiClient.Controls.Add(this.radio_multi);
            this.group_multiClient.Location = new System.Drawing.Point(12, 292);
            this.group_multiClient.Name = "group_multiClient";
            this.group_multiClient.Size = new System.Drawing.Size(89, 89);
            this.group_multiClient.TabIndex = 15;
            this.group_multiClient.TabStop = false;
            this.group_multiClient.Text = "다클라 여부";
            this.group_multiClient.Enter += new System.EventHandler(this.groupBox4_Enter);
            // 
            // radio_single
            // 
            this.radio_single.AutoSize = true;
            this.radio_single.Checked = true;
            this.radio_single.Location = new System.Drawing.Point(17, 24);
            this.radio_single.Name = "radio_single";
            this.radio_single.Size = new System.Drawing.Size(53, 16);
            this.radio_single.TabIndex = 12;
            this.radio_single.TabStop = true;
            this.radio_single.Text = "1클라";
            this.radio_single.UseVisualStyleBackColor = true;
            // 
            // radio_multi
            // 
            this.radio_multi.AutoSize = true;
            this.radio_multi.Location = new System.Drawing.Point(17, 56);
            this.radio_multi.Name = "radio_multi";
            this.radio_multi.Size = new System.Drawing.Size(53, 16);
            this.radio_multi.TabIndex = 13;
            this.radio_multi.Text = "3클라";
            this.radio_multi.UseVisualStyleBackColor = true;
            this.radio_multi.Click += new System.EventHandler(this.radio_multi_Click);
            // 
            // group_multiClientName
            // 
            this.group_multiClientName.Controls.Add(this.label_3client);
            this.group_multiClientName.Controls.Add(this.label_2client);
            this.group_multiClientName.Controls.Add(this.textBox2);
            this.group_multiClientName.Controls.Add(this.textBox1);
            this.group_multiClientName.Location = new System.Drawing.Point(109, 292);
            this.group_multiClientName.Name = "group_multiClientName";
            this.group_multiClientName.Size = new System.Drawing.Size(226, 89);
            this.group_multiClientName.TabIndex = 16;
            this.group_multiClientName.TabStop = false;
            this.group_multiClientName.Text = "다클라 폴더 이름";
            // 
            // label_3client
            // 
            this.label_3client.AutoSize = true;
            this.label_3client.Location = new System.Drawing.Point(13, 58);
            this.label_3client.Name = "label_3client";
            this.label_3client.Size = new System.Drawing.Size(35, 12);
            this.label_3client.TabIndex = 3;
            this.label_3client.Text = "3클라";
            // 
            // label_2client
            // 
            this.label_2client.AutoSize = true;
            this.label_2client.Location = new System.Drawing.Point(13, 28);
            this.label_2client.Name = "label_2client";
            this.label_2client.Size = new System.Drawing.Size(35, 12);
            this.label_2client.TabIndex = 2;
            this.label_2client.Text = "2클라";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(60, 51);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(151, 21);
            this.textBox2.TabIndex = 1;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(60, 24);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(151, 21);
            this.textBox1.TabIndex = 0;
            // 
            // btn_patch
            // 
            this.btn_patch.Location = new System.Drawing.Point(192, 398);
            this.btn_patch.Name = "btn_patch";
            this.btn_patch.Size = new System.Drawing.Size(143, 46);
            this.btn_patch.TabIndex = 17;
            this.btn_patch.Text = "패치 시작";
            this.btn_patch.UseVisualStyleBackColor = true;
            this.btn_patch.Click += new System.EventHandler(this.button1_Click);
            // 
            // check_keepFile
            // 
            this.check_keepFile.AutoSize = true;
            this.check_keepFile.Location = new System.Drawing.Point(13, 398);
            this.check_keepFile.Name = "check_keepFile";
            this.check_keepFile.Size = new System.Drawing.Size(100, 16);
            this.check_keepFile.TabIndex = 18;
            this.check_keepFile.Text = "패치파일 유지";
            this.check_keepFile.UseVisualStyleBackColor = true;
            // 
            // check_shortcut
            // 
            this.check_shortcut.AutoSize = true;
            this.check_shortcut.Location = new System.Drawing.Point(13, 428);
            this.check_shortcut.Name = "check_shortcut";
            this.check_shortcut.Size = new System.Drawing.Size(152, 16);
            this.check_shortcut.TabIndex = 19;
            this.check_shortcut.Text = "바탕화면 바로가기 생성";
            this.check_shortcut.UseVisualStyleBackColor = true;
            // 
            // pic_github
            // 
            this.pic_github.Image = ((System.Drawing.Image)(resources.GetObject("pic_github.Image")));
            this.pic_github.Location = new System.Drawing.Point(6, 505);
            this.pic_github.Name = "pic_github";
            this.pic_github.Size = new System.Drawing.Size(16, 16);
            this.pic_github.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pic_github.TabIndex = 20;
            this.pic_github.TabStop = false;
            // 
            // linkLabel_github
            // 
            this.linkLabel_github.AutoSize = true;
            this.linkLabel_github.Location = new System.Drawing.Point(28, 509);
            this.linkLabel_github.Name = "linkLabel_github";
            this.linkLabel_github.Size = new System.Drawing.Size(42, 12);
            this.linkLabel_github.TabIndex = 21;
            this.linkLabel_github.TabStop = true;
            this.linkLabel_github.Text = "GitHub";
            // 
            // pic_naver
            // 
            this.pic_naver.Image = ((System.Drawing.Image)(resources.GetObject("pic_naver.Image")));
            this.pic_naver.Location = new System.Drawing.Point(6, 483);
            this.pic_naver.Name = "pic_naver";
            this.pic_naver.Size = new System.Drawing.Size(16, 16);
            this.pic_naver.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pic_naver.TabIndex = 22;
            this.pic_naver.TabStop = false;
            // 
            // linkLabel_blog
            // 
            this.linkLabel_blog.AutoSize = true;
            this.linkLabel_blog.Location = new System.Drawing.Point(28, 487);
            this.linkLabel_blog.Name = "linkLabel_blog";
            this.linkLabel_blog.Size = new System.Drawing.Size(30, 12);
            this.linkLabel_blog.TabIndex = 23;
            this.linkLabel_blog.TabStop = true;
            this.linkLabel_blog.Text = "Blog";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(298, 487);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(37, 12);
            this.label8.TabIndex = 24;
            this.label8.Text = "v1.0.0";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(215, 509);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(120, 12);
            this.label9.TabIndex = 25;
            this.label9.Text = "Made By byungmeo";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(347, 644);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.linkLabel_blog);
            this.Controls.Add(this.pic_naver);
            this.Controls.Add(this.linkLabel_github);
            this.Controls.Add(this.pic_github);
            this.Controls.Add(this.check_shortcut);
            this.Controls.Add(this.check_keepFile);
            this.Controls.Add(this.btn_patch);
            this.Controls.Add(this.group_multiClientName);
            this.Controls.Add(this.group_multiClient);
            this.Controls.Add(this.group_version);
            this.Controls.Add(this.group_path);
            this.Controls.Add(this.group_selectServer);
            this.Controls.Add(this.btn_applyPatch);
            this.Controls.Add(this.btn_startPatch);
            this.Name = "Form1";
            this.Text = "GersangPatchMaster";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.group_selectServer.ResumeLayout(false);
            this.group_selectServer.PerformLayout();
            this.group_path.ResumeLayout(false);
            this.group_path.PerformLayout();
            this.group_version.ResumeLayout(false);
            this.group_version.PerformLayout();
            this.group_multiClient.ResumeLayout(false);
            this.group_multiClient.PerformLayout();
            this.group_multiClientName.ResumeLayout(false);
            this.group_multiClientName.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pic_github)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_naver)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_checkVersion;
        private System.Windows.Forms.TextBox tb_version;
        private System.Windows.Forms.Label label_patchDate;
        private System.Windows.Forms.Button btn_startPatch;
        private System.Windows.Forms.TextBox tb_gersangPath;
        private System.Windows.Forms.Button btn_openPathFinder;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button btn_applyPatch;
        private System.Windows.Forms.RadioButton radio_mainServer;
        private System.Windows.Forms.RadioButton radio_testServer;
        private System.Windows.Forms.GroupBox group_selectServer;
        private System.Windows.Forms.GroupBox group_path;
        private System.Windows.Forms.GroupBox group_version;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label_frontVersionCount;
        private System.Windows.Forms.GroupBox group_multiClient;
        private System.Windows.Forms.RadioButton radio_single;
        private System.Windows.Forms.RadioButton radio_multi;
        private System.Windows.Forms.GroupBox group_multiClientName;
        private System.Windows.Forms.Label label_3client;
        private System.Windows.Forms.Label label_2client;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btn_patch;
        private System.Windows.Forms.CheckBox check_keepFile;
        private System.Windows.Forms.CheckBox check_shortcut;
        private System.Windows.Forms.PictureBox pic_github;
        private System.Windows.Forms.LinkLabel linkLabel_github;
        private System.Windows.Forms.PictureBox pic_naver;
        private System.Windows.Forms.LinkLabel linkLabel_blog;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
    }
}


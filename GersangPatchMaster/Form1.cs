using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Windows.Forms;


namespace GersangPatchMaster
{   
    public partial class Form1 : Form
    {
        string patchInfoDir;
        Uri infoUrl;
        string version;

        public Form1()
        {
            //프로그램 실행시 Data 폴더 확인 및 없을경우 Data 폴더 생성
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(Application.StartupPath + @"\PatchInfoFiles");
            if (!di.Exists) { di.Create(); }
            patchInfoDir = di.ToString();

            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            version = textBox1.Text;

            //패치버전을 입력하지 않았다면 메시지를 출력합니다.
            if(string.IsNullOrEmpty(version))
            {
                MessageBox.Show("패치버전을 입력해주세요.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            //패치버전은 반드시 숫자로 5자 입니다.
            if(version.Length != 5)
            {
                MessageBox.Show("올바른 버전을 입력해주세요. (5자리가 아닙니다!)\n예시) 30417", "오류", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            //해당 패치 버전이 언제 거상 패치 서버에 게시된 것인지 확인합니다.
            try
            {
                //코드 출처 : https://docs.microsoft.com/ko-kr/dotnet/api/system.exception?view=net-5.0

                Uri myUri = new Uri(@"http://akgersang.xdn.kinxcdn.com/Gersang/Patch/Gersang_Server/Client_info_File/" + version);
                // Creates an HttpWebRequest for the specified URL.
                HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(myUri);
                HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();

                DateTime today = DateTime.Now;

                // Uses the LastModified property to compare with today's date.
                if (DateTime.Compare(today, myHttpWebResponse.LastModified) == 0)
                {
                    Console.WriteLine("\nThe requested URI entity was modified today");
                    label1.Text = "오늘 게시된 업데이트입니다.";
                }
                else
                {
                    if (DateTime.Compare(today, myHttpWebResponse.LastModified) == 1)
                    {
                        Console.WriteLine("\nThe requested URI was last modified on:{0}",
                                            myHttpWebResponse.LastModified);
                        label1.Text = myHttpWebResponse.LastModified.ToString() + "\n에 게시된 업데이트입니다.";
                    }
                }

                infoUrl = myUri;
                // Releases the resources of the response.
                myHttpWebResponse.Close();
            } catch(System.Net.WebException exception)
            {
                MessageBox.Show("유효하지 않은 버전입니다. 거상 홈페이지 공지사항에 게시된 버전을 입력해주세요.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);  
                Console.WriteLine(exception);
                button2.Enabled = false;
                label2.Text = "선택 버전 : ";
                label1.Text = "패치 버전을 확인해주세요.";
                return;
            }

            button2.Enabled = true;
            label2.Text = "선택 버전 : v" + version;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            //코드 출처 : https://stackoverflow.com/questions/463299/how-do-i-make-a-textbox-that-only-accepts-numbers

            //버전 명은 숫자 외에 입력이 되지 않도록 합니다.

            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //infoUrl = new Uri("http://akgersang.xdn.kinxcdn.com/Gersang/Patch/Gersang_Server/Client_info_File/" + version); //거상 패치 정보 파일이 저장된 서버 링크
            string targetdownloadedFile = patchInfoDir + @"\" + version; //패치 정보 파일이 저장될 위치와 파일명

            DownloadManager downloadManager = new DownloadManager();
            downloadManager.DownloadFile(infoUrl.ToString(), targetdownloadedFile); //패치 정보 파일을 PatchInfoFiles 폴더에 설치합니다.


            //버전 확인때 유효하지 않은 버전인지 확인 하였지만, 보험으로 한번 더 체크한다. (빈 파일이라면 잘못된 버전이라는 뜻)
            FileInfo infoFile = new FileInfo(targetdownloadedFile);
            if (infoFile.Length == 0)
            {
                MessageBox.Show("유효하지 않은 버전입니다. 거상 홈페이지 공지사항에 게시된 버전을 입력해주세요.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                infoFile.Delete();
                return;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            button2.Enabled = false; //버전 확인을 해야 패치 시작이 가능합니다.
        }
    }
}

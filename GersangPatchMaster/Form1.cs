using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace GersangPatchMaster
{   
    public partial class Form1 : Form
    {
        string patchInfoDir;
        Uri infoUrl;
        string version;
        int patchFileCount;
        int downloadCompletedCount;
        Stopwatch sw;

        public Form1()
        {
            //프로그램 실행시 Data 폴더 확인 및 없을경우 Data 폴더 생성
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(Application.StartupPath + @"\PatchInfoFiles");
            if (!di.Exists) { di.Create(); }
            patchInfoDir = di.ToString();

            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            button2.Enabled = false; //버전 확인을 해야 패치 시작이 가능합니다.
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
            patchFileCount = 0;
            downloadCompletedCount = 0;

            //패치 정보 파일을 다운로드 받아온다
            string patchInfoFilePath = patchInfoDir + @"\" + version; //패치 정보 파일이 저장될 위치와 파일명

            WebClient infoDownloader = new WebClient();
            infoDownloader.DownloadFile(infoUrl, patchInfoFilePath); //패치 정보 파일이 완료되기 전에 패치 정보 파일에 접근해버려서 동기식으로 다운
            //downloadFile(infoUrl, patchInfoFilePath); //패치 정보 파일을 PatchInfoFiles 폴더에 설치합니다.

            //패치 정보 파일에서 패치 파일 리스트를 뽑아낸다
            var files = new Dictionary<string, string>(); //key값으로 패치파일의경로, Value값으로 패치파일다운로드주소를 저장합니다.

            string[] lines = File.ReadAllLines(patchInfoFilePath, Encoding.Default); //패치정보파일에서 모든 텍스트를 읽어옵니다.
            patchFileCount = lines.Length - 5; //쓸모없는4줄 + EOF줄
            Console.WriteLine("다운받아야하는 패치 파일의 갯수 : " + patchFileCount);

            //패치정보파일의 첫 4줄은 쓸모없으므로 생략하고, 5번째 줄부터 읽습니다.
            for (int i = 4; i < lines.Length; i++)
            {
                string[] row = lines[i].Split('\t'); //한 줄을 탭을 간격으로 나눕니다.

                //만약 EOF가 등장했다면 루프를 빠져나갑니다.
                if (row[0] == ";EOF")
                {
                    break;
                }

                //
                string patchFilePath = row[3] + row[1];
                string patchFileUri = @"http://akgersang.xdn.kinxcdn.com/Gersang/Patch/Gersang_Server/Client_Patch_File" + patchFilePath.Replace('\\', '/');

                files.Add(patchFilePath, patchFileUri);
            }

            //버전이름의 폴더가 없다면 생성합니다.
            System.IO.DirectoryInfo versionDirectory = new System.IO.DirectoryInfo(Application.StartupPath + @"\" + version);
            if (!versionDirectory.Exists) { versionDirectory.Create(); }

            sw = new Stopwatch();
            sw.Start();
            //버전이름의 폴더에 패치 파일을 다운로드합니다.
            foreach (var file in files)
            {
                string filePath = versionDirectory.ToString() + @"\" + file.Key;
                Uri uri = new Uri(file.Value.ToString());
                downloadFile(uri, filePath);
            }
        }

        private async void downloadFile(Uri downloadUrl, string filePath)
        {
            WebClient client = new WebClient();

            // 마치 사용자가 브라우저에서 작업하는 것처럼 꾸미는 코드?
            client.Headers.Add("User-Agent", "Mozilla/4.0 (compatible; MSIE 8.0)");

            string fileName = filePath.Substring(filePath.LastIndexOf('\\') + 1); //파일이름만 추출합니다

            string withoutFileName = filePath.Remove(filePath.LastIndexOf('\\') + 1); //폴더경로만 추출합니다

            //패치 파일은 지정된 경로에 다운됩니다. 해당 폴더가 없다면 생성합니다.
            System.IO.DirectoryInfo fileDirectory = new System.IO.DirectoryInfo(withoutFileName);
            if (!fileDirectory.Exists) { fileDirectory.Create(); }

            //하나의 패치 파일 다운로드를 완료하면, 콘솔에 메시지를 출력합니다.
            client.DownloadFileCompleted += (object sender, AsyncCompletedEventArgs e) =>
            {
                // display completion status.
                if (e.Error != null)
                {
                    Console.WriteLine("다운로드 중 예기치 못한 오류가 발생하였습니다!");
                    Console.WriteLine("오류 메시지 : " + e.Error.Message);
                } else
                {
                    Console.WriteLine(fileName + " 다운로드 완료! " + (patchFileCount - (++downloadCompletedCount)) + "개 남음!");
                    if(downloadCompletedCount == patchFileCount)
                    {
                        Console.WriteLine("모든 패치파일 다운로드 완료!");
                        sw.Stop();
                        Console.WriteLine("다운로드 완료까지 " + sw.ElapsedMilliseconds.ToString() + "ms초 경과");
                    }
                }
            };

            //지정된 경로에 패치 파일 다운로드를 시작합니다.
            client.DownloadFileAsync(downloadUrl, filePath);
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Windows.Forms;
using SearchOption = System.IO.SearchOption;

namespace GersangPatchMaster {
    public partial class Form1 : Form {
        //
        private string patchInfoDir;
        private Uri infoUrl;
        private string version;
        private int patchFileCount;
        private int downloadCompletedCount;
        private Stopwatch sw;
        //

        private string patchUrl;

        public Form1() {
            this.MaximizeBox = false;

            //프로그램 실행시 Data 폴더 확인 및 없을경우 Data 폴더 생성
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(Application.StartupPath + @"\PatchInfoFiles");
            if (!di.Exists) { di.Create(); }
            patchInfoDir = di.ToString();

            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
            if (radio_testServer.Checked)
                patchUrl = @"http://akgersang.xdn.kinxcdn.com/Gersang/Patch/Test_Server/";
            else
                patchUrl = @"http://akgersang.xdn.kinxcdn.com/Gersang/Patch/Gersang_Server/";

            tb_gersangPath.Text = @"G:\AKInteractive\Gersang";
            label_patchDate.Text = "";
            label_frontVersionCount.Text = "";

            //저장되어있는 거상 설치 경로가 유효한지 확인합니다.
        }

        //////////////////
        //서버 선택 그룹//
        //////////////////
        private void radio_mainServer_Click(object sender, EventArgs e) {
            this.patchUrl = @"http://akgersang.xdn.kinxcdn.com/Gersang/Patch/Gersang_Server/";
        }

        private void radio_testServer_Click(object sender, EventArgs e) {
            this.patchUrl = @"http://akgersang.xdn.kinxcdn.com/Gersang/Patch/Test_Server/";
        }

        //////////////////
        //거상 경로 그룹//
        //////////////////
        private void btn_openPathFinder_Click(object sender, EventArgs e) {
            folderBrowserDialog1.SelectedPath = null;
            folderBrowserDialog1.ShowDialog(); //PathFinder 열기

            string selectedPath = folderBrowserDialog1.SelectedPath;

            try {
                //Gersang.exe 파일이 없는 경우 거상 폴더가 아니라고 메시지를 띄웁니다.
                if (Directory.GetFiles(selectedPath, "Gersang.exe", SearchOption.TopDirectoryOnly).Length <= 0) {
                    MessageBox.Show("제대로 된 거상 경로를 지정해주세요. (Gersang.exe 파일이 있는 경로)"
                        , "오류", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                    return;
                }
            } catch(Exception ex) {
                MessageBox.Show(ex.Message);
            }
            
            tb_gersangPath.Text = folderBrowserDialog1.SelectedPath;
        }

        //////////////////
        //패치버전  그룹//
        //////////////////
        private void btn_checkVersion_Click(object sender, EventArgs e) {
            //거상 설치 경로를 지정하지 않은 경우
            if(tb_gersangPath.TextLength == 0) {
                MessageBox.Show("거상 설치 경로를 지정해주세요.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            version = tb_version.Text;

            //패치버전을 입력하지 않은 경우
            if (string.IsNullOrEmpty(version)) {
                MessageBox.Show("패치버전을 입력해주세요.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            //패치버전은 반드시 숫자로 5자 입니다.
            if(version.Length != 5) {
                MessageBox.Show("올바른 버전을 입력해주세요. (5자리가 아닙니다!)\n예시) 30417", "오류", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            //현재 거상 버전을 확인합니다
            FileStream fs = System.IO.File.OpenRead(tb_gersangPath.Text + @"/Online/vsn.dat");
            BinaryReader br = new System.IO.BinaryReader(fs);
            int currentVer = -(br.ReadInt32() + 1);
            label_currentVersion.Text = "현재 거상 버전 : " + currentVer;
            Console.WriteLine("현재 본클라 버전은? : " + currentVer);
            fs.Close();
            br.Close();
            //

            //같은 버전 예외 처리는 게시날짜 확인 후 진행
            if (Int16.Parse(version) < currentVer) {
                MessageBox.Show("구버전 입니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                if(!check_oldVersion.Checked)
                    return;
            }

            label_frontVersionCount.Text = version + "->" + currentVer;

            //해당 패치 버전이 언제 거상 패치 서버에 게시된 것인지 확인합니다.
            try {
                Uri myUri = new Uri(patchUrl + @"Client_info_File/" + version);

                // Creates an HttpWebRequest for the specified URL.
                HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(myUri);
                HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();

                DateTime today = DateTime.Now;

                // Uses the LastModified property to compare with today's date.
                if (DateTime.Compare(today, myHttpWebResponse.LastModified) == 0) {
                    label_patchDate.Text = "오늘 게시된 업데이트입니다.";
                } else {
                    if (DateTime.Compare(today, myHttpWebResponse.LastModified) == 1) {
                        label_patchDate.Text = "패치 게시날짜 : \n" + myHttpWebResponse.LastModified.ToString();
                    }
                }

                infoUrl = myUri;

                // Releases the resources of the response.
                myHttpWebResponse.Close();
            } catch(System.Net.WebException exception) {
                MessageBox.Show("존재하지 않는 버전입니다. 다른 버전을 입력해주세요."
                    , "오류", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);  

                Console.WriteLine(exception);
                return;
            }

            if (Int16.Parse(version) == currentVer) {
                //같은 버전이라도 서버점검 끝나기 전 파일 바꿔치기 하는 경우 있으므로, 물어봄
                DialogResult dr = MessageBox.Show("현재 설치된 버전과 동일한 버전입니다. 그래도 설치하시겠습니까?\n" +
                    "거상 점검이 끝나기전 미리 패치를 다운받으실 때,\n" +
                    "AK측에서 가끔 파일을 바꾸는 경우가 있습니다.", "똑같은 버전", MessageBoxButtons.YesNo);

                if (dr == DialogResult.No) {
                    return;
                } else {
                    /*
                    System.Net.WebClient tempWebClient = new System.Net.WebClient();
                    tempWebClient.Headers.Add("User-Agent", "Run");
                    string downloadUrl = patchUrl + @"Client_info_File/" + version;
                    tempWebClient.DownloadFile(new Uri(downloadUrl), patchInfoDir + @"\" + version + ".txt");
                    return;
                    */
                }
            }

            //최종 버전까지 필요한 패치 정보 파일을 모두 다운받습니다. (구버전 배려)
            System.Net.WebClient webClient = new System.Net.WebClient();
            webClient.Headers.Add("User-Agent", "Run");

            int patchCount = 0;
            for (int i = 30414 + 1; i <= Int16.Parse(version); i++) {
                string downloadUrl = patchUrl + @"Client_info_File/" + i;
                try {
                    webClient.DownloadFile(new Uri(downloadUrl), patchInfoDir + @"\" + i + ".txt");
                    label_debug.Text = i + " 버전 패치정보 파일 다운로드 성공\r\n";

                    /*
                    txt_LogBox.Text += i + " 버전 패치파일 다운로드 및 압축해제 시작\r\n";
                    DownloadPatchFiles(i - 1, i);
                    txt_LogBox.Text += i + " 버전 패치파일 다운로드 및 압축해제 완료\r\n";
                    */
                    patchCount++;
                } catch (Exception err) {
                    //패치파일이 존재하지 않으므로 다음으로 진행
                }
            }

            label_frontVersionCount.Text = currentVer + "->" + version + " (총 " + patchCount + "번의 패치가 존재합니다.)";

            //몇번의 패치가 존재하든, 한꺼번에 패치하기위해 패치정보파일을 새로 만듭니다.
            if(patchCount > 1) {
                using (StreamWriter wr = new StreamWriter(Application.StartupPath + @"\PatchInfoFiles\" + 30414 + "-" + version + ".txt")) {
                    wr.WriteLine("Hello World!");
                }
            }
        }

        private void tb_version_KeyPress(object sender, KeyPressEventArgs e) {
            //코드 출처 : https://stackoverflow.com/questions/463299/how-do-i-make-a-textbox-that-only-accepts-numbers

            //버전 명은 숫자 외에 입력이 되지 않도록 합니다.

            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.')) {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1)) {
                e.Handled = true;
            }
        }

        //////////////////
        //다클라여부그룹//
        //////////////////
        private void radio_multi_Click(object sender, EventArgs e) {
            //현재 거상 설치 경로가 NTFS인지 확인
            //띄어쓰기 확인??(안해도 됨)
        }

        //////////////////
        //다클폴더  그룹//
        //////////////////



        //////////////////
        //그룹없는컨트롤//
        //////////////////
        //패치 시작 확인 버튼
        private void btn_startPatch_Click(object sender, EventArgs e) {
            //거상 경로 텍스트박스가 비어있는 경우
            if(tb_gersangPath.TextLength == 0) {
                MessageBox.Show("거상 경로를 지정해주세요.");
                return;
            }

            //경로에 Gersang.exe파일이 없는 경우
            if (Directory.GetFiles(tb_gersangPath.Text, "Gersang.exe", System.IO.SearchOption.TopDirectoryOnly).Length <= 0) {
                MessageBox.Show("제대로 된 거상 경로를 지정해주세요. (Gersang.exe 파일이 있는 경로)");
                return;
            }

            //다운로드 하는 동안 폼을 비활성화 시킵니다.
            this.Enabled = false;

            patchFileCount = 0;
            downloadCompletedCount = 0;

            //현재 버전 확인 코드 출처 : 기존 GersangPatcher 개발자분의 코드 
            System.IO.FileStream fs = System.IO.File.OpenRead(tb_gersangPath.Text + "\\Online\\vsn.dat");
            System.IO.BinaryReader br = new System.IO.BinaryReader(fs);
            int currentVer = -(br.ReadInt32() + 1);
            Console.WriteLine("현재 본클라 버전은? : " + currentVer);
            fs.Close();
            br.Close();
            //

            //패치 정보 파일을 다운로드 한다.
            string patchInfoFilePath = patchInfoDir + @"\" + version; //패치 정보 파일이 저장될 위치와 파일명
            WebClient infoDownloader = new WebClient();
            infoDownloader.DownloadFile(infoUrl, patchInfoFilePath); //패치 정보 파일이 완료되기 전에 패치 정보 파일에 접근해버려서 동기식으로 다운

            //패치 정보 파일에서 패치 파일 리스트를 뽑아낸다
            Dictionary<string, string> files = new Dictionary<string, string>(); //key값으로 패치파일의경로, Value값으로 패치파일다운로드주소를 저장합니다.

            string[] lines = File.ReadAllLines(patchInfoFilePath, Encoding.Default); //패치정보파일에서 모든 텍스트를 읽어옵니다.
            patchFileCount = lines.Length - 5; //쓸모없는4줄 + EOF줄
            Console.WriteLine("다운받아야하는 패치 파일의 갯수 : " + patchFileCount);

            //패치정보파일의 첫 4줄은 쓸모없으므로 생략하고, 5번째 줄부터 읽습니다.
            for (int i = 4; i < lines.Length; i++) {
                string[] row = lines[i].Split('\t'); //한 줄을 탭을 간격으로 나눕니다.

                //만약 EOF가 등장했다면 루프를 빠져나갑니다.
                if (row[0] == ";EOF") {
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
            //버전이름의 폴더에 패치 파일을 다운로드합니다. (버전이름_unpack 폴더에는 압축해제된 패치 파일)
            foreach (var file in files) {
                string filePath = versionDirectory.ToString() + file.Key;
                Uri uri = new Uri(file.Value.ToString());
                downloadFile(uri, filePath);
            }
        }

        

        private void btn_applyPatch_Click(object sender, EventArgs e) {
            DirectoryInfo dirPath = new System.IO.DirectoryInfo(Application.StartupPath + @"\" + version);

            //압축해제한 패치 파일을 지정한 경로의 본클라에 적용시킵니다.
            copyFolder(Application.StartupPath + '\\' + version, tb_gersangPath.Text);

            MessageBox.Show("패치 적용이 완료되었습니다.");
        }

        //패치파일 거상경로에 복사-붙여놓기(덮어쓰기)
        private void copyFolder(string srcPath, string destPath) {
            int count = 0;

            //Now Create all of the directories
            foreach (string dirPath in Directory.GetDirectories(srcPath, "*", System.IO.SearchOption.AllDirectories))
                Directory.CreateDirectory(dirPath.Replace(srcPath, destPath));

            //Copy all the files & Replaces any files with the same name
            foreach (string newPath in Directory.GetFiles(srcPath, "*.*", System.IO.SearchOption.AllDirectories)) {
                File.Copy(newPath, newPath.Replace(srcPath, destPath), true);
                count++;
            }

            Console.WriteLine("총 파일 복사 갯수 : " + count);
            //FileSystem.CopyDirectory(srcPath, destPath, UIOption.AllDialogs);
        }

        //파일 다운로드 코드 (
        private void downloadFile(Uri downloadUrl, string filePath) {
            string fileName = filePath.Substring(filePath.LastIndexOf('\\') + 1); //파일이름만 추출합니다

            //이미 패치 파일을 다운로드한 경우, 다운받지 않습니다.
            //Console.WriteLine("Exist : " + filePath.Remove(filePath.Length - 4));
            if (File.Exists(filePath.Remove(filePath.Length - 4))) {
                Console.WriteLine(fileName + "는 이미 존재합니다! " + (patchFileCount - (++downloadCompletedCount)) + "개 남음!");
                if (downloadCompletedCount == patchFileCount) {
                    Console.WriteLine("모든 패치파일 다운로드 및 압축해제 완료!");
                    sw.Stop();
                    Console.WriteLine("다운로드 완료까지 " + sw.ElapsedMilliseconds.ToString() + "ms초 경과");

                    this.Enabled = true;
                }

                return;
            }

            using (WebClient client = new WebClient()) {
                // 마치 사용자가 브라우저에서 작업하는 것처럼 꾸미는 코드?
                client.Headers.Add("User-Agent", "Mozilla/4.0 (compatible; MSIE 8.0)");

                //패치 파일은 지정된 경로에 다운됩니다. 해당 폴더가 없다면 생성합니다.
                DirectoryInfo fileDirectory = new DirectoryInfo(new FileInfo(filePath).DirectoryName);
                if (!fileDirectory.Exists) { fileDirectory.Create(); }


                //하나의 패치 파일 다운로드를 완료하면, 압축해제 후 압축파일을 삭제하는 콜백메서드를 연결합니다.
                client.DownloadFileCompleted += (object sender, AsyncCompletedEventArgs e) => {
                    // display completion status.
                    if (e.Error != null) {
                        Console.WriteLine("다운로드 중 예기치 못한 오류가 발생하였습니다!");
                        Console.WriteLine("오류 메시지 : " + e.Error.Message);
                    } else {
                        Console.WriteLine(fileName + " 다운로드 완료! " + (patchFileCount - (++downloadCompletedCount)) + "개 남음!");
                        try {
                            ZipFile.ExtractToDirectory(filePath, new FileInfo(filePath).DirectoryName);
                            File.Delete(filePath);
                            Console.WriteLine(fileName + " 압축해제 및 압축파일 삭제 완료!");
                        } catch(Exception ex) {
                            Console.WriteLine("downloadFileCompleted : " + ex.Message);
                        }

                        if (downloadCompletedCount == patchFileCount) {
                            Console.WriteLine("모든 패치파일 다운로드 및 압축해제 완료!");
                            sw.Stop();
                            Console.WriteLine("다운로드 완료까지 " + sw.ElapsedMilliseconds.ToString() + "ms초 경과");

                            this.Enabled = true;
                        }
                    }
                };

                //지정된 경로에 패치 파일 다운로드를 시작합니다.
                client.DownloadFileAsync(downloadUrl, filePath);
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e) {

        }

        private void groupBox3_Enter(object sender, EventArgs e) {

        }

        private void groupBox4_Enter(object sender, EventArgs e) {

        }

        private void button1_Click(object sender, EventArgs e) {

        }

        private void label8_Click(object sender, EventArgs e) {

        }
    }
}

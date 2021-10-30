using IWshRuntimeLibrary;
using Octokit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;

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
        private Dictionary<string, string> patchFileList;
        private bool isVersionChecked;

        public Form1() {
            checkUpdate();

            this.MaximizeBox = false;
            isVersionChecked = false;

            //프로그램 실행시 Data 폴더 확인 및 없을경우 Data 폴더 생성
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(System.Windows.Forms.Application.StartupPath + @"\PatchInfoFiles");
            if (!di.Exists) { di.Create(); }
            patchInfoDir = di.ToString();

            InitializeComponent();
        }

        private async void checkUpdate() {
            try {
                //Get all releases from GitHub
                //Source: https://octokitnet.readthedocs.io/en/latest/getting-started/
                GitHubClient client = new GitHubClient(new ProductHeaderValue("Byungmeo"));
                IReadOnlyList<Release> releases = await client.Repository.Release.GetAll("byungmeo", "GersangPatchMaster");
                //Setup the versions
                Version latestGitHubVersion = new Version(releases[0].TagName);
                Version localVersion = new Version("1.0.0"); //Replace this with your local version. 
                                                             //Only tested with numeric values.

                Console.WriteLine("깃허브에 마지막으로 게시된 버전 : " + latestGitHubVersion);
                //Compare the Versions
                //Source: https://stackoverflow.com/questions/7568147/compare-version-numbers-without-using-split-function
                int versionComparison = localVersion.CompareTo(latestGitHubVersion);
                if (versionComparison < 0) {
                    //The version on GitHub is more up to date than this local release.
                    Console.WriteLine("구버전입니다! 업데이트 메시지박스를 출력합니다!");

                    DialogResult dr = MessageBox.Show(releases[0].Body + "\n\n업데이트 하시겠습니까? (GitHub 접속)",
                        "업데이트 안내", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                    if(dr == DialogResult.Yes) {
                        System.Diagnostics.Process.Start("https://github.com/byungmeo/GersangPatchMaster/releases/latest");
                    }
                } else if (versionComparison > 0) {
                    //This local version is greater than the release version on GitHub.
                    Console.WriteLine("깃허브에 릴리즈된 버전보다 최신입니다!");
                } else {
                    //This local Version and the Version on GitHub are equal.
                    Console.WriteLine("현재 버전은 최신버전입니다!");
                }
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e) {
            label_patchDate.Text = "";
            label_frontVersionCount.Text = "";

            bool isTestServer = Properties.Settings.Default.isTestServer;
            string gersangPath = Properties.Settings.Default.gersangPath;

            if (isTestServer)
                radio_testServer.PerformClick();
            else
                radio_mainServer.PerformClick();

            if (radio_testServer.Checked)
                patchUrl = @"http://akgersang.xdn.kinxcdn.com/Gersang/Patch/Test_Server/";
            else
                patchUrl = @"http://akgersang.xdn.kinxcdn.com/Gersang/Patch/Gersang_Server/";

            if(gersangPath.Length != 0) {
                try {
                    if (Directory.GetFiles(gersangPath, "Gersang.exe", SearchOption.TopDirectoryOnly).Length <= 0) {
                        MessageBox.Show("거상 설치 경로가 현재 유효하지 않습니다. 다시 설정해주세요."
                            , "오류", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                        tb_gersangPath.Clear();
                        return;
                    }
                    tb_gersangPath.Text = gersangPath;
                } catch(Exception ex) {
                    Console.WriteLine("Form1_Load_Error...\n" + ex.Message);
                }
            }
        }

        //////////////////
        //서버 선택 그룹//
        //////////////////
        private void radio_mainServer_Click(object sender, EventArgs e) {
            if (tb_gersangPath.TextLength != 0) {
                try {
                    //GerSangKRTest.ini 파일이 있는 경우 본서버 폴더가 아니라고 메시지를 띄웁니다.
                    if (Directory.GetFiles(tb_gersangPath.Text, "GerSangKRTest.ini", SearchOption.TopDirectoryOnly).Length > 0) {
                        DialogResult dr = MessageBox.Show("현재 지정된 거상 경로가 테스트서버 클라이언트인 것 같습니다.\n계속 진행하시겠습니까?"
                            , "거상 경로", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (dr == DialogResult.No) {
                            radio_testServer.PerformClick();
                            return;
                        }
                    }
                } catch (Exception ex) {
                    MessageBox.Show(ex.Message);
                }
            }

            this.patchUrl = @"http://akgersang.xdn.kinxcdn.com/Gersang/Patch/Gersang_Server/";
            Properties.Settings.Default.isTestServer = false;
            Properties.Settings.Default.Save();
        }

        private void radio_testServer_Click(object sender, EventArgs e) {
            if(tb_gersangPath.TextLength != 0) {
                try {
                    //GerSangKRTest.ini 파일이 없는 경우 테스트서버 폴더가 아니라고 메시지를 띄웁니다.
                    if (Directory.GetFiles(tb_gersangPath.Text, "GerSangKRTest.ini", SearchOption.TopDirectoryOnly).Length <= 0) {
                        DialogResult dr = MessageBox.Show("현재 지정된 거상 경로가 본서버 클라이언트인 것 같습니다.\n그래도 계속 진행하시겠습니까?"
                            , "거상 경로", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if(dr == DialogResult.No) {
                            radio_mainServer.PerformClick();
                            return;
                        }
                    }
                } catch (Exception ex) {
                    MessageBox.Show(ex.Message);
                }
            }

            this.patchUrl = @"http://akgersang.xdn.kinxcdn.com/Gersang/Patch/Test_Server/";
            Properties.Settings.Default.isTestServer = true;
            Properties.Settings.Default.Save();
        }

        //////////////////
        //거상 경로 그룹//
        //////////////////
        private void btn_openPathFinder_Click(object sender, EventArgs e) {
            folderBrowserDialog1.SelectedPath = null;

            DialogResult dr = folderBrowserDialog1.ShowDialog(); //PathFinder 열기
            if(dr == DialogResult.Cancel) {
                return;
            }

            string selectedPath = folderBrowserDialog1.SelectedPath;

            DirectoryInfo di = new DirectoryInfo(selectedPath);

            try {
                //Gersang.exe 파일이 없는 경우 거상 폴더가 아니라고 메시지를 띄웁니다.
                if (di.GetFiles("Gersang.exe", SearchOption.TopDirectoryOnly).Length <= 0) {
                    MessageBox.Show("제대로 된 거상 경로를 지정해주세요. (Gersang.exe 파일이 있는 경로)"
                        , "오류", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                    return;
                }

                bool isTestServer;
                if(di.GetFiles("GerSangKRTest.ini", SearchOption.TopDirectoryOnly).Length > 0) {
                    isTestServer = true;
                } else {
                    isTestServer = false;
                }

                if (radio_mainServer.Checked && isTestServer) {
                    dr = MessageBox.Show("해당 경로는 테스트서버 경로인 것 같습니다.\n그래도 계속 진행하시겠습니까?"
                        , "거상 경로", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (dr != DialogResult.Yes) {
                        return;
                    }
                } 
                
                if(!radio_mainServer.Checked && !isTestServer) {
                    dr = MessageBox.Show("해당 경로는 본서버 경로인 것 같습니다.\n그래도 계속 진행하시겠습니까?"
                        , "거상 경로", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (dr != DialogResult.Yes) {
                        return;
                    }
                }
            } catch(Exception ex) {
                MessageBox.Show(ex.Message);
            }
            
            tb_gersangPath.Text = folderBrowserDialog1.SelectedPath;
            Properties.Settings.Default.gersangPath = folderBrowserDialog1.SelectedPath;
            Properties.Settings.Default.Save();
        }

        //////////////////f
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
            }

            label_frontVersionCount.Text = currentVer + "->" + version;

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

                isVersionChecked = false;
                Console.WriteLine(exception);
                return;
            }

            List<string> patchList = new List<string>();
            bool isSameVersion = false;

            if (Int16.Parse(version) == currentVer) {
                //같은 버전이라도 서버점검 끝나기 전 파일 바꿔치기 하는 경우 있으므로, 물어봄
                DialogResult dr = MessageBox.Show("현재 설치된 버전과 동일한 버전입니다. 그래도 설치하시겠습니까?\n" +
                    "거상 점검이 끝나기전 미리 패치를 다운받으실 때,\n" +
                    "AK측에서 가끔 파일을 바꾸는 경우가 있습니다.", "똑같은 버전", MessageBoxButtons.YesNo);

                if (dr == DialogResult.No) {
                    return;
                } else {
                    System.Net.WebClient tempWebClient = new System.Net.WebClient();
                    tempWebClient.Headers.Add("User-Agent", "Run");
                    string downloadUrl = patchUrl + @"Client_info_File/" + version;
                    tempWebClient.DownloadFile(new Uri(downloadUrl), patchInfoDir + @"\" + version + ".txt");
                    isSameVersion = true;
                    patchList.Add(version);
                }
            }

            if (!isSameVersion) {
                //최종 버전까지 필요한 패치 정보 파일을 모두 다운받습니다. (구버전 배려)
                System.Net.WebClient webClient = new System.Net.WebClient();
                webClient.Headers.Add("User-Agent", "Mozilla/4.0 (compatible; MSIE 8.0)");

                for (int i = currentVer + 1; i <= Int16.Parse(version); i++) {
                    string downloadUrl = patchUrl + @"Client_info_File/" + i;
                    try {
                        webClient.DownloadFile(new Uri(downloadUrl), patchInfoDir + @"\" + i + ".txt");
                        InsertLog(i + " 버전 패치정보 파일 다운로드 성공\r\n");

                        /*
                        txt_LogBox.Text += i + " 버전 패치파일 다운로드 및 압축해제 시작\r\n";
                        DownloadPatchFiles(i - 1, i);
                        txt_LogBox.Text += i + " 버전 패치파일 다운로드 및 압축해제 완료\r\n";
                        */
                        patchList.Add(i.ToString());
                    } catch (Exception ex) {
                        //다운로드 실패 시 다음 버전으로 넘어갑니다
                        //Console.WriteLine(ex);
                    }
                }
            }
            

            label_frontVersionCount.Text = currentVer + "->" + version + " (총 " + patchList.Count + "번의 패치가 존재합니다.)";


            //패치 정보 파일에서 패치 파일 리스트를 뽑아낸다
            Dictionary<string, string> files = new Dictionary<string, string>(); //key값으로 파일이름, value값으로 경로 저장

            //몇번의 패치가 존재하든, 한꺼번에 패치하기위해 여러 패치정보파일에서 중복없이 파일 리스트를 뽑아옵니다.
            if (patchList.Count >= 1) {
                using (StreamWriter wr = new StreamWriter(patchInfoDir + @"\" + currentVer + "-" + version + ".txt")) { //디버깅용으로 새로운 정보 파일을 생성합니다.
                    wr.WriteLine("파일명\t경로"); //디버깅용
                    foreach (string item in patchList) {
                        string[] lines = System.IO.File.ReadAllLines(patchInfoDir + @"\" + item + ".txt", Encoding.Default); //패치정보파일에서 모든 텍스트를 읽어옵니다.

                        //패치정보파일의 첫 4줄은 쓸모없으므로 생략하고, 5번째 줄부터 읽습니다.
                        for (int i = 4; i < lines.Length; i++) {
                            string[] row = lines[i].Split('\t'); //한 줄을 탭을 간격으로 나눕니다. (디버깅용)

                            //만약 EOF가 등장했다면 루프를 빠져나갑니다.
                            if (row[0] == ";EOF") {
                                break;
                            }

                            if(!files.ContainsKey(row[1])) {
                                files.Add(row[1], row[3].Remove(0,1));
                                wr.WriteLine(row[1] + "\t" + row[3].Remove(0,1)); //디버깅용
                            }
                        }
                    }
                }
            }

            patchFileList = files;
            isVersionChecked = true;
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
        private void radio_single_Click(object sender, EventArgs e) {
            group_multiClientName.Enabled = false;
        }

        private void radio_multi_Click(object sender, EventArgs e) {
            //현재 거상 설치 경로가 NTFS인지 확인
            //띄어쓰기 확인??(안해도 됨)
            group_multiClientName.Enabled = true;
        }

        //////////////////
        //다클폴더  그룹//
        //////////////////
        private void check_second_CheckedChanged(object sender, EventArgs e) {
            CheckBox c = (CheckBox)sender;
            if(c.Checked) {
                tb_second.Enabled = true;
            } else {
                tb_second.Enabled = false;
            }
        }

        private void check_third_CheckedChanged(object sender, EventArgs e) {
            CheckBox c = (CheckBox)sender;
            if (c.Checked) {
                tb_third.Enabled = true;
            } else {
                tb_third.Enabled = false;
            }
        }


        //////////////////
        //그룹없는컨트롤//
        //////////////////

        //패치 시작 버튼
        private void btn_patch_Click(object sender, EventArgs e) {
            //거상 경로 텍스트박스가 비어있는 경우
            if(tb_gersangPath.TextLength == 0) {
                MessageBox.Show("거상 경로를 지정해주세요.");
                return;
            }

            //패치버전 텍스트박스가 비어있는 경우
            if (tb_version.TextLength == 0) {
                MessageBox.Show("버전을 지정해주세요.");
                return;
            }

            //버전 확인을 안 한 경우
            if(!isVersionChecked) {
                MessageBox.Show("\"버전 확인\" 버튼을 눌러 버전을 확인해주세요.");
                return;
            }

            //3클라에 체크하였으나, 클라 폴더 이름이 비어있는경우
            if(radio_multi.Checked) {
                if(check_second.Checked && tb_second.TextLength == 0) {
                    MessageBox.Show("2클 폴더 이름을 입력 해주세요.");
                    return;
                }

                if(check_third.Checked && tb_third.TextLength == 0) {
                    MessageBox.Show("3클 폴더 이름을 입력 해주세요.");
                    return;
                }

                //3클라 체크했으면서 아무 클라도 사용하지 않는 경우
                if(!check_second.Checked && !check_third.Checked) {
                    MessageBox.Show("1클라만 패치를 적용합니다.");
                    radio_single.PerformClick();
                }
            }

            //다운로드 하는 동안 폼을 비활성화 시킵니다.
            this.Enabled = false;

            patchFileCount = patchFileList.Count;
            downloadCompletedCount = 0;

            //버전이름의 폴더가 없다면 생성합니다.
            System.IO.DirectoryInfo versionDirectory = new System.IO.DirectoryInfo(System.Windows.Forms.Application.StartupPath + @"\" + version);
            if (!versionDirectory.Exists) { versionDirectory.Create(); }

            sw = new Stopwatch();
            sw.Start();

            //버전이름의 폴더에 패치 파일을 다운로드합니다.
            string fileServerLink = patchUrl + "Client_Patch_File/";
            foreach (var item in patchFileList) {
                Uri fileLink = new Uri(fileServerLink + item.Value + item.Key);
                string filePath = versionDirectory.FullName + @"\" + item.Value + item.Key;
                downloadFile(fileLink, filePath);
            }

            isVersionChecked = false;
        }

        //다클라 생성
        private void CreateClient() {
            ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo();
            Process p = new System.Diagnostics.Process();

            psi.FileName = @"cmd";
            psi.WorkingDirectory = tb_gersangPath.Text + @"\..";
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = false; //IF WANT DEBUG, SET "TRUE"
            psi.RedirectStandardInput = true;
            psi.RedirectStandardError = true;

            p.StartInfo = psi;
            p.Start();

            //CMD 구문 실행 (p는 AKInteractive 폴더에 머물러있는상태)
            Debug.WriteLine("CreateDirectory 진입 전");
            CreateDirectory(ref p);
            Debug.WriteLine("CreateSymbolicLink 진입 전");
            CreateSymbolicLink(ref p);
            //
            Thread.Sleep(500);

            Debug.WriteLine("CopyFile 진입 전");
            CopyFile();
            Debug.WriteLine("CopyFile 진입 종료");

            p.StandardInput.Close();
            p.WaitForExit();
            p.Close();
        }

        private void CreateSymbolicLink(ref Process p) {
            string sourcePath = tb_gersangPath.Text;
            string secondPath = sourcePath + @"\..\" + tb_second.Text;
            string thirdPath = sourcePath + @"\..\" + tb_third.Text;

            //char, eft, fnt, music, Online, pal, tempeft, Temporary Autopath, tile, yfnt, XIGNCODE
            string[] targetDirectorys = { "char", "eft", "fnt", "music", "Online", "pal", "tempeft", "Temporary Autopath", "tile", "yfnt", "XIGNCODE" };
            List<string> targetDirectorysList = new List<string>(targetDirectorys);

            Debug.WriteLine("CreateSymbolicLink - Online관련 진입 전");

            //본클과 클라들이 세팅을 서로 독립적으로 유지하고싶을때
            FixedOnlineDirectory(secondPath, ref p);
            FixedOnlineDirectory(thirdPath, ref p);
            targetDirectorysList.Remove("Online"); //Online폴더의 심볼릭링크를 생성하지않도록 리스트에서 삭제
            

            Debug.WriteLine("CreateSymbolicLink - foreach관련 진입 전");
            //최종적으로 targetDirectorysList에 있는 폴더들을 심볼릭링크로 생성합니다.
            foreach (string target in targetDirectorysList) {
                //mklink /d \Gersang\char \Gersang2\char
                InsertLog(@"mklink /d """ + secondPath + @"\" + target + @""" """ + sourcePath + @"\" + target + @"""" + Environment.NewLine + "\n");
                InsertLog(@"mklink /d """ + thirdPath + @"\" + target + @""" """ + sourcePath + @"\" + target + @"""" + Environment.NewLine + "\n");

                p.StandardInput.Write(@"mklink /d """ + secondPath + @"\" + target + @""" """ + sourcePath + @"\" + target + @"""" + Environment.NewLine);
                p.StandardInput.Write(@"mklink /d """ + thirdPath + @"\" + target + @""" """ + sourcePath + @"\" + target + @"""" + Environment.NewLine);
            }
        }

        private void CopyFile() {
            //file, .dll, .exe, .ln, .gcs, .gts, .ini, .ico
            string fileName;
            string destFile;
            string sourcePath = tb_gersangPath.Text;
            string secondPath = sourcePath + @"\..\" + tb_second.Text;
            string thirdPath = sourcePath + @"\..\" + tb_third.Text;

            if (Directory.Exists(sourcePath)) {
                var Files = Directory.GetFiles(sourcePath, "*.*", SearchOption.TopDirectoryOnly)
                .Where(s => s.EndsWith(".dll") || s.EndsWith(".exe") || s.EndsWith(".ln") || s.EndsWith(".gcs")
                || s.EndsWith(".gts") || s.EndsWith(".ini") || s.EndsWith(".ico"));

                foreach (string s in Files) {
                    fileName = Path.GetFileName(s);

                    //second
                    destFile = Path.Combine(secondPath, fileName);
                    System.IO.File.Copy(s, destFile, true);

                    //third
                    destFile = Path.Combine(thirdPath, fileName);
                    System.IO.File.Copy(s, destFile, true);
                }
            }
        }

        private void CreateDirectory(ref Process p) {
            p.StandardInput.Write(@"mkdir " + tb_second.Text + Environment.NewLine);
            p.StandardInput.Write(@"mkdir " + tb_third.Text + Environment.NewLine);
        }

        private bool IsSymbolic(string path) {
            FileInfo pathInfo = new FileInfo(path);
            return pathInfo.Attributes.HasFlag(FileAttributes.ReparsePoint);
        }

        //Online폴더는 예외적으로 작업
        private void FixedOnlineDirectory(string subClientPath, ref Process p) {
            string sourcePath = tb_gersangPath.Text + @"\Online";
            string onlinePath = subClientPath + @"\Online";

            //이미 생성된 Online 폴더가 있을 경우
            if (Directory.Exists(onlinePath)) {
                //심볼릭 링크인지 확인합니다. 심볼릭이라면, 세팅을 공유하고있으므로, 삭제하고 일반 폴더를 만듭니다.
                if (IsSymbolic(onlinePath)) {
                    Directory.Delete(onlinePath);
                    Directory.CreateDirectory(onlinePath);
                }
            } else {
                //애초에 처음 클라폴더를 생성하는 경우입니다
                Directory.CreateDirectory(onlinePath);
            }

            //이미 파일이 있는 경우를 제외하고 본클의 Online\위치에있는 파일들을 복사해옵니다.
            //2021-08-19 : KeySetting.dat만 복사하고, 나머지는 심블릭파일로 만든다
            foreach (string target in Directory.GetFiles(sourcePath, "*.*", SearchOption.TopDirectoryOnly)) {
                //Path.Combine함수 특징상 @"\"를 앞에 붙이지 말 것 (상대경로 절대경로 이해)
                //target = 전체경로
                //fileName = 경로제외하고 파일 이름만 (ex : KeySetting.dat)
                string fileName = new FileInfo(target).Name;

                //KeySetting.dat은 복사
                if (fileName == "KeySetting.dat") {

                    if (!System.IO.File.Exists(onlinePath + @"\" + fileName)) {
                        string destFile = Path.Combine(onlinePath, fileName);
                        System.IO.File.Copy(target, destFile, true);
                    }
                } else {
                    InsertLog(@"mklink """ + onlinePath + @"\" + fileName + @""" """ + sourcePath + @"\" + fileName + @"""" + Environment.NewLine + "\n");
                    //그 외 파일은 심볼릭으로 (파일은 /d 옵션이 필요없다)
                    p.StandardInput.Write(@"mklink """ + onlinePath + @"\" + fileName + @""" """ + sourcePath + @"\" + fileName + @"""" + Environment.NewLine);
                }
            }

            //본클 Online폴더-하위폴더들의 심볼릭링크를 만듭니다.
            foreach (string target in Directory.GetDirectories(sourcePath, "*.*", SearchOption.TopDirectoryOnly)) {
                string dirName = @"\" + new DirectoryInfo(target).Name;
                InsertLog(@"mklink /d """ + onlinePath + dirName + @""" """ + sourcePath + dirName + @"""" + Environment.NewLine + "\n");
                p.StandardInput.Write(@"mklink /d """ + onlinePath + dirName + @""" """ + sourcePath + dirName + @"""" + Environment.NewLine);
            }
        }

        private void applyPatch() {
            DirectoryInfo dirPath = new System.IO.DirectoryInfo(System.Windows.Forms.Application.StartupPath + @"\" + version);

            try {
                //압축해제한 패치 파일을 지정한 경로의 본클라에 적용시킵니다.
                copyFolder(System.Windows.Forms.Application.StartupPath + '\\' + version, tb_gersangPath.Text);
                MessageBox.Show("패치 적용이 완료되었습니다.");
            } catch(Exception ex) {
                MessageBox.Show("패치 복사-붙여넣기 중 오류가 발생하였습니다.\n거상이 켜져있는지 확인해주세요.");
            }
        }

        //패치파일 거상경로에 복사-붙여놓기(덮어쓰기)
        private void copyFolder(string srcPath, string destPath) {
            int count = 0;

            try {
                //Now Create all of the directories
                foreach (string dirPath in Directory.GetDirectories(srcPath, "*", System.IO.SearchOption.AllDirectories))
                    Directory.CreateDirectory(dirPath.Replace(srcPath, destPath));

                //Copy all the files & Replaces any files with the same name
                foreach (string newPath in Directory.GetFiles(srcPath, "*.*", System.IO.SearchOption.AllDirectories)) {
                    System.IO.File.Copy(newPath, newPath.Replace(srcPath, destPath), true);
                    count++;
                }

                Console.WriteLine("총 파일 복사 갯수 : " + count);
                //FileSystem.CopyDirectory(srcPath, destPath, UIOption.AllDialogs);
            } catch (Exception ex) {
                throw (ex);
            }
        }

        //파일 다운로드 코드 (
        private void downloadFile(Uri downloadUrl, string filePath) {
            string fileName = filePath.Substring(filePath.LastIndexOf('\\') + 1); //파일이름만 추출합니다

            //이미 패치 파일을 다운로드한 경우, 다운받지 않습니다.
            //Console.WriteLine("Exist : " + filePath.Remove(filePath.Length - 4));
            if (System.IO.File.Exists(filePath.Remove(filePath.Length - 4))) {
                InsertLog(fileName + "는 이미 존재합니다! " + (patchFileCount - (++downloadCompletedCount)) + "개 남음!\n");
                if (downloadCompletedCount == patchFileCount) {
                    InsertLog("모든 패치파일 다운로드 및 압축해제 완료!\n");
                    sw.Stop();
                    InsertLog("다운로드 완료까지 " + sw.ElapsedMilliseconds.ToString() + "ms초 경과\n");

                    if (check_noApply.Checked) {
                        MessageBox.Show("패치 수동 적용을 선택하셨습니다.\n" +
                            "패치 파일은 버전명으로 된 폴더에 들어있습니다.\n" +
                            "적용시키고자 하신다면, 거상 폴더에 복사-붙여놓기 하시기 바랍니다.");
                    } else {
                        applyPatch();
                        InsertLog("패치 적용 완료!\n");
                    }

                    if (radio_multi.Checked) {
                        CreateClient();
                        InsertLog("다클라 생성 완료!\n");
                    }

                    if (check_shortcut.Checked) {
                        CreateShortcut();
                        InsertLog("바로가기 생성 완료!\n");
                    }

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
                        InsertLog("다운로드 중 예기치 못한 오류가 발생하였습니다!");
                        InsertLog("오류 메시지 : " + e.Error.Message);
                    } else {
                        InsertLog(fileName + " 다운로드 완료! " + (patchFileCount - (++downloadCompletedCount)) + "개 남음!\n");
                        try {
                            ZipFile.ExtractToDirectory(filePath, new FileInfo(filePath).DirectoryName);
                            System.IO.File.Delete(filePath);
                        } catch(Exception ex) {
                            Console.WriteLine("downloadFileCompleted : " + ex.Message);
                        }

                        //다운로드가 다 되었다면, 시간을 측정하고, 패치 적용 및 바로가기를 생성합니다.
                        if (downloadCompletedCount == patchFileCount) {
                            InsertLog("모든 패치파일 다운로드 및 압축해제 완료!\n");
                            sw.Stop();
                            InsertLog("다운로드 완료까지 " + sw.ElapsedMilliseconds.ToString() + "ms초 경과\n");

                            if (check_noApply.Checked) {
                                MessageBox.Show("패치 수동 적용을 선택하셨습니다.\n" +
                                    "패치 파일은 버전명으로 된 폴더에 들어있습니다.\n" +
                                    "적용시키고자 하신다면, 거상 폴더에 복사-붙여놓기 하시기 바랍니다.");
                            } else {
                                applyPatch();
                                InsertLog("패치 적용 완료!\n");
                            }

                            if (radio_multi.Checked) {
                                CreateClient();
                                InsertLog("다클라 생성 완료!\n");
                            }

                            if (check_shortcut.Checked) {
                                CreateShortcut();
                                InsertLog("바로가기 생성 완료!\n");
                            }

                            this.Enabled = true;
                        }
                    }
                };

                //지정된 경로에 패치 파일 다운로드를 시작합니다.
                client.DownloadFileAsync(downloadUrl, filePath);
            }
        }

        //바로가기 생성
        private void CreateShortcut() {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            DirectoryInfo directoryInfo = new DirectoryInfo(desktopPath);

            string sourcePath = tb_gersangPath.Text;
            string[] temp = sourcePath.Split('\\');
            string sourceName = temp[temp.Length - 1];

            WshShell wsh = new WshShell();

            string firstName = desktopPath.ToString() + @"\" + sourceName + ".lnk";
            FileInfo firstShortcut = new FileInfo(firstName);
            if (!firstShortcut.Exists) {
                IWshShortcut firstWshShortcut = wsh.CreateShortcut(firstShortcut.FullName);
                firstWshShortcut.TargetPath = sourcePath + @"\Run.exe";
                firstWshShortcut.Save();
            }

            if (!radio_multi.Checked)
                return;

            if(check_second.Checked) {
                string secondName = desktopPath.ToString() + @"\" + tb_second.Text + ".lnk";
                FileInfo secondShortcut = new FileInfo(secondName);
                if (!secondShortcut.Exists) {
                    IWshShortcut secondWshShortcut = wsh.CreateShortcut(secondShortcut.FullName);
                    secondWshShortcut.TargetPath = sourcePath + @"\..\" + tb_second.Text + @"\Run.exe";
                    secondWshShortcut.Save();
                }
            }
            
            if(check_third.Checked) {
                string thirdName = desktopPath.ToString() + @"\" + tb_third.Text + ".lnk";
                FileInfo thirdShortcut = new FileInfo(thirdName);
                if (!thirdShortcut.Exists) {
                    IWshShortcut thirdWshShortcut = wsh.CreateShortcut(thirdShortcut.FullName);
                    thirdWshShortcut.TargetPath = sourcePath + @"\..\" + tb_third.Text + @"\Run.exe";
                    thirdWshShortcut.Save();
                }
            }
        }

        private void InsertLog(string log) {
            this.rtb_logBox.Focus();
            this.rtb_logBox.AppendText(log);  // 데이터 추가 
            this.rtb_logBox.ScrollToCaret();   // 스크롤 맨 아래로

            //int linecnt = this.rtb_logBox.Lines.Count();   // 라인 갯수 구함
            //if (linecnt > 10) this.rtb_logBox.Clear();    //  라인갯수 10 row 생성시 초기화 
        }

        private void linkLabel_blog_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            try {
                VisitBlog();
            } catch (Exception ex) {
                MessageBox.Show("Unable to open link that was clicked.");
            }
        }

        private void linkLabel_github_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            try {
                VisitGithub();
            } catch (Exception ex) {
                MessageBox.Show("Unable to open link that was clicked.");
            }
        }
        private void VisitBlog() {
            linkLabel_blog.LinkVisited = true;
            System.Diagnostics.Process.Start("https://blog.naver.com/kog5071/222272427500");
        }

        private void VisitGithub() {
            linkLabel_github.LinkVisited = true;
            System.Diagnostics.Process.Start("https://github.com/byungmeo/GersangPatchMaster/releases");
        }
    }
}

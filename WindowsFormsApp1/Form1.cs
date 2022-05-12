using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            axKHOpenAPI1.CommConnect();
            // 로그인 기능
            axKHOpenAPI1.OnEventConnect += onEventConnect;
            button1.Click += stockSearch;
            axKHOpenAPI1.OnReceiveTrData += onReceiveTrData;
            
        }

        public void onEventConnect(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnEventConnectEvent e)
        {   // dataGridView에 종목코드,종목명채우는 함수
            if (e.nErrCode == 0) {
                string 종목코드리스트 = axKHOpenAPI1.GetCodeListByMarket("0");
                string[] 종목코드 = 종목코드리스트.Split(';');
                for (int i = 0; i < 종목코드.Length; i++) {
                    dataGridView1.Rows.Add(); // 열 만들기
                    dataGridView1["종목조회_종목코드", i].Value = 종목코드[i]; // 종목코드
                    dataGridView1["종목조회_종목명", i].Value = axKHOpenAPI1.GetMasterCodeName(종목코드[i]); // 종목이름
                    string a = axKHOpenAPI1.GetMasterCodeName("종목코드");
                }
            }
        
        }

        public void onReceiveTrData(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveTrDataEvent e)
        {
            if (e.sRQName == "종목정보요청")
            {
                string 종목이름 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "종목명").Trim();
                string 현재가 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "현재가").Trim();
                string 전일대비 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "전일대비").Trim();
                string 거래량 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "거래량").Trim();
                string 등락률 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "등락율").Trim();

                label2.Text = 종목이름;
                label4.Text = 현재가.Substring(1);
                label6.Text = 전일대비;
                label8.Text = 거래량;
                label10.Text = 등락률;
            }
        }

        public void stockSearch(object sender, EventArgs e)
        {
            string 종목코드 = textBox1.Text;
            if (textBox1.Text.Length > 0) {
                axKHOpenAPI1.SetInputValue("종목코드", 종목코드);
                axKHOpenAPI1.CommRqData("종목정보요청", "opt10001", 0, "5000");
            }
        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

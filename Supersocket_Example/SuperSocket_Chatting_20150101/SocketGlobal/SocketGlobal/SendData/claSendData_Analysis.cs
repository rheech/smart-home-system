using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketGlobal.SendData
{
	/// <summary>
	/// 오리지널 데이터를 명령ep 바꿔주는 클래스 입니다.
	/// </summary>
	public class claSendData_Analysis
	{
		/// <summary>
		/// 분석전의 원본 데이터
		/// </summary>
		public dataOriginal OriginalData { get; set; }

		/// <summary>
		/// 원본 데이터를 분석한 데이터
		/// </summary>
		public dataSend DataSend;

		/// <summary>
		/// 분리된 데이터의 문자열 타입
		/// </summary>
		public string Data;
		/// <summary>
		/// 분리된 데이터의 1단 구분자로 자른 데이터
		/// </summary>
		public string[] Datas;



		public claSendData_Analysis()
		{
		}
		public claSendData_Analysis(byte[] byteOri)
		{
			//원본 저장
			dataOriginal dataOri = new dataOriginal();
			dataOri.Data = byteOri;
			this.OriginalData = dataOri;

			//클래스로 변환
			DataSend = new dataSend(this.OriginalData);
		}
		public claSendData_Analysis(dataOriginal insDataOri)
		{
			//원본 저장
			this.OriginalData = insDataOri;

			//클래스로 변환
			DataSend = new dataSend(this.OriginalData);
		}

		/// <summary>
		/// 분리된 데이터를 1단 구분자로 잘라 준다.
		/// </summary>
		public void CutData1ToDatas()
		{
			this.Data = claGlobal.g_SocketUtile.ByteToString(this.DataSend.Data_Byte);
			this.Datas = this.Data.Split(claGlobal.g_Division1);
		}

	}
}

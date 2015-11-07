using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocketGlobal.SendData;

namespace SocketGlobal.LargeData
{
	/// <summary>
	/// 큰데이터를 보내기 위한 클래스
	/// </summary>
	public class claLargeData_SendData
	{
		/// <summary>
		/// 보낼 파일 데이터
		/// </summary>
		private byte[] m_byteFile;
		/// <summary>
		/// 읽은 파일의 크기
		/// </summary>
		public int TotalLength { get; set;}

		/// <summary>
		/// 잘린 데이터의 갯수
		/// </summary>
		public int CutCount { get; set;}
		
		/// <summary>
		/// 분해한 데이터
		/// </summary>
		public List<dataSend> ListSendData;

		/// <summary>
		/// 보내는 쪽에서 구분하기위한 인덱스
		/// </summary>
		public int Index { get; set; }
		/// <summary>
		/// 서버로 부터 받은 인덱스
		/// </summary>
		public int DBIndex { get; set; }

		public claLargeData_SendData()
		{
			this.ListSendData = new List<dataSend>();

		}

		/// <summary>
		/// 지정한 파일을 버퍼에 저장한다.
		/// </summary>
		/// <param name="sDir"></param>
		public void LoadFile(string sDir)
		{	
			FileStream fs = new FileStream(sDir, FileMode.Open);
			this.TotalLength = (int)fs.Length;
			this.m_byteFile = new byte[this.TotalLength];
			fs.Read(this.m_byteFile, 0, this.TotalLength);
			fs.Close();

			//잘린갯수 계산
			this.CutCount = this.TotalLength / (claGlobal.g_CutByteSize);
		}

		/// <summary>
		/// 버퍼에 있는 데이터를 전송용 데이터로 분해 한다.
		/// </summary>
		public void CutLargeData()
		{
			dataSend insDSTemp;
			
			//데이터 자르기
			
			CutCount = 0;
			//이번컷의 오프셋 시작 위치
			int nBufOffsetStart = 0;

			//남은 바이트수
			int nRemainByte = 0;

			//자를 크기
			int nCutByteSize = claGlobal.g_CutByteSize;

			//붙일 DB인덱스 만들기
			byte[] byteDBIndex = claGlobal.g_SocketUtile.IntToByte(this.DBIndex);

			do
			{
				//남은 데이터가 있는지 확인 하고
				nRemainByte = this.m_byteFile.Length - nBufOffsetStart;
				if (nCutByteSize <= nRemainByte)
				{
					//남은 데이터가 있을때는 최대치를 컷바이트 크기로 해야 한다.
					nRemainByte = nCutByteSize;
					//그리고 남은 데이터가 없다면 파일쪼게기가 끝난 것이기 때문에 초기화 할 필요가 없다.
				}

				//데이터 생성
				insDSTemp = new dataSend();
				insDSTemp.Command = claCommand.Command.LargeData_Receive;
				insDSTemp.Data_Byte = new byte[claGlobal.g_DataHeader1Size + claGlobal.g_DataHeader2Size + nRemainByte];

				//데이터 인덱스를 붙여준다.
				Buffer.BlockCopy(byteDBIndex
									, 0
									, insDSTemp.Data_Byte
									, 0
									, claGlobal.g_DataHeader2Size);
				//데이터 카운터를 붙여준다.
				Buffer.BlockCopy(claGlobal.g_SocketUtile.IntToByte(this.CutCount)
									, 0
									, insDSTemp.Data_Byte
									, claGlobal.g_DataHeader1Size
									, claGlobal.g_DataHeader2Size);
				//데이터 복사
				Buffer.BlockCopy(this.m_byteFile
									, 0 + nBufOffsetStart
									, insDSTemp.Data_Byte
									, claGlobal.g_DataHeader1Size + claGlobal.g_DataHeader2Size
									, nRemainByte);
				
				//리스트에 추가
				this.ListSendData.Add(insDSTemp);

				//오프셋 용량을 늘려준다.
				nBufOffsetStart += nCutByteSize;

				++CutCount;
			}
			while (this.m_byteFile.Length >= nBufOffsetStart);
		}


	}
}

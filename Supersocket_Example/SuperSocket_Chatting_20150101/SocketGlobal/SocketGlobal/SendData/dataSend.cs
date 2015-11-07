using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketGlobal.SendData
{
	/// <summary>
	/// 메시지를 주고받기전에 바이트로 변환전 데이터를 가지고 있는 클래스.
	/// </summary>
	public class dataSend
	{
		/// <summary>
		/// 명령어
		/// </summary>
		public claCommand.Command Command { get; set; }

		/// <summary>
		/// 데이터(byte[]), 우선순위 1.
		/// </summary>
		public byte[] Data_Byte { get; set; }
		/// <summary>
		/// 데이터(string), 우선순위 3.
		/// 문자열리스트로된 리스트를 문자열로 변환한 데이터.
		/// </summary>
		public string Data_String { get; set; }

		/// <summary>
		/// 데이터(List), 우선순위 2.
		/// </summary>
		public List<string> Data_List { get; set; }

		private void ResetClass()
		{
			Data_List = new List<string>();
		}

		public dataSend()
		{
			ResetClass();

			this.Command = claCommand.Command.None;
			this.Data_String = "";
		}
		
		public dataSend(dataOriginal dataOri)
		{
			ResetClass();

			this.DataOriginalToThis(dataOri);
		}

		public dataSend(claCommand.Command typeCommand, params string[] sDatas)
		{
			ResetClass();

			this.Command = typeCommand;

			foreach (string sTemp in sDatas)
			{
				this.Data_List.Add(sTemp);
			}

			//입력된 데이터를 변환 한다.
			this.CreateDataSend();

		}

		/// <summary>
		/// 지정한 리스트의 내용을 구분자를 이용하여 한줄로 만든다.
		/// </summary>
		/// <param name="listString"></param>
		/// <param name="sDivision"></param>
		/// <returns></returns>
		private string ListToString(List<string> listString, string sDivision)
		{
			if( 0 >= listString.Count)
			{	//리스트에 내용이 없으면 빈값을 준다.
				return null;
			}

			//리스트에 내용이 있으면 구분자를 이용하여 한줄로 만든다.
			StringBuilder sbReturn = new StringBuilder();
			//리스트의 내용을 한줄로 만든다.
			foreach (string sFor in listString)
			{
				sbReturn.Append(sFor);
				sbReturn.Append(sDivision);
			}

			return sbReturn.ToString();
		}

		

		/// <summary>
		/// 명령어를 바이트 형태로 바꾼다.
		/// </summary>
		/// <returns></returns>
		private byte[] ByteToCommand()
		{
			//여기의 크기는 'claGlobal.g_CommandSize'로 결정함
			return claGlobal.g_SocketUtile.StringToByte(
					string.Format("{0:D4}", this.Command.GetHashCode()));
		}

		/// <summary>
		/// 데이터오리지널을 이 클래스로 만들어 줍니다.
		/// </summary>
		/// <param name="dataOri"></param>
		public void DataOriginalToThis(dataOriginal dataOri)
		{
			this.DataOriginalToThis(dataOri.Data);
		}

		private void DataOriginalToThis(byte[] byteOri)
		{
			byte[] byteTemp;

			//명령어를 잘라 붙여 넣는다.
			byteTemp = new byte[claGlobal.g_CommandSize];
			//명령어 복사
			Buffer.BlockCopy(byteOri
								, 0
								, byteTemp
								, 0
								, claGlobal.g_CommandSize);
			//명령어로 변환
			this.Command = (claCommand.Command)claGlobal.g_SocketUtile.ByteToInt(byteTemp);

			//데이터 복사
			this.Data_Byte = new byte[byteOri.Length - claGlobal.g_CommandSize];
			Buffer.BlockCopy(byteOri
								, claGlobal.g_CommandSize
								, this.Data_Byte
								, 0
								, byteOri.Length - claGlobal.g_CommandSize);
		}

		public dataOriginal CreateDataOriginal()
		{
			if ((null == Data_Byte)
				|| (0 >= Data_Byte.Length))
			{	//문자열 데이터다.
				return CreateDataOriginal_String();
			}
			else
			{	//바이트 데이터다.
				return CreateDataOriginal_Byte();
			}
		}


		private dataOriginal CreateDataOriginal_String()
		{
			//입력된 데이터를 변환 한다.
			this.CreateDataSend();

			//바이트 처리를 한다.
			return CreateDataOriginal_Byte();
		}

		private dataOriginal CreateDataOriginal_Byte()
		{
			//리턴할 보내기용 원본 데이터
			dataOriginal dataReturn = new dataOriginal();
			//데이터 공간 확보
			dataReturn.Data = new byte[claGlobal.g_CommandSize + this.Data_Byte.Length];

			//명령어 복사
			Buffer.BlockCopy(ByteToCommand()
								, 0
								, dataReturn.Data
								, 0
								, claGlobal.g_CommandSize);
			//데이터 복사
			Buffer.BlockCopy(this.Data_Byte
								, 0
								, dataReturn.Data
								, claGlobal.g_CommandSize
								, this.Data_Byte.Length);

			return dataReturn;
		}

		/// <summary>
		/// 입력된 데이터를 한줄짜리 데이터로 변환 합니다.
		/// </summary>
		private void CreateDataSend()
		{
			//문자열리스트를 한줄짜리 문자열로 변환한다.
			string sResult
				= this.ListToString(this.Data_List
									, claGlobal.g_Division1.ToString());

			if (null != sResult)
			{	//리스트에 내용이 있을 때만 리스트결과를 쓴다.
				this.Data_String = sResult;
			}

			//문자열 데이터를 바이트로 변환 해준다.
			this.Data_Byte = claGlobal.g_SocketUtile.StringToByte(this.Data_String);
		}


	}
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketGlobal.LargeData
{
	/*
	 * 큰데이터 처리는 일단 메모리에 적재시켜놨다가 업로드가 끝나면 파일을 기록하는 방식이다.
	 * 그러므로 너무 큰파일을 올리는 것은 서버에 부담을 준다.(나중에 IO를 이용한 기록방식으로 바꿀 예정)
	 */

	/// <summary>
	/// 큰데이터 메니저에서 사용하는 열거형
	/// </summary>
	public enum typeLargeDataManager
	{
		/// <summary>
		/// 기본값
		/// </summary>
		None = 0,

		/// <summary>
		/// 완료
		/// </summary>
		Complete,
		/// <summary>
		/// 일치하는 요소가 없다.
		/// </summary>
		NoItem,
	}

	/// <summary>
	/// 큰데이터를 처리하기위한 메니저
	/// </summary>
	public class claLargeData_GetManager
	{
		/// <summary>
		/// 업로드되는 파일의 인덱스로 사용될 카운터
		/// </summary>
		public int m_nFileCount = 0;

		/// <summary>
		/// 관리되는 데이터 리스트
		/// </summary>
		private List<claLargeData_GetData> m_ListGetData
			= new List<claLargeData_GetData>();

		
		/// <summary>
		/// 보내는 쪽에서 보낸 정보를 토대로 데이터를 받을 준비를 한다.
		/// </summary>
		/// <param name="typeLD"></param>
		/// <param name="sFileName"></param>
		/// <param name="nTotalLength"></param>
		/// <param name="nTotalCount"></param>
		/// <param name="nIndex"></param>
		/// <returns></returns>
		public int Start(typeLargeData typeLD
						, string sFileName
						, int nTotalLength
						, int nTotalCount
						, int nIndex)
		{
			++this.m_nFileCount;

			//리스트에 추가할 객체 만들기
			claLargeData_GetData insLargeData = new claLargeData_GetData();
			//데이터 세팅
			insLargeData.SettingData(typeLD
									, sFileName
									, nTotalLength
									, nTotalCount
									, this.m_nFileCount);

			//기본경로 세팅
			insLargeData.Dir = @"c:\";

			//리스트에 추가
			this.m_ListGetData.Add(insLargeData);

			return this.m_nFileCount;
		}

		public typeLargeDataManager AddData(byte[] byteData
											, out int nDBIndex
											, out int nCount)
		{
			typeLargeDataManager typeReturn = typeLargeDataManager.None;

			byte[] byteTemp;

			//1. DB인덱스 추출
			byteTemp = new byte[claGlobal.g_DataHeader1Size];
			Buffer.BlockCopy(byteData
								, 0
								, byteTemp
								, 0
								, claGlobal.g_DataHeader1Size);
			//람다식 오류문제때문에 임시변수를 선언해야 한다-_-;;
			int nDBIndex_Temp = claGlobal.g_SocketUtile.ByteToInt(byteTemp);
			nDBIndex = nDBIndex_Temp;

			//2. 추출한 DB인덱스를 이용해서 대상을 찾는다.
			claLargeData_GetData insLD = this.m_ListGetData.Find(x => x.DBIndex == nDBIndex_Temp);

			//3. 데이터 저장
			//대상이 있나?
			if (null != insLD)
			{	//있다!
				//3-1. 데이터 순서 추출
				byteTemp = new byte[claGlobal.g_DataHeader2Size];
				Buffer.BlockCopy(byteData
									, claGlobal.g_DataHeader1Size
									, byteTemp
									, 0
									, claGlobal.g_DataHeader2Size);
				nCount = claGlobal.g_SocketUtile.ByteToInt(byteTemp);

				//3-2. 데이터 추출
				int nCutByteSize = -1;
				if (claGlobal.g_CutByteSize > (insLD.Length_Total - insLD.Length_Now))
				{	//남은 데이터 컷데이터 보다 작다.
					//남은 크기 만큼만 자른다.
					nCutByteSize = insLD.Length_Total - insLD.Length_Now;
				}
				else
				{	//남은 데이터가 컷데이터 보다 많다.
					//1개 크기 만큼 자른다.
					nCutByteSize = claGlobal.g_CutByteSize;
				}

				byteTemp = new byte[nCutByteSize];
				Buffer.BlockCopy(byteData
									, claGlobal.g_DataHeader1Size + claGlobal.g_DataHeader1Size
									, byteTemp
									, 0
									, nCutByteSize);
				int nLength = byteTemp.Length;

				//3-3. 데이터 기록
				Buffer.BlockCopy(byteTemp
									, 0
									, insLD.Data
									, nCount  * claGlobal.g_CutByteSize
									, nCutByteSize);

				//추출한 데이터의 상태기록
				insLD.Count_Now += 1;			//데이터 갯수
				insLD.Length_Now += nLength;	//데이터 용량

				typeReturn = typeLargeDataManager.Complete;
			}
			else
			{	//없다
				typeReturn = typeLargeDataManager.NoItem;
				nCount = -1;
			}

			return typeReturn;
		}

		/// <summary>
		/// 모든 데이터가 전송되었는지 확인한다.
		/// </summary>
		/// <param name="nDBIndex"></param>
		/// <returns></returns>
		public bool CheckData(int nDBIndex)
		{
			//DB인덱스를 이용해서 대상을 찾는다.
			claLargeData_GetData insLD = this.m_ListGetData.Find(x => x.DBIndex == nDBIndex);

			return this.CheckData(insLD);			
		}

		/// <summary>
		/// 모든 데이터가 전송되었는지 확인한다.
		/// </summary>
		/// <param name="insLD_GD"></param>
		/// <returns></returns>
		public bool CheckData(claLargeData_GetData insLD_GD)
		{
			bool bReturn = false;

			if (insLD_GD.Count_Total <= insLD_GD.Count_Now)
			{
				bReturn = true;
			}
			else
			{
				bReturn = false;
			}

			return bReturn;	
		}

		/// <summary>
		/// 받은 큰데이터를 처리합니다.
		/// </summary>
		/// <param name="nDBIndex"></param>
		/// <returns></returns>
		public bool CompleteData(int nDBIndex)
		{
			bool bReturn = false;

			//DB인덱스를 이용해서 대상을 찾는다.
			claLargeData_GetData insLD = this.m_ListGetData.Find(x => x.DBIndex == nDBIndex);

			//데이터 전송이 끝났는지 확인
			if ( true == this.CheckData(insLD))
			{	//데이터를 모두 받았다.

				bReturn = true;

				switch (insLD.TypeLargeData)
				{
					case typeLargeData.BigMessage:	//큰메시지
						bReturn = Processing_BigMessage(insLD);
						break;
					case typeLargeData.File:		//파일
						bReturn = Processing_File(insLD);
						break;
					default:	//기본처리를 데이터를 처리 할 수 없음을 의미한다.
						bReturn = false;
						break;
				}
			}

			return bReturn;
		}

		private bool Processing_BigMessage(claLargeData_GetData insLD_GD)
		{
			bool bReturn = false;
			insLD_GD.DataCompleteRead = true;
			return bReturn;
		}

		/// <summary>
		/// 저장된 데이터를 파일로 저장합니다.
		/// </summary>
		/// <param name="insLD_GD"></param>
		/// <returns></returns>
		private bool Processing_File(claLargeData_GetData insLD_GD)
		{
			bool bReturn = false;

			if (true == insLD_GD.DataCompleteRead)
			{	//이미 완료 처리가 되었다.
				bReturn = true;
			}
			else
			{	//아직 완료가 되지 않았다.
				try
				{
					//저장할 파일 생성
					FileStream fs 
						= new FileStream(insLD_GD.Dir + insLD_GD.FileName
										, FileMode.Create
										, FileAccess.Write);

					//파일 저장
					BinaryWriter bw = new BinaryWriter(fs);
					bw.Write(insLD_GD.Data, 0, insLD_GD.Length_Total);
					bw.Close();

					bReturn = true;
					insLD_GD.DataCompleteRead = true;
				}
				catch
				{
					bReturn = false;
				}
			}

			return bReturn;
		}


	}
}

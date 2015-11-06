﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomClass
{
	public class claEncoding
	{
		public string ByteToString(byte[] byteData, int index, int count)
		{
			return Encoding.UTF8.GetString(byteData, index, count);
		}
		public string ByteToString(byte[] byteData)
		{
			return Encoding.UTF8.GetString(byteData);
		}
		public byte[] StringToByte(string sData)
		{
			return Encoding.UTF8.GetBytes(sData);
		}
	}
}

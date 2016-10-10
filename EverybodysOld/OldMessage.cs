using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EverybodysOld
{
	public class OldMessage
	{
		public OldMessage(string bas, params object[] stuff)
		{
			param = stuff;
			switch(bas)
			{
				case "init":
					Type = OldMessageType.Init;
					break;
				case "update":
					Type = OldMessageType.Movement;
					break;
				case "change":
					Type = OldMessageType.Block;
					break;
				case "add":
					Type = OldMessageType.Add;
					break;
				case "left":
					Type = OldMessageType.Left;
					break;
				default:
					Type = OldMessageType.Unknown;
					break;
			}
		}

		public OldMessageType Type { get; private set; }
		public object[] param { get; private set; }

		//We don't need to worry about errors.

		public string GetString(uint index)
		{
			return (param[index].ToString());
		}

		public double GetDouble(int index)
		{
			return Convert.ToDouble(param[index]);
		}

		public int GetInt(int index)
		{
			return Convert.ToInt32(param[index]);
		}
	}

	public enum OldMessageType
	{
		Unknown = -1,
		Block = 0,
		Movement = 1,
		Add = 2,
		Left = 3, 
		Init = 4
	}
}

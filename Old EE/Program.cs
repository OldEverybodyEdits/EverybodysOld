using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using EverybodysOld;

namespace Old_EE
{
	class Program
	{
		static FreeOldBot bot;

		static void Main(string[] args)
		{
			bot = new FreeOldBot();
			//Join the world and wait for the bot to connect
			bot.JoinWorld(0, 0);
			while (!bot.Connected)
			{
				Thread.Sleep(100);
			}
			//
			bot.OnMessage += bot_OnMessage;
			//It's connected
			bot.MoveToBlock(10, 10);
			Console.Write("Join");
			while (true)
			{
				try
				{
					foreach (OldPlayer i in bot.Players)
					{
						bot.PlaceBlock(Convert.ToInt16(i.X / 16), Convert.ToInt16(i.Y / 16), Block.Dot);
						bot.PlaceBlock(Convert.ToInt16(i.X / 16)-1, Convert.ToInt16(i.Y / 16), Block.Dot);
						bot.PlaceBlock(Convert.ToInt16(i.X / 16)+1, Convert.ToInt16(i.Y / 16), Block.Dot);
						bot.PlaceBlock(Convert.ToInt16(i.X / 16), Convert.ToInt16(i.Y / 16)-1, Block.Dot);
						bot.PlaceBlock(Convert.ToInt16(i.X / 16), Convert.ToInt16(i.Y / 16)+1, Block.Dot);
					}
				}
				catch
				{

				}
			}
		}

		static void bot_OnMessage(object Sender, EverybodysOld.OldMessage e)
		{
			if(e.Type == OldMessageType.Block)
			{
				//bot.PlaceBlock(e.GetInt(0), e.GetInt(1)-1, Block.Dot);
			}
		}
	}
}

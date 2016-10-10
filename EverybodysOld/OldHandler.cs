using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlayerIOClient;

namespace EverybodysOld
{
	internal static class OldHandler
	{
		public static Client OldEEClient = null;

		public static void GenerateOldEEClient(string GameId)
		{
			OldEEClient = PlayerIO.Connect(GameId, "public", "whatever", "", "");
		}

		public static Connection GetNewConnection(string RoomId)
		{
			if(OldEEClient == null)
			{
				GenerateOldEEClient("everybody-edits-old-gue3mggr0mppaimep8jw");
			}
			return OldEEClient.Multiplayer.CreateJoinRoom("0x0", "FlixelWalker1", true, null, null);
		}
	}
}

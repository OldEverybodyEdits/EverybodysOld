using PlayerIOClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EverybodysOld
{
	/// <summary>
	/// Occurrs when the bot disconnects
	/// </summary>
	/// <param name="Sender"></param>
	/// <param name="Message"></param>
	public delegate void OnDisconnect(object Sender, string Message);

	/// <summary>
	/// Occurrs when the bot recieves a message
	/// </summary>
	/// <param name="Sender"></param>
	/// <param name="e">The message</param>
	public delegate void OnMessage(object Sender, OldMessage e);

	/// <summary>
	/// An OldBot that works for Old EE
	/// </summary>
    public class OldBot
    {
		public OldBot()
		{
			con = null;
		}

		#region Variables
		internal Connection con = null;
		public event OnDisconnect OnDisconnect;
		public event OnMessage OnMessage;
		public List<OldPlayer> Players = new List<OldPlayer>();
		public Block[,] World { get { return WorldBlocks; } }
		public bool Connected = false;

		internal Block[,] WorldBlocks = new Block[100, 100];
		private string WorldId = "";
		#endregion

		/// <summary>
		/// Join a room at the certain row and column
		/// </summary>
		/// <param name="RoomX">The X of the room</param>
		/// <param name="RoomY">The Y of the room</param>
		public void JoinWorld(int RoomX, int RoomY)
		{
			if (!(RoomX > -1 && RoomX < 9))
				throw new ArgumentException("The RoomX must be greater than -1, and less than 9. 0-8");

			if (!(RoomY > -1 && RoomY < 6))
				throw new ArgumentException("The RoomY must be greater than -1, and less than 6. 0-5");

			WorldId = string.Concat(RoomX.ToString(), "x", RoomY.ToString());
			con = OldHandler.GetNewConnection(WorldId);

			con.OnMessage += MessageHandler;
			con.OnDisconnect +=	DisconnectHandler;

			con.Send("init");
		}

		/// <summary>
		/// Get a player within the list
		/// </summary>
		/// <param name="Id">The Id of the player</param>
		/// <returns></returns>
		public OldPlayer GetPlayer(int Id)
		{
			foreach(OldPlayer i in Players)
			{
				if (i.Id == Id)
					return i;
			}
			return null;
		}

		/// <summary>
		/// Place a block at X/Y, and only place it if the block isn't already in the world
		/// </summary>
		/// <param name="X">The X of the world</param>
		/// <param name="Y">The Y of the world</param>
		/// <param name="block">The block to place</param>
		public void PlaceBlock(int X, int Y, Block block)
		{
			//We only want to place blocks if we need to
			if (WorldBlocks[X, Y] != block)
			{
				con.Send("change", X, Y, (int)block);
				Thread.Sleep(1); //Not instant otherwise we'd disconnect really fast
			}
		}

		/// <summary>
		/// Move to X*16 and Y*16.
		/// </summary>
		/// <param name="X">X</param>
		/// <param name="Y">Y</param>
		public void MoveToBlock(int X, int Y)
		{
			con.Send("update", Convert.ToDouble(X * 16), Convert.ToDouble(Y * 16), 0, 0, 0, 0, 0, 0);
		}

		/// <summary>
		/// Move to X and Y
		/// </summary>
		/// <param name="X">X</param>
		/// <param name="Y">Y</param>
		public void MoveTo(double X, double Y)
		{
			con.Send("update", Convert.ToDouble(X * 16), Convert.ToDouble(Y * 16), 0, 0, 0, 0, 0, 0);
		}

		/// <summary>
		/// Move to a place
		/// </summary>
		/// <param name="X">X</param>
		/// <param name="Y">Y</param>
		/// <param name="SpeedX">SpeedX</param>
		/// <param name="SpeedY">SpeedY</param>
		/// <param name="ModifierX">ModifierX</param>
		/// <param name="ModifierY">ModifierY</param>
		/// <param name="mx">mx</param>
		/// <param name="my">my</param>
		public void Move(double X, double Y, double SpeedX, double SpeedY, double ModifierX, double ModifierY, double mx, double my)
		{
			con.Send("update", X, Y, SpeedX, SpeedY, ModifierX, ModifierY, mx, my);
		}

		/// <summary>
		/// Change the face
		/// </summary>
		/// <param name="smiley">The smiley to change to</param>
		public void ChangeFace(Smiley smiley)
		{
			con.Send("face", (int)smiley);
		}

		#region Private Message Handlers
		//Event handlers to make life easy
		private void MessageHandler(object sender, Message e)
		{
			//Old EE is terribly coded, so we have to cope with it's coding.
			switch(e.Type)
			{
				case "init":
					WorldBlocks = new Block[100, 100];
					int X = 0, Y = 0;
					string[] RoomData = e.GetString(0).Replace("\r", "").Split('\n');
					foreach(string i in RoomData)
					{
						foreach(string x in i.Split(','))
						{
							WorldBlocks[X, Y] = (Block)Convert.ToInt32(x);
								X++;
						}
						Y++;
					}
					Connected = true;
					break;
				case "add":
					Players.Add(new OldPlayer(e.GetInt(0), (Smiley)e.GetInt(1)));
					break;
				case "face":
					if (GetPlayer(e.GetInt(0)) != null)
					GetPlayer(e.GetInt(0)).Face = (Smiley)e.GetInt(1);
					break;
				case "left":
					if(GetPlayer(e.GetInt(0)) == null)
					{
						Players.Remove(GetPlayer(e.GetInt(0)));
					}
					break;
				case "change":
					WorldBlocks[e.GetInt(0), e.GetInt(1)] = (Block)e.GetInt(2);
					break;
				case "update":
					//While a player is pressing a key, they constantly send an update message.
					if (GetPlayer(e.GetInt(0)) != null)
					{
						GetPlayer(e.GetInt(0)).X = e.GetDouble(1);
						GetPlayer(e.GetInt(0)).Y = e.GetDouble(2);
					}
					else
					{
						OldPlayer ret = new OldPlayer(e.GetInt(0), Smiley.Smiley);
						ret.X = e.GetDouble(1);
						ret.Y = e.GetDouble(2);
						Players.Add(ret);
					}
					break;
			}
			//Notify the player of a message
			if (OnMessage != null)
			{
				List<object> objs = new List<object>();

				for(uint o = 0; o < e.Count; o++)
					objs.Add(e[o]);

				OnMessage(sender, new OldMessage(e.Type, objs.ToArray()));
			}
		}

		private void DisconnectHandler(object sender, string Message)
		{
			Connected = false;
			if (OnDisconnect != null)
			{
				OnDisconnect(sender, Message);
			}
		}
		#endregion
	}

	/// <summary>
	/// A FreeOldBot
	/// 
	/// Gives you the free will to get the connection, and much more stuff that you can't normally edit.
	/// </summary>
	public class FreeOldBot : OldBot
	{
		/// <summary>
		/// Gets the connection the bot is using
		/// </summary>
		/// <returns>The connection the bot is using</returns>
		public Connection Connection { get { return con; } set { con = value; } }

		/// <summary>
		/// Edits the raw world array
		/// </summary>
		/// <param name="X">The X position</param>
		/// <param name="Y">The Y position</param>
		/// <param name="set">The block to set it to</param>
		public void EditWorld(int X, int Y, Block set)
		{
			WorldBlocks[X, Y] = set;
		}

		/// <summary>
		/// Edit the entire world array
		/// </summary>
		/// <param name="Set">The new world array</param>
		public void EditWorld(Block[,] Set)
		{
			if (Set.GetLength(0) < 100)
				throw new ArgumentException("The width of the array has to be at least 100");

			if (Set.GetLength(1) < 100)
				throw new ArgumentException("The height of the array has to be at least 100");

			WorldBlocks = Set;
		}

		/// <summary>
		/// Edit the value of a player
		/// </summary>
		/// <param name="id">The Id of the player</param>
		/// <param name="NewPlayer">The new player</param>
		public void EditPlayer(int id, FreeOldPlayer NewPlayer)
		{
			if (GetPlayer(id) != null)
			{
				GetPlayer(id).Id = NewPlayer.Id;
				GetPlayer(id).Face = NewPlayer.Face;
				GetPlayer(id).X = NewPlayer.X;
				GetPlayer(id).Y = NewPlayer.Y;
			}
		}

		/// <summary>
		/// Get a FreeOldPlayer
		/// </summary>
		/// <param name="id">The Id of them</param>
		/// <returns>The FreeOldPlayer to get</returns>
		public FreeOldPlayer GetPlayer(int id)
		{
			if (GetPlayer(id) == null)
				return null;

			FreeOldPlayer ret = new FreeOldPlayer(id, GetPlayer(id).Face);
			ret.X = GetPlayer(id).X;
			ret.Y = GetPlayer(id).Y;
			return ret;
		}
	}
}

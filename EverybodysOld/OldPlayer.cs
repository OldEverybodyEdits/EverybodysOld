using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EverybodysOld
{
	public class OldPlayer
	{
		public OldPlayer(int Id_Player, Smiley Face_Player)
		{
			Id = Id_Player;
			Face = Face_Player;
		}

		public int Id { get; internal set; }
		public Smiley Face { get; internal set; }

		public double X { get; internal set; }
		public double Y { get; internal set; }
	}

	public class FreeOldPlayer : OldPlayer
	{
		public FreeOldPlayer(int Id_Player, Smiley Face_Player) : base(Id_Player, Face_Player)
		{
			Id = Id_Player;
			Face = Face_Player;
		}

		public void SetId(int id)
		{
			Id = id;
		}

		public void SetFace(Smiley face)
		{
			Face = face;
		}

		public void SetX(double x)
		{
			X = x;
		}

		public void SetY(double y)
		{
			Y = y;
		}
	}
}

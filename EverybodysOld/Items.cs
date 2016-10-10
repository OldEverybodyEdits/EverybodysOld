using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EverybodysOld
{
	public enum Block
	{
		Empty = 0,
		Arrow_Left = 1,
		Arrow_Up = 2,
		Arrow_Right = 3,
		Dot = 4,

		Basic_Gray = 5, //For different spellings of "gray"
		Basic_Grey = 5,

		Basic_Blue = 6,

		Basic_Magenta = 7, //For different variations of the purple basic block.
		Basic_Purple = 7,

		Basic_Red = 8,
		Basic_Yellow = 9,
		Basic_Green = 10,
		Basic_Cyan = 11,
		SquareSmiley = 12,
		Brick_Brown = 13,
		Brick_Teal = 14,
		Brick_Purple = 15,
		Brick_Magenta = 16,
		Brick_Green = 17,
		Metal_White = 18,

		Metal_Orange = 19, //They look similar, orange/red
		Metal_Red = 19,

		Metal_Yellow = 20
	}

	public enum Smiley
	{
		Smiley = 0,
		Grin = 1,
		Tongue = 2,
		Happy = 3,
		Annoyed = 4,
		Sad = 5
	}
}

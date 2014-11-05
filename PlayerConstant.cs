using UnityEngine;
using System.Collections;

public class PlayerConstant {
	public const float PLAYER_SPEED	= 2;

	public class SoundIndex {
		public const int 	FIRE	= 0,
							PANG	= 1;
	}

	public class TypeBullet {
		public const int	SIMPLE		= 0,
							DOUBLE		= 1,
							TRIPLE		= 2,
							QUADRA		= 3,
							PENTA		= 4;
	}

	public class Position {
		public const float 	NEO_LEFT		= -2.73f,
							NEO_RIGHT		= 2.73f,
							NEO_Y			= -4.8f;

		public const float 	X_WAIT_REBORN = -8;

		public static Vector3 POS_WAIT_REBORN = new Vector3 (X_WAIT_REBORN, NEO_Y, 0);
		public static Vector3 POS_REBORN = new Vector3 (0, NEO_Y, 0);
	}

	public class BulletInfo {
		public const float	DELTA_Y_BULLET = 0.34f;
		public const float 	V_BULLET = 6;
	}

	public class TimeInfo {
		public const float TIME_COUNT_DOWN_FIRE = 0.6f;
		public const float TIME_REBORN = 5;

		public const float TIME_FLASH_ON_START = 3;
		public const float TIME_CHANGE_FLASH	= 0.1f;
	}


	public class PlayerState {
		public const int		ON_CONTROL		= 1,
								ON_PANG			= 2,
								ON_REVIVAL		= 3,
								ON_GAME_OVER	= 4,
								ON_FLASH		= 5;
	}
}

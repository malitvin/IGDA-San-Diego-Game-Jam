using System.Collections;

namespace GhostGen
{
	public interface IGameState
	{
		void Init( Hashtable changeStateData );

		void Step( float deltaTime );
        void FixedStep(float deltaTime);

		void Exit();
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.RpgLogic {

	public interface INamed {
		string Name { get; }
    }
    public interface ITag {
        string Tag { get; set; }
    }

    public interface ICapaciteActive<T> : ITargeting, INamed
			where T : ITargetable {
		void AffectSelf( T target );
		void AffectTarget( T caster, T target );
		void AffectTargets( T caster, IEnumerable<T> targets );
	}

	public interface IModifier<C> : INamed {
		void AffectAgent( C a );
		void UnaffectAgent( C a );
	}

	public delegate void SelectionChangedHandler( INamedCollection sender );

	public interface INamedCollection {
		IEnumerable<INamed> NamedCollection { get; }
        void RemoveNamed( INamed item );
        void AddNamed( INamed item );
		event SelectionChangedHandler SelectionChanged;
    }
}

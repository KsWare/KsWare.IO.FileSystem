using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KsWare.IO.FileSystem.Tests {

	public class Fixture {
		private List<Action> _actions=new List<Action>();

		public Fixture() { }

		public Fixture(params Action[] actions) {
			_actions.AddRange(actions);
		}

		public void Add(Action action) { }

		public void Cleanup() {
			foreach (var action in _actions) {
				action.Invoke();
			}
		}
	}
}

using System.Collections;
using System.Collections.Generic;

namespace CL_Argument_Parser
{
	class MultiDictionary<T_Key, T_Values> : IEnumerable<KeyValuePair<T_Key, List<T_Values>>>
	{
		private Dictionary<T_Key, List<T_Values>> _backing = new Dictionary<T_Key, List<T_Values>>();

		public Dictionary<T_Key, List<T_Values>> backing => _backing;

		public void Add(T_Key key, T_Values value)
		{
			if (_backing.TryGetValue(key, out List<T_Values> result)) {
				result.Add(value);
			}
			else {
				_backing.Add(key, new List<T_Values> { value });
			}
		}

		public void AddRange(T_Key key, IReadOnlyList<T_Values> values)
		{
			if (_backing.TryGetValue(key, out List<T_Values> result)) {
				result.AddRange(values);
			}
			else {
				_backing.Add(key, new List<T_Values>(values));
			}
		}

		public bool TryGetValue(T_Key key, out List<T_Values> result)
		{
			return _backing.TryGetValue(key, out result);
		}

		public int Count {
			get => _backing.Count;
		}

		public bool ContainsKey(T_Key key)
		{
			return _backing.ContainsKey(key);
		}

		public IEnumerator<KeyValuePair<T_Key, List<T_Values>>> GetEnumerator()
		{
			foreach (var item in _backing) {
				yield return item;
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

	}

}

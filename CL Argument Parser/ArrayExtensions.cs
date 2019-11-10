namespace CLAP
{
    public static class ArrayExtensions
    {
		public static bool Contains(this string[] array, string element)
		{
			if (array == null) return false;
			for (int i = 0; i < array.Length; i++) {
				if (array[i].Equals(element)) return true;
			}
			return false;
		}
    }
}

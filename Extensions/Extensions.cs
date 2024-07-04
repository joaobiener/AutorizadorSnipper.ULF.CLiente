using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using System.Web;

namespace AutorizadorSnipper.ULF.Cliente.Extensions;

public static class Extensions
{

	public static T Next<T>(this T src) where T : struct
	{
		if (!typeof(T).IsEnum) throw new ArgumentException(String.Format("Argument {0} is not an Enum", typeof(T).FullName));

		T[] Arr = (T[])Enum.GetValues(src.GetType());
		int j = Array.IndexOf<T>(Arr, src) + 1; 
		return (Arr.Length == j) ? Arr[0] : Arr[j];
	}

	public static T Previous<T>(this T src) where T : struct
	{
		if (!typeof(T).IsEnum) throw new ArgumentException(String.Format("Argument {0} is not an Enum", typeof(T).FullName));

		T[] Arr = (T[])Enum.GetValues(src.GetType());
		int j = Array.IndexOf<T>(Arr, src) - 1;
		return (j == -1) ? Arr[Arr.Length-1] : Arr[j];
	}

	//Use exemplo: StatusEnum MyStatus = "Active".ToEnum<StatusEnum>();
	public static T ToEnum<T>(this string value)
	{
		return (T)Enum.Parse(typeof(T), value, true);
	}
}

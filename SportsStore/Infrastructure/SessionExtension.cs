using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace SportsStore.Infrastructure
{
	public static class SessionExtension
	{
		public static void SetJson(this ISession session, string key, object value)
		{
			session.SetString(key, JsonSerializer.Serialize(value));
		}

		public static T GetJson<T>(this ISession session, string key)
		{
			string value = session.GetString(key);
			return value == null ?
				default(T) :
				JsonSerializer.Deserialize<T>(value);
		}
	}
}

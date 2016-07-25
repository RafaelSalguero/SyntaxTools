using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxTools.Global
{
    /// <summary>
    /// A global thread-safe dictionary of friendly names for guids
    /// </summary>
    internal static class GuidNames
    {
        private static ConcurrentDictionary<Guid, string> Names = InitNames();
        private static ConcurrentDictionary<Guid, string> InitNames()
        {
            var Ret = new ConcurrentDictionary<Guid, string>();
            Ret.TryAdd(Guid.Empty, "[any]");
            return Ret;
        }

        /// <summary>
        /// Pair a guid with a friendly name. Returns the same Guid
        /// </summary>
        /// <param name="Token"></param>
        /// <param name="Name"></param>
        internal static Guid SetName(Guid Token, string Name)
        {
            Names[Token] = Name;
            return Token;
        }

        /// <summary>
        /// Pair a new guid with a friendly name. Returns the same Guid
        /// </summary>
        /// <param name="Token"></param>
        /// <param name="Name"></param>
        internal static Guid SetName(string Name)
        {
            return SetName(Guid.NewGuid(), Name);
        }

        /// <summary>
        /// Gets the friendly name of the given Guid
        /// </summary>
        /// <param name="Token"></param>
        /// <returns></returns>
        internal static string GetName(Guid Token)
        {
            string result;
            if (Names.TryGetValue(Token, out result))
                return result;
            else
                return Token.ToString();
        }

        /// <summary>
        /// Returns true if the given guid have a friendly name
        /// </summary>
        /// <param name="Token"></param>
        /// <returns></returns>
        internal static bool HasName(Guid Token)
        {
            return Names.ContainsKey(Token);
        }
    }
}
